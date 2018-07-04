using System;
using FB_Data_Analysis.Classes.Util;
using OpenQA.Selenium;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.Classes.FBCategories {
    public class ProfileMovies : Extractor, IPageTab {

        public ProfileMovies(User user) : base(user) {
        } 

        public void Scrap(string url) {
            var id = "pagelet_timeline_medley_movies";

            Print($"Scrapping -> {url}", ConsoleColor.DarkRed);

            //Driver.Url = url;

            Wait(1000, 200);

            var butts = GetTabButtons(id);

            for (var i = 0; i < butts.Count; i++) {
                var webElement = butts[i];
                ClickNavBarButton(i);

                ScrollToBottom();

                CustomExtract(webElement.Text, "Movies", id);
                //Extract(webElement.Text, "Music", id);
                Print("------------------------------ ------------------ -------------");

                ScrollToElement(id);
            }
        }

        private void CustomExtract(string type, string miscCategoryName, string tabId) {
            var allMovies = Driver.
                FindElementsByXPath($"//div[@id='{tabId}']//div[@class='_gx6 _agv']");

            foreach (var webElement in allMovies) {
                var name = webElement.FindElement(By.CssSelector("a"))?.Text;
                var subText = "";

                if (type == "Watched") {
                    subText = webElement.FindElement(By.ClassName("timestampContent"))?.Text;
                }

                var qq = type == "Watched" ? "time -> " : "";
                
                //var likes = webElement.FindElements(By.XPath("//span[@class='_14a_']//span"))[1]?.Text;
                
                Print($"{type}: {name} {qq} {subText}");
            }
        }
        
    }
}