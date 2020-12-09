using System;

namespace Asperand.IrcBallistic.Core.Module
{
    public class ModuleStatistics
    {
        public ModuleStatus Status { get; init; }
        public DateTime? ExecutionStartedTime { get; init; }
        public TimeSpan AverageNopTime { get; init; }
        public TimeSpan AverageOpTime { get; init; }
        public int DefaultTimeout { get; set; }
        public float ExceptionRatio { get; init; }
    }
}