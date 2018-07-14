using System;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.MobileVersion.MFBCategories {
    public struct FavoriteQuotes : IProfileField {
        
        public string Quote;

        public void Log() {
            var x = GetType().GetFields();
            foreach (var fieldInfo in x) {
                Print($"{fieldInfo.Name} -> {fieldInfo.GetValue(this)}", ConsoleColor.Green);
            }
        }
    }
}