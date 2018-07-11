﻿using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.Classes.Util {
    public class Extractor {
        protected readonly ChromeDriver Driver;
        protected readonly User User;

        private readonly string[] _banned = {"Cities"};

        protected Extractor(User user) {
            Driver = SeleniumProvider.Driver;
            User = user;
        }

        protected void Extract(string type, string miscCategoryName, string tabId, string title) {
            var d = Driver.FindElementById(tabId);
            GetFieldAndLink(d, type, miscCategoryName, title);
        }

        protected void ClickNavBarButton(int buttonIndex, IWebElement element) {
            var navButtons = element.FindElements(By.ClassName("_3sz"));
            
            if (navButtons.Count - 1 < buttonIndex) return;
            
            Print($"Clicking on {navButtons[buttonIndex]?.Text}", ConsoleColor.Gray);
            
            ScrollToElement(navButtons[buttonIndex]);
            
            ScrollUpSome();
            
            navButtons[buttonIndex].Click();
            
            Wait(1000, 200);
        }

        private void GetFieldAndLink(ISearchContext element, string type, string miscCategoryName, string title) {
            var dataBoxes = element.FindElements(By.ClassName("_3owb"));
            for (var i = 0; i < dataBoxes.Count; i++) {
                var webElement = dataBoxes[i];
                var row = webElement.FindElement(By.ClassName("_gx7"));
                var key = row?.Text;
                var value = row?.GetAttribute("href");
                //Print($"[{type}] {key}: {value}", ConsoleColor.DarkGreen);
                if (key?.Trim().Length == 0) key = $"{title}_{i}";
                User.Misc.AddData(miscCategoryName, $"[{type}] {key} -> {value}");
            }
        }

        public void Scrap(string title, string id, string name) {
            
            Print($"Scrapping {name} -> {title}", ConsoleColor.DarkRed);
            
            //Driver.Url = url;

            Wait(1000, 200);

            var butts = GetTabButtons(id);

            for (var i = 0; i < butts.Count; i++) {
                var webElement = butts[i];

                if (name == "CheckIns") {
                    if(!CheckMapButtons(webElement.Text)) continue;
                }
                
                
                if(IsButtonBanned(webElement)) continue;
                
                ClickNavBarButton(i, SeleniumProvider.Driver.FindElementById(id));
                
                ScrollToBottom();
                
                Extract(webElement.Text, name, id, title);
                Print("------------------------------ ------------------ -------------");
                
                ScrollToElement(id);
            }
        }

        private bool CheckMapButtons(string s) {
            return s == "Places" || s == "Recent";
        }

        private bool IsButtonBanned(IWebElement button) {
            return _banned.Contains(button.Text);
        }
        
        private void CustomMoviesExtract(string type, string miscCategoryName, string tabId) {
            var allMovies = Driver.
                FindElementsByXPath($"//div[@id='{tabId}']//div[@class='_gx6 _agv']");

            foreach (var webElement in allMovies) {
                var name = webElement.FindElement(By.CssSelector("a"))?.Text;
                var subText = "";
                var timed = false;

                if (type == "Watched" || type == "Read") {
                    subText = webElement.FindElement(By.ClassName("timestampContent"))?.Text;
                    timed = true;
                }

                var qq = timed ? "time -> " : "";
                
                //var likes = webElement.FindElements(By.XPath("//span[@class='_14a_']//span"))[1]?.Text;
                
                //Print($"{type}: {name} {qq} {subText}");
                
                User.Misc.AddData(miscCategoryName, $"[{type}] {name} {qq} {subText}");
            }
        }

        public void GeneralScrap(string title, string id) {

            Print($"Scrapping -> {title}", ConsoleColor.DarkRed);

            //Driver.Url = url;

            Wait(1000, 200);

            var butts = GetTabButtons(id);

            for (var i = 0; i < butts.Count; i++) {
                var webElement = butts[i];
                ClickNavBarButton(i, SeleniumProvider.Driver.FindElementById(id));

                ScrollToBottom();

                CustomMoviesExtract(webElement.Text, title, id);
                //Extract(webElement.Text, "Music", id);
                Print("------------------------------ ------------------ -------------");

                ScrollToElement(id);
            }
        }
    }
}