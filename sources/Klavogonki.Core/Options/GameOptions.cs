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
        /// Delay in milliseconds between keyboard inputs
        /// </summary>
        public int Delay { get; set; }
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