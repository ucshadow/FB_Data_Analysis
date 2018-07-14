using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using static System.Console;
using static System.Diagnostics.Stopwatch;

namespace FB_Data_Analysis.Classes {
    public static class Helpers {
        public static int GlobalDelay = 1500;

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

            var t = DateTime.Now;
            var h = t.Hour < 10 ? $"0{t.Hour}" : $"{t.Hour}";
            var m = t.Minute < 10 ? $"0{t.Minute}" : $"{t.Minute}";
            var s = t.Second < 10 ? $"0{t.Second}" : $"{t.Second}";
            var time = $"{h}:{m}:{s}";
            WriteLine($"{time} {spaced}  ->  {message}");
        }

        public static void Print(string message, ConsoleColor textColor,
            [CallerMemberName] string methodName = "",
            [CallerFilePath] string className = "") {
            var cName = className.Split('\\').Last().Split('.').First();

            ForegroundColor = textColor;

            var spaced = CalculateDistance($"[ {cName}: {methodName} ]");

            var t = DateTime.Now;
            var h = t.Hour < 10 ? $"0{t.Hour}" : $"{t.Hour}";
            var m = t.Minute < 10 ? $"0{t.Minute}" : $"{t.Minute}";
            var s = t.Second < 10 ? $"0{t.Second}" : $"{t.Second}";
            var time = $"{h}:{m}:{s}";
            WriteLine($"{time} {spaced}  ->  {message}");

            ForegroundColor = ConsoleColor.White;
        }

        public static bool ElementIsPresent(ISearchContext element, By by) {
            var all = element.FindElements(by);
            return all.Count > 0;
        }

        public static IWebElement ElementIsPresent(ISearchContext element, By by, bool get) {
            var all = element.FindElements(by);
            return all.Count > 0 ? all[0] : null;
        }

        public static ReadOnlyCollection<IWebElement> ElementIsPresent(ISearchContext element, By by, int get) {
            var all = element.FindElements(by);
            return all;
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
            for (var i = s.Length; i < 55; i++) {
                s += " ";
            }

            return s;
        }

        /// <summary>
        /// Scroll x times
        /// used var instead of let/const beacause redeclaration needs to happen
        /// if the same script is ran on the same page more than once
        /// </summary>
        /// <param name="times"></param>
        /// <param name="timeBetween"></param>
        public static void ScrollXTimes(int times, int timeBetween) {
            SeleniumProvider.Driver
                .ExecuteScript("var count = 0;" +
                               "var x = setInterval(() => {" +
                               "    scrollBy(0, 2000);" +
                               "    count++;" +
                               $"    if(count > {times}) " + "{" +
                               "        clearInterval(x);" +
                               "    }" +
                               "}" + $", {timeBetween});");
        }

        public static void ScrollToElement(By by) {
            var element = SeleniumProvider.Driver.FindElement(by);
            var action = new Actions(SeleniumProvider.Driver);
            action.MoveToElement(element);
            action.Perform();
            Wait(1000, 500);
        }

        public static void ScrollToElement(IWebElement element) {
            var action = new Actions(SeleniumProvider.Driver);
            action.MoveToElement(element);
            action.Perform();
            Wait(1000, 500);
        }

        public static void ScrollToElement(string elementId) {
            SeleniumProvider.Driver.ExecuteScript($"document.getElementById('{elementId}').scrollIntoView()");
            Wait(1000, 500);
        }

        public static void ScrollUpSome() {
            SeleniumProvider.Driver.ExecuteScript("scrollBy(0, -1000)");
            Wait(500, 100);
        }

        public static void ScrollToCheckIns() {
            var body = SeleniumProvider.Driver.FindElement(By.CssSelector("body"));
            for (var i = 0; i < 2; i++) {
                body.SendKeys(Keys.PageDown);
                Wait(1000, 500);
            }
        }

        public static void ScrollToBottom() {
            var driver = SeleniumProvider.Driver;

            var body = driver.FindElement(By.CssSelector("body"));
            var curHeight = driver.ExecuteScript("return document.body.scrollHeight + ''");

            while (true) {
                body.SendKeys(Keys.PageDown);
                Wait(GlobalDelay, 500);
                var q = driver.ExecuteScript("return document.body.scrollHeight + ''");

                int.TryParse((string) q, out var a);
                int.TryParse((string) curHeight, out var b);

                if (a == b) {
                    return;
                }

                curHeight = q;
            }
        }

        public static void ScrollToBottom(int milisecondsInterval) {
            var watch = StartNew();

            var driver = SeleniumProvider.Driver;

            var body = driver.FindElement(By.CssSelector("body"));
            var curHeight = driver.ExecuteScript("return document.body.scrollHeight + ''");

            while (true) {
                body.SendKeys(Keys.PageDown);
                body.SendKeys(Keys.PageDown);
                body.SendKeys(Keys.PageDown);
                body.SendKeys(Keys.PageDown);
                Wait(milisecondsInterval, 100);
                var q = driver.ExecuteScript("return document.body.scrollHeight + ''");

                int.TryParse((string) q, out var a);
                int.TryParse((string) curHeight, out var b);


//                if (watch.ElapsedMilliseconds > 1000 * maxTimeInSeconds) {
//                    watch.Stop();
//                    Print($"Limit {maxTimeInSeconds} reached {watch.ElapsedMilliseconds / 1000}, stopping...");
//                    return;
//                }

                if (a == b) {
                    // double check with delay
                    Wait(2000, 1000);

                    var q2 = driver.ExecuteScript("return document.body.scrollHeight + ''");
                    int.TryParse((string) q2, out var a2);
                    int.TryParse((string) curHeight, out var b2);

                    if (a2 == b2) {
                        watch.Stop();
                        Print($"Done in {watch.ElapsedMilliseconds / 1000} seconds");

                        return;
                    }

                    Print($"Fake done detecked, scrolling...", ConsoleColor.Magenta);
                    //ScrollToBottom(milisecondsInterval);
                }

                curHeight = q;
            }
        }

        public static void ClickOnMore(string elementId) {
            //var element = SeleniumProvider.Driver.FindElementById(elementId);
            SeleniumProvider.Driver.FindElement(By.XPath($"//div[@id='{elementId}']//a[@class='_3t3']")).Click();
        }

        public static void GoBackToAbout() {
            var but = SeleniumProvider.Driver.FindElementsByClassName("_6-6")[1];
            ScrollToElement("fbProfileCover");
            but.Click();
            Wait(GlobalDelay, 1000);
        }

        public static bool TabIsPresent(string tabName) {
            var allTabs = SeleniumProvider.Driver.FindElementsByClassName("_51sx");
            foreach (var webElement in allTabs) {
                if (webElement?.Text.Trim() == tabName) {
                    return true;
                }
            }

            return false;
        }

        public static void OpenNewTabAndFocus() {
//            SeleniumProvider.Driver.ExecuteScript("window.open('about:blank', '-blank')");
//            Wait(500, 100);
//            var tabs = SeleniumProvider.Driver.WindowHandles;
//            SeleniumProvider.Driver.SwitchTo().Window(tabs.Last());
            
            const string script = "var d=document;" +
                                  //"var x = d.createElement('div');" +
                                  "var a = d.createElement('a');" +
                                  "a.id='clickid12';" +
                                  "a.target='_blank';" +
                                  "a.href='"+ "about:blank" + "';" +
                                  "a.innerHTML='dhkasjhdksahdkash';" +
                                  "var parent = d.getElementById('globalContainer') || d.getElementById('viewport');" + 
                                  //"x.appendChild(a)" +
                                  "parent.appendChild(a);";
            // Execute the JavaScript
            SeleniumProvider.Driver.ExecuteScript(script);
            // Click the new element
            Wait(1000, 100);
            SeleniumProvider.Driver.FindElementById("clickid12").Click();
            // Switch to the new tab
            SeleniumProvider.Driver.SwitchTo().Window(SeleniumProvider.Driver.WindowHandles.Last());
                
                
            Wait(200, 100);
        }

        public static void FocusMainTab() {
            var tabs = SeleniumProvider.Driver.WindowHandles;
            SeleniumProvider.Driver.SwitchTo().Window(tabs.First());
        }

        public static void CloseTab() {
            SeleniumProvider.Driver.Close();
            //SeleniumProvider.Driver.ExecuteScript("window.close()");
        }

        public static void CloseAndSwitchToMainTab() {
            CloseTab();
            FocusMainTab();
        }

        public static ReadOnlyCollection<IWebElement> GetTabButtons(string tabId) {
            Wait(1000, 500);
            var b = SeleniumProvider.Driver.FindElementById(tabId);
            var butts = b.FindElements(By.ClassName("_3sz"));
            //var butts = SeleniumProvider.Driver.FindElementsByClassName("_3sz");
            Print($"Found by xpath {butts.Count}");
            return butts;
        }

        /// <summary>
        /// Clicks either check-ins or the first with a See all button
        /// </summary>
        public static void ClickFirstButton() {
            var cats = new[] {
                "map", "sports", "music", "movies", "tv", "books", "likes", "reviews",
                "groups"
            };

            var allPresent = SeleniumProvider.Driver.FindElements(By
                .XPath("//div[contains(@id, 'pagelet_timeline_medley')]"));

            foreach (var webElement in allPresent) {
                var actualId = webElement.GetAttribute("id");

                // if the current web element has a cat in it's id
                if (!cats.Any(e => actualId.Contains(e))) continue;
                // check if it has a See All button

                // ToDO: change from hard coded ID !!!!!!!!!!!!!!!!!!!!!
                var but = SeleniumProvider.Driver
                    .FindElementsByXPath("//div[@id='pagelet_timeline_medley_movies']//span[@class='_3t5 fwb']");
                if (but.Count <= 0) continue;
                Print($"Clicking on button for tab with id {actualId}", ConsoleColor.Cyan);
                but[0].Click();
                return;
            }

            Print("No more cats", ConsoleColor.Magenta);
        }
    }
}