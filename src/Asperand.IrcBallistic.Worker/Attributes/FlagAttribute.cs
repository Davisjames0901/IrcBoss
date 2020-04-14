using System;

namespace Asperand.IrcBallistic.Worker.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FlagAttribute : Attribute
    {
        public readonly string Flag;
        public readonly string HelpText;
        
        public FlagAttribute(string flag, string helpText = null)
        {
            Flag = flag;
            HelpText = helpText;
        }
    }
}