using System;
using FB_Data_Analysis.Classes.Util;
using OpenQA.Selenium;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.Classes.FBCategories {
    public class ProfileMovies : Extractor, IPageTab {

        public ProfileMovies(User user) : base(user) {
        } 

        public void Scrap(string title) {
            var id = "pagelet_timeline_medley_movies";

            MoviesScrap(title, id);
        }

//        private void CustomMoviesExtract(string type, string miscCategoryName, string tabId) {
//            var allMovies = Driver.
//                FindElementsByXPath($"//div[@id='{tabId}']//div[@class='_gx6 _agv']");
//
//            foreach (var webElement in allMovies) {
//                var name = webElement.FindElement(By.CssSelector("a"))?.Text;
//                var subText = "";
//
//                if (type == "Watched") {
//                    subText = webElement.FindElement(By.ClassName("timestampContent"))?.Text;
//                }
//
//                var qq = type == "Watched" ? "time -> " : "";
//                
//                //var likes = webElement.FindElements(By.XPath("//span[@class='_14a_']//span"))[1]?.Text;
//                
//                Print($"{type}: {name} {qq} {subText}");
//                
//                User.Misc.AddData(miscCategoryName, $"[{type}] {name} {qq}: {subText}");
//            }
//        }
        
    }
}