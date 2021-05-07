using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Core.Interfaces;
using Asperand.IrcBallistic.Utilities.CollectionExtensions;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Core.Module
{
    public abstract class ModuleBase : IModule
    {
        private readonly ILogger<ModuleBase> _log;
        private readonly Queue<TimeSpan> _opTimes;
        private readonly Queue<TimeSpan> _nopTimes;
        private DateTime? _executionStartedTime;
        private int _totalRuns;
        private int _exceptions;
        private bool _isExecuting;
        private bool _isPanicking;
        private Task _executingTask;

        public ModuleBase(ILogger<ModuleBase> log)
        {
            _log = log;
            _opTimes = new Queue<TimeSpan>();
            _nopTimes = new Queue<TimeSpan>();
            _totalRuns = 0;
            _exceptions = 0;
        }

        public Task Handle(ModuleEvent request)
        {
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
        
        public abstract bool IsEagerModule { get; }
        public abstract int TimeoutSeconds { get; }
        protected abstract Task<ModuleResult> Execute(IRequest payload, IConnection connection);
    }
}