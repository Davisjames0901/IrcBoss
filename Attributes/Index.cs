using System;

namespace Digman.Io.IrcBalistic.Attributes
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