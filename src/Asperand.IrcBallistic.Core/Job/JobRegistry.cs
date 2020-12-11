using System.Collections.Generic;
using Asperand.IrcBallistic.Core.Interfaces;

namespace Asperand.IrcBallistic.Core.Job
{
    public class JobRegistry
    {
        public List<BaseJob> AllJobs { get; set; }

        public void RegisterJob(BaseJob job, IConnection connection)
        {
            
        }
    }
}