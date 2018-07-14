using System;
using FB_Data_Analysis.Classes.Util;
using OpenQA.Selenium;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.Classes.FBCategories {
    public class ProfileEvents : Extractor, IPageTab {
        
        public ProfileEvents(User user) : base(user) {
        }

        public void Scrap(string title) {
            
            const string id = "pagelet_timeline_medley_events";
            
            Print($"Scrapping {id} -> {title}", ConsoleColor.DarkRed);
            
            var elements = Driver.FindElementById("id").FindElements(By.ClassName("_4cbv"));

            ClickNavBarButton(1, Driver.FindElementById("id"));
            
            foreach (var webElement in elements) {
                var a = webElement.FindElement(By.CssSelector("a"));
                var time = webElement.FindElement(By.ClassName("_4cbu")).Text;

                var name = a.Text;
                var href = a.GetAttribute("href");
                
                Print($"name {name}, time {time}, href {href}", ConsoleColor.Yellow);
                
//                User.Misc.AddData("Events", $"{name} | " +
//                                            $"{time} | " +
//                                            $"{href} | ");
                User.Misc.AddData("Events", "Name", name);
                User.Misc.AddData("Events", "Time", time);
                User.Misc.AddData("Events", "Href", href);
            }
        }
        
    }
}