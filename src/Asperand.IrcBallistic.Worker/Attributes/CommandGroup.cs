using System;

namespace Asperand.IrcBallistic.Worker.Attributes
{
  
  [AttributeUsage(AttributeTargets.Class)]
  public class CommandGroup : Attribute
  {
    private readonly string _commandName;
    private readonly string _helpText;
    public CommandGroup(string commandGroupName, string helpText = null)
    {
        _commandName = commandGroupName;
        _helpText = helpText;
    }

    public string CommandName => _commandName;
    public string HelpText => _helpText;
  }
}