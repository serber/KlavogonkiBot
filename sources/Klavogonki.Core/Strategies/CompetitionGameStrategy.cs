using System;
using System.Threading;
using Klavogonki.Common;
using Klavogonki.Common.Auth;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Klavogonki.Core.Strategies
{
    /// <summary>
    /// Launches the game in competition mode
    /// In this mode, you can run only one game in parallel
    /// </summary>
    public class CompetitionGameStrategy : BaseGameStrategy
    {
        public CompetitionGameStrategy(IWebDriver webDriver, ITextExtractor textExtractor,
            IInputMapBuilder inputMapBuilder,
            IAuthenticationService authenticationService,
            ILogger<CompetitionGameStrategy> logger) : base(webDriver, authenticationService, textExtractor,
            inputMapBuilder, logger)
        {
        }

        /// <inheritdoc cref="BaseGameStrategy.StartGame"/>
        protected override void StartGame()
        {
            WebDriver.Navigate().GoToUrl(Constants.CompetitionUrl);

            Thread.Sleep(TimeSpan.FromSeconds(5));

            var wait = new WebDriverWait(WebDriver, TimeSpan.FromMinutes(1));
            var competitionButton = wait.Until(driver => driver.FindElement(By.ClassName("competition")));

            competitionButton.Click();

            var competitionAlert = WebDriver.FindElement(By.Id("competition_alert"));
            if (competitionAlert.Displayed)
            {
                var competitionRememberCheckBox = WebDriver.FindElement(By.Id("competition_remember"));
                if (competitionRememberCheckBox != null)
                {
                    competitionRememberCheckBox.Click();

                    var alert = WebDriver.SwitchTo().Alert();
                    alert?.Accept();

                    var competitionAcceptButton = WebDriver.FindElement(By.Id("competition_btn_accept"));
                    competitionAcceptButton?.Click();
                }
            }
        }
    }
}