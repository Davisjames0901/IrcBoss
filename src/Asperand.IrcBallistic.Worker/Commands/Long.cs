using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Module.Command;
using Asperand.IrcBallistic.Module.Command.Attributes;
using Asperand.IrcBallistic.Module.Command.Enum;

namespace Asperand.IrcBallistic.Worker.Commands
{
    [CommandGroup("long", "Echos the input provided.")]
    public class Long : BaseCommand
    {
        [Content]
        public string Content { get; set; }
        public override async Task<CommandResult> Execute(CancellationToken token)
        {
            Thread.Sleep(15000);
            await SendMessage(Content);
            return CommandResult.Success;
        }
    }
}