using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Worker.Commands;
using Asperand.IrcBallistic.Worker.Interfaces;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Worker.Classes
{
    public class CommandEngine
    {
        private readonly ILogger<CommandEngine> _log;
        private readonly ConcurrentBag<(int Pid, string Name, DateTime StartTime, Task Task)> _processes;

        public CommandEngine(ILogger<CommandEngine> log)
        {
            _log = log;
            _processes = new ConcurrentBag<(int, string, DateTime, Task)>();
        }

        public int StartCommand(ICommand command, CommandRequest request, IConnection source)
        {
            command.Context = new CommandExecutionContext
            {
                Request = request,
                SourceConnection = source
            };
            
            var task = command.Execute(request);
            _processes.Add((task.Id, $"{source.Name}:{request.CommandName}", DateTime.Now, task));
            
            return task.Id;
        }

        public (int Pid, string Name, double RunMinutes) GetProcess(int pid)
        {
            return _processes
                .Where(p => p.Pid == pid)
                .Select(p => (p.Pid, p.Name, (DateTime.Now - p.StartTime).TotalMinutes))
                .SingleOrDefault();
        }
    }
}