using System;

namespace Asperand.IrcBallistic.Core.Job.Data
{
    public class JobStatistics
    {
        public JobStatus Status { get; init; }
        public DateTime? ExecutionStartedTime { get; init; }
        public TimeSpan AverageNopTime { get; init; }
        public TimeSpan AverageOpTime { get; init; }
        public int DefaultTimeout { get; set; }
        public float ExceptionRatio { get; init; }
        public TimeSpan? RunTime => ExecutionStartedTime == null ? null : DateTime.Now - ExecutionStartedTime;
        public TimeSpan? StuckTime => RunTime?.TotalSeconds > DefaultTimeout
            ? new TimeSpan(RunTime.Value.Ticks) - TimeSpan.FromSeconds(DefaultTimeout)
            : null;
    }
}