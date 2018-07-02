using System;
using System.Collections.Generic;
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

            var spaced = CalculateDistance($"[ {cName}: {methodName} ]");
            
            WriteLine($"{spaced}  ->  {message}");
        }

        public static void Print(string message, ConsoleColor textColor,
            [CallerMemberName] string methodName = "",
            [CallerFilePath] string className = "") {

            var cName = className.Split('\\').Last().Split('.').First();
            
            ForegroundColor = textColor;
            
            var spaced = CalculateDistance($"[ {cName}: {methodName} ]");
            
            WriteLine($"{spaced}  ->  {message}");
            
            ForegroundColor = ConsoleColor.White;
        }

        public static bool ElementIsPresent(ISearchContext element, By by) {
            var all = element.FindElements(by);
            //Print($"Found elements for {by} {all.Count}");
            return all.Count > 0;
        }

        public static string ExtractTextValueFromElement(ISearchContext element, bool scrapDivs) {
            var res = new List<string>();
            var allSpans = element.FindElements(By.CssSelector("span"));
            var allDivs = element.FindElements(By.CssSelector("div"));
            var allAs = element.FindElements(By.CssSelector("a"));
            
            foreach (var webElement in allSpans) {
                if (webElement?.Text.Length > 1 && !SuperContains(res, webElement?.Text)) {
                    res.Add(webElement?.Text);
                }
            }

            if (scrapDivs) {
                foreach (var webElement in allDivs) {
                    if (webElement?.Text.Length > 1 && !SuperContains(res, webElement?.Text)) {
                        res.Add(webElement?.Text);
                    }
                }
            }
            
            foreach (var webElement in allAs) {
                var href = webElement?.GetAttribute("href");
                if (href?.Length > 1 && !res.Contains(href)) {
                    res.Add(href);
                }
            }

            return string.Join(" | ", res);
        }

        private static bool SuperContains(IEnumerable<string> list, string s) {
            return list.Any(s1 => OneInTheOther(s1, s));
        }

        private static bool OneInTheOther(string a, string b) {
            return a.Length > b.Length ? a.Contains(b) : b.Contains(a);
        }

        private static string CalculateDistance(string s) {
            for (var i = s.Length; i < 50; i++) {
                s += " ";
            }
            return s;
        }
    }
}