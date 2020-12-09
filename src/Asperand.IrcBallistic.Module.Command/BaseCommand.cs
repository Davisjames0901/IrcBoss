using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Core;
using Asperand.IrcBallistic.Module.Command.Data;
using Asperand.IrcBallistic.Module.Command.Enum;
using Asperand.IrcBallistic.Module.Command.Interfaces;

namespace Asperand.IrcBallistic.Module.Command
{
    public abstract class BaseCommand : ICommand
    {
        public CommandExecutionContext Context { get; set; }

        public abstract Task<CommandResult> Execute(CancellationToken token);

        protected Task SendMessage(string message, bool isAction = false)
        {
            return Context.SourceConnection.WriteMessage(new Response
            {
                Target = Context.Request.Target,
                Text = message,
                IsAction = isAction
            });
        }
    }
}