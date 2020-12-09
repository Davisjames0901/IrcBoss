using System.Diagnostics;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Core.Interfaces;
using Asperand.IrcBallistic.Core.Module;
using Asperand.IrcBallistic.Module.Command.Data;
using Asperand.IrcBallistic.Module.Command.Engine;
using Asperand.IrcBallistic.Module.Command.Interfaces;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Module.Command
{
    public class CommandModule : ModuleBase
    {
        private readonly ILogger<CommandModule> _log;
        private readonly CommandEngine _commandEngine;
        private readonly CommandMetadataAccessor _commandAccessor;
        private readonly ISerializer _serializer;

        public override int TimeoutSeconds => 10;
        public override bool IsEagerModule => true;
        
        public CommandModule(
            ILogger<CommandModule> log,
            CommandEngine commandEngine,
            CommandMetadataAccessor commandAccessor,
            ISerializer serializer): base(log)
        {
            _log = log;
            _commandEngine = commandEngine;
            _commandAccessor = commandAccessor;
            _serializer = serializer;
        }

        protected override Task<ModuleResult> Execute<T>(IRequest payload, T connection)
        {
            var request = _serializer.Deserialize(payload);
            if(request is not null)
                return Task.FromResult(HandleMessage(request, connection));
            return Task.FromResult(ModuleResult.Nop);
        }

        private ModuleResult HandleMessage(CommandRequest request, IConnection connection)
        {
            var timer = Stopwatch.StartNew();
            
            _log.LogInformation("Received command request");
            var command = _commandAccessor.LocateCommandGroup(request.CommandName);
            _commandAccessor.PopulateCommand(command, request);
            if (command == null)
                return ModuleResult.Nop;

            var pid = _commandEngine.StartCommand(command, request, connection);
            _log.LogInformation($"Started process with pid of {pid}");
            timer.Stop();
            _log.LogWarning($"Took {timer.ElapsedMilliseconds}ms to find and execute command!");
            return ModuleResult.Op;
        }
    }
}