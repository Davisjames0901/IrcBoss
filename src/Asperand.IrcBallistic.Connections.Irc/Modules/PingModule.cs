using System.Threading.Tasks;
using Asperand.IrcBallistic.Core.Interfaces;
using Asperand.IrcBallistic.Core.Module;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Connections.Irc.Modules
{
    public class PingModule: ModuleBase
    {
        public PingModule(ILogger<IModule> log) : base(log)
        { }
        
        public override bool IsEagerModule => true;
        public override int TimeoutSeconds => 10;
        
        protected override async Task<ModuleResult> Execute<T>(IRequest payload, T connection)
        {
            var irc = connection as IrcConnection;
            var request = (IrcRequest) payload;
            if (request.LineTokens[0] == "PING")
            {
                await irc.WriteMessage("PONG");
                return ModuleResult.Op;
            }

            return ModuleResult.Nop;
        }
    }
}