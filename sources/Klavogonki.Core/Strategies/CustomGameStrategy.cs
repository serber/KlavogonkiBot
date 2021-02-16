using System;
using System.Linq;
using Klavogonki.Common;
using Klavogonki.Common.Auth;
using Klavogonki.Core.Enums;
using Klavogonki.Core.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;

namespace Klavogonki.Core.Strategies
{
    /// <summary>
    /// Creates a new game based on input parameters
    /// </summary>
    public class CustomGameStrategy : BaseGameStrategy
    {
        private readonly CustomGameOptions _customGameOptions;

        public CustomGameStrategy(IWebDriver webDriver, IAuthenticationService authenticationService,
            ITextExtractor textExtractor, IOptions<CustomGameOptions> customGameOptions, IOptions<GameOptions> options,
            ILogger<CustomGameStrategy> logger) : base(webDriver, authenticationService, textExtractor, options, logger)
        {
            _customGameOptions = customGameOptions.Value;
        }

        /// <inheritdoc cref="BaseGameStrategy.StartGame"/>
        protected override void StartGame()
        {
            WebDriver.Navigate().GoToUrl(Constants.CreateUrl);

            SelectGameMode();

            Logger.LogInformation($"Selected game mode {_customGameOptions.GameMode}");

            var submitButton = WebDriver.FindElement(By.Id("submit_btn"));
            submitButton.Click();
        }

        /// <summary>
        /// Selects the game mode
        /// </summary>
        private void SelectGameMode()
        {
            var items = WebDriver.FindElements(By.XPath("//*[@id=\"create\"]/dl/dt/table/tbody/tr"));

            var block = items.FirstOrDefault(x => x.GetAttribute("class").Contains(GetClassName(_customGameOptions.GameMode)));

            block?.Click();
        }

        /// <summary>
        /// Returns the name of the css class
        /// </summary>
        /// <param name="gameMode">Game mode</param>
        private static string GetClassName(GameMode gameMode)
        {
            switch (gameMode)
            {
                case GameMode.Normal:
                    return "gametype-normal";
                case GameMode.Vocabulary:
                    return "gametype-voc";
                case GameMode.Unmistakable:
                    return "gametype-noerror";
                case GameMode.Letters:
                    return "gametype-chars";
                case GameMode.Marathon:
                    return "gametype-marathon";
                case GameMode.Sprint:
                    return "gametype-sprint";
                case GameMode.Abrakadabra:
                    return "gametype-abra";
                case GameMode.Digits:
                    return "gametype-digits";
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameMode), gameMode, null);
            }
        }
    }
}