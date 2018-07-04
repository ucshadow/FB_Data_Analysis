using System;
using System.ComponentModel;
using FB_Data_Analysis.Classes.Util;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.Classes.FBCategories {
    public class ProfileCheckIns : Extractor, IPageTab {

        //private readonly ChromeDriver _driver;
        //private User _user;

        public ProfileCheckIns(User user) : base(user) {
            
        }

        public void Scrap(string url) {
            //if (!TabIsPresent("Check-Ins")) return;
            
            Print($"Scrapping Check-Ins -> {url}", ConsoleColor.DarkRed);
            
            
            Driver.Url = url;
            
            //ScrollToElement("pagelet_timeline_medley_map");
            //ClickOnMore("pagelet_timeline_medley_map");

            Wait(1000, 200);

            ScrollToBottom();

            Extract("Place", "CheckIns", "pagelet_timeline_medley_map");

            ScrollToElement("pagelet_timeline_medley_map");

            // Special click since tab 1 is inactive.
            ClickNavBarButton(2);
            Print("------------------------------ ------------------ -------------");
            
            ScrollToBottom();
            
            Extract("Recent", "CheckIns", "pagelet_timeline_medley_map");



            //GoBackToAbout();
        }

//        private void Extract(string type) {
//            var d = _driver.FindElementById("pagelet_timeline_medley_map");
//            GetFieldAndLink(d, type);
//        }
//
//        private void GetFieldAndLink(ISearchContext element, string type) {
//            var dataBoxes = element.FindElements(By.ClassName("_3owb"));
//            for (var i = 0; i < dataBoxes.Count; i++) {
//                var webElement = dataBoxes[i];
//                var row = webElement.FindElement(By.ClassName("_gx7"));
//                var key = row?.Text;
//                var value = row?.GetAttribute("href");
//                Print($"[{type}] {key}: {value}", ConsoleColor.DarkGreen);
//                if (key?.Trim().Length == 0) key = $"Place_{i}";
//                _user.Checkins.CheckIns.Add($"[{type}] {key}: {value}");
//            }
//        }

    }
    
    
}