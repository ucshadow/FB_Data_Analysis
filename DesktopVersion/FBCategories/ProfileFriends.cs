using System;
using FB_Data_Analysis.Classes;
using FB_Data_Analysis.Classes.Util;
using FB_Data_Analysis.DesktopVersion.Util;
using FB_Data_Analysis.MobileVersion;

namespace FB_Data_Analysis.DesktopVersion.FBCategories {
    public class ProfileFriends :Extractor, IPageTab {
        
        public ProfileFriends(User user) : base(user) {
        }

        public void Scrap(string url) {

            var u = $"{User.Url.Replace("www", "m")}/friends";
            
            Helpers.Print($"Scrapping {u}", ConsoleColor.DarkRed);

            var list = BigCats.GetFriends(u);
            list.ForEach(e => {
                User.Misc.AddData("Friends", e);
            });
        }
        
    }
}