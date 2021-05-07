using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Core.Interfaces;
using Asperand.IrcBallistic.Core.Module;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Core
{
    public abstract class ConnectionBase: IConnection
    {
        protected bool IsRunning;
        private readonly ILogger<IConnection> _log;
        private readonly List<ModuleBase> _modules;
        
        public ConnectionBase(IEnumerable<ModuleBase> modules, ILogger<IConnection> log)
        {
            _log = log;
            _modules = modules.ToList();
        }
        public Task Stop()
        {
            throw new System.NotImplementedException("There are no brakes on this train!");
        }

        public void Start()
        {
            if (IsRunning) return;
            IsRunning = true;
            InternalStart();
        }

        public void Handle(IRequest request)
        {
            foreach (var module in _modules)
            {
                //Todo: we need to pull the statistics stuff out of the module and create the job registry
                new Thread(() => module.Handle(new ModuleEvent(request, this))).Start();
            }
        }

        public abstract string Name { get; }
        public abstract Task WriteMessage(Response message);
        public void RegisterAction()
        {
            throw new NotImplementedException();
        }

        public abstract void InternalStart();

        public abstract void Dispose();
    }
}