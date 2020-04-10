using System;

namespace Asperand.IrcBallistic.Worker.Attributes
{
  public class Index: Attribute
  {
    private readonly int _value;
    public int Value => _value;

    public Index(int value)
    {
        _value = value;
    }

  }
}