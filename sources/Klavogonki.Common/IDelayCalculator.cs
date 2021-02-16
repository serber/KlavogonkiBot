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
        /// <param name="text">Text</param>
        /// <param name="speed">Speed in symbol per minute</param>
        int Calculate(string text, int speed);
    }
}