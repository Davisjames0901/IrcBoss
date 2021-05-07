using System.Collections.Generic;

namespace Asperand.IrcBallistic.Module.Command.Data
{
    public class CommandRequest
    {
        public string CommandName { get; set; }
        public string Content { get; set; }
        public string Raw { get; set; }
        public string Target { get; set; }
        public string RequesterUsername { get; set; }
    }
}