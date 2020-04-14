using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Worker.Attributes;
using Asperand.IrcBallistic.Worker.Modules.Command.Dependencies;

namespace Asperand.IrcBallistic.Worker.Modules.Command.Commands
{
    [CommandGroup("echo", "Echos the input provided.")]
    public class Echo : BaseCommand
    {
        [Content]
        public string Content { get; set; }
        public override async Task<CommandResult> Execute(CancellationToken token)
        {
            await SendMessage(Content);
            return CommandResult.Success;
        }
    }
}