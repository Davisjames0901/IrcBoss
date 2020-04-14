using System;

namespace Asperand.IrcBallistic.Worker.Attributes
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