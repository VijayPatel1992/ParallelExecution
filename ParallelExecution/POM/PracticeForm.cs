using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ParallelExecution.TestUtility;

namespace ParallelExecution.POM
{
    public class PracticeForm
    {
        #region Webdriver 

        private IWebDriver Driver;
        UtilityClass _UtilityClass = new UtilityClass();

        #endregion

        #region Constructor
        public PracticeForm(IWebDriver Driver)
        {
            this.Driver = Driver;
            
        }

        #endregion

        #region Elements

        By FirstName = By.Id("firstName");       
        public IWebElement TxtBoxLastName => Driver.FindElement(By.Id("lastName"));
        public IWebElement TxtBoxUserEmail => Driver.FindElement(By.Id("userEmail"));

        By RadioBtnGender(string Gender)
        {
            return By.XPath("//*[@id='genterWrapper']//input/../label[text() = '" + Gender + "']");
        }


        public IWebElement TxtBoxMobileNumber => Driver.FindElement(By.Id("userNumber"));

        public IWebElement DatePickerDateOfBirth => Driver.FindElement(By.Id("dateOfBirthInput"));

        public IWebElement TxtBoxSubject => Driver.FindElement(By.Id("subjectsInput"));


        string JSPathHobbiesCheckbox(int Index)
        {
            return "return document.querySelector('#hobbiesWrapper>div div:nth-child(" + Index + ") input')"
;
        }
        public IWebElement InputBoxChooseFile => Driver.FindElement(By.Id("uploadPicture"));


        By BtnChooseFile = By.XPath("//*[@id='uploadPicture']");
        public IWebElement TxtBoxCurrentAddress => Driver.FindElement(By.Id("currentAddress"));

        public IWebElement DDLSelectState => Driver.FindElement(By.XPath("//*[@id='state']"));

       

        By DDLStateOptions = By.XPath("//*[@id='state']//div[contains(@id, 'react-select')]");

        public IWebElement DDLSelectCity => Driver.FindElement(By.XPath("//*[@id='city']"));

        public IWebElement BtnSubmit => Driver.FindElement(By.Id("submit"));


        By DDLCityOptions = By.XPath("//*[@id='city']//div[contains(@id, 'react-select')]");


        By TableFieldValue(string FieldName)
        {
            return By.XPath("//div[text() ='Thanks for submitting the form']/ancestor::div[@class= 'modal-content']//tr/td[text() = '" + FieldName + "']/following-sibling::td");
        }

        #endregion

        #region Methods

        public void FillPracticeForm(int DtRowPtr, DataTable PracticeFormData, string UploadFilePath)
        {
            WebDriverWait BrowserWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            Actions _Actions = new Actions(Driver);
            //_Actions.DragAndDrop();
            IJavaScriptExecutor Js = (IJavaScriptExecutor)Driver;

            #region Fill basic details


            //Wait for First Name textbox becomes clickable and then send key.

            
            BrowserWait.Until(d => Driver.FindElement(FirstName)).SendKeys(PracticeFormData.Rows[DtRowPtr]["FirstName"].ToString());
            _UtilityClass.ScrollToElement(Driver, TxtBoxLastName);

            //Fill value for Last Name
            TxtBoxLastName.SendKeys(PracticeFormData.Rows[DtRowPtr]["LastName"].ToString());
           

            //Fill value for Email
            TxtBoxUserEmail.SendKeys(PracticeFormData.Rows[DtRowPtr]["Email"].ToString());

            //Select Gender Radio button.
            Driver.FindElement(RadioBtnGender(PracticeFormData.Rows[DtRowPtr]["Gender"].ToString())).Click();
            

            //Fill value for Mobile number
            TxtBoxMobileNumber.SendKeys(PracticeFormData.Rows[DtRowPtr]["MobileNumber"].ToString());

            #endregion

            #region Birth Date
            _UtilityClass.ScrollToElement(Driver, DatePickerDateOfBirth);
            Thread.Sleep(1000);
            DatePickerDateOfBirth.Click();
            _Actions.KeyDown(Keys.Control).SendKeys("A").KeyUp(Keys.Control).Build().Perform();
            DatePickerDateOfBirth.SendKeys(PracticeFormData.Rows[DtRowPtr]["DateOfBirthd"].ToString());
            DatePickerDateOfBirth.SendKeys(Keys.Enter);
            Thread.Sleep(1000);

            #endregion

            #region Subject

            _UtilityClass.ScrollToElement(Driver, TxtBoxSubject);
            //Fill value of subject
            TxtBoxSubject.Click();

            string[] ArrSubject = PracticeFormData.Rows[DtRowPtr]["Subject"].ToString().Split(';');

            foreach (string SubjectToSelect in ArrSubject)
            {
                TxtBoxSubject.SendKeys(SubjectToSelect);
                TxtBoxSubject.SendKeys(Keys.Enter);
                Thread.Sleep(500);
            }

            #endregion

            #region Hobbies

            string[] Hobbies = PracticeFormData.Rows[DtRowPtr]["Hobbies"].ToString().Split(';');
            IList<IWebElement> EleHobbies = Driver.FindElements(By.XPath("//*[@id='hobbiesWrapper']//input/parent::div"));

            foreach (string Hoby in Hobbies)
            {
                for (int i = 0; i < EleHobbies.Count; i++)
                {
                    if (Hoby.Equals(EleHobbies[i].Text.ToString()))
                    {
                        IWebElement checkbox = (IWebElement)Js.ExecuteScript(JSPathHobbiesCheckbox(i + 1));

                        if (!(bool)((IJavaScriptExecutor)Driver).ExecuteScript("return arguments[0].checked;", checkbox))
                        {
                            EleHobbies[i].Click();
                        }
                    }

                }
            }

            #endregion

            #region File Upload

             WebDriverWait _wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            _wait.Until(d => Driver.FindElement(BtnChooseFile));
            IWebElement EleBrowseButton = Driver.FindElement(By.Id("uploadPicture"));
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", EleBrowseButton);
            //EleBrowseButton.Click();
            Thread.Sleep(2000);
            _UtilityClass.FileUploader(Path.Combine(UploadFilePath, PracticeFormData.Rows[DtRowPtr]["PicturePath"].ToString()));

            #endregion

            #region Current Address

            _UtilityClass.ScrollToElement(Driver, TxtBoxCurrentAddress);
            TxtBoxCurrentAddress.SendKeys(PracticeFormData.Rows[DtRowPtr]["CurrentAddress"].ToString());

            #endregion

            #region Select State
            DDLSelectState.Click();
            //_Actions.KeyDown(Keys.Tab).KeyUp(Keys.Tab).Build().Perform();
            //Thread.Sleep(500);
            //_Actions.KeyDown(Keys.Space).KeyUp(Keys.Space).Build().Perform();
            //Thread.Sleep(500);
            BrowserWait.Until(d => Driver.FindElement(DDLStateOptions));
            IList<IWebElement> StateOptions = Driver.FindElements(DDLStateOptions);

            
            foreach (IWebElement State in StateOptions)
            {
                if (PracticeFormData.Rows[DtRowPtr]["State"].ToString().Equals(State.Text.ToString()))
                {

                    _UtilityClass.ScrollToElement(Driver, State);
                    Thread.Sleep(500);
                    State.Click();
                    break;
                }
            }

            #endregion

            #region Select City
            Thread.Sleep(3000);
            //TestUtility.UtilityClass.JavaScriptClick(DDLSelectCity);
            DDLSelectCity.Click();



            //_Actions.KeyDown(Keys.Tab).KeyUp(Keys.Tab).Build().Perform();
            //Thread.Sleep(500);
            //_Actions.KeyDown(Keys.Space).KeyUp(Keys.Space).Build().Perform();
            //Thread.Sleep(500);
            BrowserWait.Until(d => Driver.FindElement(DDLCityOptions));
            IList<IWebElement> CityOptions = Driver.FindElements(DDLCityOptions);

          

            //IList<IWebElement> StateOptions = Driver.FindElements(By.XPath("//*[@id='state']//div[contains(@id, 'react-select')]"));
            foreach (IWebElement City in CityOptions)
            {
                if (PracticeFormData.Rows[DtRowPtr]["City"].ToString().Equals(City.Text.ToString()))
                {
                    //_UtilityClass.JavaScriptClick(Driver, City);
                    City.Click();
                    break;
                }
            }

            #endregion

            //Click on submie button with java script executor.
            Thread.Sleep(3000);
            BtnSubmit.Click();
            

        }

        public string GetAnyFieldValueOfSubmittedForm(string FieldName)
        {
            WebDriverWait BrowserWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));

            return BrowserWait.Until(d => Driver.FindElement(TableFieldValue(FieldName))).Text;
        }

        #endregion
    }
}
