using System;
using FB_Data_Analysis.Classes;
using FB_Data_Analysis.DesktopVersion.Util;
using OpenQA.Selenium;

namespace FB_Data_Analysis.DesktopVersion.FBCategories {
    public class ProfileReviews : Extractor, IPageTab {
        
        public ProfileReviews(User user) : base(user) {
        }

        public void Scrap(string title) {
            const string id = "pagelet_timeline_medley_reviews";
            
            Helpers.Print($"Scrapping {id} -> {title}", ConsoleColor.DarkRed);
            
            ExtractData(id);
        }

        private void ExtractData(string id) {
            var allElements = Driver.FindElementByXPath($"//div[@id='{id}']").FindElements(By.ClassName("_5mt_"));

            for (var i = 0; i < allElements.Count; i++) {
                var element = allElements[i];
                var title = element.FindElement(By.CssSelector("a"));
                var name = title?.Text;
                var href = title?.GetAttribute("href");

                var ratingExists = Helpers.ElementIsPresent(element, By.TagName("u"));

                var rating = ratingExists ? element.FindElement(By.TagName("u"))?.Text : "";

                var timeStamp = GetTimeStamp(element);

                var seeMore = IsSeeMorePresent(element);

                seeMore?.Click();

                var ratingText = GetRatingText(element);
                Helpers.Print($"( {allElements.Count - i} ) Adding {title}", ConsoleColor.Yellow);
                User.Misc.AddData("Reviews", new[] {name, href, rating, timeStamp, ratingText});
            }
        }

        private string GetTimeStamp(ISearchContext element) {
            var check = element.FindElements(By.ClassName("timestampContent"));
            return check.Count > 0 ? check[0]?.Text : "";
        }

        private IWebElement IsSeeMorePresent(ISearchContext element) {
            var check = element.FindElements(By.ClassName("see_more_link"));
            return check.Count > 0 ? check[0] : null;
        }

        private string GetRatingText(ISearchContext element) {
            var check = element.FindElements(By.ClassName("_5mu1"));
            return check.Count > 0 ? check[0]?.Text : "";
        }
        
    }
}