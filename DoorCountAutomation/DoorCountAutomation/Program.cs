using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace SeleniumTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //driver.HideCommandPromptWindow = true;
            // navigate to URL  "https://l.vemcount.com/embed/pane/xIgeG9iTUypChTb"
            if (args.Length == 0)
            {
                Console.WriteLine("Arguments empty. Aborting program.");
                System.Environment.Exit(1);
            }

            Console.WriteLine("Attempting to grab door count. ");

            //create the reference for the browser  
            //IWebDriver driver = new ChromeDriver();

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--log-level=3");
            //options.Proxy = null;
            IWebDriver driver = new ChromeDriver(new ChromeOptions { Proxy = null });

            //string driverPath = @"C:\where ever the driver it\";
            //var chromeOptions = new ChromeOptions();
            //chromeOptions.Proxy = null;
            //IWebDriver browser = new ChromeDriver(driverPath, chromeOptions);


            driver.Navigate().GoToUrl(args[0]);

            Thread.Sleep(3000);

            String kk = driver.FindElement(By.CssSelector("div.flex")).GetAttribute("innerHTML");

            int tmp = kk.IndexOf("<span>") + 6;
            int test = kk.IndexOf("</span>\r\n");

            kk = kk.Substring(tmp, test - tmp);

            StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "\\DoorCount.txt");
            sw.WriteLine(kk);   //door count
            //File.WriteAllLines(sw, );
            //sw.WriteLine(1);   //door count recorded
            Console.Write("Door Count: " + kk);
            //Close the file
            sw.Close();

            //close the browser  
            driver.Quit();
        }
    }
}
