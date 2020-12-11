using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Core.Job.Data;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Core.Job
{
    public abstract class BaseJob : IDisposable
    {
        private List<Action<JobStatistics>> _troubleCallbacks;
        private Task _executingTask;
        protected readonly ILogger<BaseJob> Log;
        protected bool IsPanicking;

        public BaseJob(ILogger<BaseJob> log)
        {
            Log = log;
            _troubleCallbacks = new List<Action<JobStatistics>>();
        }

        public void RegisterTroubleCallback(Action<JobStatistics> action)
        {
            _troubleCallbacks.Add(action);
        }

        protected void Panic()
        {
            Log.LogError("Starting panic!");
            IsPanicking = true;
            if (_troubleCallbacks.Count == 0)
            {
                Log.LogError("No trouble callback, Killing task violently buckle up");
                Kill();
                return;
            }

            while (IsPanicking)
            {
                Log.LogError($"Calling for help... I think ive been stuck for {JobStats.StuckTime}");
                RaiseTrouble();
                Thread.Sleep(10_000);
            }
        }

        protected void RaiseTrouble()
        {
            foreach (var action in _troubleCallbacks)
            {
                action(JobStats);
            }
        }

        public virtual void Kill()
        {
            IsPanicking = false;
            Dispose();
        }

        public abstract JobStatistics JobStats { get; }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~BaseJob()
        {
            Dispose(false);
        }
    }
}