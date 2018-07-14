using System;
using FB_Data_Analysis.Classes.UserFields;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.DesktopVersion.Models {
    public class EventsModel : IProfileField {

        public string Name;
        public string Time;
        public string Url;

        public void AddData(string[] data) {
            Name = data[0];
            Time = data[1];
            Url = data[2];
        }

        public void Log() {
            Print($"[{Time}] {Name} -> {Url}", ConsoleColor.Green);
        }
        
    }
}