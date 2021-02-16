namespace Klavogonki.Common
{
    /// <summary>
    /// Service for extracting game text from HTML
    /// </summary>
    public interface ITextExtractor
    {
        /// <summary>
        /// Extract text from HTML page
        /// </summary>
        /// <param name="html">HTML page source</param>
        string Extract(string html);
    }
}