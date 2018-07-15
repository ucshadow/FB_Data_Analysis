using System;
using FB_Data_Analysis.Classes.UserFields;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.DesktopVersion.Models.AboutModels {
    public class BasicInformationModel : IProfileField {
        
        public string Name;
        public string Value;

        public void AddData(string[] data) {
            Name = data[0];
            Value = data[1];
        }

        public void Log() {
            Print($"{Name} -> {Value}", ConsoleColor.Green);
        }
    }
}