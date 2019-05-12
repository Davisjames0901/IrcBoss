using System;

namespace Digman.Io.IrcBalistic.Attributes
{
  public class Content: Attribute
  {
    private readonly bool _includeCommand;

    public Content(bool includeCommand = false)
    {
        _includeCommand = includeCommand;
    }
    public bool IncludeCommand => _includeCommand;
  }
}