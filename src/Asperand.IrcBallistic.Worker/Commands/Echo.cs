using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Module.Command;
using Asperand.IrcBallistic.Module.Command.Attributes;
using Asperand.IrcBallistic.Module.Command.Enum;

namespace Asperand.IrcBallistic.Worker.Commands
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
//localhost/mytest/get
//   My Test Get -a 
public class MyTestController //:Controller
{
    public void Index()
    {
        
    }

    public void Get()
    {
        
    }
} 