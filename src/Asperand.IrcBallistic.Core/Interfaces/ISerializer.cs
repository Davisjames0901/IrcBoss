using Asperand.IrcBallistic.Core.Events;

namespace Asperand.IrcBallistic.Core.Interfaces
{
  public interface ISerializer
  {
    IEvent Deserialize(string message, EventType eventType);
    string Serialize(MessageResponse messageResponse);
  }
}