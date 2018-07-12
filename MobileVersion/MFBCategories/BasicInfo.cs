using System;
using System.Collections.Generic;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.MobileVersion.MFBCategories {
    public class BasicInfo : IProfileField {

        public Dictionary<string, string> Data;

        public BasicInfo() {
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