using System;
using System.Collections.Generic;
using Digman.Io.IrcBalistic.Abstracts;

namespace Digman.Io.IrcBalistic.Connections
{
    public class CommandLineConnection : Connection
    {
        public CommandLineConnection(Action<string, string, Connection> callBack, Dictionary<string, string> permissions) : base(callBack, permissions)
        {
            MessageFlag = '?';
        }

        public override string Name => $"Cmd {Environment.MachineName}";

        public override void Listener()
        {
            while (Open)
            {
                SendMessage(MessageRecieved(Console.ReadLine(), "DigMan"));
            }
        }

        public override void SendMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
