using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Digman.Io.IrcBalistic.Abstracts;
using Digman.Io.IrcBalistic.Classes;
using Digman.Io.IrcBalistic.Connections;

namespace Digman.Io.IrcBalistic
{
  public class ConnectionManager
  {
    public Dictionary<Connection, Thread> Connections;
    public Dictionary<string, List<string>> Bindings;
    private readonly CommandEngine _commandEngine;


    private Dictionary<string, string> permissions = new Dictionary<string, string>
        {
            {"asperand","admin"},
            {"digman","admin"}
        };

    public ConnectionManager()
    {
      Connections = new Dictionary<Connection, Thread>();
      Bindings = new Dictionary<string, List<string>>();
      _commandEngine = new CommandEngine();
    }

    public void Start()
    {
      //Connections.Add(new CommandLineConnection(ProcessMessage, permissions), null);
      Connections.Add(new IrcConnection("DrDigBotTesting", "#davc", ProcessMessage, new ConnectionConfig
      {
        MessageFlag = '.',
        Permissions = new Dictionary<string, string>
        {
            {"asperand","admin"},
            {"digman","admin"}
        }
      }), null);

      foreach (var item in Connections)
      {
        new Thread(item.Key.Listener).Start();

      }
    }

    protected void ProcessMessage(Message message)
    {
      //var group = Bindings.SingleOrDefault(x => x.Value.Any(z => z == connection.Name.ToLower())).Value;
      ResponsePacket response = null;
      if (!string.IsNullOrWhiteSpace(message.Command))
      {
        response = _commandEngine.ExecuteCommand(message);
      }

      if (response?.Responses == null || !response.Responses.Any())
      {
        return;
      }

      message.SourceConnection.SendMessage(response);
    }
  }
}
