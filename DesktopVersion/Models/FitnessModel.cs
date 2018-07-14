using System;
using FB_Data_Analysis.Classes.UserFields;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.DesktopVersion.Models {
    public class FitnessModel : IProfileField {

        public string Type;
        public string Name;
        public string Description;
        public string Url;

        public void AddData(string[] data) {
            Type = data[0];
            Name = data[1];
            Description = data[2];
            Url = data[3];
        }

        public void Log() {
            Print($"[{Type}] {Name} -> {Description}", ConsoleColor.Green);
        }
        
    }
}