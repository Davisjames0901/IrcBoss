using System.Diagnostics;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Connections.Irc;
using Asperand.IrcBallistic.Core.Interfaces;
using Asperand.IrcBallistic.Module.Command.Data;
using Asperand.IrcBallistic.Module.Command.Engine;
using Asperand.IrcBallistic.Module.Command.Interfaces;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Module.Command
{
    public class CommandModule : IResponsiveModule
    {
        private readonly ILogger<CommandModule> _log;
        private readonly CommandEngine _commandEngine;
        private readonly CommandMetadataAccessor _commandAccessor;
        private readonly ISerializer _serializer;

        public bool IsEagerModule => true;

        public CommandModule(
            ILogger<CommandModule> log,
            CommandEngine commandEngine,
            CommandMetadataAccessor commandAccessor,
            ISerializer serializer)
        {
            _log = log;
            _commandEngine = commandEngine;
            _commandAccessor = commandAccessor;
            _serializer = serializer;
        }

        public Task Handle<T>(IRequest payload, T connection) where T : IConnection
        {
            var request = _serializer.Deserialize(payload);
            if(request is not null)
                HandleMessage(request, connection);
            return Task.CompletedTask;
        }

        public Task WriteMessage(IResponse response, IConnection source)
        {
            return (source as IrcConnection).WriteMessage(_serializer.Serialize(response));
        }

        private void HandleMessage(CommandRequest request, IConnection connection)
        {
            var timer = Stopwatch.StartNew();
            
            _log.LogInformation("Received command request");
            var command = _commandAccessor.LocateCommandGroup(request.CommandName);
            _commandAccessor.PopulateCommand(command, request);
            if (command == null)
            {
                return;
            }

            var pid = _commandEngine.StartCommand(command, request, connection, this);
            _log.LogInformation($"Started process with pid of {pid}");
            timer.Stop();
            _log.LogWarning($"Took {timer.ElapsedMilliseconds}ms to find and execute command!");
        }
    }
}