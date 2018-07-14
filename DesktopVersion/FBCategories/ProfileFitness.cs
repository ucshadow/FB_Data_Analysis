using System;
using FB_Data_Analysis.Classes;
using FB_Data_Analysis.DesktopVersion.Util;
using OpenQA.Selenium;

namespace FB_Data_Analysis.DesktopVersion.FBCategories {
    public class ProfileFitness : Extractor, IPageTab {
        
        public ProfileFitness(User user) : base(user) {
        }

        public void Scrap(string title) {

            const string id = "pagelet_timeline_medley_fitness";

            Helpers.Print($"Scrapping {id} -> {title}", ConsoleColor.DarkRed);

            var container = Driver.FindElementById(id);
            
            var butts = Helpers.GetTabButtons(id);
            
            for (var i = 0; i < butts.Count; i++) {
                
                ClickButtonNav(container, i);
                
                Helpers.ScrollToBottom();
                
                CustomExtract(container);
                Helpers.Print("------------------------------ ------------------ -------------");
                
                Helpers.ScrollToElement(id);
            }
        }

        private void CustomExtract(ISearchContext container) {
            var elements = container.FindElements(By.CssSelector("td"));
            for (var i = 0; i < elements.Count; i++) {
                var element = elements[i];
                var a = element.FindElement(By.CssSelector("a"));

                var name = a.GetAttribute("data-appname");
                var href = a.GetAttribute("href");

                var type = Helpers.ExtractTextValueFromElement(element.FindElement(By.ClassName("appCategories")), true);

                var description = element.FindElement(By.ClassName("description")).Text;

                Helpers.Print($"( {elements.Count - i} ) Adding {name}", ConsoleColor.Yellow);
                User.Misc.AddData("Fitness", new[] {type, name, description, href});
            }
        }
        
        
        // toDO: merge with the one in Extractor!
        private void ClickButtonNav(ISearchContext container, int buttonIndex) {
            var navButtons = container.FindElements(By.ClassName("_3sz"));

            if (navButtons.Count - 1 < buttonIndex) return;
            
            Helpers.Print($"Clicking on {navButtons[buttonIndex]?.Text}", ConsoleColor.Gray);
            
            Helpers.ScrollToElement(navButtons[buttonIndex]);
            
            Helpers.ScrollUpSome();
            
            navButtons[buttonIndex].Click();
            
            Helpers.Wait(1000, 200);
        }

    }
}