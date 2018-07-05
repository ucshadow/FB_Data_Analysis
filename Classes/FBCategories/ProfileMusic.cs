using System;
using FB_Data_Analysis.Classes.Util;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.Classes.FBCategories {
    public class ProfileMusic : Extractor, IPageTab {
        
        public ProfileMusic(User user) : base(user) {
        } 
        
        public void Scrap(string title) {

            Scrap(title, "pagelet_timeline_medley_music", "Music");
            
//            var id = "pagelet_timeline_medley_music";
//            
//            Print($"Scrapping Music -> {url}", ConsoleColor.DarkRed);
//            
//            Driver.Url = url;
//
//            Wait(1000, 200);
//
//            var butts = GetTabButtons(id);
//
//            for (var i = 0; i < butts.Count; i++) {
//                var webElement = butts[i];
//                ClickNavBarButton(i);
//                
//                ScrollToBottom();
//                
//                Extract(webElement.Text, "Music", id);
//                Print("------------------------------ ------------------ -------------");
//                
//                ScrollToElement(id);
//            }
        }
        
    }
}