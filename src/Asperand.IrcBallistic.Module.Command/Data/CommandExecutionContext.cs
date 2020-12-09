using Asperand.IrcBallistic.Core.Interfaces;

namespace Asperand.IrcBallistic.Module.Command.Data
{
    public class CommandExecutionContext
    {
        public IConnection SourceConnection { get; set; }
        public CommandRequest Request { get; set; }
    }
}