namespace Klavogonki.Common
{
    /// <summary>
    /// Service for calculate delay between inputs
    /// </summary>
    public interface IDelayCalculator
    {
        /// <summary>
        /// Calculates delay
        /// </summary>
        /// <param name="speed">Speed in symbol per minute</param>
        int Calculate(int speed);
    }
}