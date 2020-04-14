using System.Threading;
using System.Threading.Tasks;

namespace Asperand.IrcBallistic.Worker.Modules.Command.Dependencies
{
  public interface ICommand
  {
    CommandExecutionContext Context { get; set; }
    Task<CommandResult> Execute(CancellationToken token);
    void RemoveCallbacks();
  }
}