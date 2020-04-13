using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Asperand.IrcBallistic.Worker.Classes
{
    public class UserContainer
    {
        public UserContainer()
        {
            Users = new ConcurrentBag<User>(); 
        }
        public ConcurrentBag<User> Users { get; set; }

        public string GetLastMessageByName(string name)
        {
            return GetUserByName(name)?.LastMessage;
        }

        public User GetUserByName(string name)
        {
            return Users.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}