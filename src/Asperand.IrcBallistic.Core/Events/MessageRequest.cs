
using Asperand.IrcBallistic.Core.Interfaces;

namespace Asperand.IrcBallistic.Core.Events
{
  public class MessageRequest : IEvent
  {
    public string SourceUserName { get; set; }
    public string Text { get; set; }
    public string Target { get; set; }
  }
}