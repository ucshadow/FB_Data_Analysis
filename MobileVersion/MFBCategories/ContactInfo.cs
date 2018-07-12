using System;
using System.Collections.Generic;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.MobileVersion.MFBCategories {
    public class ContactInfo : IProfileField {

        public Dictionary<string, string> Data;

        public ContactInfo() {
            Data = new Dictionary<string, string>();
        }
        
        public void Log() {
            var x = GetType().GetFields();
            foreach (var fieldInfo in x) {
                Print($"{fieldInfo.Name} -> {fieldInfo.GetValue(this)}", ConsoleColor.Green);
            }
        }
    }
}