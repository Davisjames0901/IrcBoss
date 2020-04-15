using System.Collections.Generic;
using Asperand.IrcBallistic.Core.Interfaces;

namespace Asperand.IrcBallistic.Core.Events
{
    public class UserDiscovery : IEvent
    {
        public List<string> Usernames { get; set; }
    }
}