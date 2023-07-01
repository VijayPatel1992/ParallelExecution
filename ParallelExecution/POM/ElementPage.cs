using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ParallelExecution.TestUtility;


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
            PageFactory.InitElements(Driver, this);
        }

        #endregion



        #region Elemennts

        [FindsBy(How = How.XPath, Using = "//*[@id='withOptGroup']")]
        private IWebElement DDLSelectValue { get; set; }

        By UploadedFilePath = By.XPath("//*[@id='uploadedFilePath']");

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

        By BtnChooseFile = By.XPath("//*[@id='uploadFile']");


        By GridRecords = By.XPath("//button[@id = 'addNewRecordButton']/../../following-sibling::div//div[@class='rt-td']");

        By RecordToDelete(string RecordToDelete)
        {
            return By.XPath("//button[@id = 'addNewRecordButton']/../../following-sibling::div//div[@class='rt-td' and text() ='" + RecordToDelete + "']/parent::div//span[@title='Delete']");
        }

        #endregion

        public void ClickOnLeftPaneElement(IWebDriver Driver, string groupheader, string ElementName)
        {
            WebDriverWait BrowserWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));

            if (BrowserWait.Until(ExpectedConditions.ElementExists(DivGroupHeader(groupheader))).GetAttribute("class").Equals("element-list collapse"))
            {
                _UtilityClass.ScrollToElement(Driver, Driver.FindElement(GrouHeaderToClick(groupheader)));
                
                BrowserWait.Until(ExpectedConditions.ElementToBeClickable(GrouHeaderToClick(groupheader))).Click();
            }

            _UtilityClass.ScrollToElement(Driver, Driver.FindElement(LeftPaneElement(groupheader, ElementName)));
            IWebElement Ele = BrowserWait.Until(ExpectedConditions.ElementToBeClickable(LeftPaneElement(groupheader, ElementName)));

            _UtilityClass.JavaScriptClick(Driver, Ele);
           
        }

        public void SelectValueFromDroDown()
        {
            _UtilityClass.SelectValueFromResponsiveDDL(Driver, DDLSelectValue, DDLSelectValueEntry, "Group 1, option 2");

        }

        public void UploadFile(string FilePathFromToUpload)
        {

            UtilityClass _UtilityClass = new UtilityClass();
           WebDriverWait _wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            IWebElement EleBrowseButton = _wait.Until(ExpectedConditions.ElementIsVisible(BtnChooseFile));


            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", EleBrowseButton);
            _UtilityClass.TakeScreenShot("VIjay", Driver);


            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", EleBrowseButton);
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
            WebDriverWait _wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            IWebElement EleUploadedFilePath = _wait.Until(ExpectedConditions.ElementIsVisible(UploadedFilePath));
            _UtilityClass.TakeScreenShot("abc", Driver);
            return EleUploadedFilePath.Text.ToString();
        }

        public void DeleteRecordFromGrid(string NameReocrdsToDelete)
        {

            WebDriverWait _wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(GridRecords));

            _wait.Until(ExpectedConditions.ElementToBeClickable(RecordToDelete(NameReocrdsToDelete))).Click();

        }

        public bool VerifyDeletedRecordsInGrid(string DeletedRecords)
        {
            bool IsDeletedRecordExist = false;

            WebDriverWait _wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));

            _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(GridRecords));

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
