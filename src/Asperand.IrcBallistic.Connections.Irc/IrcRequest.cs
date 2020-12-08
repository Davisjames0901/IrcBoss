using System.Linq;
using Asperand.IrcBallistic.Core.Interfaces;

namespace Asperand.IrcBallistic.Connections.Irc
{
    public class IrcRequest : IRequest
    {
        public IrcRequest(string line)
        {
            Line = line;
            LineTokens = line.Split(' ');
            Action = LineTokens[1].ToLower();
            var tokens = Line.Split(':');
            Username = tokens[1].Split('!')[0].ToLower();
            Content = string.Join(":", tokens.Skip(2)).ToLower();
            Target = tokens[1].Trim().Split(' ').Last().ToLower();
        }

        public string[] LineTokens { get; }
        public string Line { get; }
        public string Username { get; }
        public string Content { get; }
        public string Target { get; }
        public string Action { get; }
    }
}