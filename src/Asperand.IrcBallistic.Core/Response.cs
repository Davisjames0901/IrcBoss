namespace Asperand.IrcBallistic.Core
{
    public class Response
    {
        public string Text { get; init; }
        public bool IsAction { get; init; }
        public string Target { get; init; }
    }
}