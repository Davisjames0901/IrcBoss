using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Worker.Attributes;
using Asperand.IrcBallistic.Worker.Classes;
using Asperand.IrcBallistic.Worker.Messages;

namespace Asperand.IrcBallistic.Worker.Commands
{
    [CommandGroup("echo", "Echos the input provided.")]
    public class Echo : BaseCommand
    {
        public override async Task<CommandExecutionResult> Execute(CommandRequest request, CancellationToken token)
        {
            await SendMessage(request.Content);
            return CommandExecutionResult.Success;
        }
    }
}