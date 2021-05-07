using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Module.Command;
using Asperand.IrcBallistic.Module.Command.Attributes;
using Asperand.IrcBallistic.Module.Command.Engine;
using Asperand.IrcBallistic.Module.Command.Enum;
using CommandLine;

namespace Asperand.IrcBallistic.Worker.Commands
{
    [CommandGroup("kill", "Kills a process")]
    public class KillProcess : BaseCommand<KillProcessOptions>
    {
        private readonly CommandEngine _commandEngine;
        public KillProcess(CommandEngine commandEngine)
        {
            _commandEngine = commandEngine;
        }
        
        public override async Task<CommandResult> Execute(KillProcessOptions options, CancellationToken token)
        {
            var result = _commandEngine.KillProcess(options.Pid);
            if (!result)
            {
                await SendMessage("I couldn't kill that process, are you sure it exists?");
                return CommandResult.Failed;
            }
            await SendMessage("Done. (In cold mechanical tone)");
            return CommandResult.Success;
        }
    }

    public class KillProcessOptions
    {
        [Option()]
        public int Pid { get; set; }
    }
}