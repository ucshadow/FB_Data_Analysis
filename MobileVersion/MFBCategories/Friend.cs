using System;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.MobileVersion.MFBCategories {
    public struct Friend : IProfileField {
        
        public string Name;
        public string WorkPlace;
        public string Url;

        public void Log() {
            var x = GetType().GetFields();
            foreach (var fieldInfo in x) {
                Print($"{fieldInfo.Name} -> {fieldInfo.GetValue(this)}", ConsoleColor.Green);
            }
        }
    }
}