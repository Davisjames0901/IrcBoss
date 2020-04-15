using System.Collections.Generic;

namespace Asperand.IrcBallistic.Module.Command.Data
{
    public class CommandMetadata
    {
        public string CommandName { get; set; }
        public string HelpText { get; set; }
        public Dictionary<string, string> Flags { get; set; }
        public string ContentHelpText { get; set; }
    }
}