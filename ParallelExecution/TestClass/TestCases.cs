using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using ParallelExecution.POM;
using ParallelExecution.TestUtility;
using System;
using System.Configuration;
using System.IO;
using static ParallelExecution.POM.HomePage;

namespace ParallelExecution.TestClass
{
    [TestClass]
    public class TestCases
    {
        private ServiceProvider _serviceProvider;
        private IServiceScope _scope;
        private IWebDriver _driver;

        // Page objects and utilities injected from DI
        private HomePage _homePage;
        private ElementPage _elementPage;
        private PracticeForm _practiceForm;
        private UtilityClass _utilityClass;
        private ExcelUtility _excelUtility;

        [TestInitialize]
        public void Setup()
        {
            _serviceProvider = ServiceRegistration.ConfigureServices();
            _scope = _serviceProvider.CreateScope();

            var provider = _scope.ServiceProvider;

            _driver = provider.GetRequiredService<IWebDriver>();
            _homePage = provider.GetRequiredService<HomePage>();
            _elementPage = provider.GetRequiredService<ElementPage>();
            _practiceForm = provider.GetRequiredService<PracticeForm>();
            _utilityClass = provider.GetRequiredService<UtilityClass>();
            _excelUtility = provider.GetRequiredService<ExcelUtility>();

            // Navigate to base URL
            _driver.Navigate().GoToUrl(ConfigurationManager.AppSettings["URL"]);
        }

        [TestCleanup]
        public void TearDown()
        {
            _driver.Quit();
            _scope.Dispose();
            _serviceProvider.Dispose();
        }

        [TestMethod]
        public void Test2_Test()
        {
            _homePage.ClickOnElements(_driver);
            _utilityClass.WaitForAjaxLoad(_driver);
            _elementPage.ClickOnLeftPaneElement(_driver,
                _utilityClass.GetDescriptionFromEnum(EnumLeftPaneGroupHeader.Elements),
                _utilityClass.GetDescriptionFromEnum(EnumLeftPaneElementList.CheckBox));
        }

        [TestMethod]
        public void Test5_VerifyUploadFunctionality()
            
        {
            string rootpath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            string filepath = Path.Combine(rootpath,"FilesToUPload", "Picture.jpg");
            Console.WriteLine(filepath);

            _homePage.ClickOnElements(_driver);
            _utilityClass.WaitForAjaxLoad(_driver);

            _elementPage.ClickOnLeftPaneElement(_driver,
                _utilityClass.GetDescriptionFromEnum(EnumLeftPaneGroupHeader.Elements),
                _utilityClass.GetDescriptionFromEnum(EnumLeftPaneElementList.UploadAndDownload));

            _elementPage.UploadFile(filepath);
            Assert.AreEqual(@"C:\fakepath\Picture.jpg", _elementPage.MethodUploadedFilePath());
        }
    }
}
