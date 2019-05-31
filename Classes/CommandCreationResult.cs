using System.Collections.Generic;

namespace Digman.Io.IrcBalistic.Classes
{
  public class CommandCreationResult
  {
    public object Command { get; set; }
    public List<string> Errors { get; set; }
  }
}