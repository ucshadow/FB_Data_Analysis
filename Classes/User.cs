using System;
using System.Collections.Generic;
using System.Linq;
using FB_Data_Analysis.Classes.UserFields;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.Classes {
    public class User {

        public AboutClass About { get; set; }
        public CheckInsClass Checkins { get; set; }

        public User() {
            About = new AboutClass();
            Checkins = new CheckInsClass();
            
        }

        public void PrintUser() {
            About.PrintAbout();
            Checkins.PrintCheckIns();
        }
        
        
        
    }

    
}