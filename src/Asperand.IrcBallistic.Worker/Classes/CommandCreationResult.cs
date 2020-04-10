using System.Collections.Generic;

namespace Asperand.IrcBallistic.Worker.Classes
{
  public class CommandCreationResult
  {
    public object Command { get; set; }
    public List<string> Errors { get; set; }
  }
}