using Asperand.IrcBallistic.Worker.Messages;

namespace Asperand.IrcBallistic.Worker.Serialization
{
  public interface ISerializer
  {
    IEvent Deserialize(string message, EventType eventType);
    string Serialize(MessageResponse messageResponse);
  }
}