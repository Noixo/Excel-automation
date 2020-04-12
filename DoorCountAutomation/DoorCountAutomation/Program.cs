using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using System.Diagnostics;
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
            String kk = "", newCount = "";
            Boolean stable = false;
            Program p = new Program();
            IWebDriver driver;

            // ChromeDriver is just AWFUL because every version or two it breaks unless you pass cryptic arguments
            //AGRESSIVE: options.setPageLoadStrategy(PageLoadStrategy.NONE); // https://www.skptricks.com/2018/08/timed-out-receiving-message-from-renderer-selenium.html
            ChromeOptions options = new ChromeOptions();
            //options.AddArguments("start-maximized"); // https://stackoverflow.com/a/26283818/1689770
            options.AddArguments("enable-automation"); // https://stackoverflow.com/a/43840128/1689770
            //options.AddArguments("--headless"); // only if you are ACTUALLY running headless
            options.AddArguments("--no-sandbox"); //https://stackoverflow.com/a/50725918/1689770
            options.AddArguments("--disable-infobars"); //https://stackoverflow.com/a/43840128/1689770
            options.AddArguments("--disable-dev-shm-usage"); //https://stackoverflow.com/a/50725918/1689770
            options.AddArguments("--disable-browser-side-navigation"); //https://stackoverflow.com/a/49123152/1689770
            options.AddArguments("--disable-gpu"); //https://stackoverflow.com/questions/51959986/how-to-solve-selenium-chromedriver-timed-out-receiving-message-from-renderer-exc
            options.AddArguments("enable-features=NetworkServiceInProcess");

            //options.AddArguments(PageLoadStrategy.None);
            options.PageLoadStrategy = PageLoadStrategy.Normal;

            driver = new ChromeDriver(options);

            //driver.HideCommandPromptWindow = true;
            // navigate to URL  "https://l.vemcount.com/embed/pane/xIgeG9iTUypChTb" 'croydon

            if (args.Length == 0)
            {
                Console.WriteLine("Arguments empty. Aborting program.");
                System.Environment.Exit(1);
            } 

            Console.WriteLine("Attempting to grab door count. ");

            //create the reference for the browser  
            //IWebDriver driver = new ChromeDriver();

            //ChromeOptions options = new ChromeOptions();
            options.AddArgument("--log-level=3");
            //options.Proxy = null;
            //IWebDriver driver = new ChromeDriver(new ChromeOptions { Proxy = null });

            driver.Navigate().GoToUrl(args[0]);
            //Thread.Sleep(600);
            DateTimeOffset startTime = DateTimeOffset.Now;

            while (true)
            {
                bool success = UInt32.TryParse(args[1], out uint timeWait);
                if (DateTimeOffset.Now.Subtract(startTime).TotalMilliseconds > timeWait)
                    throw new TimeoutException();

                try
                {
                    kk = driver.FindElement(By.CssSelector("div.flex")).GetAttribute("innerHTML");
                    if (kk != "")
                    {
                        while (!stable)
                        {
                            kk = driver.FindElement(By.CssSelector("div.flex")).GetAttribute("innerHTML");
                            int tmp = kk.IndexOf("<span>") + 6;
                            int test = kk.IndexOf("</span>\r\n");

                            kk = kk.Substring(tmp, test - tmp);

                            newCount = kk;
                            Thread.Sleep(600);

                            kk = driver.FindElement(By.CssSelector("div.flex")).GetAttribute("innerHTML");
                            tmp = kk.IndexOf("<span>") + 6;
                            test = kk.IndexOf("</span>\r\n");

                            kk = kk.Substring(tmp, test - tmp);

                            if (kk == newCount)
                            {
                                p.doorCount(kk);
                                Console.Write("Success");
                                Debug.WriteLine("Success ", kk);
                                driver.Quit();
                                System.Environment.Exit(1);
                            }
                        }
                        //if (kk == "0")
                        //{
                        //    Thread.Sleep(10000);
                        //}
                        //p.doorCount(kk);
                        //p.doorCount(kk);
                        //close the browser  
                        //driver.Quit();
                    }
                }
                catch (Exception)
                {
                    Console.Write("Cannot find door count.");
                    kk = "";
                    p.doorCount(kk);
                    driver.Quit();
                    System.Environment.Exit(1);
                }
            }

            //Thread.Sleep(3500);
            //while 
            


        }
    }
 
}
