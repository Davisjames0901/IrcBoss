using System.Linq;
using Asperand.IrcBallistic.Core.Events;
using Asperand.IrcBallistic.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Connections.Irc
{
  public class IrcSerializer : ISerializer
  {
    private readonly ILogger<IrcSerializer> _log;
    public IrcSerializer(ILogger<IrcSerializer> log)
    {
      _log = log;
    }
    
    public IEvent Deserialize(string line, EventType eventType)
    {
      return eventType switch
      {
        EventType.Message => HandleMessage(line),
        EventType.UserDiscovery => HandleUserDiscovery(line),
        EventType.UserEvent => HandleUserEvent(line),
        _ => null
      };
    }

    private MessageRequest HandleMessage(string line)
    {
      var tokens = line.Split(':');
      var user = tokens[1].Split('!')[0];
      var content = string.Join(":",tokens.Where((x,y)=>y>1));
      var target = tokens[1].Trim().Split(' ').Last();
      
      var message = new MessageRequest
      {
        Text = content,
        SourceUserName = user.ToLower(),
        Target = target.ToLower()
      };
      return message;
    }

    private UserDiscovery HandleUserDiscovery(string line)
    {
      return new UserDiscovery
      {
        Usernames = line.Split(' ').Skip(6).ToList()
      };
    }

    private UserEvent HandleUserEvent(string line)
    {
      UserEventEnum? e = line.Split(' ')[1].ToLower() switch 
      {
        "join" => UserEventEnum.Joined,
        "part" => UserEventEnum.Leaving,
        "quit" => UserEventEnum.Leaving,
        _      => null
      };
      if (e == null)
      {
        _log.LogError($"User event unaccounted for. {line}");
        return null;
      }
      
      return new UserEvent
      {
        Username = line.Split(':')[1].Split('!')[0],
        Event = e.Value
      };
    }
    

    public string Serialize(MessageResponse messageResponse)
    {
      if(messageResponse.IsAction)
      {
        return $"PRIVMSG {messageResponse.Target} :ACTION {messageResponse.Text}";
      }
      return $"PRIVMSG {messageResponse.Target} :{messageResponse.Text}";
    }
  }
}