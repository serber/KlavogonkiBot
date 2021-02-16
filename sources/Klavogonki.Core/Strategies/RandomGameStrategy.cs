using Klavogonki.Common;
using Klavogonki.Common.Auth;
using Klavogonki.Core.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;

namespace Klavogonki.Core.Strategies
{
    /// <summary>
    /// Launches a random game
    /// </summary>
    public class RandomGameStrategy : BaseGameStrategy
    {
        public RandomGameStrategy(IWebDriver webDriver, IAuthenticationService authenticationService,
            ITextExtractor textExtractor, IOptions<GameOptions> options, ILogger<RandomGameStrategy> logger)
            : base(webDriver, authenticationService, textExtractor, options, logger)
        {
        }

        /// <inheritdoc cref="BaseGameStrategy.StartGame"/>
        protected override void StartGame()
        {
            WebDriver.Navigate().GoToUrl(Constants.RandomUrl);
        }
    }
}