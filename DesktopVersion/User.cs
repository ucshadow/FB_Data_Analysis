using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FB_Data_Analysis.Classes.UserFields;
using Newtonsoft.Json;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.Classes {
    public class User {

        public AboutClass About { get; set; }
        public MiscClass Misc { get; set; }
        public string Url { get; set; }

        public User() {
            About = new AboutClass();
            Misc = new MiscClass();
            
        }

        public void PrintUser() {
            About.PrintAbout();
            Misc.PrintMisc();
        }

        public void Jsonise() {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            var path = $"{Directory.GetCurrentDirectory()}/output/{Url.Split("?")[0].Split("/").Last()}";
            Print($"Writing to {path}");
            File.WriteAllText($"{path}_.json", json);
        }
        
    }

    
}