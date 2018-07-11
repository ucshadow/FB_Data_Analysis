using System;
using System.IO;
using FB_Data_Analysis.Classes.Util;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.Classes.FBCategories {
    public class ProfileDummy : Extractor, IPageTab {
        
        public ProfileDummy(User user) : base(user) {}

        public void Scrap(string title) {
            Print($"Dummy profile {title}", ConsoleColor.Red);
            Console.Beep(5000, 5000);
            WriteLog(title);
        }

        private void WriteLog(string title) {
            using (var sw = File.AppendText(Directory.GetCurrentDirectory() + "/log.txt")) {
                sw.WriteLine($"{title} | {User.Url}");
            }
        }
    }
}