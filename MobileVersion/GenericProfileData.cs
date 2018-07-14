using System;
using System.Collections.Generic;
using FB_Data_Analysis.MobileVersion.MFBCategories;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.MobileVersion {
    public class GenericProfileData {
        
        public Dictionary<string, List<IProfileField>> Data;
        public string Url;

        public GenericProfileData() {
            Data = new Dictionary<string, List<IProfileField>>();
        }

        public void AddData(string key, IProfileField value) {
            if (Data.ContainsKey(key)) {
                Data[key].Add(value);
            }
            else {
                Data[key] = new List<IProfileField> {value};
            }
        }

//        public void AddData(string key, string value) {
//            if (Data.ContainsKey(key)) {
//                Print($"Adding {value} to {key} list", ConsoleColor.DarkYellow);
//                Data[key].Add(value);
//            }
//            else {
//                Data.Add(key, new List<string>());
//                Print($"Creating {key} and adding {value}", ConsoleColor.Yellow);
//                Data[key].Add(value);
//            }
//        }
    }
}