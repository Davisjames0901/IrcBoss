using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Core;
using Asperand.IrcBallistic.Core.Interfaces;
using Asperand.IrcBallistic.Core.Module;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Connections.Irc
{
    public class IrcConnection : ConnectionBase
    {
        public override string Name => $"IRC-{_config.Channel}-{_config.DefaultNickname}";

        private StreamWriter _writer;
        private StreamReader _reader;
        private TcpClient _tcpConnection;
        private string _identity;
        private readonly Thread _thread;
        private readonly IrcConfiguration _config;
        private readonly ILogger<IrcConnection> _log;

        private const string User = "USER IRCbot 0 * :IRCbot";

        public IrcConnection(IrcConfiguration config, IEnumerable<ModuleBase> modules, ILogger<IrcConnection> log)
            : base(modules, log)
        {
            _thread = new Thread(Listener);
            _config = config;
            _log = log;
        }

        public override void InternalStart()
        {
            _thread.Start();
        }

        private async void Listener()
        {
            _log.LogInformation("Starting IrcConnection");
            try
            {
                await InitConnection();
                while (IsRunning)
                {
                    string inputLine;
                    while ((inputLine = await _reader.ReadLineAsync()) != null)
                    {
                        Handle(new IrcRequest(inputLine));
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
                    var splitInput = currentLine.Split(' ');

                    if (currentLine.StartsWith($":{_config.DefaultNickname}!"))
                    {
                        _identity = currentLine.Split(' ')[0];
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

        public async Task WriteMessage(string message)
        {
            await _writer.WriteLineAsync($"{_identity} {message}");
            await _writer.FlushAsync();
        }


        public override Task WriteMessage(Response response)
        {
            var messageString = response.IsAction
                ? $"PRIVMSG {response.Target} :ACTION {response.Text}"
                : $"PRIVMSG {response.Target} :{response.Text}";
            return WriteMessage(messageString);
        }

        public async Task Whois(IEnumerable<string> usernames)
        {
            foreach (var username in usernames)
            {
                await _writer.WriteLineAsync($"{_identity} WHOIS {username}");
            }

            await _writer.FlushAsync();
        }

        public override void Dispose()
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