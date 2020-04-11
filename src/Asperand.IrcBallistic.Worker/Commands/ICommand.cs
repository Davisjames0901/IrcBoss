using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Worker.Classes;
using Asperand.IrcBallistic.Worker.Messages;

namespace Asperand.IrcBallistic.Worker.Commands
{
  public interface ICommand
  {
    CommandExecutionContext Context { get; set; }
    CommandRequest ValidateAndParse(Request commandRequest);
    Task<CommandExecutionResult> Execute(CommandRequest request);
  }
}