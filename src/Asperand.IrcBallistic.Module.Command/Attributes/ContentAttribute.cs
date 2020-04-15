using System;

namespace Asperand.IrcBallistic.Module.Command.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ContentAttribute : Attribute
    {
        public readonly string HelpText;
        
        public ContentAttribute(string helpText = null)
        {
            HelpText = helpText;
        }
    }
}