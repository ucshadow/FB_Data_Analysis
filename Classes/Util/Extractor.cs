using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.Classes.Util {
    public class Extractor {
        protected readonly ChromeDriver Driver;
        private readonly User _user;

        protected Extractor(User user) {
            Driver = SeleniumProvider.Driver;
            _user = user;
        }

        protected void Extract(string type, string miscCategoryName, string tabId) {
            var d = Driver.FindElementById(tabId);
            GetFieldAndLink(d, type, miscCategoryName);
        }

        protected void ClickNavBarButton(int buttonIndex) {
            var navButtons = Driver.FindElementsByClassName("_3sz");
            Print($"Clicking on {navButtons[buttonIndex]?.Text}", ConsoleColor.Gray);
            navButtons[buttonIndex].Click();
            
            Wait(1000, 200);
        }

        private void GetFieldAndLink(ISearchContext element, string type, string miscCategoryName) {
            var dataBoxes = element.FindElements(By.ClassName("_3owb"));
            for (var i = 0; i < dataBoxes.Count; i++) {
                var webElement = dataBoxes[i];
                var row = webElement.FindElement(By.ClassName("_gx7"));
                var key = row?.Text;
                var value = row?.GetAttribute("href");
                Helpers.Print($"[{type}] {key}: {value}", ConsoleColor.DarkGreen);
                if (key?.Trim().Length == 0) key = $"Place_{i}";
                _user.Misc.AddData(miscCategoryName, $"[{type}] {key}: {value}");
            }
        }

        public void Scrap(string url, string id, string name) {
            
            Print($"Scrapping {name} -> {url}", ConsoleColor.DarkRed);
            
            //Driver.Url = url;

            Wait(1000, 200);

            var butts = GetTabButtons(id);

            for (var i = 0; i < butts.Count; i++) {
                var webElement = butts[i];
                ClickNavBarButton(i);
                
                ScrollToBottom();
                
                Extract(webElement.Text, name, id);
                Print("------------------------------ ------------------ -------------");
                
                ScrollToElement(id);
            }
        }
    }
}