using System.Collections.Generic;

namespace Asperand.IrcBallistic.Module.User.Events
{
    public class UserDiscovery : IEvent
    {
        public List<string> Usernames { get; init; }
    }
}