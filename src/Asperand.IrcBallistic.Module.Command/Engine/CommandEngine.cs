using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Core.Interfaces;
using Asperand.IrcBallistic.Module.Command.Data;
using Asperand.IrcBallistic.Module.Command.Enum;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Module.Command.Engine
{
    public class CommandEngine
    {
        private readonly ILogger<CommandEngine> _log;
        private readonly ConcurrentDictionary<int, (string Name, DateTime StartTime, Task Task, CancellationTokenSource TokenSource)> _processes;
        public CommandEngine(ILogger<CommandEngine> log)
        {
            _log = log;
            _processes = new ConcurrentDictionary<int, (string, DateTime, Task, CancellationTokenSource)>();
        }

        public int StartCommand(ICommand command, CommandRequest request, IConnection source)
        {
            command.Context = new CommandExecutionContext
            {
                Request = request,
                SourceConnection = source
            };
            
            var tokenSource = new CancellationTokenSource();
            var task = command.Execute(tokenSource.Token);
            ScheduleProcess(task, command, $"{request.RequesterUsername}:{source.Name}:{request.CommandName}", tokenSource);
            
            return task.Id;
        }

        private void ScheduleProcess(Task<CommandResult> task, ICommand command, string processName, CancellationTokenSource tokenSource)
        {
            _processes.TryAdd(task.Id, (processName, DateTime.Now, task, tokenSource));
            _log.LogInformation($"Started pid: {task.Id}, Name: {processName}");
            task.ContinueWith(x =>
            {
                _processes.TryRemove(x.Id, out _);
                command.RemoveCallbacks();
                
                if (x.IsCanceled)
                    _log.LogWarning($"Pid: {x.Id} was canceled.");
                
                else if (x.Result == CommandResult.Failed)
                    _log.LogError($"Pid: {x.Id} returned failed status. Name {processName}");
                
                else if (x.Result == CommandResult.Success)
                    _log.LogInformation($"Pid: {x.Id} completed successfully");
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
            _processes.TryRemove(pid, out _);
            return true;
        }
        
    }
}