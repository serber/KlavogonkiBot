using Klavogonki.Common;

namespace Klavogonki.Core
{
    /// <inheritdoc cref="IDelayCalculator"/>
    public class AverageDelayCalculator : IDelayCalculator
    {
        /// <inheritdoc cref="IDelayCalculator.Calculate"/>
        public int Calculate(string text, int speed)
        {
            var delay = 60000 / text.Length;

            return delay;
        }
    }
}