using System;
using System.Collections.Generic;
using System.Linq;
using FB_Data_Analysis.Classes.UserFields;
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
            
        }
        
    }

    
}