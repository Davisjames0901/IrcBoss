using System.Collections.Generic;
using Digman.Io.IrcBalistic.Abstracts;

namespace Digman.Io.IrcBalistic.Classes
{
  public class Message
  {
    public User SourceUser { get; set; }
    public string Target { get; set; }
    public string Text { get; set; }
    public string[] Arguments { get; set; }
    public string Command { get; set; }
    public List<Flag> Flags { get; set; }
    public Connection SourceConnection { get; set; }
  }
}