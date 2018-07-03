using System;
using System.Collections.Generic;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.Classes.UserFields {
    public class AboutClass {
        public object this[string propertyName] {
            get => GetType().GetProperty(propertyName).GetValue(this, null);
            set => GetType().GetProperty(propertyName).SetValue(this, value, null);
        }

        public Dictionary<string, string> Work { get; set; }
        public Dictionary<string, string> ProfessionalSkills { get; set; }
        public Dictionary<string, string> Education { get; set; }
        public Dictionary<string, string> CurrentCityAndHometown { get; set; }
        public Dictionary<string, string> OtherPlacesLived { get; set; }
        public Dictionary<string, string> ContactInformation { get; set; }
        public Dictionary<string, string> BasicInformation { get; set; }
        public Dictionary<string, string> WebsitesAndSocialLinks { get; set; }
        public Dictionary<string, string> Relationship { get; set; }
        public Dictionary<string, string> FamilyMembers { get; set; }
        public Dictionary<string, string> AboutPerson { get; set; }
        public Dictionary<string, string> FavoriteQuotes { get; set; }
        public Dictionary<string, string> LifeEvents { get; set; }

        private readonly Dictionary<string, Dictionary<string, string>> _allFields;


        public AboutClass() {
            Work = new Dictionary<string, string>();
            ProfessionalSkills = new Dictionary<string, string>();
            Education = new Dictionary<string, string>();
            CurrentCityAndHometown = new Dictionary<string, string>();
            OtherPlacesLived = new Dictionary<string, string>();
            ContactInformation = new Dictionary<string, string>();
            BasicInformation = new Dictionary<string, string>();
            WebsitesAndSocialLinks = new Dictionary<string, string>();
            Relationship = new Dictionary<string, string>();
            FamilyMembers = new Dictionary<string, string>();
            AboutPerson = new Dictionary<string, string>();
            FavoriteQuotes = new Dictionary<string, string>();
            LifeEvents = new Dictionary<string, string>();

            _allFields = new Dictionary<string, Dictionary<string, string>> {
                {"Work", Work},
                {"Professional Skills", ProfessionalSkills},
                {"Education", Education},
                {"Current City And Hometown", CurrentCityAndHometown},
                {"Other Places Lived", OtherPlacesLived},
                {"Contact Information", ContactInformation},
                {"Basic Information", BasicInformation},
                {"Websites and social links", WebsitesAndSocialLinks},
                {"Relationship", Relationship},
                {"Family Members", FamilyMembers},
                {"About Person", AboutPerson},
                {"Favorite Quotes", FavoriteQuotes},
                {"Life Events", LifeEvents},
            };
        }

        public void AddData(string listName, string value) {
            Print($"adding {value} to {listName}", ConsoleColor.DarkYellow);
            GetType().GetProperty(listName).PropertyType.GetMethod("Add")
                .Invoke(this[listName], new object[] {value});
        }

        public void AddData(string dictionaryName, string key, string value) {
            Print($"adding {key}: {value} to {dictionaryName} len -> {dictionaryName.Length}", ConsoleColor.Yellow);
            GetType().GetProperty(dictionaryName).PropertyType.GetMethod("Add")
                .Invoke(this[dictionaryName], new object[] {key, value});
        }

        public void PrintAbout() {
            Console.WriteLine();
            foreach (var kvp in _allFields) {
                Print($"{kvp.Key}: ", ConsoleColor.Red);
                Console.WriteLine("");
                foreach (var kvpl in kvp.Value) {
                    Print($"{kvpl.Key}: ", ConsoleColor.DarkGreen);
                    Print($"   {kvpl.Value}", ConsoleColor.Yellow);
                }
            }

//            Print("Work: \n");
//            foreach (var kvp in Work) {
//                Print($"{kvp.Key}: {kvp.Value}", ConsoleColor.DarkGreen);
//            }
//
//            Print("Skills: \n");
//            foreach (var kvp in ProfessionalSkills) {
//                Print($"{kvp.Key}: {kvp.Value}", ConsoleColor.DarkGreen);
//            }
//
//            Print("Education: \n");
//            foreach (var kvp in Education) {
//                Print($"{kvp.Key}: {kvp.Value}", ConsoleColor.DarkGreen);
//            }
        }
    }
}