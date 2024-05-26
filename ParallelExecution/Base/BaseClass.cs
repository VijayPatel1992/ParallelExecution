using IronXL.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using ParallelExecution.TestReporter;
using ParallelExecution.TestUtility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace ParallelExecution.Base
{
    public class BaseClass
    {
        public IWebDriver Driver;
        public static string Browser = ConfigurationManager.AppSettings["Browser"].ToUpper();
        public static string rootpath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
        public static string ReportPath = Path.Combine(Path.GetDirectoryName(Directory.GetParent(Directory.GetParent(rootpath).ToString()).ToString()), "ReportAndScreenShot-" + DateTime.Now.ToString("yyyy-MM-dd"));
        public static string ScreenSortPath = Path.Combine(ReportPath, "Screenshot");
        
        public static string CreatedExcelFilePath = Path.Combine(ReportPath, "ExcelFiles");
       // private static ExtentReports ReportManager { get; set; }
        public static string HtmlReportFullPath { get; set; }

        public TestContext TestContext { get; set; }

      //  private static ExtentTest CurrentTestCase { get; set; }

        [TestInitialize]
        public void InitializeTest()
        {
            CreateReportDirectory();
            //StartReporter();

            switch (Browser)
            {

                case "CHROME":
                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsBrowserOptionEnable"]).Equals(true))
                    {
                        ChromeOptions option = new ChromeOptions();
                        option.AddArgument("--headless");
                        option.AddArgument("start-maximized");
                        option.AcceptInsecureCertificates = true;
                        option.AddUserProfilePreference("disable-popup-blocking", "true");
                        //Driver = new ChromeDriver(rootpath + "\\Driver\\chromedriver.exe", option);
                        Driver = new ChromeDriver();
                    }
                    else
                    
                    {
                        Driver = new ChromeDriver();
                        Driver.Manage().Window.Maximize();
                    }
                    break;

                case "FIREFOX":

                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsBrowserOptionEnable"]).Equals(true))
                    {
                        FirefoxOptions option = new FirefoxOptions();
                        option.AddArgument("start-maximized");
                        option.AcceptInsecureCertificates = true;
                        option.SetPreference("disable-popup-blocking", "true");
                        Driver = new FirefoxDriver(rootpath + "\\Driver\\geckodriver.exe", option);

                    }
                    else
                    {
                        Driver = new FirefoxDriver();
                        Driver.Manage().Window.Maximize();

                    }
                    break;

                 default:
                    Driver = new ChromeDriver(rootpath + "\\Driver\\chromedriver.exe");
                    Driver.Manage().Window.Maximize();
                    break;

               
            }
            Driver.Url = ConfigurationManager.AppSettings["URL"];
            Driver.Manage().Window.Maximize();
            
        }


        public  void CreateReportDirectory()
        {
            DirectoryInfo ReportDirectory = new System.IO.DirectoryInfo(BaseClass.ScreenSortPath);

            if (!ReportDirectory.Exists)
                Directory.CreateDirectory(BaseClass.ReportPath);
            Directory.CreateDirectory(BaseClass.ScreenSortPath);
            Directory.CreateDirectory(BaseClass.CreatedExcelFilePath);

            HtmlReportFullPath = $"{BaseClass.ReportPath}\\TestResults.html";
        }

        [ClassInitialize]
        public void Reportstarting()
        {
            CreateReportDirectory();
            StartReporter();

        }

        [ClassCleanup]
        public void ReportFlush()
        {
            //ReportManager.Flush();
        }

        public  void StartReporter()
        {
            //var htmlReporter = new ExtentHtmlReporter(HtmlReportFullPath);
            //ReportManager = new ExtentReports();
            ////htmlReporter.LoadConfig(BaseClass.rootpath + "\\extent-config.xml");
            //ReportManager.AttachReporter(htmlReporter);
            //ReportManager.AddSystemInfo("Environment", "QA");
            //ReportManager.AddSystemInfo("User Name", "Vija Patel");
            

            //CurrentTestCase = ReportManager.CreateTest(TestContext.TestName);
        }


        [TestCleanup]
        public void TearDown()
        {
            if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed)
            {
                UtilityClass _UtilityClass = new UtilityClass();
                _UtilityClass.TakeScreenShot(TestContext.TestName, Driver);
            }
            
            Driver.Dispose();        
        }
    }
}
