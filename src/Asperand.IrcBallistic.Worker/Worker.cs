using System;
using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Core;
using Asperand.IrcBallistic.InversionOfControl;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _log;
        private readonly ConnectionManager _connectionManager;
        private readonly ContainerValidator _validator;

        public Worker(ILogger<Worker> log, ConnectionManager connectionManager, ContainerValidator validator)
        {
            _log = log;
            _connectionManager = connectionManager;
            _validator = validator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _validator.Validate("./test.txt");
            _connectionManager.Start();
        }
    }
}