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
        public void doorCount(String kk)
        {
 

            StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "\\DoorCount.txt");
            if (kk == "" || kk == "0")
            {
                sw.WriteLine("1");
            }
            else
            {
                sw.WriteLine("0");  //door count obtained
            }
            sw.WriteLine(kk);   //door count
                                //File.WriteAllLines(sw, );
                                //sw.WriteLine(1);   //door count recorded
            Console.Write("Door Count: " + kk);

            //Close the file
            sw.Close();
        }

        static void Main(string[] args)
        {
            String kk = "";
            Program p = new Program();

            //driver.HideCommandPromptWindow = true;
            // navigate to URL  "https://l.vemcount.com/embed/pane/xIgeG9iTUypChTbt" 'croydon

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

            driver.Navigate().GoToUrl(args[0]);

            DateTimeOffset startTime = DateTimeOffset.Now;

            while (true)
            {
                try
                {
                    kk = driver.FindElement(By.CssSelector("div.flex")).GetAttribute("innerHTML");
                    if (kk != "")
                    {
                        int tmp = kk.IndexOf("<span>") + 6;
                        int test = kk.IndexOf("</span>\r\n");

                        kk = kk.Substring(tmp, test - tmp);
                        p.doorCount(kk);
                        //p.doorCount(kk);
                        //close the browser  
                        driver.Quit();
                    }
                }
                catch (Exception)
                {
                    Console.Write("Cannot find door count.");
                    kk = "";
                    p.doorCount(kk);
                }

                if (DateTimeOffset.Now.Subtract(startTime).TotalMilliseconds > 10000)
                    throw new TimeoutException();
            }

            //Thread.Sleep(3500);
            //while 
            


        }
    }
 
}
