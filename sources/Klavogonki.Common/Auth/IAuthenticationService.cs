namespace Klavogonki.Common.Auth
{
    /// <summary>
    /// Authentication service 
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Authenticate user
        /// </summary>
        bool Authenticate();
    }
}