using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Asperand.IrcBallistic.Worker.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Asperand.IrcBallistic.Worker.Modules.Command.Dependencies
{
    public class CommandMetadataAccessor
    {
        private readonly IServiceProvider _services;
        public CommandMetadataAccessor(IServiceProvider services)
        {
            _services = services;
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
            if (string.IsNullOrWhiteSpace(commandName))
            {
                return null;
            }
            var test = _services.GetService<IEnumerable<ICommand>>();
            return test
                .SingleOrDefault(x =>
                    string.Equals(
                        (Attribute.GetCustomAttribute(x.GetType(), typeof(CommandGroup)) as CommandGroup)?.CommandName,
                        commandName,
                        StringComparison.InvariantCultureIgnoreCase));
        }

        public ICommand PopulateCommand(ICommand command, CommandRequest request)
        {
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
    }
}