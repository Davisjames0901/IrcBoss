using System;
using Asperand.IrcBallistic.Worker.Connections;
using Asperand.IrcBallistic.Worker.Events;
using Asperand.IrcBallistic.Worker.Modules.Command.Dependencies;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Worker.Modules.Command
{
    public class CommandModule : IModule
    {
        private readonly ILogger<CommandModule> _log;
        private readonly CommandEngine _commandEngine;
        private readonly CommandMetadataAccessor _commandAccessor;
        private readonly ArgumentParser _argumentParser;
        private IConnection _source;

        public bool IsEagerModule => true;
        public bool IsReinstatable => false;

        public CommandModule(
            ILogger<CommandModule> log,
            CommandEngine commandEngine,
            CommandMetadataAccessor commandAccessor,
            ArgumentParser argumentParser)
        {
            _log = log;
            _commandEngine = commandEngine;
            _commandAccessor = commandAccessor;
            _argumentParser = argumentParser;
        }
        public void RegisterConnection(IConnection connection)
        {
            if (_source != null)
            {
                _log.LogError("Only one connection per module please :)");
                return;
            }
            _source = connection;
            connection.RegisterCallback(EventType.Message, e => HandleMessage(e as MessageRequest));
        }

        private void HandleMessage(MessageRequest messageRequest)
        {
            if (messageRequest.Text[0] != _source.MessageFlag)
            {
                return;
            }
            
            _log.LogInformation("Received command request");
            var commandRequest = _argumentParser.ParseCommandRequest(messageRequest);
            var command = _commandAccessor.LocateCommandGroup(commandRequest.CommandName);
            if (command == null)
            {
                return;
            }

            var pid = _commandEngine.StartCommand(command, commandRequest, _source);
            _log.LogInformation($"Started process with pid of {pid}");
        }
    }
}