using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Asperand.IrcBallistic.Worker.Modules.UserManagement.Dependencies
{
    public class UserContainer
    {
        public UserContainer()
        {
            Users = new ConcurrentDictionary<string, User>(); 
        }
        public ConcurrentDictionary<string, User> Users { get; set; }

        public string GetLastMessageByName(string name)
        {
            return GetUserByName(name)?.LastMessage;
        }

        public User GetUserByName(string name)
        {
            var loweredName = name.ToLower();
            return Users.ContainsKey(loweredName) ? Users[loweredName] : null;
        }
    }
}