using System;

namespace Digman.Io.IrcBalistic.Attributes
{
  
  [AttributeUsage(AttributeTargets.Class)]
  public class CommandGroup : Attribute
  {
    private readonly string _value;
    public CommandGroup(string commandGroupName)
    {
        _value = commandGroupName;
    }

    public string Value => _value;
  }
}