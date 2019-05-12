using System;

namespace Digman.Io.IrcBalistic.Attributes
{
  public class CommandName : Attribute
  {
    private readonly string _name;
    public CommandName(string name)
    {
        _name = name.ToLower();
    }
    public string Name => _name;
  }
}