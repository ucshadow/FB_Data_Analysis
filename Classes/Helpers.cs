using System;
using System.Linq;
using System.Runtime.CompilerServices;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using static System.Console;

namespace FB_Data_Analysis.Classes {
    public static class Helpers {
        public static void Wait(double delay, double interval) {
            // Causes the WebDriver to wait for at least a fixed delay
            var now = DateTime.Now;
            var wait = new WebDriverWait(SeleniumProvider.Driver, TimeSpan.FromMilliseconds(delay)) {
                PollingInterval = TimeSpan.FromMilliseconds(interval)
            };
            wait.Until(wd => (DateTime.Now - now) - TimeSpan.FromMilliseconds(delay) > TimeSpan.Zero);
        }


        public static void ChangeImplicitWaitTimeout(int seconds) {
            SeleniumProvider.Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(seconds);
        }

        public static void Print(string message,
            [CallerMemberName] string methodName = "",
            [CallerFilePath] string className = "") {
            
            var cName = className.Split('\\').Last().Split('.').First();
            
            ForegroundColor = ConsoleColor.White;
            WriteLine($"[ {cName}: {methodName} ]  ->  {message}");
        }

        public static void Print(string message, ConsoleColor textColor,
            [CallerMemberName] string methodName = "",
            [CallerFilePath] string className = "") {

            var cName = className.Split('\\').Last().Split('.').First();
            
            ForegroundColor = textColor;
            WriteLine($"[ {cName}: {methodName} ]  ->  {message}");
            ForegroundColor = ConsoleColor.White;
        }

        public static bool ElementIsPresent(ISearchContext element, By by) {
            var all = element.FindElements(by);
            //Print($"Found elements for {by} {all.Count}");
            return all.Count > 0;
        }
    }
}