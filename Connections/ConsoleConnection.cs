using System;
using System.Collections.Generic;
using Digman.Io.IrcBalistic.Abstracts;
using Digman.Io.IrcBalistic.Classes;

namespace Digman.Io.IrcBalistic.Connections
{
    public class CommandLineConnection : Connection
    {
        public CommandLineConnection(Action<Message> callBack, ConnectionConfig config) : base(callBack, config)
        {
        }

        public override string Name => $"Cmd {Environment.MachineName}";

        public override void Listener()
        {
            while (Open)
            {
                //SendMessage(MessageRecieved(Console.ReadLine(), "DigMan"));
            }
        }

        public override void SendMessage(ResponsePacket message)
        {
            Console.WriteLine(message);
        }
    }
}
