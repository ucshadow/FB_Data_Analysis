using System;
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
        private bool _firstLoad = true;

        private Dictionary<string, IPageTab> _workers;
        private List<string> _alreadyVisited;

        public Navigator(User user) {
            _driver = SeleniumProvider.Driver;
            _user = user;
            _workers = new Dictionary<string, IPageTab>();
            _alreadyVisited = new List<string>();
            AddWorkers();
        }

        /// <summary>
        /// Add a worker for each tab
        /// </summary>
        private void AddWorkers() {
            _workers.Add("Check-Ins", new ProfileCheckIns(_user));
            _workers.Add("Sports", new ProfileSports(_user));
            _workers.Add("Music", new ProfileMusic(_user));
            _workers.Add("Movies", new ProfileMovies(_user));

            _workers.Add("Friends", new ProfileDummy(_user));
            _workers.Add("Photos", new ProfileDummy(_user));
            _workers.Add("Videos", new ProfileDummy(_user));
            _workers.Add("TV Shows", new ProfileDummy(_user));
            _workers.Add("Books", new ProfileDummy(_user));
            _workers.Add("Apps and Games", new ProfileDummy(_user));
            _workers.Add("Likes", new ProfileDummy(_user));
            _workers.Add("Events", new ProfileDummy(_user));
            _workers.Add("Fitness", new ProfileDummy(_user));
            _workers.Add("Reviews", new ProfileDummy(_user));
            _workers.Add("Notes", new ProfileDummy(_user));
        }

        
        // toDo: change to work based on existance of see all button and not 
        // todo: based on index!
        public void Perform() {
            //ScrollToBottom();

            // a elements
            var allTabSeeAllButton = _driver.FindElementsByClassName("_3t5");

            // also a's, get the text, should be [1:] to syncronise with the buttons
            var allTabTitles = _driver.FindElementsByClassName("_51sx");

            ScrollToElement(allTabSeeAllButton[0]);

            while (allTabSeeAllButton.Count > 0) {
                if (_firstLoad) {
                    ClickFirstButton();
                    ScrollToBottom();
                    allTabSeeAllButton = _driver.FindElementsByClassName("_3t5");
                    allTabTitles = _driver.FindElementsByClassName("_51sx");
                    _firstLoad = false;
                }
                else {
                    var category = allTabTitles[0].Text;

                    if (_workers.ContainsKey(category)) {
                        Print($"Found category for worker {category}");
                        _alreadyVisited.Add(category);

                        Wait(1000, 500);

                        // start from check ins, the rest are skipped
                        ScrollToElement(allTabSeeAllButton[0]);
                        Wait(1000, 500);
                        try { 
                            allTabSeeAllButton[0].Click();
                            
                        }
                        catch (Exception e) {
                            Console.WriteLine(e);
                            Print($"{category} has no button! ", ConsoleColor.Magenta);
                            
                            Wait(1000, 500);
                            continue;
                        }
                        ScrollToBottom();
                        
                    }
                    else {
                        Print($"{category} has no worker, waiting 2 seconds :D");
                        Wait(2000, 500);
                    }

                    allTabSeeAllButton = _driver.FindElementsByClassName("_3t5");
                    allTabTitles = _driver.FindElementsByClassName("_51sx");
                }
            }
        }
    }
}