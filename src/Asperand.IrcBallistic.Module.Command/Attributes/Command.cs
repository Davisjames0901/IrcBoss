using System;

namespace Asperand.IrcBallistic.Module.Command.Attributes
{
    public class Command : Attribute
    {
        public Command(string name = "")
        {
            Name = name.ToLower();
        }

        public string Name { get; }
    }
}