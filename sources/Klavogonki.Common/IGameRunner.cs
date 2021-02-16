namespace Klavogonki.Common
{
    /// <summary>
    /// Service for launching a series of games
    /// </summary>
    public interface IGameRunner
    {
        /// <summary>
        /// Launches a series of games
        /// </summary>
        void Run();
    }
}