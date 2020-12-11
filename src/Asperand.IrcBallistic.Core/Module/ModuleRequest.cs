using Asperand.IrcBallistic.Core.Interfaces;

namespace Asperand.IrcBallistic.Core.Module
{
    public class ModuleEvent :IEvent
    {
        public ModuleEvent(IRequest request, IConnection connection)
        {
            Request = request;
            Connection = connection;
        }
        public IRequest Request { get; }
        public IConnection Connection { get; }
    }
}