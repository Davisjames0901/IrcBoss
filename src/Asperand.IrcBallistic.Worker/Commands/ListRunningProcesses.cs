using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Module.Command;
using Asperand.IrcBallistic.Module.Command.Attributes;
using Asperand.IrcBallistic.Module.Command.Engine;
using Asperand.IrcBallistic.Module.Command.Enum;

namespace Asperand.IrcBallistic.Worker.Commands
{
    [CommandGroup("lsproc", "Lists all running processes")]
    public class ListRunningProcesses:BaseCommand
    {
        private readonly CommandEngine _commandEngine;
        public ListRunningProcesses(CommandEngine commandEngine)
        {
            _commandEngine = commandEngine;
        }
        public override async Task<CommandResult> Execute(CancellationToken token)
        {
            var processes = _commandEngine.GetRunningProcesses().ToList();
            foreach (var p in processes)
            {
                await SendMessage($"Id: {p.Pid}; Name: {p.Name}; Run Minutes: {p.RunMinutes}");
            }

            if (processes.Count == 0)
            {
                await SendMessage($"Im not doing anything :)");
            }

            return CommandResult.Success;
        }
    }
}