using System;
using FB_Data_Analysis.Classes.UserFields;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.DesktopVersion.Models.AboutModels {
    public class CurrentCityAndHometownModel : IProfileField {
        
        public string Name;
        public string Url;
        public string Description;

        public void AddData(string[] data) {
            Name = data[0];
            Url = data[1];
            Description = data[2];
        }

        public void Log() {
            Print($"{Name} -> {Description}", ConsoleColor.Green);
        }
    }
}