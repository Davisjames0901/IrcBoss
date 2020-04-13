using System.Collections.Generic;

namespace Asperand.IrcBallistic.Worker.Messages
{
    public class UserDiscovery : IEvent
    {
        public List<string> Usernames { get; set; }
    }
}