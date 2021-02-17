using Microsoft.Extensions.Configuration;

namespace Klavogonki.App
{
    /// <summary>
    /// <see cref="IConfiguration"/> factory
    /// </summary>
    internal static class ConfigurationFactory
    {
        /// <summary>
        /// Create <see cref="IConfiguration"/>
        /// </summary>
        internal static IConfiguration Create()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}