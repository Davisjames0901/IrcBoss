using Asperand.IrcBallistic.Core.Interfaces;

namespace Asperand.IrcBallistic.Connections.Irc
{
    public class IrcResponse : IResponse
    {
        public string Text { get; init; }
        public bool IsAction { get; init; }
        public string Target { get; init; }
    }
}