using System;
using System.Reflection.Metadata;
using FB_Data_Analysis.Classes.Util;
using OpenQA.Selenium;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.Classes.FBCategories {
    public class ProfileReviews : Extractor, IPageTab {
        
        public ProfileReviews(User user) : base(user) {
        }

        public void Scrap(string title) {
            const string id = "pagelet_timeline_medley_reviews";
            
            Print($"Scrapping {id} -> {title}", ConsoleColor.DarkRed);
            
            ExtractData(id);
        }

        private void ExtractData(string id) {
            var allElements = Driver.FindElementByXPath($"//div[@id='{id}']").FindElements(By.ClassName("_5mt_"));

            foreach (var element in allElements) {
                var title = element.FindElement(By.CssSelector("a"));
                var name = title?.Text;
                var href = title?.GetAttribute("href");

                var rating = element.FindElement(By.TagName("u"))?.Text;

                var timeStamp = GetTimeStamp(element);

                var seeMore = IsSeeMorePresent(element);

                seeMore?.Click();

                var ratingText = GetRatingText(element);
                
//                Print($"Name {name}, href {href}, rating {rating} timestamp {timeStamp}, text {ratingText}"
//                    , ConsoleColor.Yellow);
                
                User.Misc.AddData("Reviews", $"{name} | " +
                                             $"{href} | " +
                                             $"{rating} | " +
                                             $"{timeStamp} | " +
                                             $"{ratingText}");
                
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