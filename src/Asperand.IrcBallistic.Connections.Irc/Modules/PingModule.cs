using System.Threading.Tasks;
using Asperand.IrcBallistic.Core.Interfaces;

namespace Asperand.IrcBallistic.Connections.Irc.Modules
{
    public class PingModule: IModule
    {
        public bool IsEagerModule => true;
        
        public Task Handle<T>(IRequest requestMessage, T connection) where T : IConnection
        {
            var request = (IrcRequest) requestMessage;
            return request.LineTokens[0] == "PING" ? connection.WriteMessage("PONG") : Task.CompletedTask;
        }
    }
}