using Asperand.IrcBallistic.Core.Interfaces;

namespace Asperand.IrcBallistic.Core.Events
{
    public class UserEvent : IEvent
    {
        public string Username { get; set; }
        public UserEventEnum Event { get; set; }
    }

    public enum UserEventEnum
    {
        Joined,
        Leaving
    }
}