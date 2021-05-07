using System;
using System.Collections.Generic;
using Asperand.IrcBallistic.Module.Command.Attributes;
using Asperand.IrcBallistic.Module.Command.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Module.Command.Engine
{
    public class CommandMetadataAccessor
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<CommandMetadataAccessor> _log;
        private readonly Dictionary<string, Func<ICommand>> _commandLookup;
        public CommandMetadataAccessor(IServiceProvider services, ILogger<CommandMetadataAccessor> log)
        {
            _log = log;
            _services = services;
            _commandLookup = CreateCommandLookup();
        }
        
        public ICommand LocateCommandGroup(string commandName)
        {
            return _commandLookup.TryGetValue(commandName, out var command) ? command.Invoke() : null;
        }

        private Dictionary<string, Func<ICommand>> CreateCommandLookup()
        {
            var commandLookup = new Dictionary<string, Func<ICommand>>();
            var commands = _services.GetService<IEnumerable<ICommand>>();
            foreach (var command in commands)
            {
                var commandGroupAttribute = Attribute.GetCustomAttribute(command.GetType(), typeof(CommandGroup)) as CommandGroup;
                if (commandGroupAttribute == null || string.IsNullOrWhiteSpace(commandGroupAttribute.CommandName))
                {
                    _log.LogError($"Command didnt have command group attribute or command name was empty. Command: {command.GetType()}");
                    continue;
                }
                commandLookup.Add(commandGroupAttribute.CommandName, () => ResolveCommand(command.GetType()));
                _log.LogInformation($"Loaded command {commandGroupAttribute.CommandName}");
            }

            return commandLookup;
        }

        private ICommand ResolveCommand(Type type)
        {
            return (ICommand) _services.GetService(type);
        }
    }
}