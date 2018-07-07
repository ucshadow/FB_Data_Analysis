using FB_Data_Analysis.Classes.Util;

namespace FB_Data_Analysis.Classes.FBCategories {
    public class ProfileAppsAndGames : Extractor, IPageTab {
        public ProfileAppsAndGames(User user) : base(user) {
        } 
        
        public void Scrap(string title) {
            var id = "pagelet_timeline_medley_games";

            GeneralScrap(title, id);
        }
    }
}