using System;
using FB_Data_Analysis.Classes.UserFields;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.DesktopVersion.Models.AboutModels {
    public class LifeEventsModel : IProfileField {
        
        public string Year;
        public string Event;

        public void AddData(string[] data) {
            Year = data[0];
            Event = data[1];
        }

        public void Log() {
            Print($"{Year} -> {Event}", ConsoleColor.Green);
        }
    }
}