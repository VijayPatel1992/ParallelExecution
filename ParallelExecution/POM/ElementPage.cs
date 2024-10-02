using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ParallelExecution.TestUtility;
using OpenQA.Selenium.Interactions;


namespace ParallelExecution.POM
{
    public class ElementPage
    {

        #region Variable initializations. 

        private IWebDriver Driver;
        UtilityClass _UtilityClass = new UtilityClass();

        #endregion

        #region Constructor

        public ElementPage(IWebDriver Driver)
        {
            this.Driver = Driver;
        }

        #endregion



        #region Elemennts

        //[FindsBy(How = How.XPath, Using = "//*[@id='withOptGroup']")]
        //private IWebElement DDLSelectValue { get; set; }

        public IWebElement DDLSelectValue => Driver.FindElement(By.XPath("//*[@id='withOptGroup']"));


        By UploadedFilePath = By.Id("uploadedFilePath");

        By DDLSelectValueEntry = By.XPath("//*[@id='withOptGroup']//div[contains(@class, 'option')]");

        By GrouHeaderToClick(string groupheader)
        {
            return By.XPath("//*[@class='element-group']//*[text() ='" + groupheader + "']");
        }

        By DivGroupHeader(string groupheader)
        {
            return By.XPath("//*[@class='element-group']//*[text() ='" + groupheader + "']/../../../div[contains(@class, 'element-list')]");
        }

        By LeftPaneElement(string groupheader, string ElementName)
        {
            return By.XPath("//*[@class='element-group']//*[text() ='" + groupheader + "']/../../..//span[text() = '" + ElementName + "']");
        }

        By BtnChooseFile = By.Id("uploadFile");


        By GridRecords = By.XPath("//button[@id = 'addNewRecordButton']/../../following-sibling::div//div[@class='rt-td']");

        By RecordToDelete(string RecordToDelete)
        {
            return By.XPath("//button[@id = 'addNewRecordButton']/../../following-sibling::div//div[@class='rt-td' and text() ='" + RecordToDelete + "']/parent::div//span[@title='Delete']");
        }

        #endregion

        public void ClickOnLeftPaneElement(IWebDriver Driver, string groupheader, string ElementName)
        {
            //d => DriverFixture.Driver.FindElement(elementLocator)
            WebDriverWait BrowserWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));    
            Thread.Sleep(5000);
                _UtilityClass.ScrollToElement(Driver, Driver.FindElement(GrouHeaderToClick(groupheader)));

                BrowserWait.Until(d => Driver.FindElement(GrouHeaderToClick(groupheader))).Click();

            Thread.Sleep(2000);


            if (BrowserWait.Until(d => Driver.FindElement(DivGroupHeader(groupheader))).GetAttribute("class").Equals("element-list collapse"))
            {
                _UtilityClass.ScrollToElement(Driver, Driver.FindElement(GrouHeaderToClick(groupheader)));

                BrowserWait.Until(d => Driver.FindElement(GrouHeaderToClick(groupheader))).Click();
            }

            Thread.Sleep(2000);

            _UtilityClass.ScrollToElement(Driver, Driver.FindElement(LeftPaneElement(groupheader, ElementName)));
            Thread.Sleep(2000);
            IWebElement Ele = BrowserWait.Until(d => Driver.FindElement(LeftPaneElement(groupheader, ElementName)));

            Ele.Click();
            _UtilityClass.JavaScriptClick(Driver, Ele);
           
        }

        public void SelectValueFromDroDown()
        {
           // _UtilityClass.SelectValueFromResponsiveDDL(Driver, DDLSelectValue, DDLSelectValueEntry, "Group 1, option 2");

        }

        public void UploadFile(string FilePathFromToUpload)
        {

            UtilityClass _UtilityClass = new UtilityClass();
           WebDriverWait BrowserWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));

             BrowserWait.Until(d => Driver.FindElement(BtnChooseFile));

            IWebElement element = Driver.FindElement(By.CssSelector("input#uploadFile"));

            // Cast the driver to IJavaScriptExecutor
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;

            // Execute the JavaScript to click the element
            js.ExecuteScript("arguments[0].click();", element);

            IWebElement EleBrowseButton = Driver.FindElement(By.CssSelector("input#uploadFile"));


            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", EleBrowseButton);
       


            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", EleBrowseButton);
            _UtilityClass.JavaScriptClick(Driver, EleBrowseButton);
            //EleBrowseButton.Click();
            Thread.Sleep(2000);
            _UtilityClass.FileUploader(FilePathFromToUpload);
            
        }

        public void UploadFileWithAutoIt(string FilePathFromToUpload)
        {

            //WebDriverWait _wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            //IWebElement EleBrowseButton = _wait.Until(ExpectedConditions.ElementIsVisible(BtnChooseFile));

            //((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", EleBrowseButton);
            //Thread.Sleep(2000);
            //_UtilityClass.TakeScreenShot("abc", Driver);
            //AutoItX.WinWait("Open", "", 10);
            //AutoItX.ControlFocus("Open", "", "Edit1");
            //AutoItX.ControlSetText("Open", "", "Edit1", FilePathFromToUpload);
            //AutoItX.ControlClick("Open", "", "Button1");
        } 

        public string MethodUploadedFilePath()
        {
            WebDriverWait BrowserWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));

            IWebElement EleUploadedFilePath = BrowserWait.Until(d => Driver.FindElement(UploadedFilePath));

            //IWebElement EleUploadedFilePath = _wait.Until(ExpectedConditions.ElementIsVisible(UploadedFilePath));
            _UtilityClass.TakeScreenShot("abc", Driver);
            return EleUploadedFilePath.Text.ToString();
        }

        public void DeleteRecordFromGrid(string NameReocrdsToDelete)
        {

            WebDriverWait BrowserWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));

            BrowserWait.Until(d => Driver.FindElement(GridRecords));


            //_wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(GridRecords));


            BrowserWait.Until(d => Driver.FindElement(RecordToDelete(NameReocrdsToDelete))).Click();



        }

        public bool VerifyDeletedRecordsInGrid(string DeletedRecords)
        {
            bool IsDeletedRecordExist = false;

            WebDriverWait BrowserWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));

            BrowserWait.Until(d => Driver.FindElement(GridRecords));

            IList<IWebElement> GridCollection = Driver.FindElements(GridRecords);

            foreach (WebElement Record in GridCollection)
            {
                string abc = Record.Text;


                if (Record.Text.Equals(DeletedRecords))
                {
                    IsDeletedRecordExist = true;
                    break;
                }
            }

            return IsDeletedRecordExist;
        }
    }
}
