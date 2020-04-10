using Asperand.IrcBallistic.Worker.Messages;

namespace Asperand.IrcBallistic.Worker.Serialization
{
  public interface ISerializer
  {
    Request Deserialize(string message);
    string Serialize(Response response);
  }
}