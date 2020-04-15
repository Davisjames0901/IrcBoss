using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Module.Command.Data;
using Asperand.IrcBallistic.Module.Command.Enum;

namespace Asperand.IrcBallistic.Module.Command
{
  public interface ICommand
  {
    CommandExecutionContext Context { get; set; }
    Task<CommandResult> Execute(CancellationToken token);
    void RemoveCallbacks();
  }
}