using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using ParallelExecution.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;

namespace ParallelExecution.TestUtility
{
    public class UtilityClass
    {
        #region Variables


        #endregion

        #region Screnshot

        /// <summary>
        /// Captures Screenshot wand has specified filename 
        /// </summary>
        /// <param name="filename"> Screenshot FileName </param>
        /// <returns>Returns the FielName</returns>
        public string TakeScreenShot(string filename, IWebDriver Driver)
        {
            filename = filename + " " + DateTime.Now.ToString("yyyy_MM_dd-HHmmss") + ".jpeg";
            filename = Path.Combine(BaseClass.ScreenSortPath, filename);

            ((ITakesScreenshot)Driver).GetScreenshot().SaveAsFile(filename);

            return filename;
        }

        #endregion

        #region Enum

        /// <summary>
        /// GetDescriptionFromEnum(Enum value) -- Get Value of Enum from description.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetDescriptionFromEnum(Enum value)
        {
            DescriptionAttribute attribute = value.GetType()
            .GetField(value.ToString())
            .GetCustomAttributes(typeof(DescriptionAttribute), false)
            .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }

        #endregion      

        /// <summary>
        /// ScrollToElement(IWebElement ele) -- TO Scroll in to the Element
        /// </summary>
        /// <param name="ele">Iwebelement where needs to scroll</param>
        public void ScrollToElement(IWebDriver Driver, IWebElement ele )
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", ele);
        }


        /// <summary>
        /// ScrollToElement(IWebElement ele) -- To Click on element with Java script executor
        /// </summary>
        /// <param name="ele">Iwebelement where needs to scroll</param>
        public void JavaScriptClick(IWebDriver Driver, IWebElement ele)
        {
            IJavaScriptExecutor executor = (IJavaScriptExecutor)Driver;
            executor.ExecuteScript("arguments[0].click();", ele);
        }


        /// <summary>
        /// WaitForElementToBeClickable(IWebElement ele) - Wait to Element to be Clickabel.
        /// </summary>
        /// <param name="ele">Iwebelement</param>
        public void WaitForElementToBeClickable(IWebDriver Driver, By ele, int WaitForSeconds = 20)
        {
            WebDriverWait BrowserWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(WaitForSeconds));

            BrowserWait.Until(d => Driver.FindElement(ele));
            
        }

        /// <summary>
        /// An expectation to wait and check an element is either invisible or not present in the DOM.
        /// </summary>
        /// <param name="locator">Locator for the web element.</param>
        /// <param name="waitType">Type of wait from <see cref="WaitType"/>.</param>
        public void WaitForElementToBeInvisible(IWebDriver Driver, By locator)
        {
            WebDriverWait BrowserWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            bool result = false;
            try
            {
                BrowserWait.Until(d=> Driver.FindElement(locator));
                if (!result)
                    throw new WebDriverTimeoutException();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);

            }
        }

        /// <summary>
        /// SelectValueFromResponsiveDDL(IWebElement dropDownList, By DropDownListEntriesLocator, string itemToClick) -- Select value from responsive Drop down.
        /// </summary>
        /// <param name="DropDown">Drop down Element to open Drop down list with click.</param>
        /// <param name="DropDownListEntriesLocator">Locator of DDL value.</param>
        /// <param name="ItemToSelect">Test of Drop down value to select.</param>
        public void SelectValueFromResponsiveDDL(IWebDriver Driver, By Locator_DropDown, By DropDownListEntriesLocator, string ItemToSelect, int WaitForSeconds = 20)
        {
            WebDriverWait BrowserWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(WaitForSeconds));
            IWebElement DropDown= Driver.FindElement(Locator_DropDown);
            //supply initial char
            ScrollToElement(Driver, DropDown);
            WaitForElementToBeClickable(Driver, Locator_DropDown);
            DropDown.Click();
            //dropDownList.Click();
            Thread.Sleep(2000);

            //wait for auto suggest list
            BrowserWait.Until(d => Driver.FindElement(DropDownListEntriesLocator));
            IList<IWebElement> elements = Driver.FindElements(DropDownListEntriesLocator);

            foreach (var ele in elements)
            {
                if (ele.Text.Equals(ItemToSelect))
                {
                    ele.Click();
                    break;
                }
            }
        }

        /// <summary>
        /// An expectation to wait for ajax load to be completed.
        /// </summary>
        public void WaitForAjaxLoad(IWebDriver Driver)
        {
            try
            {
                bool jQueryDefined = (bool)((IJavaScriptExecutor)Driver).ExecuteScript(@"return typeof jQuery != 'undefined'");
                if (jQueryDefined)
                {
                    TimeSpan timeOut = TimeSpan.FromSeconds(Convert.ToInt32(ConfigurationManager.AppSettings["PageLoadTimeout"]));
                    WebDriverWait wait = new WebDriverWait(Driver, timeOut);
                    wait.Until(driver => (bool)((IJavaScriptExecutor)Driver).ExecuteScript(@"return jQuery.active == 0"));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// An expectation to wait for browser load action to be completed.
        /// </summary>
        public void WaitForBrowserLoad(IWebDriver Driver)
        {
            string state = string.Empty;
            TimeSpan TimeOut = TimeSpan.FromSeconds(Convert.ToInt32(ConfigurationManager.AppSettings["PageLoadTimeout"]));
            try
            {
                WebDriverWait wait = new WebDriverWait(Driver, TimeOut);
                wait.Until(drv =>
                {
                    try
                    {
                        state = ((IJavaScriptExecutor)Driver).ExecuteScript(@"return document.readyState").ToString();
                    }
                    catch (InvalidOperationException)
                    {
                        //Ignore
                    }
                    //In IE7 there are chances we may get state as loaded instead of complete
                    return (state.Equals("complete", StringComparison.InvariantCultureIgnoreCase) || state.Equals("loaded", StringComparison.InvariantCultureIgnoreCase));
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// An expectation to wait for network calls to be finished.
        /// </summary>
        public void WaitForNetworkCalls(IWebDriver Driver)
        {
            string activeCalls = "0";

            string script = @"source:(" +
                "function(){" +
                "var send = XMLHttpRequest.prototype.send;" +
                "var release = function(){ --XMLHttpRequest.active };" +
                "var onloadend = function(){ setTimeout(release, 1) };" +
                "XMLHttpRequest.active = 0;" +
                "XMLHttpRequest.prototype.send = function() {" +
                "++XMLHttpRequest.active;" +
                "this.addEventListener('loadend', onloadend, true);" +
                "send.apply(this, arguments);" +
                "};})();";

            ((IJavaScriptExecutor)Driver).ExecuteScript(script);
            TimeSpan timeOut = TimeSpan.FromSeconds(Convert.ToInt32(ConfigurationManager.AppSettings["PageLoadTimeout"]));
            try
            {
                WebDriverWait wait = new WebDriverWait(Driver, timeOut);
                wait.Until(driver =>
                {
                    try
                    {
                        activeCalls = ((IJavaScriptExecutor)Driver).ExecuteScript(@"return XMLHttpRequest.active").ToString();
                    }
                    catch (InvalidOperationException)
                    {
                        // Ignore
                    }
                    bool flag = activeCalls.Equals("0", StringComparison.InvariantCultureIgnoreCase);
                    return flag;
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
            }

        }

        /// <summary>
        /// Waits for a webpage to load, considering Ajax, Angular, JavaScript and any other background network calls.
        /// </summary>
        public void WaitForPageLoad(IWebDriver Driver)
        {
            try
            {
                WaitForBrowserLoad(Driver);
                WaitForAjaxLoad(Driver);
                WaitForNetworkCalls(Driver);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Wait until an element is no longer attached to the DOM.
        /// </summary>
        /// <param name="locator">Locator for the web element.</param>
        /// <param name="SecondsToWait">Maximum seconds to wait for staleness<see/>.</param>
        public void WaitForElementStaleness(IWebDriver Driver, By Loactor_ele, int SecondsToWait = 20)
        {
            //bool result = false;
            //try
            //{
            //    WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(SecondsToWait));
            //    result = wait.Until(d => Driver.FindElement(Loactor_ele));
            //    if (!result)
            //        throw new WebDriverTimeoutException();
            //}

            //catch (Exception e)
            //{
            //    Console.WriteLine(e.StackTrace);
            //    Console.WriteLine(e.Message);
            //}
        }


        /// <summary>
        /// Interacts with windows form to select file from windows explorer
        /// </summary>
        /// <param name="fileLocation"> Location of file to be uploaded</param>
        /// <param name="windowTitle">Title of windows dialog, by default set to "Open" </param>
        public void FileUploader( string fileLocation, string windowTitle = "Open")
        {

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = Directory.GetCurrentDirectory() + "\\AutoIT\\FileUploadScript.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = $"\"{fileLocation}\"";

            // Call WaitForExit and then the using statement will close.
            using (Process exeProcess = Process.Start(startInfo))
            {
                exeProcess.WaitForExit();
            }
        }

        public IWebElement ShadowDOM_ele(IWebDriver Driver, string ExecutorString)
        {
            IWebElement _element = null;

            try
            {
                IJavaScriptExecutor Js = (IJavaScriptExecutor)Driver;
                _element = ((IWebElement)Js.ExecuteScript("return " + ExecutorString));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _element;
        }
    }
}
