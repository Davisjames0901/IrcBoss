using System;
using System.Collections.Generic;

namespace Digman.Io.IrcBalistic.Abstracts
{
    public abstract class CommandSet
    {
        // public CommandSet()
        // {
        //     PrefixCommands = new Dictionary<string, Command>();
        //     PatternCommands = new Dictionary<string, Command>();
        //     GenerateCommands();
        // }
        // public Dictionary<string, Command> PrefixCommands { get; set; }
        // public Dictionary<string, Command> PatternCommands { get; set; }
        protected abstract void GenerateCommands();
        public virtual string Name { get => this.GetType().ToString(); }

    }
}
