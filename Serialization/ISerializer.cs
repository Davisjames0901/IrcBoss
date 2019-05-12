using Digman.Io.IrcBalistic.Abstracts;
using Digman.Io.IrcBalistic.Classes;

namespace Digman.Io.IrcBalistic.Serialization
{
  public interface ISerializer
  {
    Message DeserializeMessage(string message);
    string SerializeResponse(Response response);
  }
}