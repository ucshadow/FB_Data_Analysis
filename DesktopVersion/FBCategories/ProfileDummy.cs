using System;
using System.IO;
using FB_Data_Analysis.Classes;
using FB_Data_Analysis.DesktopVersion.Util;

namespace FB_Data_Analysis.DesktopVersion.FBCategories {
    public class ProfileDummy : Extractor, IPageTab {
        
        public ProfileDummy(User user) : base(user) {}

        public void Scrap(string title) {
            Helpers.Print($"Dummy profile {title}", ConsoleColor.Red);
            //WriteLog(title);
        }

        private void WriteLog(string title) {
            using (var sw = File.AppendText(Directory.GetCurrentDirectory() + "/log.txt")) {
                sw.WriteLine($"{title} | {User.Url}");
            }
        }
    }
}