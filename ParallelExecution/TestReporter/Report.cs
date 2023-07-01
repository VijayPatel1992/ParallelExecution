using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelExecution.TestReporter
{
    public static class Report
    {
        
            private static ExtentReports extent;
            private static ExtentHtmlReporter htmlReporter;

            public static ExtentReports GetInstance()
            {
                if (extent == null)
                {
                    string reportPath = @"..\..\Reports\ExtentReport.html";
                    htmlReporter = new ExtentHtmlReporter(reportPath);
                    extent = new ExtentReports();
                    extent.AttachReporter(htmlReporter);
                }
                return extent;
            }
        }
}
