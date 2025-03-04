using System.Diagnostics;

namespace WordSearch.Server.Shared.PerfTimer
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="timeLimit"></param>
    public class PerfTimer(int timeLimit) : Stopwatch, IPerfTimer
    {
        public int TimeLimit { get; } = timeLimit;

        public bool IsValid()
        {
            return ElapsedMilliseconds < TimeLimit;
        }
    }
}
