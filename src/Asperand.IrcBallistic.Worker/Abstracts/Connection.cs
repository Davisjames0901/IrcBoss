// using System;
// using System.Collections.Generic;
// using Asperand.IrcBallistic.Worker.Classes;
// using Asperand.IrcBallistic.Worker.Messages;
//
// namespace Asperand.IrcBallistic.Worker.Abstracts
// {
//     public abstract class Connection
//     {
//         public List<CommandSet> Commands;
//         public Dictionary<string, string> Permissions;
//         public List<User> IgnoreUsers;
//         public Dictionary<string, string> UsersLastMessage;
//         public bool Open;
//         public abstract string Name { get; }
//
//         protected Action<Request> CallBack;
//
//         public char MessageFlag { get; set; }
//
//         public Connection(Action<Request> callBack, ConnectionConfig config)
//         {
//             CallBack = callBack;
//             MessageFlag = config.MessageFlag;
//             Open = true;
//             Commands = new List<CommandSet>();
//             IgnoreUsers = new List<User>();
//             Permissions = config.Permissions;
//             UsersLastMessage = new Dictionary<string, string>();
//         }
//
//         public abstract void SendMessage(ResponsePacket message);
//
//         public abstract void Listener();
//
//         protected virtual string NotRecognized
//         {
//             get => "Command not recognised.";
//         }
//
//         // public string PatternCommand(string message, string user)
//         // {
//         //     foreach (var commandSet in Commands)
//         //     {
//         //         foreach(var command in commandSet.PatternCommands)
//         //         {
//         //             var reg = new Regex(command.Key, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
//         //             if(reg.IsMatch(message))
//         //             {
//         //                 return command.Value.CallBack(message, user, this);
//         //             }
//         //         }
//         //     }
//         //     return null;
//         // }
//
//         // public Command GetCommand(string name)
//         // {
//         //     return Commands.SingleOrDefault(x => x.PrefixCommands.ContainsKey(name)).PrefixCommands[name];
//         // }
//
//         public string GetRole(User user)
//         {
//             //todo we need to make a call to verify identity here.
//             return Permissions.ContainsKey(user.Name.ToLower()) ? Permissions[user.Name.ToLower()] : null;
//         }
//     }
// }