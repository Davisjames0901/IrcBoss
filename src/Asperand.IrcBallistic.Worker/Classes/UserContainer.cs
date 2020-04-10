using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Asperand.IrcBallistic.Worker.Classes
{
    public class UserContainer
    {
        public UserContainer()
        {
            Users = new ConcurrentBag<User>(); 
        }
        public ConcurrentBag<User> Users { get; set; }
    }
}