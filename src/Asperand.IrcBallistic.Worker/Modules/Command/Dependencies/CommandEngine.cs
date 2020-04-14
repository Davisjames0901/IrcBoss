using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Worker.Commands;
using Asperand.IrcBallistic.Worker.Interfaces;
using Asperand.IrcBallistic.Worker.Modules.Command.Dependencies;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Worker.Classes
{
    public class CommandEngine
    {
        private readonly ILogger<CommandEngine> _log;
        private readonly ConcurrentDictionary<int, (string Name, DateTime StartTime, Task Task, CancellationTokenSource TokenSource)> _processes;
        private readonly CommandMetadataAccessor _commandMetadataAccessor;
        public CommandEngine(ILogger<CommandEngine> log, CommandMetadataAccessor commandMetadataAccessor)
        {
            _log = log;
            _processes = new ConcurrentDictionary<int, (string, DateTime, Task, CancellationTokenSource)>();
            _commandMetadataAccessor = commandMetadataAccessor;
        }

        public int StartCommand(ICommand command, CommandRequest request, IConnection source)
        {
            command.Context = new CommandExecutionContext
            {
                Request = request,
                SourceConnection = source
            };
            _commandMetadataAccessor.PopulateCommand(command, request);
            
            var tokenSource = new CancellationTokenSource();
            var task = command.Execute(request, tokenSource.Token);
            ScheduleProcess(task, command, $"{request.RequesterUsername}:{source.Name}:{request.CommandName}", tokenSource);
            
            return task.Id;
        }

        private void ScheduleProcess(Task<CommandExecutionResult> task, ICommand command, string processName, CancellationTokenSource tokenSource)
        {
            _processes.TryAdd(task.Id, (processName, DateTime.Now, task, tokenSource));
            _log.LogInformation($"Started pid: {task.Id}, Name: {processName}");
            task.ContinueWith(x =>
            {
                _log.LogInformation($"Pid: {task.Id} finished.");
                if (x.Result == CommandExecutionResult.Failed)
                {
                    _log.LogError($"Pid: {task.Id} returned failed status. Name {processName}");
                }
                _processes.TryRemove(task.Id, out _);
                command.RemoveCallbacks();
            });
        }

        public IEnumerable<(int Pid, string Name, double RunMinutes)> GetRunningProcesses()
        {
            return _processes
                .Select(p => (p.Key, p.Value.Name, (DateTime.Now - p.Value.StartTime).TotalMinutes));
        }

        public bool KillProcess(int pid)
        {
            if (!_processes.ContainsKey(pid))
                return false;
            
            _processes[pid].TokenSource.Cancel();
            return true;
        }
        
    }
}