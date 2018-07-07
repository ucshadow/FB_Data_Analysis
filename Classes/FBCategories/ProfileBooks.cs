using FB_Data_Analysis.Classes.Util;

namespace FB_Data_Analysis.Classes.FBCategories {
    public class ProfileBooks : Extractor, IPageTab {
        
        public ProfileBooks(User user) : base(user) {
        } 
        
        public void Scrap(string title) {
            var id = "pagelet_timeline_medley_books";

            GeneralScrap(title, id);
        }
        
    }
}