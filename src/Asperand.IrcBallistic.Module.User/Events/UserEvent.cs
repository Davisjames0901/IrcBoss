namespace Asperand.IrcBallistic.Module.User.Events
{
    public class UserEvent : IEvent
    {
        public string Username { get; init; }
        public UserEventEnum Event { get; init; }
    }

    public enum UserEventEnum
    {
        Joined,
        Leaving
    }
}