using System.Threading.Tasks;
using Asperand.IrcBallistic.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Asperand.IrcBallistic.Core.Module
{
    public abstract class Job : ModuleBase
    {
        protected Job(ILogger<ModuleBase> log) : base(log)
        { }
    }
}