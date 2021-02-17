using Klavogonki.Common;
using Klavogonki.Common.Auth;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;

namespace Klavogonki.Core.Strategies
{
    /// <summary>
    /// Launches a random game
    /// </summary>
    public class RandomGameStrategy : BaseGameStrategy
    {
        public RandomGameStrategy(IWebDriver webDriver, IAuthenticationService authenticationService, IInputMapBuilder inputMapBuilder,
            ITextExtractor textExtractor, ILogger<RandomGameStrategy> logger)
            : base(webDriver, authenticationService, textExtractor, inputMapBuilder, logger)
        {
        }

        /// <inheritdoc cref="BaseGameStrategy.StartGame"/>
        protected override void StartGame()
        {
            WebDriver.Navigate().GoToUrl(Constants.RandomUrl);
        }
    }
}