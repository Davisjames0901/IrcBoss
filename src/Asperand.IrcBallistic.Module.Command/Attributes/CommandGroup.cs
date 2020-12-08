using System;

namespace Asperand.IrcBallistic.Module.Command.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandGroup : Attribute
    {
        public CommandGroup(string commandGroupName, string helpText = null)
        {
            CommandName = commandGroupName;
            HelpText = helpText;
        }

        public string CommandName { get; }
        public string HelpText { get; }
    }
}