using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Core.Interfaces;
using Asperand.IrcBallistic.Core.Job;
using Asperand.IrcBallistic.Core.Job.Data;
using Asperand.IrcBallistic.Utilities.CollectionExtensions;
using Asperand.IrcBallistic.Utilities.Concurrancy;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Core.Module
{
    public abstract class ModuleBase : EventfulJob<ModuleEvent>, IModule
    {
        private readonly Queue<TimeSpan> _opTimes;
        private readonly Queue<TimeSpan> _nopTimes;
        private DateTime? _executionStartedTime;
        private int _totalRuns;
        private int _exceptions;
        private bool _isExecuting;
        private bool _isPanicking;
        private Task _executingTask;

        public ModuleBase(ILogger<ModuleBase> log) :base(log)
        {
            _opTimes = new Queue<TimeSpan>();
            _nopTimes = new Queue<TimeSpan>();
            _totalRuns = 0;
            _exceptions = 0;
        }

        public override JobStatistics JobStats => new()
        {
            Status = GetCurrentStatus(),
            DefaultTimeout = TimeoutSeconds,
            ExceptionRatio = (float) _exceptions / _totalRuns,
            AverageNopTime = new TimeSpan((long) _nopTimes.Average(x => x.Ticks)),
            AverageOpTime = new TimeSpan((long) _opTimes.Average(x => x.Ticks)),
            ExecutionStartedTime = _executionStartedTime
        };

        public override Task Handle(ModuleEvent request)
        {
            if (_isExecuting)
            {
                Locks.SimpleInverseSpinLock(ref _isExecuting, TimeoutSeconds, Panic);
            }

            _isPanicking = false;
            _isExecuting = true;
            _executionStartedTime = DateTime.Now;

            var timer = Stopwatch.StartNew();
            _executingTask = Execute(request.Request, request.Connection)
                .ContinueWith(x => Complete(timer, x));
            return _executingTask;
        }
        
        private void Complete(Stopwatch timer, Task<ModuleResult> task)
        {
            _totalRuns++;
            timer.Stop();
            if (task.IsFaulted)
            {
                Log.LogError("Module handle failed", task.Exception);
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
                    Log.LogWarning($"Module result not handled in statistics: {task.Result}");
                    break;
            }

            _isExecuting = false;
            _executionStartedTime = null;
        }
        
        private JobStatus GetCurrentStatus()
        {
            if (_isPanicking)
                return JobStatus.Panicking;
            if (_isExecuting)
                return JobStatus.Running;
            return JobStatus.Idle;
        }

        public override void Kill()
        {
            _isExecuting = false;
            base.Kill();
        }
        
        public abstract bool IsEagerModule { get; }
        public abstract int TimeoutSeconds { get; }
        protected abstract Task<ModuleResult> Execute(IRequest payload, IConnection connection);
    }
}