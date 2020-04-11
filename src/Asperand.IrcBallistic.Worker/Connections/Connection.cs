using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Worker.Classes;
using Asperand.IrcBallistic.Worker.Interfaces;
using Asperand.IrcBallistic.Worker.Messages;
using Asperand.IrcBallistic.Worker.Serialization;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Worker.Connections
{
    public abstract class Connection: IConnection 
    {
        private readonly ISerializer _serializer;
        private readonly List<Action<Request>> _callbacks;
        private readonly CommandEngine _commandEngine;
        private readonly CommandLocator _commandLocator;
        protected readonly UserContainer _userContainer;
        private readonly ILogger _log; 
        private readonly char _commandFlag;
        
        protected Connection(ISerializer serializer, 
            UserContainer userContainer, 
            ILogger log, 
            CommandEngine commandEngine,
            CommandLocator commandLocator,
            char commandFlag)
        {
            _serializer = serializer;
            _userContainer = userContainer;
            _commandFlag = commandFlag;
            _commandEngine = commandEngine;
            _commandLocator = commandLocator;
            _log = log;
            _callbacks = new List<Action<Request>>();
        }

        public bool IsOpen { get; protected set; }

        public Task SendMessage(Response response)
        {
            var serializedResponse = _serializer.Serialize(response);
            return WriteMessage(serializedResponse);
        }
        public void MessageReceived(string message)
        {
            var request = _serializer.Deserialize(message);
            _callbacks.ForEach(x=> x.Invoke(request));
            if (request.Text[0] != _commandFlag)
            {
                return;
            }

            var commandName = request.Text.Split(_commandFlag)?[1]?.Split(' ')?[0];
            if (string.IsNullOrWhiteSpace(commandName))
            {
                return;
            }
            var command = _commandLocator.LocateCommandGroup(commandName);
            if (command == null)
            {
                return;
            }

            var commandRequest = command.ValidateAndParse(request);
            var test = _commandEngine.StartCommand(command, commandRequest, this);
            SendMessage(new Response
            {
                Target = request.Target,
                Text = $"started pid {test} liek gud boi",
                IsAction = true
            });
        }
        
        public void RegisterCallback(Action<Request> callback)
        {
            _callbacks.Add(callback);
        }

        public abstract string Name { get; }
        public abstract void Start();
        public abstract Task Stop();
        public abstract Task WriteMessage(string message);
    }
}