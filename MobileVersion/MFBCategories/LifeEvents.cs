using System;
using System.Collections.Generic;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.MobileVersion.MFBCategories {
    public class LifeEvents : IProfileField {

        public Dictionary<string, List<string>> Data;

        public LifeEvents() {
            Data = new Dictionary<string, List<string>>();
        }

        public void AddData(string key, string value) {
            if (Data.ContainsKey(key)) {
                Data[key].Add(value);
            }
            else {
                Data.Add(key, new List<string>(){value});
            }
        }
        
        public void Log() {
            var x = GetType().GetFields();
            foreach (var fieldInfo in x) {
                Print($"{fieldInfo.Name} -> {fieldInfo.GetValue(this)}", ConsoleColor.Green);
            }
        }
    }
}