using System.Collections.Generic;

namespace Asperand.IrcBallistic.Worker.Classes
{
    public class CommandRequest
    {
        public string CommandName { get; set; }
        public Dictionary<string, string> Flags { get; set; }
        public string Content { get; set; }
        public string Raw { get; set; }
        public string Target { get; set; }
        public string RequesterUsername { get; set; }
    }
}