using Asperand.IrcBallistic.Worker.Events;

namespace Asperand.IrcBallistic.Worker.Serialization
{
  public interface ISerializer
  {
    IEvent Deserialize(string message, EventType eventType);
    string Serialize(MessageResponse messageResponse);
  }
}