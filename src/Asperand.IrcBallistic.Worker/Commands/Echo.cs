using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Module.Command;
using Asperand.IrcBallistic.Module.Command.Attributes;
using Asperand.IrcBallistic.Module.Command.Enum;

namespace Asperand.IrcBallistic.Worker.Commands
{
    [CommandGroup("echo", "Echos the input provided.")]
    public class Echo : BaseCommand<EchoOptions>
    {
        public override async Task<CommandResult> Execute(EchoOptions options, CancellationToken token)
        {
            await SendMessage(options.Content);
            return CommandResult.Success;
        }
    }

    public class EchoOptions
    {
        [Content]
        public string Content { get; set; }
    }
}