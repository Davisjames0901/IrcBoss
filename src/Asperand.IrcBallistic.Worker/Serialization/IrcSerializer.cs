using System.Linq;
using Asperand.IrcBallistic.Worker.Messages;

namespace Asperand.IrcBallistic.Worker.Serialization
{
  public class IrcSerializer : ISerializer
  {
    public IEvent Deserialize(string line, EventType eventType)
    {
      switch (eventType)
      {
        case EventType.Message:
          return HandleMessage(line);
        
        case EventType.UserDiscovery:
          return HandleUserDiscovery(line);
      }

      return null;
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