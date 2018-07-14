using System;
using System.Linq;
using FB_Data_Analysis.Classes;
using FB_Data_Analysis.Classes.Util;
using FB_Data_Analysis.DesktopVersion.Util;
using OpenQA.Selenium;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.DesktopVersion.FBCategories {
    public class ProfileLikes : Extractor, IPageTab{
        
        public ProfileLikes(User user) : base(user) {
        } 
        
        public void Scrap(string title) {
            OpenNewTabAndFocus();
            var id = "pagelet_timeline_medley_likes";
            
            Print($"Scrapping {id} -> {title}", ConsoleColor.DarkRed);
            
            var u = $"{User.Url.Replace("www", "m")}/about";

            Driver.Url = u;
            Wait(2000, 1000);
            
            ExtractData();
            CloseAndSwitchToMainTab();
        }

        private void ExtractData() {
            ScrollToBottom();

            var rows = Driver.FindElementsByXPath("//div[@class='_55wr _4g33 _52we _5b6o touchable _592p _25mv']");
            var link = rows.Where(e => e.Text.Contains("LIKES")).
                Select(e => e.FindElement(By.CssSelector("a")).GetAttribute("href"))
                .First();
            Driver.Url = link;
            
            var rows2 = Driver.FindElementsByXPath("//div[@class='_55wr _4g33 _52we _5b6o touchable _592p _25mv']");
            var link2 = rows2.Where(e => e.Text.Contains("ALL LIKES")).
                Select(e => e.FindElement(By.CssSelector("a")).GetAttribute("href"))
                .First();
            Driver.Url = link2;
            
            Wait(4000, 1000);
            ScrollToBottom(500);

            var all = Driver.FindElementsByClassName("_1a5p");
            for (var index = 0; index < all.Count; index++) {
                var element = all[index];
                var name = element.Text;
                var href = element.FindElement(By.CssSelector("a")).GetAttribute("href");
                Print($"( {all.Count - index} ) Adding {name}", ConsoleColor.Green);
                User.Misc.AddData("Likes", new[] {name, href});
            }
        }
        
    }
}