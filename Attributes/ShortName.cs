using System;

namespace Digman.Io.IrcBalistic.Attributes
{
  public class ShortName: Attribute
  {
    private readonly char _flag;
    public ShortName(char flag)
    {
        _flag = flag;
    }

    public char Flag => _flag;
  }
}