using FB_Data_Analysis.Classes.Util;

namespace FB_Data_Analysis.Classes.FBCategories {
    public class ProfileTvShows : Extractor, IPageTab {
        
        public ProfileTvShows(User user) : base(user) {
        } 
        
        public void Scrap(string title) {
            var id = "pagelet_timeline_medley_tv";

            GeneralScrap(title, id);
        }
        
    }
}