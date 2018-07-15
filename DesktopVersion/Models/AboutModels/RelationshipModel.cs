using System;
using FB_Data_Analysis.Classes.UserFields;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.DesktopVersion.Models.AboutModels {
    public class RelationshipModel : IProfileField {
        
        public string Type;
        public string Value;
        public string Url;

        public void AddData(string[] data) {
            Type = data[0];
            Value = data[1];
            Url = data[2];
        }

        public void Log() {
            Print($"{Type} -> {Value}", ConsoleColor.Green);
        }
    }
}