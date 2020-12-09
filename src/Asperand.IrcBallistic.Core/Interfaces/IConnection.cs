using System;
using System.Threading.Tasks;

namespace Asperand.IrcBallistic.Core.Interfaces
{
  public interface IConnection : IDisposable
  {
    string Name { get; }
    Task Stop();
    void Start();
    public Task WriteMessage(Response message);
  }
}