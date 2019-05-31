using System;
using System.Linq;
using Digman.Io.IrcBalistic.Abstracts;
using Digman.Io.IrcBalistic.Classes;
using Digman.Io.IrcBalistic.Connections;

namespace Digman.Io.IrcBalistic.Serialization
{
  public class IrcSerializer : ISerializer
  {
    private IrcConnection _connection;
    public IrcSerializer(IrcConnection ircConnection)
    {
      _connection = ircConnection;
    }

    public Message DeserializeMessage(string message)
    {
      var deserializedMessage = new Message();
      var messageTokens = message.Split(' ');
      if (HandlePing(messageTokens))
      {
        return null;
      }
      switch(messageTokens[1].ToLower())
      {
        case "privmsg":
          return ProcessPrivateMessage(message);
        default:
          break;
      }
      

      return deserializedMessage;
    }

    public string SerializeResponse(Response response)
    {
      if(response.IsAction)
      {
        return $"{_connection._ident} PRIVMSG {response.Target} :ACTION {response.Text}";
      }
      return $"{_connection._ident} PRIVMSG {response.Target} :{response.Text}";
    }

    private bool HandlePing(string[] messageTokens)
    {
      if (messageTokens[0] == "PING")
      {
        _connection.WriteMessage($"PONG {messageTokens[1]}");
        return true;
      }
      return false;
    }



    private Message ProcessPrivateMessage(string args)
    {
      var tokens = args.Split(':');
      var user = tokens[1].Split('!')[0];
      var content = string.Join(":",tokens.Where((x,y)=>y>1));
      var target = tokens[1].Trim().Split(' ').Last();
      
      var message = new Message
      {
        Text = content.Remove(0, 1),
        SourceUser = _connection.GetUserByName(user),
        Target = target.ToLower() == _connection.Nickname.ToLower()? user : target,
        SourceConnection = _connection,
      };
      if(content.StartsWith(_connection.MessageFlag.ToString()))
      {
        message.CommandGroup = content.Split(' ')[0].Split(_connection.MessageFlag)[1];
      }
      return message;
    }


  }
}