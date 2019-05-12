using System.Collections.Generic;

namespace Digman.Io.IrcBalistic.Classes
{
  public class Response
  {
    public string Text { get; set; }
    public bool IsAction { get; set; }
    public string Target { get; set; }
  }
}