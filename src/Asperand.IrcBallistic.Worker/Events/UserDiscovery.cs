using System.Collections.Generic;

namespace Asperand.IrcBallistic.Worker.Events
{
    public class UserDiscovery : IEvent
    {
        public List<string> Usernames { get; set; }
    }
}