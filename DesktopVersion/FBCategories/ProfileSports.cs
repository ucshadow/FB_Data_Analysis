using System;
using FB_Data_Analysis.Classes.Util;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.Classes.FBCategories {
    public class ProfileSports : Extractor, IPageTab {

        public ProfileSports(User user) : base(user) {
        }

        public void Scrap(string title) {
            
            Scrap(title, "pagelet_timeline_medley_sports", "Sports");
            
//            Print($"Scrapping Sports -> {url}", ConsoleColor.DarkRed);
//            
//            Driver.Url = url;
//            
//            //ScrollToElement("pagelet_timeline_medley_map");
//            //ClickOnMore("pagelet_timeline_medley_map");
//
//            Wait(1000, 200);
//
//            
//
//            var butts = GetTabButtons("pagelet_timeline_medley_sports");
//            
//
//            for (var i = 0; i < butts.Count; i++) {
//                var webElement = butts[i];
//                ClickNavBarButton(i);
//                
//                ScrollToBottom();
//                
//                Extract(webElement.Text, "Sports", "pagelet_timeline_medley_sports");
//                Print("------------------------------ ------------------ -------------");
//                
//                ScrollToElement("pagelet_timeline_medley_sports");
//            }

//            Extract("Tab_1", "Sports");
//
//
//            if(!ClickNavBarButton(1)) return;
//            
//            Print("------------------------------ ------------------ -------------");
//            
//            ScrollToBottom();
//            
//            Extract("Tab_2", "Sports");
        }

    }
}