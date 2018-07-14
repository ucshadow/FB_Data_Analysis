using FB_Data_Analysis.Classes;
using FB_Data_Analysis.DesktopVersion.Util;

namespace FB_Data_Analysis.DesktopVersion.FBCategories {
    public class ProfileCheckIns : Extractor, IPageTab {

        //private readonly ChromeDriver _driver;
        //private User _user;

        public ProfileCheckIns(User user) : base(user) {}

        public void Scrap(string title) {
            Scrap(title, "pagelet_timeline_medley_map", "CheckIns");
        }

    }
    
    
}