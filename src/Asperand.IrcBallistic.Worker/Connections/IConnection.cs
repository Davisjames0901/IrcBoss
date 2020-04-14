using System;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Worker.Events;

namespace Asperand.IrcBallistic.Worker.Connections
{
  public interface IConnection
  {
    string Name { get; }
    char MessageFlag { get; }
    bool IsOpen { get; }

    Task SendMessage(MessageResponse messageResponse);
    Task Stop();
    void Start();
    Guid RegisterCallback(EventType eventType, Action<IEvent> callback);
    public void RemoveCallback(Guid id);
  }
}