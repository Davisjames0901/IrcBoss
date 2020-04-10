using System;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Worker.Messages;

namespace Asperand.IrcBallistic.Worker.Interfaces
{
  public interface IConnection
  {
    string Name { get; }
    bool IsOpen { get; }

    Task SendMessage(Response response);
    Task Stop();
    void Start();
    void RegisterCallback(Action<Request> callback);
  }
}