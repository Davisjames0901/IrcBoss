using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Digman.Io.IrcBalistic.Abstracts;
using Digman.Io.IrcBalistic.CommandSets;
using Digman.Io.IrcBalistic.Connections;

namespace Digman.Io.IrcBalistic
{
    public class ConnectionManager
    {
        public Dictionary<Connection, Thread> Connections;
        public Dictionary<string, List<string>> Bindings;

        private Dictionary<string, string> permissions = new Dictionary<string, string>
        {
            {"asperand","admin"},
            {"digman","admin"}
        };

        public ConnectionManager()
        {
            Connections = new Dictionary<Connection, Thread>();
            Bindings = new Dictionary<string, List<string>>();
        }

        public void Start()
        {
            Connections.Add(new CommandLineConnection(ProcessMessage, permissions), null);
            Connections.Add(new IrcConnection("DrDigBotTesting", "#davc", ProcessMessage, permissions), null);
            foreach (var item in Connections)
            {
                new Thread(item.Key.Listener).Start();
                item.Key.Commands.Add(new Default(this));
                item.Key.Commands.Add(new Patterns());
                item.Key.Commands.Add(new PhraseCommands());

            }
        }

        protected void ProcessMessage(string message, string userName, Connection connection)
        {
            var group = Bindings.SingleOrDefault(x => x.Value.Any(z => z == connection.Name.ToLower())).Value;
            if(group == null)
            {
                return;
            }

            foreach (var item in group)
            {
                if(item != connection.Name.ToLower())
                {
                    Connections.Single(x => x.Key.Name.ToLower() == item.ToLower()).Key.SendMessage($"<{userName}> {message}");
                }
            }
        }
    }
}
