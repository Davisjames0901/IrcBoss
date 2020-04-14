using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Worker.Configuration;
using Asperand.IrcBallistic.Worker.Events;
using Asperand.IrcBallistic.Worker.Serialization;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Worker.Connections
{
    public class IrcConnection : Connection
    {
        //Interface props
        public override string Name => $"IRC-{_config.Channel}-{_config.DefaultNickname}";

        //public connection specific props
        public string Identity;

        //private props
        private StreamWriter _writer;
        private StreamReader _reader;
        private TcpClient _tcpConnection;
        private readonly Thread _thread;
        private readonly IrcConfiguration _config;
        private readonly ILogger<IrcConnection> _log;

        private const string User = "USER IRCbot 0 * :IRCbot";

        public IrcConnection(IrcConfiguration config,
            IrcSerializer serializer,
            ILogger<IrcConnection> log)
            : base(
                serializer, log, config.MessageFlag)
        {
            _thread = new Thread(Listener);
            _config = config;
            _log = log;
        }

        public override void Start()
        {
            if (!IsOpen)
            {
                _thread.Start();
            }
        }

        private async void Listener()
        {
            _log.LogInformation("Starting IrcConnection");
            try
            {
                await InitConnection();
                while (IsOpen)
                {
                    string inputLine;
                    while ((inputLine = await _reader.ReadLineAsync()) != null)
                    {
                        await HandleLine(inputLine);
                    }
                }
            }
            catch (Exception e)
            {
                _log.LogError(e, "Error occured maintaining IRC connection");
            }
        }

        private async Task InitConnection()
        {
            _tcpConnection = new TcpClient(_config.ServerHostName, _config.ServerPort);
            IsOpen = true;
            var stream = _tcpConnection.GetStream();
            _reader = new StreamReader(stream);
            _writer = new StreamWriter(stream);
            await WriteMessage($"NICK {_config.DefaultNickname}");
            await WriteMessage(User);

            var identFound = false;
            var channelJoined = false;
            while (!identFound && !channelJoined)
            {
                string currentLine;
                while ((currentLine = _reader.ReadLine()) != null && !(identFound && channelJoined))
                {
                    _log.LogDebug(currentLine);
                    string[] splitInput = currentLine.Split(' ');

                    if (currentLine.StartsWith($":{_config.DefaultNickname}!"))
                    {
                        Identity = currentLine.Split(' ')[0];
                        identFound = true;
                    }
                    else if (splitInput[1] == "001")
                    {
                        await WriteMessage($"JOIN {_config.Channel}");
                        channelJoined = true;
                    }
                }
            }
        }

        private async Task HandleLine(string line)
        {
            var lineTokens = line.Split(' ');
            if (lineTokens[0] == "PING")
            {
                await WriteMessage($"PONG {lineTokens[1]}");
            }
            else if (string.Equals(lineTokens[1], "privmsg", StringComparison.CurrentCultureIgnoreCase))
            {
                EventReceived(line, EventType.Message);
            }
            else if (string.Equals(lineTokens[1], "quit", StringComparison.CurrentCultureIgnoreCase)
            || string.Equals(lineTokens[1], "part", StringComparison.CurrentCultureIgnoreCase))
            {
                EventReceived(line, EventType.Quit);
            }
            else if (string.Equals(lineTokens[1], "join", StringComparison.CurrentCultureIgnoreCase))
            {
                EventReceived(line, EventType.Join);
            }
            else if (string.Equals(lineTokens[4], _config.Channel, StringComparison.InvariantCultureIgnoreCase)
                     && string.Equals(lineTokens[5], ':' + _config.DefaultNickname))
            {
                EventReceived(line, EventType.UserDiscovery);
                //await Whois(lineTokens.Skip(6));
            }

            //todo handle the whois that comes back and update the users
        }

        public override async Task WriteMessage(string message)
        {
            await _writer.WriteLineAsync($"{Identity} {message}");
            await _writer.FlushAsync();
        }

        public async Task Whois(IEnumerable<string> usernames)
        {
            foreach (var username in usernames)
            {
                await _writer.WriteLineAsync($"{Identity} WHOIS {username}");
            }

            await _writer.FlushAsync();
        }

        public override async Task Stop()
        {
            await WriteMessage($"{Identity} QUIT :I'll Show myself out. ");
            DisposeConnection();
            _thread.Join();
        }

        private void DisposeConnection()
        {
            _writer.Close();
            _writer.Dispose();
            _reader.Close();
            _reader.Dispose();
            _tcpConnection.Close();
            _tcpConnection.Dispose();
        }
    }
}