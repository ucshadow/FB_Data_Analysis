using System;
using FB_Data_Analysis.Classes;
using FB_Data_Analysis.Classes.UserFields;

namespace FB_Data_Analysis.DesktopVersion.Models {
    public class LikesModel : IProfileField {
        
        public string Name;
        public string Url;

        public void AddData(string[] data) {
            Name = data[0];
            Url = data[1];
        }

        public void Log() {
            Helpers.Print($"{Name} -> {Url}", ConsoleColor.Green);
        }
    }
}