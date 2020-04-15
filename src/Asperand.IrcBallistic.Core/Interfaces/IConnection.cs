using System;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Core.Events;

namespace Asperand.IrcBallistic.Core.Interfaces
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