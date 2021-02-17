using System;
using System.Globalization;
using System.Threading;
using Klavogonki.Common;
using Klavogonki.Common.Auth;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Klavogonki.Core.Strategies
{
    /// <inheritdoc cref="IGameStrategy"/>
    public abstract class BaseGameStrategy : IGameStrategy, IDisposable
    {
        private readonly IAuthenticationService _authenticationService;

        private readonly ITextExtractor _textExtractor;

        private readonly IInputMapBuilder _inputMapBuilder;

        protected readonly IWebDriver WebDriver;

        protected readonly ILogger Logger;

        private bool _authenticated;

        protected BaseGameStrategy(IWebDriver webDriver, IAuthenticationService authenticationService, ITextExtractor textExtractor, IInputMapBuilder inputMapBuilder, ILogger logger)
        {
            _authenticationService = authenticationService;
            _textExtractor = textExtractor;
            _inputMapBuilder = inputMapBuilder;
            
            WebDriver = webDriver;
            Logger = logger;

            _authenticated = false;
        }

        /// <inheritdoc cref="IGameStrategy"/>
        public bool Play()
        {
            try
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
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);

                return false;
            }
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
                var checkBox = howtoplayPanel.FindElement(By.Id("chk-howtoplay"));
                checkBox?.Click();

                var button = howtoplayPanel.FindElement(By.XPath(".//input[@type='button']"));
                button?.Click();
            }

            Thread.Sleep(TimeSpan.FromSeconds(1));

            var wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(5));
            var startButton = wait.Until(driver => driver.FindElement(By.Id("host_start")));
            if (startButton.Displayed)
            {
                startButton.Click();
            }

            Thread.Sleep(TimeSpan.FromSeconds(5));

            wait = new WebDriverWait(WebDriver, GetWaitingTime());
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

                var inputMaps = _inputMapBuilder.GetInputMaps(text);
                
                foreach (var entry in inputMaps)
                {
                    if (entry.ErrorSymbols.Length > 0)
                    {
                        foreach (var errorSymbol in entry.ErrorSymbols)
                        {
                            input.SendKeys($"{errorSymbol}");
                            Thread.Sleep(entry.Delay);
                        }

                        for (var i = 0; i < entry.ErrorSymbols.Length; i++)
                        {
                            input.SendKeys(Keys.Backspace);
                        }

                        Thread.Sleep(entry.Delay);
                    }

                    input.SendKeys($"{entry.Symbol}");
                    Thread.Sleep(entry.Delay);

                    var csssAttribute = input.GetAttribute("class");
                    if (csssAttribute != null && csssAttribute.Contains("error"))
                    {
                        Logger.LogError("Error in input found");

                        return false;
                    }
                }

                Thread.Sleep(TimeSpan.FromSeconds(5));
                
                return true;
            }
            catch (ElementNotInteractableException)
            {
                return true;
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
                    Logger.LogInformation($"Game will start in {waitingTime} at {DateTime.Now.Add(waitingTime)}");

                    return waitingTime.Add(TimeSpan.FromSeconds(30));
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