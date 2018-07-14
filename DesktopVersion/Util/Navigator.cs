using System;
using System.Collections.Generic;
using System.Linq;
using FB_Data_Analysis.Classes.FBCategories;
using OpenQA.Selenium;
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
        
        private readonly string[] _cats = {
            "map", "sports", "music", "movies", "tv", "books" , /*"likes",*/ "reviews",
            "events", "games", "fitness",
            // undiscovered
            //"groups", "events"
        };

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
            _workers.Add("TV Shows", new ProfileTvShows(_user));
            _workers.Add("Books", new ProfileBooks(_user));
            _workers.Add("Apps and Games", new ProfileAppsAndGames(_user));
            _workers.Add("Likes", new ProfileLikes(_user));
            _workers.Add("Reviews", new ProfileReviews(_user));
            _workers.Add("Events", new ProfileEvents(_user));
            _workers.Add("Fitness", new ProfileFitness(_user));

            _workers.Add("Friends", new ProfileDummy(_user));
            _workers.Add("Photos", new ProfileDummy(_user));
            _workers.Add("Videos", new ProfileDummy(_user));
            _workers.Add("Notes", new ProfileDummy(_user));
        }

        public void Perform() {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Print("Performing...", ConsoleColor.Red);
            
            var allCats = _driver.FindElements(By.XPath("//div[contains(@id, 'pagelet_timeline_medley')]"));
            
            foreach (var webElement in allCats) {
                var id = "";

                try {
                    id = webElement.GetAttribute("id");
                    
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                    Perform();
                    continue;
                }
                if (CatIsAllowed(id) && !_alreadyVisited.Contains(id)) {
                    
                    Print($"{id} is allowed", ConsoleColor.Blue);
                    
                    var but = GetButtonIfAny(id);
                    var title = GetTitle(id);
                    
                    Print($"Got button {but} and title {title}", ConsoleColor.Green);
                    
                    if (but != null) {
                        ScrollToElement(but);
                        Wait(1000, 500);

                        try { 
                            but.Click();
                        }
                        catch (Exception e) {
                            Print($"Button not clickable, scrolling up");
                            ScrollUpSome();
                            but.Click();
                        }
                    }

                    if (id == "pagelet_timeline_medley_likes") {
                        Print("Likes detected, scrolling faster...", ConsoleColor.Red);
                        
                        // scroll faster for likes, this can take a while
                        ScrollToBottom(500);
                    }
                    else {
                        ScrollToBottom();
                    }
                    
                    
                    Print($"Scrapping {id}, this should take a while...", ConsoleColor.DarkRed);

                    if (_workers.ContainsKey(title)) {
                        _workers[title].Scrap(title);
                        _alreadyVisited.Add(id);
                    }
                }
                else {
                    Print($"{id} is not allowed, skipping...", ConsoleColor.DarkGray);
                    
                }
            }
            watch.Stop();
            Print($"Done in {watch.ElapsedMilliseconds / 1000} seconds");
        }

        private bool CatIsAllowed(string tabId) {
            return _cats.Any(tabId.Contains);
        }

        private IWebElement GetButtonIfAny(string tabId) {
            var check = _driver.FindElementsByXPath($"//div[@id='{tabId}']//span[@class='_3t5 fwb']");


            Print($"Getting button for {tabId} -> {check.Count}", ConsoleColor.DarkGreen);
            return check.Count > 0 ? check[0] : null;
        }

        private string GetTitle(string id) {
            var check = _driver.FindElementsByXPath($"//div[@id='{id}']//a[@class='_51sx']");
            return check.Count > 0 ? check[0]?.Text : "Momo";
        }
    }
}