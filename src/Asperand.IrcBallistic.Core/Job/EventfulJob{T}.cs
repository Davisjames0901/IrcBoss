using System.Threading.Tasks;
using Asperand.IrcBallistic.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Core.Job
{
    public abstract class EventfulJob<T> : BaseJob where T : IEvent
    {
        protected EventfulJob(ILogger<BaseJob> log) : base(log)
        { }
        
        public abstract Task Handle(T payload);
    }
}