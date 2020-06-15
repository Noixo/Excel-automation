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
using System.Timers;
using System.Net;

namespace SeleniumTest
{
	class Program
	{
		private static System.Timers.Timer aTimer;
		private static IWebDriver driver;

		private static readonly Program p = new Program();

		private void exitProgram()
		{
			System.Environment.Exit(1);
		}
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
			Console.WriteLine("Door Count: " + kk);

			//Close the file
			sw.Close();
		}

		public void openUsingSelenium(string url, int timeWaitToStable)
		{
			String kk = "", newCount = "";

			//driver.HideCommandPromptWindow = true;
			// navigate to URL  "https://l.vemcount.com/embed/pane/xIgeG9iTUypChTb" 'croydon

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

			//IWebDriver driver;
			driver = new ChromeDriver(options);

			options.AddArgument("--log-level=3");
			driver.Navigate().GoToUrl(url);

			while (true)
			{
				kk = returnDoorCount();

				if (kk != "" && kk != "0")
				{
					kk = returnDoorCount();

					newCount = kk;
					Thread.Sleep(timeWaitToStable); //1000 ideal

					kk = returnDoorCount();

					if (kk == newCount)
					{
						p.doorCount(kk);
						driver.Quit();
						p.exitProgram();
					}
				}
			}

		}

		public void openUsingHTTP(string url)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			Stream resStream = response.GetResponseStream();

			using (StreamReader reader = new StreamReader(resStream))
			{
				string contents = reader.ReadToEnd();
				int startOffset = contents.IndexOf(":") + 1;    //+1 to grab first number element
				int endOffset = contents.IndexOf(",");
				contents = contents.Substring(startOffset, (endOffset - startOffset));

				Console.WriteLine("DoorCount: {0}", contents);
				p.doorCount(contents);
				//string time = (localDate - DateTime.Now).ToString("ss");
				//Console.WriteLine("time taken: {0}", time);
			}
		}
		
		/*args[]
		 * 0: URL
		 * 1: overall timeout value
		 * 2: wait value after website has been opened
		*/
		static void Main(string[] args)
		{
			//Program p = new Program();
			//DateTime localDate = DateTime.Now;
			//String kk = "", newCount = "";

			if (args.Length == 0)
			{
				Console.WriteLine("Arguments empty. Aborting program.");
				p.exitProgram();
			}


			Console.WriteLine("Attempting to grab door count. ");

			UInt32.TryParse(args[1], out uint timeOutVal);
			Int32.TryParse(args[2], out int timeWaitToStable);

			SetTimer(timeOutVal);    //Set global time out timer in case something gets stuck		//40000

			//check which version to use
			if (args[0].Contains("data"))	//if the url is for the json code, use HTTP request
			{
				p.openUsingHTTP(args[0]);
			}
			else if(args[0].Contains("pane"))  //if just the raw website link, open selenium
			{
				Console.WriteLine("warning: This might stop working if google chrome is updated. Using json version is more recommended");
				p.openUsingSelenium(args[0], timeWaitToStable);
			}
			else
			{
				Console.WriteLine("ERROR: supplied web url not expected.");
				p.doorCount("");
				p.exitProgram();
			}
		}
		private static string returnDoorCount()
		{
			String doorCount = "";
			doorCount = driver.FindElement(By.CssSelector("div.flex")).GetAttribute("innerHTML");
			int tmp = doorCount.IndexOf("<span>") + 6;
			int test = doorCount.IndexOf("</span>\r\n");
			doorCount = doorCount.Substring(tmp, test - tmp);
			return doorCount;
		}
		private static void SetTimer(uint timer)
		{
			// Create a timer with a two second interval.
			aTimer = new System.Timers.Timer(timer);
			// Hook up the Elapsed event for the timer. 
			aTimer.Elapsed += OnTimedEvent;
			aTimer.AutoReset = true;
			aTimer.Enabled = true;
		}
		//once timer expires
		private static void OnTimedEvent(Object source, ElapsedEventArgs e)
		{
			Program p = new Program();
			Console.WriteLine("Cannot find door count.");
			p.doorCount("");
			aTimer.Stop();
			aTimer.Dispose();
			p.exitProgram();
		}
	}
}
