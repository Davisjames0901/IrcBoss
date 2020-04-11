using Asperand.IrcBallistic.Worker.Interfaces;

namespace Asperand.IrcBallistic.Worker.Classes
{
    public class CommandExecutionContext
    {
        public IConnection SourceConnection { get; set; }
        public CommandRequest Request { get; set; }
    }
}