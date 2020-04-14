using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Worker.Events;
using Asperand.IrcBallistic.Worker.Serialization;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Worker.Connections
{
    public abstract class Connection: IConnection 
    {
        private readonly ISerializer _serializer;
        private readonly Dictionary<EventType, List<(Action<IEvent> Handle, Guid Id)>> _callbacks;
        private readonly ILogger _log; 
        private readonly char _commandFlag;
        private readonly object _callbackLock = new object();
        
        protected Connection(ISerializer serializer,
            ILogger log, 
            char commandFlag)
        {
            _serializer = serializer;
            _commandFlag = commandFlag;
            _log = log;
            _callbacks = new Dictionary<EventType, List<(Action<IEvent>, Guid)>>();
        }

        public bool IsOpen { get; protected set; }

        public char MessageFlag => _commandFlag;

        public Task SendMessage(MessageResponse messageResponse)
        {
            var serializedResponse = _serializer.Serialize(messageResponse);
            return WriteMessage(serializedResponse);
        }
        protected void EventReceived(string message, EventType eventType)
        {
            var request = _serializer.Deserialize(message, eventType);
            lock (_callbackLock)
            {
                if(_callbacks.ContainsKey(eventType))
                    _callbacks[eventType]?.ForEach(x => new Thread(() => x.Handle.Invoke(request)).Start());
            }
        }
        
        public Guid RegisterCallback(EventType eventType, Action<IEvent> callback)
        {
            lock (_callbackLock)
            {
                var id = Guid.NewGuid();
                if (_callbacks.ContainsKey(eventType))
                {
                    _callbacks[eventType].Add((callback, id));
                    return id;
                }

                _callbacks.Add(eventType, new List<(Action<IEvent>, Guid)> {(callback, id)});
                _log.LogInformation($"Registered callback for type: {eventType} with an id: {id}");
                return id;
            }
        }

        public void RemoveCallback(Guid id)
        {
            lock (_callbackLock)
            {
                _log.LogInformation($"Removing callback with Id {id}");
                _callbacks.Values
                    .Single(g => g.Any(x => x.Id == id))
                    .RemoveAll(c => c.Id == id);
            }
        }

        public abstract string Name { get; }
        public abstract void Start();
        public abstract Task Stop();
        public abstract Task WriteMessage(string message);
    }
}