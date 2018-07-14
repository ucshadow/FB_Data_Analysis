using FB_Data_Analysis.Classes;
using FB_Data_Analysis.Classes.Util;
using FB_Data_Analysis.DesktopVersion.Util;

namespace FB_Data_Analysis.DesktopVersion.FBCategories {
    public class ProfileAppsAndGames : Extractor, IPageTab {
        public ProfileAppsAndGames(User user) : base(user) {
        } 
        
        public void Scrap(string title) {
            var id = "pagelet_timeline_medley_games";

            GeneralScrap(title, id);
        }
    }
}