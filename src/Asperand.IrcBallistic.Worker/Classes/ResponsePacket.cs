using System.Collections.Generic;
using Asperand.IrcBallistic.Worker.Messages;

namespace Asperand.IrcBallistic.Worker.Classes
{
  public class ResponsePacket
  {
    public List<Response> Responses { get; set; }
  }
}