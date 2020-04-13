using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Worker.Classes;
using Asperand.IrcBallistic.Worker.Interfaces;
using Asperand.IrcBallistic.Worker.Messages;

namespace Asperand.IrcBallistic.Worker.Commands
{
    public abstract class BaseCommand : ICommand
    {
        public CommandExecutionContext Context { get; set; }
        public IConnection Connection => Context.SourceConnection;
        
        public abstract Task<CommandExecutionResult> Execute(CommandRequest request, CancellationToken token);
    }
}