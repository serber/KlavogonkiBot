using Klavogonki.Common.Auth;
using Klavogonki.Core.Options;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;

namespace Klavogonki.Core.Auth
{
    /// <inheritdoc cref="IAuthenticationService"/>
    public class LoginPasswordAuthenticationService : IAuthenticationService
    {
        private readonly IWebDriver _webDriver;
        
        private readonly AuthenticationOptions _options;
        
        public LoginPasswordAuthenticationService(IWebDriver webDriver, IOptions<AuthenticationOptions> options)
        {
            _webDriver = webDriver;
            _options = options.Value;
        }

        /// <inheritdoc cref="IAuthenticationService.Authenticate"/>
        public bool Authenticate()
        {
            _webDriver.Navigate().GoToUrl("http://klavogonki.ru");

            var loginButton = _webDriver.FindElement(By.Id("login-link"));
            if (loginButton == null)
            {
                return false;
            }

            loginButton.Click();

            var loginInput = _webDriver.FindElement(By.Id("username"));
            var passwordInput = _webDriver.FindElement(By.Id("password"));

            if (loginInput == null || passwordInput == null)
            {
                return false;
            }

            loginInput.SendKeys(_options.Login);
            passwordInput.SendKeys(_options.Password);

            var submitButton = _webDriver.FindElement(By.Id("login_form_submit"));

            if (submitButton == null)
            {
                return false;
            }

            submitButton.Click();

            return true;
        }
    }
}