using System;
using FB_Data_Analysis.Classes.Util;
using OpenQA.Selenium;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.Classes.FBCategories {
    public class ProfileFitness : Extractor, IPageTab {
        
        public ProfileFitness(User user) : base(user) {
        }

        public void Scrap(string title) {

            const string id = "pagelet_timeline_medley_fitness";

            Print($"Scrapping {id} -> {title}", ConsoleColor.DarkRed);

            var container = Driver.FindElementById(id);
            
            var butts = GetTabButtons(id);
            
            for (var i = 0; i < butts.Count; i++) {
                
                ClickButtonNav(container, i);
                
                ScrollToBottom();
                
                CustomExtract(container);
                Print("------------------------------ ------------------ -------------");
                
                ScrollToElement(id);
            }
        }

        private void CustomExtract(ISearchContext container) {
            
            var elements = container.FindElements(By.CssSelector("td"));
            foreach (var element in elements) {
                var a = element.FindElement(By.CssSelector("a"));

                var name = a.GetAttribute("data-appname");
                var href = a.GetAttribute("href");

                var type = ExtractTextValueFromElement(element.FindElement(By.ClassName("appCategories")), true);

                var description = element.FindElement(By.ClassName("description")).Text;
            
                User.Misc.AddData("Fitness", $"{name} | " +
                                             $"{type} | " +
                                             $"{description} | " +
                                             $"{href} | ");
                
            }
            
            
        }
        
        
        // toDO: merge with the one in Extractor!
        private void ClickButtonNav(ISearchContext container, int buttonIndex) {
            var navButtons = container.FindElements(By.ClassName("_3sz"));

            if (navButtons.Count - 1 < buttonIndex) return;
            
            Print($"Clicking on {navButtons[buttonIndex]?.Text}", ConsoleColor.Gray);
            
            ScrollToElement(navButtons[buttonIndex]);
            
            ScrollUpSome();
            
            navButtons[buttonIndex].Click();
            
            Wait(1000, 200);
        }

    }
}