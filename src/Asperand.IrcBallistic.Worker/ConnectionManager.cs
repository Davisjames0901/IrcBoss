using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Worker.Classes;
using Asperand.IrcBallistic.Worker.Connections;
using Asperand.IrcBallistic.Worker.Interfaces;
using Asperand.IrcBallistic.Worker.Messages;
using Asperand.IrcBallistic.Worker.Modules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Worker
{
    public class ConnectionManager
    {
        private readonly IEnumerable<IConnection> _connections;
        private readonly ILogger<ConnectionManager> _log;
        private readonly IServiceProvider _services;

        public ConnectionManager(IEnumerable<IConnection> connections,
            ILogger<ConnectionManager> log,
            IServiceProvider services)
        {
            _services = services;
            _connections = connections;
            _log = log;
        }

        public void Start()
        {
            _log.LogInformation("Starting connections");
            foreach (var connection in _connections)
            {
                var modules = _services.GetService<IEnumerable<IModule>>().Where(x=>x.IsEagerModule);
                connection.Start();
                _log.LogInformation($"Registering modules for {nameof(connection)}");
                foreach (var module in modules)
                {
                    _log.LogInformation($"Registering module {nameof(module)}");
                    module.RegisterConnection(connection);
                }
            }
        }

        public async Task Stop()
        {
            _log.LogInformation("Stopping connections");
            foreach (var connection in _connections)
            {
                await connection.Stop();
            }
        }
    }
}