﻿using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using OpenQA.Selenium;
using ParallelExecution.TestUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelExecution.POM
{
    public class HomePage
    {
        private IWebDriver driver;

        #region Constructor

        public HomePage(IWebDriver driver)
        {
            this.driver = driver;
        }

        #endregion

        #region Enum

        public enum EnumLeftPaneGroupHeader
        {
            [Description("Elements")]
            Elements,

            [Description("Forms")]
            Forms,

            [Description("Alerts, Frame & Windows")]
            AlertFrameWindow,

            [Description("Widgets")]
            Widgets,

            [Description("Interactions")]
            Interactions

        }

        public enum EnumLeftPaneElementList
        {
            #region Elements           

            [Description("Text Box")]
            TextBox,

            [Description("Check Box")]
            CheckBox,

            [Description("Radio Button")]
            RadioButton,

            [Description("Web Tables")]
            WebTables,

            [Description("Buttons")]
            Buttons,

            [Description("Upload and Download")]
            UploadAndDownload,

            #endregion

            #region Widgets

            [Description("Select Menu")]
            SelectMenu,

            #endregion


            #region Forms

            [Description("Practice Form")]
            PracticeForm


            #endregion

        }

        #endregion

        #region Elemennts

        By By_elements = By.XPath("//*[@id='app']/div/div/div[2]/div/div[1]/div/div[2]");
        public IWebElement elements => driver.FindElement(By.XPath("//*[@id='app']/div/div/div[2]/div/div[1]/div/div[2]"));

        #endregion

        UtilityClass _UtilityClass = new UtilityClass();

        #region Methods

        public void ClickOnElements(IWebDriver Driver)
        {

            _UtilityClass.WaitForElementToBeClickable(Driver, By_elements);

            _UtilityClass.ScrollToElement(Driver, elements);
            elements.Click();

        }


        #endregion
    }
}
