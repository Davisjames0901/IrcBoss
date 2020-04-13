using System;
using Asperand.IrcBallistic.Worker.Classes;
using Asperand.IrcBallistic.Worker.Interfaces;
using Asperand.IrcBallistic.Worker.Messages;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Worker.Modules.Command
{
    public class CommandModule : IModule
    {
        private readonly ILogger<CommandModule> _log;
        private readonly CommandEngine _commandEngine;
        private readonly CommandLocator _commandLocator;
        private readonly ArgumentParser _argumentParser;
        private IConnection _source;

        public bool IsEagerModule => true;
        public bool IsReinstatable => false;

        public CommandModule(
            ILogger<CommandModule> log,
            CommandEngine commandEngine,
            CommandLocator commandLocator,
            ArgumentParser argumentParser)
        {
            _log = log;
            _commandEngine = commandEngine;
            _commandLocator = commandLocator;
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
            var command = _commandLocator.LocateCommandGroup(commandRequest.CommandName);
            if (command == null)
            {
                return;
            }

            var pid = _commandEngine.StartCommand(command, commandRequest, _source);
            _log.LogInformation($"Started process with pid of {pid}");
        }
    }
}