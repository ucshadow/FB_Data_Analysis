using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FB_Data_Analysis.Classes;
using FB_Data_Analysis.Classes.UserFields;

namespace FB_Data_Analysis.DesktopVersion.UserFields {
    public class AboutClass {

        private readonly Dictionary<string, List<IProfileField>> _allFields;

        public List<IProfileField> Work { get; }
        public List<IProfileField> ProfessionalSkills { get; }
        public List<IProfileField> Education { get; }
        public List<IProfileField> CurrentCityAndHometown { get; }
        public List<IProfileField> OtherPlacesLived { get; }
        public List<IProfileField> ContactInformation { get; }
        public List<IProfileField> WebsitesAndSocialLinks { get; }
        public List<IProfileField> BasicInformation { get; }
        public List<IProfileField> Relationship { get; }
        public List<IProfileField> FamilyMembers { get; }
        public List<IProfileField> AboutPerson { get; }
        public List<IProfileField> OtherNames { get; }
        public List<IProfileField> FavoriteQuotes { get; }
        public List<IProfileField> LifeEvents { get; }


        public AboutClass() {
            Work = new List<IProfileField>();
            ProfessionalSkills = new List<IProfileField>();
            Education = new List<IProfileField>();
            CurrentCityAndHometown = new List<IProfileField>();
            OtherPlacesLived = new List<IProfileField>();
            ContactInformation = new List<IProfileField>();
            WebsitesAndSocialLinks = new List<IProfileField>();
            BasicInformation = new List<IProfileField>();
            Relationship = new List<IProfileField>();
            FamilyMembers = new List<IProfileField>();
            AboutPerson = new List<IProfileField>();
            OtherNames = new List<IProfileField>();
            FavoriteQuotes = new List<IProfileField>();
            LifeEvents = new List<IProfileField>();

            _allFields = new Dictionary<string, List<IProfileField>> {
                {"Work", Work},
                {"ProfessionalSkills", ProfessionalSkills},
                {"Education", Education},
                {"CurrentCityAndHometown", CurrentCityAndHometown},
                {"OtherPlacesLived", OtherPlacesLived},
                {"ContactInformation", ContactInformation},
                {"WebsitesAndSocialLinks", WebsitesAndSocialLinks},
                {"BasicInformation", BasicInformation},
                {"Relationship", Relationship},
                {"FamilyMembers", FamilyMembers},
                {"AboutPerson", AboutPerson},
                {"OtherNames", OtherNames},
                {"FavoriteQuotes", FavoriteQuotes},
                {"LifeEvents", LifeEvents},
            };
        }
        
        public void AddData(string listName, string[] data) {
            var noSpace = Regex.Replace(listName, @"\s+", "");

            var obj = (IProfileField) 
                GetInstance($"FB_Data_Analysis.DesktopVersion.Models.AboutModels.{noSpace}Model");
            obj.AddData(data);

            _allFields[noSpace].Add(obj);
        }

        private object GetInstance(string strFullyQualifiedName) {
            var type = Type.GetType(strFullyQualifiedName);
            //Print($"Initial type {strFullyQualifiedName} is null", ConsoleColor.DarkYellow);
            if (type != null)
                return Activator.CreateInstance(type);
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies()) {
                //Print($"current asm {asm.FullName}", ConsoleColor.DarkYellow);
                type = asm.GetType(strFullyQualifiedName);
                if (type != null)
                    return Activator.CreateInstance(type);
            }

            return null;
        }

        public void AddData(string listName, string value) {
            Helpers.Print($"adding {value} to {listName}", ConsoleColor.DarkYellow);
            GetType().GetProperty(listName).PropertyType.GetMethod("Add")
                .Invoke(_allFields[listName], new object[] {value});
        }

        public void AddData(string dictionaryName, string key, string value) {
            Helpers.Print($"adding {key}: {value} to {dictionaryName} len -> {dictionaryName.Length}", ConsoleColor.Yellow);
            GetType().GetProperty(dictionaryName).PropertyType.GetMethod("Add")
                .Invoke(_allFields[dictionaryName], new object[] {key, value});
        }

        public void PrintAbout() {
            Console.WriteLine();
            foreach (var kvp in _allFields) {
                Helpers.Print($"{kvp.Key}: ", ConsoleColor.Red);
                Console.WriteLine("");
                foreach (var i in kvp.Value) {
                    i.Log();
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