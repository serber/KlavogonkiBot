using System;
using System.Globalization;
using System.Threading;
using Klavogonki.Common;
using Klavogonki.Common.Auth;
using Klavogonki.Core.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Klavogonki.Core.Strategies
{
    /// <inheritdoc cref="IGameStrategy"/>
    public abstract class BaseGameStrategy : IGameStrategy, IDisposable
    {
        private readonly IAuthenticationService _authenticationService;

        private readonly ITextExtractor _textExtractor;

        private readonly GameOptions _options;

        protected readonly IWebDriver WebDriver;

        protected readonly ILogger Logger;

        private bool _authenticated;

        protected BaseGameStrategy(IWebDriver webDriver, IAuthenticationService authenticationService, ITextExtractor textExtractor, IOptions<GameOptions> options, ILogger logger)
        {
            _authenticationService = authenticationService;
            _textExtractor = textExtractor;
            _options = options.Value;

            WebDriver = webDriver;
            Logger = logger;

            _authenticated = false;
        }

        /// <inheritdoc cref="IGameStrategy"/>
        public bool Play()
        {
            if (!_authenticated)
            {
                Authenticate();
            }

            Logger.LogInformation("Starting game");

            StartGame();

            Logger.LogInformation("Waiting for game");

            WaitGame();

            Logger.LogInformation("Game started");

            return PlayGame();
        }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            WebDriver?.Dispose();
        }

        /// <summary>
        /// Launches a new game
        /// </summary>
        protected abstract void StartGame();

        /// <summary>
        /// Awaiting the start of the game
        /// </summary>
        protected virtual void WaitGame()
        {
            var howtoplayPanel = WebDriver.FindElement(By.Id("howtoplay"));
            if (howtoplayPanel.Displayed)
            {
                var checkBox = WebDriver.FindElement(By.Id("chk-howtoplay"));
                checkBox?.Click();

                var button = WebDriver.FindElement(By.XPath("//*[@id=\"howtoplay\"]/div[2]/div/table/tbody/tr[2]/td[2]/p[5]/input"));
                button?.Click();
            }

            var startButton = WebDriver.FindElement(By.Id("host_start"));
            if (startButton.Displayed)
            {
                startButton.Click();
            }

            Thread.Sleep(TimeSpan.FromSeconds(5));

            var wait = new WebDriverWait(WebDriver, GetWaitingTime());
            wait.Until(ExpectedConditions.ElementIsEnabled(By.Id("inputtext")));
        }

        /// <summary>
        /// Plays the game
        /// </summary>
        internal virtual bool PlayGame()
        {
            try
            {
                var input = WebDriver.FindElement(By.Id("inputtext"));
                var text = _textExtractor.Extract(WebDriver.PageSource);

                if (string.IsNullOrEmpty(text))
                {
                    Logger.LogWarning("Extracted text is empty");
                    return false;
                }

                Logger.LogInformation($"Text found: {text}");

                var parts = text.Split(' ');
                foreach (var part in parts)
                {
                    foreach (var c in part)
                    {
                        input.SendKeys($"{c}");
                        Thread.Sleep(TimeSpan.FromMilliseconds(_options.Delay));
                    }

                    var csssAttribute = input.GetAttribute("class");
                    if (csssAttribute != null && csssAttribute.Contains("error"))
                    {
                        Logger.LogError("Error found");

                        return false;
                    }

                    input.SendKeys(" ");
                    Thread.Sleep(TimeSpan.FromMilliseconds(_options.Delay * 2));
                }

                return true;
            }
            catch (ElementNotInteractableException)
            {
                return true;
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);

                return false;
            }
        }

        /// <summary>
        /// Returns the waiting time for the game to start
        /// </summary>
        protected virtual TimeSpan GetWaitingTime()
        {
            var waitingTimeout = WebDriver.FindElement(By.Id("waiting_timeout"));

            if (waitingTimeout.Displayed)
            {
                var text = waitingTimeout.Text;
                if (TimeSpan.TryParse($"00:{text.Replace(" ", ":")}", new DateTimeFormatInfo(), out var waitingTime))
                {
                    Logger.LogInformation($"Game will start in {waitingTime}");

                    return waitingTime;
                }
            }

            return TimeSpan.FromMinutes(1);
        }

        /// <summary>
        /// Authenticates
        /// </summary>
        private void Authenticate()
        {
            _authenticationService.Authenticate();

            _authenticated = true;

            Logger.LogInformation("Authentication complete");
        }
    }
}