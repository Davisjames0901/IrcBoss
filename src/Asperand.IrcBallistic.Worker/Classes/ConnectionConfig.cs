using System.Collections.Generic;

namespace Asperand.IrcBallistic.Worker.Classes
{
  public class ConnectionConfig
  {
    public Dictionary<string, string> Permissions { get; set; }
    public char MessageFlag { get; set; }
  }
}