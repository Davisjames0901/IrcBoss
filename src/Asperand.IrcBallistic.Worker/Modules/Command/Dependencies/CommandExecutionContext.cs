using Asperand.IrcBallistic.Worker.Connections;

namespace Asperand.IrcBallistic.Worker.Modules.Command.Dependencies
{
    public class CommandExecutionContext
    {
        public IConnection SourceConnection { get; set; }
        public CommandRequest Request { get; set; }
    }
}