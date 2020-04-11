using System.Collections.Generic;

namespace Asperand.IrcBallistic.Worker.Classes
{
    public class CommandRequest
    {
        public string CommandName { get; set; }
        public Dictionary<string, string> Flags { get; set; }
        public string Content { get; set; }
        public string Raw { get; set; }
        public User Requester { get; set; }
    }
}