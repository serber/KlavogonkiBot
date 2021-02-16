using System;

namespace Klavogonki.Core.Options
{
    /// <summary>
    /// Game runner options
    /// </summary>
    public class GameRunnerOptions
    {
        /// <summary>
        /// Number of games running in parallel
        /// </summary>
        public int ParallelGameCount { get; set; }

        /// <summary>
        /// Working time
        /// </summary>
        public TimeSpan RunTime { get; set; }
    }
}