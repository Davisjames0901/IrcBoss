using System;

namespace Digman.Io.IrcBalistic.Attributes
{
  public class SpacesAllowed : Attribute
  {
    private bool _value;
    public SpacesAllowed(bool value)
    {
      _value = value;
    }
    public bool Value => _value;
  }
}