﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using FB_Data_Analysis.Classes;
using FB_Data_Analysis.Classes.Util;
using FB_Data_Analysis.DesktopVersion.FBCategories;
using FB_Data_Analysis.DesktopVersion.Util;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FB_Data_Analysis.DesktopVersion {
    public class Spider {
        private readonly string _loginUrl;
        private readonly string _groupUrl;
        private readonly string _email;
        private readonly string _password;
        private readonly int _userCount;
        private List<string> _visited;
        private readonly ChromeDriver _driver;

        private string _singleUser;

        private List<User> _users;

        private string _fileName;

        //private string _mainTab;
        private int _visitedProfiles;
        private ReadOnlyCollection<IWebElement> _allProfiles;

        private Navigator _nav;

        public Spider(string email, string password, string groupUrl,
            int userCount) {
            _email = email;
            _password = password;
            _groupUrl = groupUrl;
            _userCount = userCount;
            _visited = new List<string>();
            _users = new List<User>();

            AddFileToVisitedSet();

            _loginUrl = "https://www.facebook.com";
            _driver = SeleniumProvider.Driver;
            //_mainTab = _driver.CurrentWindowHandle;
            _allProfiles = new ReadOnlyCollection<IWebElement>(new List<IWebElement>());

            // toDo: ??
            //_nav = new Navigator(new User());

            //var x = _driver.FindElementsByClassName("uiProfileBlockContent");
        }

        // ToDo: check if each panel is Present!! 
        public void MainLoop() {
            Helpers.GlobalDelay = 2000;

            SingleLoop();

            Helpers.Print("Main loop started");
            Helpers.Print("");

            GetGroupPage();

            var x = new GroupParser("https://www.facebook.com/groups/1379225572136343/members/");
            x.GetEm();

            /*
             *
             * oyy tgooooa
             * 
             */

            x.UserHrefs.ForEach(e => {
                if (!_visited.Contains(e)) {
                    Helpers.Print($"Getting {e}", ConsoleColor.Black);

                    _driver.Url = e;

                    SaveHrefToVisited(e);

                    Helpers.Wait(1000, 500);

                    GetAboutTab();

                    Helpers.ScrollToBottom();

                    var u = new User {Url = e};

                    _nav = new Navigator(u);

                    _nav.Perform();

                    Helpers.Print("\n ---- \n");
                    u.PrintUser();
                    u.Jsonise();
                }
                else {
                    Helpers.Print($"{e} alredy visited, skipping...", ConsoleColor.Black);

                }
                
            });
        }

        public void SingleLoop() {
            Helpers.Print("Single loop started");
            Helpers.Print("");

            var e = "https://www.facebook.com/elena.taran.731";
//            var e = "https://www.facebook.com/elena.smeu97";
//            var e = "https://www.facebook.com/julia.gatman";
//            var e = "https://www.facebook.com/konstandina.skov"; // some of everything :D
//            var e = "https://www.facebook.com/CristinaUngurean97";
//            var e = "https://www.facebook.com/elena.smeu97";
            //var e = "https://www.facebook.com/nicole.dybowski";
                //"https://www.facebook.com/cornea.costea";

            GetGroupPage();

            Helpers.Print($"Getting {e}", ConsoleColor.Black);

            _driver.Url = e;

            Helpers.Wait(1000, 500);

            GetAboutTab();

            Helpers.ScrollToBottom();

            var u = new User {Url = e};


            _nav = new Navigator(u);

            // get about, everyone has this tab
            var about = new ProfileAbout(u);
            about.Scrap("About");

            _nav.Perform();

            //var checkIns = new ProfileCheckIns(u);

            Helpers.Print("\n ---- \n");
            //u.PrintUser();

            u.Jsonise();

            Environment.Exit(0);
        }

        private void GetAboutTab() {
            var row = _driver.FindElementById("fbTimelineHeadline");
            var buttons = row.FindElements(By.ClassName("_6-6"));
            //Wait(2000, 1000);
            buttons[1].Click();
            Helpers.Wait(1500, 500);
//            var href = buttons[1].GetAttribute("href");
//            _driver.Url = href;
            //_driver.Url = href.Count == 1 ? href[0].GetAttribute("href") : href[1].GetAttribute("href");
            //Wait(1000000, 50000);
        }

        private void OpenUserInNewTab() {
            var nextUser = GetNextUser();
            if (nextUser == null) {
                Helpers.Print("Job Done", ConsoleColor.DarkRed);
                return;
            }

            _driver.ExecuteScript("window.open('about:blank', '-blank')");
            _driver.SwitchTo().Window(_driver.WindowHandles[1]);
            _driver.Url = nextUser;
        }

        private string GetNextUser() {
            foreach (var profile in _allProfiles) {
                var userHref = profile.FindElement(By.CssSelector("a")).GetAttribute("href");
                if (!_visited.Contains(userHref)) {
                    SaveHrefToVisited(userHref);
                    Helpers.Print($"new user found: {userHref}", ConsoleColor.Cyan);
                    _visitedProfiles += 1;
                    return userHref;
                }
            }

            return null;
        }

        private void SaveHrefToVisited(string href) {
            using (var sw = File.AppendText(Directory.GetCurrentDirectory() + "/visited.dt")) {
                sw.WriteLine(href);
            }
        }
//            _allProfiles
//                .Select(x => x
//                    .FindElement(By.CssSelector("a"))
//                    .GetAttribute("href"))
//                .ToString();

        private void GetGroupPage() {
            //_driver.Url = _groupUrl;

            _driver.Url = _loginUrl;

            // should be GroupUrl! this is for testing only

            _driver.FindElementById("email").SendKeys(_email);
            _driver.FindElementById("pass").SendKeys(_password);
            _driver.FindElementById("loginbutton").Click();

            //_driver.Url = _singleUser;

            //Helpers.Wait(_driver, 2000, 1000);
            //_driver.Url = _groupUrl;
        }

        private void ScrollToLoadAllData() {
            for (var i = 0; i < _userCount / 15 + 1; i++) {
                _driver.ExecuteScript("scrollBy(0, 2000)");
                Helpers.Wait(2000, 1000);
            }

            _allProfiles = _driver.FindElementsByClassName("uiProfileBlockContent");
        }

//        private void Wait(double delay, double interval) {
//            // Causes the WebDriver to wait for at least a fixed delay
//            var now = DateTime.Now;
//            var wait = new WebDriverWait(_driver, TimeSpan.FromMilliseconds(delay)) {
//                PollingInterval = TimeSpan.FromMilliseconds(interval)
//            };
//            wait.Until(wd => (DateTime.Now - now) - TimeSpan.FromMilliseconds(delay) > TimeSpan.Zero);
//        }

        private void AddFileToVisitedSet() {
            var dir = Directory.GetCurrentDirectory();

            var lines = File.ReadAllLines(dir + "/visited.dt");
            foreach (var line in lines) {
                _visited.Add(line);
            }

            Helpers.Print($"{lines.Length} visited profiles added to visited list");

//            _fileName = new string(_groupUrl.Where(char.IsDigit).ToArray()) + ".dt";
//
//            if (!File.Exists(dir + "/" + _fileName)) {
//                Print($"Path is: {dir + "/" + _fileName}");
//                File.Create(dir + "/" + _fileName);
//                Print($"File {_fileName} was created", ConsoleColor.DarkYellow);
//            }
//            else {
//                var lines = File.ReadAllLines(dir + "/" + _fileName);
//                foreach (var line in lines) {
//                    _visited.Add(line);
//                }
//                Print($"{lines.Length} visited profiles added to visited list");
//            }
        }
    }
}