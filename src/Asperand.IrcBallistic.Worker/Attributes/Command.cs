using System;

namespace Asperand.IrcBallistic.Worker.Attributes
{
  public class Command : Attribute
  {
    private readonly string _name;
    public Command(string name = "")
    {
        _name = name.ToLower();
    }
    public string Name => _name;
  }
}