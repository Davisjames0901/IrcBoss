using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Asperand.IrcBallistic.Worker.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Worker.Modules.Command.Dependencies
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

        public IEnumerable<CommandMetadata> GetAllCommandData()
        {
            var commands = _services.GetService<IEnumerable<ICommand>>();
            foreach (var command in commands)
            {
                yield return GetCommandData(command);
            }
        }
        
        public CommandMetadata GetCommandData(ICommand command)
        {
            var commandType = command.GetType();
            var commandGroupAttribute = Attribute.GetCustomAttribute(commandType, typeof(CommandGroup)) as CommandGroup;
            return new CommandMetadata
            {
                Flags = GetMembersWithAttribute(commandType, typeof(FlagAttribute))
                    .Select(GetFlagValues)
                    .ToDictionary(x => x.Flag, x => x.HelpText),
                CommandName = commandGroupAttribute?.CommandName,
                ContentHelpText = commandGroupAttribute?.HelpText
            };
        }
        
        public ICommand LocateCommandGroup(string commandName)
        {
            return _commandLookup[commandName].Invoke();
        }

        public ICommand PopulateCommand(ICommand command, CommandRequest request)
        {
            var stopwatch = Stopwatch.StartNew();
            var flagMembers = GetMembersWithAttribute(command.GetType(), typeof(FlagAttribute));
            foreach (var item in flagMembers)
            {
                var flag = GetFlagValues(item).Flag;
                if (request.Flags.ContainsKey(flag))
                {
                    SetPropertyValue(command, item as PropertyInfo, request.Flags[flag]);
                }
            }

            var content = GetMembersWithAttribute(command.GetType(), typeof(ContentAttribute)).FirstOrDefault();
            if (content != null)
            {
                SetPropertyValue(command, content as PropertyInfo, request.Content);
            }
            stopwatch.Stop();
            _log.LogWarning($"Took {stopwatch.ElapsedMilliseconds}ms to populate command.");
            return command;
        }

        private IEnumerable<MemberInfo> GetMembersWithAttribute(Type type, Type attributeType)
        {
            return type.GetMembers()
                .Where(x => x.GetCustomAttributes(attributeType, true)
                    .Any());
        }

        private (string Flag, string HelpText) GetFlagValues(MemberInfo info)
        {
            return info.GetCustomAttributes(typeof(FlagAttribute), true)
                .Select(x => ((x as FlagAttribute).Flag, (x as FlagAttribute).HelpText))
                .FirstOrDefault();
        }

        private void SetPropertyValue(ICommand instance, PropertyInfo prop, string value)
        {
            prop.SetValue(instance, value);
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