using Digman.Io.IrcBalistic.Classes;

namespace Digman.Io.IrcBalistic.Commands
{
  public interface ICommand
  {
    ResponsePacket Execute();
  }
}