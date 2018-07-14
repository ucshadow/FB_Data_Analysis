using FB_Data_Analysis.Classes;
using FB_Data_Analysis.DesktopVersion.Util;

namespace FB_Data_Analysis.DesktopVersion.FBCategories {
    public class ProfileMusic : Extractor, IPageTab {
        
        public ProfileMusic(User user) : base(user) {
        } 
        
        public void Scrap(string title) {
            Scrap(title, "pagelet_timeline_medley_music", "Music");
        }
        
    }
}