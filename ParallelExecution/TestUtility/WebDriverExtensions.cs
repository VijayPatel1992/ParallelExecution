using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace ParallelExecution.TestUtility
{
    public static class WebDriverExtensions
    {
        /// <summary>
        /// Waits until the element located by the given locator is visible and enabled, then clicks it.
        /// </summary>
        /// <param name="driver">The IWebDriver instance.</param>
        /// <param name="locator">Locator for the element to click.</param>
        /// <param name="timeoutInSeconds">Maximum seconds to wait before timing out.</param>
        public static void waitAndClick(this IWebDriver driver, By locator, int timeoutInSeconds = 30)
        {
            if (driver == null) throw new ArgumentNullException(nameof(driver));
            if (locator == null) throw new ArgumentNullException(nameof(locator));

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));

            IWebElement element = wait.Until(d =>
            {
                try
                {
                    var el = d.FindElement(locator);
                    return (el != null && el.Displayed && el.Enabled) ? el : null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            });

            try
            {
                element.Click();
            }
            catch (Exception)
            {
                // fallback to JavaScript click if normal click fails
                try
                {
                    var js = (IJavaScriptExecutor)driver;
                    js.ExecuteScript("arguments[0].click();", element);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Could not click the element located by: " + locator, ex);
                }
            }
        }

    }
}
