using System.Collections.Concurrent;

namespace Asperand.IrcBallistic.Module.User.Data
{
    public class UserContainer
    {
        public UserContainer()
        {
            Users = new ConcurrentDictionary<string, User>(); 
        }
        public ConcurrentDictionary<string, User> Users { get; }

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