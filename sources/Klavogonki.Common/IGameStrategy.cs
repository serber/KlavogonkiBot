namespace Klavogonki.Common
{
    /// <summary>
    /// Game strategy interface
    /// </summary>
    public interface IGameStrategy
    {
        /// <summary>
        /// Starts a new game
        /// </summary>
        bool Play();
    }
}