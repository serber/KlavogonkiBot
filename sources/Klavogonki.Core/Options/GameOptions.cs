using Klavogonki.Core.Enums;
using Klavogonki.Core.Strategies;

namespace Klavogonki.Core.Options
{
    /// <summary>
    /// Common game options
    /// </summary>
    public class GameOptions
    {
        /// <summary>
        /// Input speed in symbol per minyte
        /// </summary>
        public int Speed { get; set; }

        /// <summary>
        /// Input error percent
        /// </summary>
        public int ErrorPercent { get; set; }
    }

    /// <summary>
    /// Game options for <see cref="CustomGameStrategy"/>
    /// </summary>
    public class CustomGameOptions
    {
        /// <summary>
        /// Game mode
        /// </summary>
        public GameMode GameMode { get; set; }
    }
}