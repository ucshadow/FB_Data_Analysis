using System.Collections.Generic;
using FB_Data_Analysis.Classes.FBCategories;
using OpenQA.Selenium.Chrome;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.Classes {
    
    /// <summary>
    /// Takes a url of format facebook.com/user/about and gets
    /// all available option tabs for that user
    /// </summary>
    public class Navigator {

        private ChromeDriver _driver;
        private User _user;

        private Dictionary<string, IPageTab> _workers;

        public Navigator(User user) {
            _driver = SeleniumProvider.Driver;
            _user = user;
            _workers = new Dictionary<string, IPageTab>();
            AddWorkers();
        }

        /// <summary>
        /// Add a worker for each tab
        /// </summary>
        private void AddWorkers() {
            _workers.Add("Check-Ins", new ProfileCheckIns(_user));
        }

        public void GetTabs() {
            ScrollToBottom();

            // a elements
            var allTabSeeAllButton = _driver.FindElementsByClassName("_3t3");

            // also a's, get the text, should be [1:] to syncronise with the buttons
            var allTabTitles = _driver.FindElementsByClassName("_51sx");

            for (var i = 0; i < allTabTitles.Count; i++) {
                var category = allTabTitles[i].Text;
                if (_workers.ContainsKey(category)) {
                    
                    Print($"Found category for worker {category}");
                    
                    OpenNewTabAndFocus();
                    
                    _workers[category].Scrap(allTabSeeAllButton[i - 1].GetAttribute("href"));
                    
                    CloseTab();
                    FocusMainTab();
                }
                else {
                    Print($"{category} has no worker");
                }
            }
        }
    }
}