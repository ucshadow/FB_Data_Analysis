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
        
        private readonly Dictionary<string, List<string>> _allFields;

        public List<string> CheckIns { get; set; }
        public List<string> Sports { get; set; }
        public List<string> Music { get; set; }
        public List<string> Movies { get; set; }
        public List<string> TVShows { get; set; }
        public List<string> Books { get; set; }
        public List<string> AppsandGames { get; set; }
        public List<string> Likes { get; set; }
        public List<string> Reviews { get; set; }
        public List<string> Events { get; set; }
        public List<string> Fitness { get; set; }

        public MiscClass() {
            CheckIns = new List<string>();
            Sports = new List<string>();
            Music = new List<string>();
            Movies = new List<string>();
            TVShows = new List<string>();
            Books = new List<string>();
            AppsandGames = new List<string>();
            Likes = new List<string>();
            Reviews = new List<string>();
            Events = new List<string>();
            Fitness = new List<string>();
            
            
            _allFields = new Dictionary<string, List<string>> {
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
                keyValuePair.Value.ForEach(e => Print(e, ConsoleColor.DarkYellow));
            }
        }
        
        public void AddData(string listName, string value) {

            var noSpace = Regex.Replace(listName, @"\s+", "");
            
            Print($"adding {value} to {noSpace} len -> {noSpace.Length}", ConsoleColor.Yellow);
            GetType().
                GetProperty(noSpace).PropertyType.
                GetMethod("Add").
                Invoke(this[noSpace], new object[] {value});
        }

    }
}