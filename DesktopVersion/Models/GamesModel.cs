using System;
using FB_Data_Analysis.Classes.UserFields;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.DesktopVersion.Models {
    public class GamesModel : IProfileField {

        public string Type;
        public string Name;
        public string Time;

        public void AddData(string[] data) {
            Type = data[0];
            Name = data[1];
            Time = data[2];
        }

        public void Log() {
            Print($"[{Type}] {Name} -> {Time}", ConsoleColor.Green);
        }
        
    }
}