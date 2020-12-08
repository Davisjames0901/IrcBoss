using System;
using System.Collections.Generic;
using System.Linq;
using Asperand.IrcBallistic.Connections.Irc;
using Asperand.IrcBallistic.Core.Interfaces;
using Asperand.IrcBallistic.Module.User.Events;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Module.User.Irc
{
    public class IrcSerializer : ISerializer
    {
        private readonly ILogger<IrcSerializer> _log;
        private readonly IrcConfiguration _config;

        public IrcSerializer(ILogger<IrcSerializer> log, IrcConfiguration config)
        {
            _log = log;
            _config = config;
        }
        
        public IEvent Deserialize(IRequest requestMessage)
        {
            var request = (IrcRequest) requestMessage;
            if (request.Action == "quit" || request.Action == "part" || request.Action == "join")
            {
                return HandleUserEvent(request);
            }
            if (SafeCompareIndex(request.LineTokens, 4, _config.Channel) && SafeCompareIndex(request.LineTokens, 5, ':' + _config.DefaultNickname))
            {
                return HandleUserDiscovery(request);
            }

            return null;
        }

        //Todo: this needs moved to the connection. Responses need to be uniform across all platforms so it doesnt make sense for this to be here...
        public string Serialize(IResponse messageResponse)
        {
            var response = (IrcResponse) messageResponse;
            return response.IsAction ? $"PRIVMSG {response.Target} :ACTION {response.Text}" : $"PRIVMSG {response.Target} :{response.Text}";
        }
        
        private UserEvent HandleUserEvent(IrcRequest request)
        {
            UserEventEnum? e = request.Action switch 
            {
                "join" => UserEventEnum.Joined,
                "part" => UserEventEnum.Leaving,
                "quit" => UserEventEnum.Leaving,
                _      => null
            };
            if (e != null)
                return new UserEvent
                {
                    Username = request.Line.Split(':')[1].Split('!')[0],
                    Event = e.Value
                };
            
            _log.LogError($"User event unaccounted for. {request.Line}");
            return null;

        }
        
        private static UserDiscovery HandleUserDiscovery(IrcRequest request)
        {
            return new()
            {
                Usernames = request.LineTokens.Skip(6).ToList()
            };
        }

        private static bool SafeCompareIndex(IReadOnlyList<string> array, int index, string value)
        {
            return array.Count > index && string.Equals(array[index], value, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}