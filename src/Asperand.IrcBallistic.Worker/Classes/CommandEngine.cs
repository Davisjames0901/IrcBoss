using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        private readonly ConcurrentDictionary<int, (string Name, DateTime StartTime, Task Task)> _processes;

        public CommandEngine(ILogger<CommandEngine> log)
        {
            _log = log;
            _processes = new ConcurrentDictionary<int, (string Name, DateTime StartTime, Task Task)>();
        }

        public int StartCommand(ICommand command, CommandRequest request, IConnection source)
        {
            command.Context = new CommandExecutionContext
            {
                Request = request,
                SourceConnection = source
            };
            
            var task = command.Execute(request);
            ScheduleProcess(task, $"{source.Name}:{request.CommandName}");
            
            return task.Id;
        }

        private void ScheduleProcess(Task<CommandExecutionResult> task, string processName)
        {
            _processes.TryAdd(task.Id, (processName, DateTime.Now, task));
            _log.LogInformation($"Started pid: {task.Id}, Name: {processName}");
            task.ContinueWith(x =>
            {
                _log.LogInformation($"Pid: {task.Id} finished.");
                if (x.Result == CommandExecutionResult.Failed)
                {
                    _log.LogError($"Pid: {task.Id} returned failed status. Name {processName}");
                }
                _processes.TryRemove(task.Id, out _);
            });
        }

        public (string Name, double RunMinutes) GetProcess(int pid)
        {
            var process = _processes[pid];
            return (process.Name, (DateTime.Now - process.StartTime).TotalMinutes);
        }

        public bool KillProcess(int pid)
        {
            if (!_processes.ContainsKey(pid))
                return false;
            
            //todo, time for cancellation tokens
            return true;
        }
        
    }
}