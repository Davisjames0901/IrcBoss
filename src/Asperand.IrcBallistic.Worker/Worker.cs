using System;
using System.Threading;
using System.Threading.Tasks;
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
            while (!stoppingToken.IsCancellationRequested)
            {
                //_log.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}