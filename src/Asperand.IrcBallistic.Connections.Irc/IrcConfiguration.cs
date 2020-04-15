namespace Asperand.IrcBallistic.Connections.Irc
{
    public class IrcConfiguration
    {
        public string DefaultNickname { get; set; }
        public char MessageFlag { get; set; }
        public string Channel { get; set; }
        public string ServerHostName { get; set; }
        public int ServerPort { get; set; }
        
    }
}