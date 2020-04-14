using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Worker.Attributes;
using Asperand.IrcBallistic.Worker.Classes;

namespace Asperand.IrcBallistic.Worker.Commands
{
    [CommandGroup("test")]
    public class Test : BaseCommand
    {
        public override async Task<CommandResult> Execute(CommandRequest request, CancellationToken token)
        {
            var times = request.Flags["t"];
            for (var i = 0; i < int.Parse(times); i++)
            {
                await SendMessage($"Iteration #{i}");
            }

            return CommandResult.Success;
        }
    }
}