using System;
using System.Collections.Generic;
using System.Linq;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.Classes {
    public class User {

        public WorkAndEducationClass WorkAndEducation;

        public User() {
            WorkAndEducation = new WorkAndEducationClass();
            
        }

        public void PrintUser() {
            WorkAndEducation.PrintUser();
        }
        
        public class WorkAndEducationClass {

            public Dictionary<string, string> Work;
            public List<string> Skills;
            public Dictionary<string, string> Education;

            public WorkAndEducationClass() {
                Work = new Dictionary<string, string>();
                Skills = new List<string>();
                Education = new Dictionary<string, string>();
                
            }
            
            public void PrintUser() {
                Print("Work: \n");
                foreach (var kvp in Work) {
                    Print($"{kvp.Key}: {kvp.Value}", ConsoleColor.DarkGreen);
                }
                
                Print("Skills: \n");
                foreach (var kvp in Skills) {
                    Print($"{kvp}, ", ConsoleColor.DarkGreen);
                }
                
                Print("Education: \n");
                foreach (var kvp in Education) {
                    Print($"{kvp.Key}: {kvp.Value}", ConsoleColor.DarkGreen);
                }
            }

        }
        
    }

    
}