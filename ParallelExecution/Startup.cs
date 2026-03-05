using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ParallelExecution.POM;
using ParallelExecution.TestUtility;

namespace ParallelExecution
{
    public static class ServiceRegistration
    {
        // Provide a trivial entry point so project can compile as an executable if required by build
        // This is a no-op main used only to satisfy the compiler when OutputType is Exe.
        public static void Main(string[] args)
        {
            // Intentionally left blank
        }

        public static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Register WebDriver
            services.AddScoped<IWebDriver>(provider =>
            {
                var options = new ChromeOptions();
                options.AddArgument("--start-maximized");
                return new ChromeDriver(options);
            });

            // Register Page Objects
            services.AddTransient<HomePage>();
            services.AddTransient<ElementPage>();
            services.AddTransient<PracticeForm>();

            // Register Utilities
            services.AddTransient<UtilityClass>();
            services.AddTransient<ExcelUtility>();

            return services.BuildServiceProvider();
        }
    }
}
