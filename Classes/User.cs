using System;
using System.Collections.Generic;
using System.Linq;
using FB_Data_Analysis.Classes.UserFields;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.Classes {
    public class User {

        public AboutClass About { get; set; }

        public User() {
            About = new AboutClass();
            
        }

        public void PrintUser() {
            About.PrintUser();
        }
        
        
        
    }

    
}