using System;
using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _log;
        private readonly ConnectionManager _connectionManager;

        public Worker(ILogger<Worker> log, ConnectionManager connectionManager)
        {
            _log = log;
            _connectionManager = connectionManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _connectionManager.Start();
        }
    }
}