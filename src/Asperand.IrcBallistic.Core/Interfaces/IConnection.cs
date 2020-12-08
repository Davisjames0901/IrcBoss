using System.Collections.Generic;
using System.Threading.Tasks;

namespace Asperand.IrcBallistic.Core.Interfaces
{
  public interface IConnection
  {
    string Name { get; }
    Task Stop();
    void Start(IEnumerable<IModule> modules);
    public Task WriteMessage(string message);
  }
}