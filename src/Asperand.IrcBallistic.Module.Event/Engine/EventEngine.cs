// using System;
// using System.Collections.Generic;
// using Asperand.IrcBallistic.Core;
// using Asperand.IrcBallistic.Core.Events;
// using Asperand.IrcBallistic.Core.Interfaces;
//
// namespace Asperand.IrcBallistic.Module.Event.Engine
// {
//     public class EventEngine
//     {
//         private readonly IConnection _connection;
//         private readonly List<Guid> _events;
//
//         public EventEngine(IConnection connection)
//         {
//             _connection = connection;
//             _events = new();
//         }
//         
//         public void RegisterCallback(EventType eventType, Action<IEvent> e)
//         {
//             _events.Add(_connection.RegisterCallback(eventType, e));
//         }
//
//         public void RegisterMessageCallback(Action<MessageRequest> action)
//         {
//             RegisterCallback(EventType.Message, e => action.Invoke(e as MessageRequest));
//         }
//         
//         public void RegisterUserDiscoveryCallback(Action<UserDiscovery> action)
//         {
//             RegisterCallback(EventType.UserDiscovery, e => action.Invoke(e as UserDiscovery));
//         }
//
//         public void RemoveCallbacks()
//         {
//             foreach (var id in _events)
//             {
//                 _connection.RemoveCallback(id);
//             }
//             _events.Clear();
//         }
//     }
// }