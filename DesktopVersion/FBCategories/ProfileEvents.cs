using System;
using FB_Data_Analysis.Classes;
using FB_Data_Analysis.Classes.Util;
using FB_Data_Analysis.DesktopVersion.Util;
using OpenQA.Selenium;

namespace FB_Data_Analysis.DesktopVersion.FBCategories {
    public class ProfileEvents : Extractor, IPageTab {
        
        public ProfileEvents(User user) : base(user) {
        }

        public void Scrap(string title) {
            
            const string id = "pagelet_timeline_medley_events";
            
            Helpers.Print($"Scrapping {id} -> {title}", ConsoleColor.DarkRed);
            
            var elements = Driver.FindElementById("id").FindElements(By.ClassName("_4cbv"));

            ClickNavBarButton(1, Driver.FindElementById("id"));

            for (var i = 0; i < elements.Count; i++) {
                var webElement = elements[i];
                var a = webElement.FindElement(By.CssSelector("a"));
                var time = webElement.FindElement(By.ClassName("_4cbu")).Text;

                var name = a.Text;
                var href = a.GetAttribute("href");

                Helpers.Print($"( {elements.Count - i} ) Adding {name}, time {time}", ConsoleColor.Yellow);

                User.Misc.AddData("Events", new[] {name, time, href});
            }
        }
        
    }
}