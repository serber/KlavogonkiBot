using System;
using OpenQA.Selenium;

namespace Klavogonki.Core
{
    /// <summary>
    /// Selenium helper class
    /// </summary>
    internal static class ExpectedConditions
    {
        internal static Func<IWebDriver, IWebElement> ElementIsEnabled(By locator)
        {
            return driver =>
            {
                var element = driver.FindElement(locator);
                return (element != null && element.Displayed && element.Enabled) ? element : null;
            };
        }
    }
}