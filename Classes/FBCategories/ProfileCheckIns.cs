using System;
using System.ComponentModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.Classes.FBCategories {
    public class ProfileCheckIns : IPageTab {

        private readonly ChromeDriver _driver;
        private User _user;

        public ProfileCheckIns(User user) {
            _user = user;
            _driver = SeleniumProvider.Driver;
        }

        public void Scrap(string url) {
            //if (!TabIsPresent("Check-Ins")) return;
            
            
            _driver.Url = url;
            
            //ScrollToElement("pagelet_timeline_medley_map");
            //ClickOnMore("pagelet_timeline_medley_map");

            Wait(1000, 200);

            ScrollToBottom();

            Extract("Place");

            ScrollToElement("pagelet_timeline_medley_map");

            var navButtons = _driver.FindElementsByClassName("_3sz");
            navButtons[2].Click();
            
            Wait(1000, 200);
            
            Print("------------------------------ ------------------ -------------");
            
            ScrollToBottom();
            
            Extract("Recent");
            
            //GoBackToAbout();
        }

        private void Extract(string type) {
            var d = _driver.FindElementById("pagelet_timeline_medley_map");
//            var boxes = d.FindElement(By.XPath("//div[@id='pagelet_timeline_medley_map']//div[@class='_5h60 _30f']"));
//            var s = ExtractTextValueFromElement(d, true);
//            Print(s);
            GetFieldAndLink(d, type);
        }

        private void GetFieldAndLink(ISearchContext element, string type) {
            var dataBoxes = element.FindElements(By.ClassName("_3owb"));
            for (var i = 0; i < dataBoxes.Count; i++) {
                var webElement = dataBoxes[i];
                var row = webElement.FindElement(By.ClassName("_gx7"));
                var key = row?.Text;
                var value = row?.GetAttribute("href");
                Print($"[{type}] {key}: {value}", ConsoleColor.DarkGreen);
                if (key?.Trim().Length == 0) key = $"Place_{i}";
                _user.Checkins.CheckIns.Add($"[{type}] {key}: {value}");
            }
        }

    }
    
    
}