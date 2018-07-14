using FB_Data_Analysis.Classes;
using FB_Data_Analysis.DesktopVersion.Util;

namespace FB_Data_Analysis.DesktopVersion.FBCategories {
    public class ProfileMovies : Extractor, IPageTab {

        public ProfileMovies(User user) : base(user) {
        } 

        public void Scrap(string title) {
            var id = "pagelet_timeline_medley_movies";

            GeneralScrap(title, id);
        }
        
    }
}