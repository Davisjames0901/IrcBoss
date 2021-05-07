using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Module.Command.Data;
using Asperand.IrcBallistic.Module.Command.Enum;

namespace Asperand.IrcBallistic.Module.Command.Interfaces
{
  public interface ICommand
  {
    CommandExecutionContext Context { get; set; }
    Task<CommandResult> Execute(string[] args, CancellationToken token);
    string GetHelpText();
  }
}