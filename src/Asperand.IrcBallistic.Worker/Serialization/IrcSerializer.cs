using System.Linq;
using Asperand.IrcBallistic.Worker.Messages;

namespace Asperand.IrcBallistic.Worker.Serialization
{
  public class IrcSerializer : ISerializer
  {
    public Request Deserialize(string line)
    {
      var tokens = line.Split(':');
      var user = tokens[1].Split('!')[0];
      var content = string.Join(":",tokens.Where((x,y)=>y>1));
      var target = tokens[1].Trim().Split(' ').Last();
      
      var message = new Request
      {
        Text = content,
        SourceUserName = user.ToLower(),
        Target = target.ToLower()
      };
      return message;
    
    }

    public string Serialize(Response response)
    {
      if(response.IsAction)
      {
        return $"PRIVMSG {response.Target} :ACTION {response.Text}";
      }
      return $"PRIVMSG {response.Target} :{response.Text}";
    }
  }
}