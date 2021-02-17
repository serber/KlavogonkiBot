using Klavogonki.Common;

namespace Klavogonki.Core
{
    /// <inheritdoc cref="IDelayCalculator"/>
    public class AverageDelayCalculator : IDelayCalculator
    {
        /// <inheritdoc cref="IDelayCalculator.Calculate"/>
        public int Calculate(int speed)
        {
            var delay = 60000 / speed;
            return delay - (delay * 30) / 100;
        }
    }
}