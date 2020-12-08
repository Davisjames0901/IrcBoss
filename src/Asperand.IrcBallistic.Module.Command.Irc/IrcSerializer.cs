using System.Linq;
using Asperand.IrcBallistic.Connections.Irc;
using Asperand.IrcBallistic.Core.Interfaces;
using Asperand.IrcBallistic.Module.Command.Data;
using Asperand.IrcBallistic.Module.Command.Interfaces;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Module.Command.Irc
{
    public class IrcSerializer: ISerializer
    {
        private readonly ILogger<IrcSerializer> _log;
        private readonly CommandConfig _config;

        public IrcSerializer(ILogger<IrcSerializer> log, CommandConfig config)
        {
            _log = log;
            _config = config;
        }

        public CommandRequest Deserialize(IRequest messageRequest)
        {
            var request = (IrcRequest) messageRequest;
            if (request.Action != "privmsg" || !request.Content.StartsWith(_config.MessageFlag))
                return null;
            var contentTokens = request.Content.Split(' ');
            var message = new CommandRequest
            {
                Content = string.Join(" ",contentTokens.Skip(1)),
                Raw = request.Line,
                RequesterUsername = request.Username,
                Target = request.Target,
                CommandName = string.Join("", contentTokens[0].Skip(1))
            };
            return message;
        }

        //todo: move this to the connection!
        public string Serialize(IResponse messageResponse)
        {
            var response = (IrcResponse) messageResponse;
            return response.IsAction ? $"PRIVMSG {response.Target} :ACTION {response.Text}" : $"PRIVMSG {response.Target} :{response.Text}";
        }
    }
}