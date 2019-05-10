using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;

namespace Digman.Io.IrcBalistic.Abstracts
{
    public abstract class Connection
    {
        public List<CommandSet> Commands;
        public Dictionary<string, string> Users;
        public List<string> IgnoreUsers;
        public Dictionary<string, string> UsersLastMessage;
        public bool Open;
        public abstract string Name { get; }

        protected Action<string, string, Connection> CallBack;

        protected char? MessageFlag { get; set; }

        public Connection(Action<string, string, Connection> callBack, Dictionary<string, string> permissions, char? messageFlag = null)
        {
            CallBack = callBack;
            MessageFlag = messageFlag;
            Open = true;
            Commands = new List<CommandSet>();
            IgnoreUsers = new List<string>();
            Users = permissions;
            UsersLastMessage = new Dictionary<string, string>();
        }

        public abstract void SendMessage(string message);

        protected string MessageRecieved(string message, string userName)
        {
            if(IgnoreUsers.Contains(userName.ToLower()))
            {
                return null;
            }
            if (!string.IsNullOrWhiteSpace(message))
            {
                if (MessageFlag == null || message[0] == MessageFlag)
                {
                    if (message[0] == MessageFlag)
                    {
                        message = message.Substring(1, message.Length - 1);
                        return PrefixCommand(message, userName);
                    }
                }
                var output = PatternCommand(message, userName);
                if(!string.IsNullOrWhiteSpace(output))
                {
                    return output;
                }
                CallBack.Invoke(message, userName, this);
            }
            if(UsersLastMessage.ContainsKey(userName.ToLower()))
            {
                UsersLastMessage[userName.ToLower()] = message;
            }
            else{
                UsersLastMessage.Add(userName.ToLower(), message);
            }
            return null;
        }

        public abstract void Listener();
        protected virtual string NotRecognized { get => "Command not recognised."; }

        public string PatternCommand(string message, string user)
        {
            foreach (var commandSet in Commands)
            {
                foreach(var command in commandSet.PatternCommands)
                {
                    var reg = new Regex(command.Key, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                    if(reg.IsMatch(message))
                    {
                        return command.Value.CallBack(message, user, this);
                    }
                }
            }
            return null;
        }

        public string PrefixCommand(string message, string user)
        {
            var tokens = message.Split(' ');
            var command = tokens[0];
            var argList = tokens.ToList();
            argList.Remove(command);
            var args = string.Join(" ", argList);
            return ExecPrefixCommand(command, user, args);
        }

        public virtual string ExecPrefixCommand(string commandString, string user, string args)
        {
            if (Commands.Any(x => x.PrefixCommands.ContainsKey(commandString.Split(' ')[0])))
                try
                {
                    var command = GetCommand(commandString);
                    if (command.HasPermission(GetRole(user)))
                    {
                        return command.CallBack.Invoke(args, user, this);
                    }
                    else
                    {
                        return $"?mock {user}";
                    }
                }
                catch (Exception)
                {
                    return $"Ambiguous command. Please let {string.Join(" or ",Users.Where(x=>x.Value == "admin"))} know.";
                }
            else
                return NotRecognized;
        }

        public Command GetCommand(string name)
        {
            return Commands.SingleOrDefault(x => x.PrefixCommands.ContainsKey(name)).PrefixCommands[name];
        }

        public string GetRole(string user)
        {
            return Users.ContainsKey(user.ToLower()) ? Users[user.ToLower()] : null;
        }
    }
}
