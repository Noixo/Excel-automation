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
            Console.Write("test case started ");
            //create the reference for the browser  
            IWebDriver driver = new ChromeDriver();
            //driver.HideCommandPromptWindow = true;
            // navigate to URL  
            driver.Navigate().GoToUrl("https://l.vemcount.com/embed/pane/xIgeG9iTUypChTb");
            Thread.Sleep(2000);
            // identify the Google search text box  
            //IWebElement ele = driver.FindElement(By.Name("q"));
            //enter the value in the google search text box  
            //ele.SendKeys("javatpoint tutorials");
            //Thread.Sleep(2000);
            //identify the google search button  

            //IWebElement ele1 = driver.FindElement(By.ClassName("w-full"));

            //driver.FindElement(By.XPath("//div[@class='w-full']/span[@class='']"));
            //IWebElement kk = driver.FindElement(By.XPath("'.//*[@id='primary']/div[2]/div[1]/div[1]/span"))   // FindElement(By.CssSelector("div.w-full>span>b"));            // driver.FindElement(By.ClassName("w-full"));//('ytp-menu-label-secondary')
            //kk = driver.FindElement(By.XPath("//span[@class='w-full']"));

             String kk = driver.FindElement(By.CssSelector("div.flex")).GetAttribute("innerHTML");

            //kk = getBetween(kk, "<span>", "</span>\r\n");   //return doorcount
            int tmp = kk.IndexOf("<span>") + 6;
            int tmpTwo = kk.IndexOf("</span>\r\n") - 11;
            int test = kk.IndexOf("</span>\r\n");

            kk = kk.Substring(tmp, test - tmp);

            // tmp = kk.IndexOf('<');



            //String kk = driver.FindElement(By.CssSelector("#w-full span.flex")).GetAttribute("innerHTML");

            //IWebElement kk = driver.FindElement(By.XPath(".//div[@class='w-full']/span"));   //.GetAttribute("span");  //By.XPath("'.//*[@id='w-full']/div[2]/div[1]/div[1]/span"));

            //System.out.println(HeaderTxtElem.getText());

            //try
            //{

                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() +  "DoorCount.txt");

                //Write a line of text
                sw.WriteLine(kk);

                //Write a second line of text
                //sw.WriteLine("From the StreamWriter class");

                //Close the file
                sw.Close();
            //}
      

            //kk = driver.FindElement(By.CssSelector("div.flex")).GetAttribute("innerHTML");
            // click on the Google search button  
            // ele1.Click();
            //Thread.Sleep(3000);
            //close the browser  
            driver.Quit();
            //Console.Write("test case ended ");
        }
    }
}
