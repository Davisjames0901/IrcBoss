using System.Collections.Generic;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Core
{
    public class ConnectionManager
    {
        private readonly IEnumerable<IConnection> _connections;
        private readonly ILogger<ConnectionManager> _log;

        public ConnectionManager(IEnumerable<IConnection> connections,
            ILogger<ConnectionManager> log)
        {
            _connections = connections;
            _log = log;
        }

        public void Start()
        {
            _log.LogInformation("Starting connections");
            foreach (var connection in _connections)
            {
                connection.Start();
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