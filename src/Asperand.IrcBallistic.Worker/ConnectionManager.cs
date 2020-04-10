using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Worker.Classes;
using Asperand.IrcBallistic.Worker.Connections;
using Asperand.IrcBallistic.Worker.Interfaces;
using Asperand.IrcBallistic.Worker.Messages;

namespace Asperand.IrcBallistic.Worker
{
  public class ConnectionManager
  {
    private readonly CommandEngine _commandEngine;
    private readonly IEnumerable<IConnection> _connections; 


    public ConnectionManager(CommandEngine commandEngine, IEnumerable<IConnection> connections)
    {
      _commandEngine = commandEngine;
      _connections = connections;
    }

    public void Start()
    {
      foreach (var item in _connections)
      {
        item.Start();
      }
    }

    public async Task Stop()
    {
      foreach (var item in _connections)
      {
        await item.Stop();
      }
    }

    protected void ProcessMessage(Request message)
    {
      //var group = Bindings.SingleOrDefault(x => x.Value.Any(z => z == connection.Name.ToLower())).Value;
      ResponsePacket response = null;
      // if (!string.IsNullOrWhiteSpace(message.Command))
      // {
      //   response = _commandEngine.ExecuteCommand(message);
      // }

      if (response?.Responses == null || !response.Responses.Any())
      {
        return;
      }

      //message.SourceConnection.SendMessage(response);
    }
  }
}
