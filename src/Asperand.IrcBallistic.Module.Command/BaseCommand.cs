using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asperand.IrcBallistic.Core.Events;
using Asperand.IrcBallistic.Core.Interfaces;
using Asperand.IrcBallistic.Module.Command.Data;
using Asperand.IrcBallistic.Module.Command.Enum;

namespace Asperand.IrcBallistic.Module.Command
{
    public abstract class BaseCommand : ICommand
    {
        private List<Guid> _callbacks = new List<Guid>();
        public CommandExecutionContext Context { get; set; }
        public IConnection Connection => Context.SourceConnection;
        
        public abstract Task<CommandResult> Execute(CancellationToken token);

        protected Task SendMessage(string message, bool isAction = false)
        {
            return Connection.SendMessage(new MessageResponse
            {
                Target = Context.Request.Target,
                Text = message,
                IsAction = isAction
            });
        }

        protected void RegisterCallback(EventType eventType, Action<IEvent> e)
        {
            _callbacks.Add(Connection.RegisterCallback(eventType, e));
        }

        protected void RegisterMessageCallback(Action<MessageRequest> action)
        {
            RegisterCallback(EventType.Message, e => action.Invoke(e as MessageRequest));
        }
        
        protected void RegisterUserDiscoveryCallback(Action<UserDiscovery> action)
        {
            RegisterCallback(EventType.UserDiscovery, e => action.Invoke(e as UserDiscovery));
        }

        public void RemoveCallbacks()
        {
            foreach (var id in _callbacks)
            {
                Connection.RemoveCallback(id);
            }
            _callbacks.Clear();
        }
    }
}