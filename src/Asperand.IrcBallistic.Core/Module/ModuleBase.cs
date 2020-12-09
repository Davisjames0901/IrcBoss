using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Core.Interfaces;
using Asperand.IrcBallistic.Utilities.CollectionExtensions;
using Asperand.IrcBallistic.Utilities.Concurrancy;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Core.Module
{
    public abstract class ModuleBase : IModule
    {
        private readonly ILogger<IModule> _log;
        private readonly Queue<TimeSpan> _opTimes;
        private readonly Queue<TimeSpan> _nopTimes;
        private DateTime? _executionStartedTime;
        private int _totalRuns;
        private int _exceptions;
        private bool _isExecuting;
        private bool _isPanicking;
        private Task _executingTask;
        private Action<ModuleStatistics> _troubleCallback;

        public ModuleBase(ILogger<IModule> log)
        {
            _log = log;
            _opTimes = new Queue<TimeSpan>();
            _nopTimes = new Queue<TimeSpan>();
            _totalRuns = 0;
            _exceptions = 0;
        }

        public ModuleStatistics ModuleStatistics => new()
        {
            Status = GetCurrentStatus(),
            DefaultTimeout = TimeoutSeconds,
            ExceptionRatio = (float) _exceptions / _totalRuns,
            AverageNopTime = new TimeSpan((long) _nopTimes.Average(x => x.Ticks)),
            AverageOpTime = new TimeSpan((long) _opTimes.Average(x => x.Ticks)),
            ExecutionStartedTime = _executionStartedTime
        };

        public Task Handle<T>(IRequest request, T connection) where T : IConnection
        {
            if (_isExecuting)
            {
                Locks.SimpleInverseSpinLock(ref _isExecuting, TimeoutSeconds, StartPanic);
            }

            _isPanicking = false;
            _isExecuting = true;
            _executionStartedTime = DateTime.Now;

            var timer = Stopwatch.StartNew();
            _executingTask = Execute(request, connection)
                .ContinueWith(x => Complete(timer, x));
            return _executingTask;
        }

        public void RegisterTroubleCallback(Action<ModuleStatistics> action)
        {
            _troubleCallback = action;
        }

        private void Complete(Stopwatch timer, Task<ModuleResult> task)
        {
            _totalRuns++;
            timer.Stop();
            if (task.IsFaulted)
            {
                _log.LogError("Module handle failed", task.Exception);
                _exceptions++;
                return;
            }

            switch (task.Result)
            {
                case ModuleResult.Nop:
                    _nopTimes.AddButDontExceed(timer.Elapsed, 100);
                    break;
                case ModuleResult.Op:
                    _opTimes.AddButDontExceed(timer.Elapsed, 100);
                    break;
                default:
                    _log.LogWarning($"Module result not handled in statistics: {task.Result}");
                    break;
            }

            _isExecuting = false;
            _executionStartedTime = null;
        }
        
        private ModuleStatus GetCurrentStatus()
        {
            if (_isPanicking)
                return ModuleStatus.Panicking;
            if (_isExecuting)
                return ModuleStatus.Running;
            return ModuleStatus.Idle;
        }

        private void StartPanic()
        {
            _log.LogError("Starting panic!");
            _isPanicking = true;
            if (_troubleCallback == null)
            {
                _log.LogError("No trouble callback, Killing task violently buckle up");
                Kill();
                return;
            }

            while (_isPanicking)
            {
                var stuckTime = (DateTime.Now - _executionStartedTime.Value).TotalSeconds - TimeoutSeconds;
                _log.LogError($"Calling for help... I think ive been stuck for {stuckTime}");
                _troubleCallback(ModuleStatistics);
                Thread.Sleep(10_000);
            }
        }

        private void Kill()
        {
            _executingTask?.Dispose();
            _isExecuting = false;
            _isPanicking = false;
        }


        public abstract bool IsEagerModule { get; }
        public abstract int TimeoutSeconds { get; }
        protected abstract Task<ModuleResult> Execute<T>(IRequest payload, T connection) where T : IConnection;
    }
}