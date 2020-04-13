using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Worker.Attributes;
using Asperand.IrcBallistic.Worker.Classes;
using Asperand.IrcBallistic.Worker.Messages;

namespace Asperand.IrcBallistic.Worker.Commands
{
    [CommandGroup("test")]
    public class Test : BaseCommand
    {
        public Test()
        {
            
        }
        public override async Task<CommandExecutionResult> Execute(CommandRequest request, CancellationToken token)
        {
            var times = request.Flags["t"];
            for (var i = 0; i < int.Parse(times); i++)
            {
                await Connection.SendMessage(new MessageResponse
                {
                    Target = "#davc",
                    Text = $"Iteration #{i}"
                });
            }

            return CommandExecutionResult.Success;
        }
    }
}