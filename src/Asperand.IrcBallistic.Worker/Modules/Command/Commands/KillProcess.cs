using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Worker.Attributes;
using Asperand.IrcBallistic.Worker.Classes;

namespace Asperand.IrcBallistic.Worker.Commands
{
    [CommandGroup("kill", "Kills a process")]
    public class KillProcess:BaseCommand
    {
        private readonly CommandEngine _commandEngine;
        public KillProcess(CommandEngine commandEngine)
        {
            _commandEngine = commandEngine;
        }

        [Flag("p", "The id of the process to kill")]
        public string Pid { get; set; }
        
        public override async Task<CommandResult> Execute(CancellationToken token)
        {
            var isNumber = int.TryParse(Pid, out var pid);
            if (!isNumber)
            {
                await SendMessage("You gotta give me an id boss... ya know, a number?");
                return CommandResult.Failed;
            }

            var result = _commandEngine.KillProcess(pid);
            if (!result)
            {
                await SendMessage("I couldn't kill that process, are you sure it exists?");
                return CommandResult.Failed;
            }
            await SendMessage("Done. (In cold mechanical tone)");
            return CommandResult.Success;
        }
    }
}