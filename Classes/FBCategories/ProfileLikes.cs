using System;
using FB_Data_Analysis.Classes.Util;
using OpenQA.Selenium;
using static FB_Data_Analysis.Classes.Helpers;
using static System.Diagnostics.Stopwatch;

namespace FB_Data_Analysis.Classes.FBCategories {
    public class ProfileLikes : Extractor, IPageTab{
        
        public ProfileLikes(User user) : base(user) {
        } 
        
        public void Scrap(string title) {
            var id = "pagelet_timeline_medley_likes";
            
            Print($"Scrapping {id} -> {title}", ConsoleColor.DarkRed);
            
            ExtractData(id);
        }

        private void ExtractData(string id) {
            var allElements = Driver.FindElementByXPath($"//div[@id='{id}']").FindElements(By.ClassName("_42ef"));
            
            if (allElements.Count <= 0) return;
            foreach (var element in allElements) {
                var container = element.FindElement(By.CssSelector("a"));
                var link = container?.GetAttribute("href");
                var name = container?.Text;
                var type = element.FindElement(By.ClassName("fsm"))?.Text;
                    
                User.Misc.AddData("Likes", $"[{type}] {name}: {link}");
            }
        }
        
    }
}