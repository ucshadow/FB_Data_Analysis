using System;
using System.Collections.Generic;
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

        public MiscClass() {
            CheckIns = new List<string>();
            Sports = new List<string>();
            Music = new List<string>();
            Movies = new List<string>();
            
            _allFields = new Dictionary<string, List<string>> {
                {"CheckIns", CheckIns},
                {"Sports", Sports},
                {"Music", Music},
                {"Movies", Movies},
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
            Print($"adding {value} to {listName} len -> {listName.Length}", ConsoleColor.Yellow);
            GetType().GetProperty(listName).PropertyType.GetMethod("Add")
                .Invoke(this[listName], new object[] {value});
        }

    }
}