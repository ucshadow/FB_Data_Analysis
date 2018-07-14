using System;
using FB_Data_Analysis.Classes;
using FB_Data_Analysis.Classes.UserFields;

namespace FB_Data_Analysis.DesktopVersion.Models {
    public class FriendsModel : IProfileField {
        
        public string Name;
        public string WorkPlace;
        public string Url;

        public void AddData(string[] data) {
            Name = data[0];
            WorkPlace = data[1];
            Url = data[2];
        }

        public void Log() {
            Helpers.Print($"[{Name}] {WorkPlace} -> {Url}", ConsoleColor.Green);
        }
    }
}