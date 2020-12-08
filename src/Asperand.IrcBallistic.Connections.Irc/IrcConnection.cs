using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Connections.Irc
{
    public class IrcConnection : IConnection
    {
        //Interface props
        public string Name => $"IRC-{_config.Channel}-{_config.DefaultNickname}";

        //private props
        private StreamWriter _writer;
        private StreamReader _reader;
        private TcpClient _tcpConnection;
        private bool _isRunning;
        private string _identity;
        private readonly Thread _thread;
        private readonly IrcConfiguration _config;
        private readonly IEnumerable<IModule> _modules;
        private readonly ILogger<IrcConnection> _log;

        private const string User = "USER IRCbot 0 * :IRCbot";

        public IrcConnection(IrcConfiguration config, IEnumerable<IModule> modules, ILogger<IrcConnection> log)
        {
            _thread = new Thread(Listener);
            _config = config;
            _modules = modules;
            _log = log;
        }

        public void Start(IEnumerable<IModule> modules)
        {
            if (_isRunning) return;
            _thread.Start();
            _isRunning = true;
        }

        private async void Listener()
        {
            _log.LogInformation("Starting IrcConnection");
            try
            {
                await InitConnection();
                while (_isRunning)
                {
                    string inputLine;
                    while ((inputLine = await _reader.ReadLineAsync()) != null)
                    {
                        var request = new IrcRequest(inputLine);
                        foreach (var module in _modules)
                        {
                            await module.Handle(request, this);
                        }
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

        public async Task Whois(IEnumerable<string> usernames)
        {
            foreach (var username in usernames)
            {
                await _writer.WriteLineAsync($"{_identity} WHOIS {username}");
            }

            await _writer.FlushAsync();
        }

        public async Task Stop()
        {
            await WriteMessage($"{_identity} QUIT :I'll Show myself out. ");
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