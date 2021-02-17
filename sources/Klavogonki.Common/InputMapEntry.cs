namespace Klavogonki.Common
{
    /// <summary>
    /// Symbol input information
    /// </summary>
    public class InputMapEntry
    {
        /// <summary>
        /// Symbol for input
        /// </summary>
        public char Symbol { get; set; }

        /// <summary>
        /// Error symbols for input
        /// </summary>
        public char[] ErrorSymbols { get; set; }

        /// <summary>
        /// Symbol input delay
        /// </summary>
        public int Delay { get; set; }
    }
}