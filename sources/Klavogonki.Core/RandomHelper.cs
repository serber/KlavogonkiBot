using System;

namespace Klavogonki.Core
{
    /// <summary>
    /// Random generation helper
    /// </summary>
    internal static class RandomHelper
    {
        internal static Random Random = new Random();
        
        /// <summary>
        /// Adds random deviation to number
        /// </summary>
        internal static int AddRandomDeviation(int number, int deviationPercent)
        {
            var deviation = (number * deviationPercent) / 100;
            return number + Random.Next(-deviation, deviation);
        }

        internal static bool IsLucky(int percent)
        {
            var random = Random.Next(1, 100);
            return random <= percent;
        }
    }
}