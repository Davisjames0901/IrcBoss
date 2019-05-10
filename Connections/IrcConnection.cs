using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Digman.Io.IrcBalistic.Abstracts;

namespace Digman.Io.IrcBalistic.Connections
{
    public class IrcConnection : Connection
    {
        public string Nickname => _nickname;
        public string Channel => _channel;
        private string _nickname;
        private string _channel;
        private string _ident;
        private StreamWriter _writer;
        private static string USER = "USER IRCbot 0 * :IRCbot";

        public override string Name => $"IRC-{_channel}-{_nickname}";

        public IrcConnection(string nickname, string channel, Action<string, string, Connection> callBack, Dictionary<string, string> permissions) : base(callBack, permissions)
        {
            _nickname = nickname;
            _channel = channel;
            MessageFlag = '.';
        }
        public override void SendMessage(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                _writer.WriteLine($"{_ident} PRIVMSG {_channel} :{message}");
                _writer.Flush();
            }
        }

        public void CloseConnection(Thread thread)
        {
            _writer.WriteLine($"{_ident} QUIT :I'll Show myself out. ");
            _writer.Flush();
            thread.Join();
        }

        public override void Listener()
        {
            NetworkStream stream;
            TcpClient irc;
            string inputLine;
            StreamReader reader;

            try
            {
                irc = new TcpClient("irc.freenode.net", 6667);
                stream = irc.GetStream();
                reader = new StreamReader(stream);
                _writer = new StreamWriter(stream);
                _writer.WriteLine("NICK " + _nickname);
                _writer.Flush();
                _writer.WriteLine(USER);
                _writer.Flush();

                while (Open)
                {
                    while ((inputLine = reader.ReadLine()) != null)
                    {
                        Console.WriteLine(inputLine);

                        string[] splitInput = inputLine.Split(' ');

                        if (splitInput[0] == "PING")
                        {
                            _writer.WriteLine($"PONG {splitInput[1]}");
                            _writer.Flush();
                        }
                        if (inputLine.StartsWith($":{_nickname}!"))
                        {
                            _ident = inputLine.Split(' ')[0];
                        }
                        switch (splitInput[1])
                        {
                            case "001":
                                _writer.WriteLine("JOIN " + _channel);
                                _writer.Flush();
                                break;
                            default:
                                if (inputLine.ToLower().Contains($"PRIVMSG {_channel}".ToLower()))
                                {
                                    var message = ProcessMessage(inputLine);
                                    var response = MessageRecieved(message.Item2, message.Item1);
                                    if(string.IsNullOrWhiteSpace(response))
                                    {
                                        break;
                                    }

                                    if(response.StartsWith("/me")) 
                                    {
                                        response = $"ACTION {response.Substring(3, response.Length-3).Trim()}";
                                    }
                                    SendMessage(response);
                                }
                                break;
                        }
                    }

                    _writer.Close();
                    reader.Close();
                    irc.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Thread.Sleep(5000);
                if (Open)
                {
                    Listener();
                }
            }
        }

        private Tuple<string, string> ProcessMessage(string args)
        {
            var tokens = args.Split(':');
            var user = tokens[1].Split('!')[0];
            var message = tokens.ToList().Last();
            return new Tuple<string, string>(user, message);
        }
    }
}
