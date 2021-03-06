﻿using FB_Data_Analysis.Classes;
using FB_Data_Analysis.DesktopVersion.Util;

namespace FB_Data_Analysis.DesktopVersion.FBCategories {
    public class ProfileBooks : Extractor, IPageTab {
        
        public ProfileBooks(User user) : base(user) {
        } 
        
        public void Scrap(string title) {
            var id = "pagelet_timeline_medley_books";

            GeneralScrap(title, id);
        }
        
    }
}