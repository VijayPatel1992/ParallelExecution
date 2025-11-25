using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ParallelExecution.TestClass
{
    [TestClass]
    public class google
    {

        [TestMethod]
        public void GoogleSearch()
        {
            string succesmessage = ParallelExecution.Resource1.SuccessMessage;
            Console.WriteLine(succesmessage);
            IWebDriver Driver = new ChromeDriver();
            Driver.Url = "https://www.google.com";
            Driver.Manage().Window.Maximize();
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Name("q")));

            IWebElement SearchBox = Driver.FindElement(By.Name("q"));
            SearchBox.SendKeys("Selenium C#");
            IList<IWebElement> Suggestions = Driver.FindElements(By.XPath("//ul[@role='listbox']//li//span"));

            Console.WriteLine("Total Suggestions: " + Suggestions.Count);
            foreach (var suggestion in Suggestions)
            {
                Console.WriteLine(suggestion.Text);
            }

            ITakesScreenshot ts = (ITakesScreenshot)Driver;
            Screenshot ss = ts.GetScreenshot();
            ss.SaveAsFile("GoogleSearch.png");

            IWebElement countryDdl = Driver.FindElement(By.Id("SIvCob"));
            SelectElement selectCountry = new SelectElement(countryDdl);
            selectCountry.SelectByText("India");

            IAlert alert = Driver.SwitchTo().Alert();
            alert.Accept();

            IWebElement framelocator = Driver.FindElement(By.XPath("//iframe[@role='presentation']"));
            Driver.SwitchTo().Frame(framelocator);
            Driver.SwitchTo().DefaultContent();

            string windowhandle = Driver.CurrentWindowHandle;
            IReadOnlyCollection<string> allwindowHandles = Driver.WindowHandles;
            foreach (var handle in allwindowHandles)
            {
                Driver.SwitchTo().Window(handle);

                if (Driver.Title.Contains("Selenium"))
                {
                    break;
                }


            }

            string[] abc = new string[3];
        }
    }
}
