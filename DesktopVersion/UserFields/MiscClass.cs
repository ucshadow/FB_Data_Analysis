using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.Classes.UserFields {
    public class MiscClass {
        
        public object this[string propertyName] {
            get => GetType().GetProperty(propertyName).GetValue(this, null);
            set => GetType().GetProperty(propertyName).SetValue(this, value, null);
        }
        
        private readonly Dictionary<string, Dictionary<string, string>> _allFields;

        public Dictionary<string, string> CheckIns { get; set; }
        public Dictionary<string, string> Sports { get; set; }
        public Dictionary<string, string> Music { get; set; }
        public Dictionary<string, string> Movies { get; set; }
        public Dictionary<string, string> TVShows { get; set; }
        public Dictionary<string, string> Books { get; set; }
        public Dictionary<string, string> AppsandGames { get; set; }
        public Dictionary<string, string> Likes { get; set; }
        public Dictionary<string, string> Reviews { get; set; }
        public Dictionary<string, string> Events { get; set; }
        public Dictionary<string, string> Fitness { get; set; }

        public MiscClass() {
            CheckIns = new Dictionary<string, string>();
            Sports = new Dictionary<string, string>();
            Music = new Dictionary<string, string>();
            Movies = new Dictionary<string, string>();
            TVShows = new Dictionary<string, string>();
            Books = new Dictionary<string, string>();
            AppsandGames = new Dictionary<string, string>();
            Likes = new Dictionary<string, string>();
            Reviews = new Dictionary<string, string>();
            Events = new Dictionary<string, string>();
            Fitness = new Dictionary<string, string>();
            
            
            _allFields = new Dictionary<string, Dictionary<string, string>> {
                {"CheckIns", CheckIns},
                {"Sports", Sports},
                {"Music", Music},
                {"Movies", Movies},
                {"TVShows", TVShows},
                {"Books", Books},
                {"AppsAndGames", AppsandGames},
                {"Likes", Likes},
                {"Reviews", Reviews},
                {"Events", Events},
                {"Fitness", Fitness},
            };
        }

        public void PrintMisc() {
            Console.WriteLine();
            foreach (var keyValuePair in _allFields) {
                Print($"{keyValuePair.Key}:", ConsoleColor.DarkRed);
                foreach (var kvp in keyValuePair.Value) {
                    Print($"{kvp.Key}: {kvp.Value}");
                }
                //keyValuePair.Value.ForEach(e => Print(e, ConsoleColor.DarkYellow));
            }
        }
        
        public void AddData(string listName, string key, string value) {

            var noSpace = Regex.Replace(listName, @"\s+", "");
            
            Print($"adding {key}: {value} to {noSpace} len -> {noSpace.Length}", ConsoleColor.Yellow);
            GetType().
                GetProperty(noSpace).PropertyType.
                GetMethod("Add").
                Invoke(this[noSpace], new object[] {key, value});
        }

    }
}