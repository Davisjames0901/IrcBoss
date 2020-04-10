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
        private readonly ILogger _log; 
        private readonly char _commandFlag;
        protected readonly UserContainer _userContainer;
        
        protected Connection(ISerializer serializer, UserContainer userContainer, ILogger log, char commandFlag)
        {
            _serializer = serializer;
            _userContainer = userContainer;
            _commandFlag = commandFlag;
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