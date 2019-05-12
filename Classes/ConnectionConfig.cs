using System.Collections.Generic;

namespace Digman.Io.IrcBalistic.Classes
{
  public class ConnectionConfig
  {
    public Dictionary<string, string> Permissions { get; set; }
    public char MessageFlag { get; set; }
  }
}