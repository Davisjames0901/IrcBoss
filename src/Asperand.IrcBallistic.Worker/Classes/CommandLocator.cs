using System;
using System.Collections.Generic;
using System.Linq;
using Asperand.IrcBallistic.Worker.Attributes;
using Asperand.IrcBallistic.Worker.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Asperand.IrcBallistic.Worker.Classes
{
    public class CommandLocator
    {
        private readonly IServiceProvider _services;
        public CommandLocator(IServiceProvider services)
        {
            _services = services;
        }

        public ICommand LocateCommandGroup(string commandName)
        {
            var test = _services.GetService<IEnumerable<ICommand>>();
            return test
                .SingleOrDefault(x =>
                    string.Equals(
                        (Attribute.GetCustomAttribute(x.GetType(), typeof(CommandGroup)) as CommandGroup)?.Value,
                        commandName,
                        StringComparison.InvariantCultureIgnoreCase));
        }
    }
}