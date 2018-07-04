using System;
using FB_Data_Analysis.Classes.Util;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.Classes.FBCategories {
    public class ProfileDummy : Extractor, IPageTab {
        
        public ProfileDummy(User user) : base(user) {}

        public void Scrap(string url) {
            Print($"Dummy profile {url}", ConsoleColor.Red);
        }
    }
}