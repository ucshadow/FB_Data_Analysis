using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using static FB_Data_Analysis.Classes.Helpers;
using static System.Diagnostics.Stopwatch;

namespace FB_Data_Analysis.Classes.Util {
    public class GroupParser {
        
        private string _groupUrl;
        private ChromeDriver _driver;
        public List<string> UserHrefs;

        public GroupParser(string groupUrl) {
            _groupUrl = groupUrl;
            _driver = SeleniumProvider.Driver;
            UserHrefs = new List<string>();
        }

        public void GetEm() {
            Print($"Getting to group page {_groupUrl}", ConsoleColor.Blue);
            _driver.Url = _groupUrl;
            Print("Scrolling to bottom...", ConsoleColor.Magenta);
            
            var watch = StartNew();
            
            ScrollToBottom(500);

            var container = _driver.FindElementById("groupsMemberSection_recently_joined");

            //var links = container.FindElements(By.CssSelector("a"));

            //var links = container.FindElements(By.XPath("//a[@role='button']"));

            var links = container.FindElements(By.ClassName("_60rg"));
            
            foreach (var webElement in links) {
                var e = webElement.GetAttribute("href");
                Print($"{e}");
                UserHrefs.Add(e);
            }
            
            watch.Stop();
            Print($"Done in {watch.ElapsedMilliseconds / 1000} seconds");
        }

    }
}