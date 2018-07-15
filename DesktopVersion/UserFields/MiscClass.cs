using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FB_Data_Analysis.Classes;
using FB_Data_Analysis.Classes.UserFields;

namespace FB_Data_Analysis.DesktopVersion.UserFields {
    public class MiscClass {

        private readonly Dictionary<string, List<IProfileField>> _allFields;
        
        public List<IProfileField> CheckIns { get; }
        public List<IProfileField> Friends { get; }
        public List<IProfileField> Likes { get; }
        public List<IProfileField> Sports { get; }
        public List<IProfileField> Music { get; }
        public List<IProfileField> Movies { get; }
        public List<IProfileField> TVShows { get; }
        public List<IProfileField> Books { get; }
        public List<IProfileField> Reviews { get; }
        public List<IProfileField> Events { get; }
        public List<IProfileField> Games { get; }
        public List<IProfileField> Fitness { get; }

        public MiscClass() {
            CheckIns = new List<IProfileField>();
            Friends = new List<IProfileField>();
            Likes = new List<IProfileField>();
            Sports = new List<IProfileField>();
            Music = new List<IProfileField>();
            Movies = new List<IProfileField>();
            TVShows = new List<IProfileField>();
            Books = new List<IProfileField>();
            Reviews = new List<IProfileField>();
            Events = new List<IProfileField>();
            Games = new List<IProfileField>();
            Fitness = new List<IProfileField>();


            _allFields = new Dictionary<string, List<IProfileField>> {
                {"CheckIns", CheckIns},
                {"Friends", Friends},
                {"Likes", Likes},
                {"Sports", Sports},
                {"Music", Music},
                {"Movies", Movies},
                {"TVShows", TVShows},
                {"Books", Books},
                {"Reviews", Reviews},
                {"Events", Events},
                {"AppsandGames", Games},
                {"Fitness", Fitness},
            };
        }

        public void PrintMisc() {
            Console.WriteLine();
            foreach (var keyValuePair in _allFields) {
                Helpers.Print($"{keyValuePair.Key}:", ConsoleColor.DarkRed);
                keyValuePair.Value.ForEach(e => e.Log());
                //keyValuePair.Value.ForEach(e => Print(e, ConsoleColor.DarkYellow));
            }
        }

        public void AddData(string listName, string[] data) {
            var noSpace = Regex.Replace(listName, @"\s+", "");
            
            //Helpers.Print($"Trying to get type for FB_Data_Analysis.DesktopVersion.Models.{noSpace}Model");

            var obj = (IProfileField) GetInstance($"FB_Data_Analysis.DesktopVersion.Models.{noSpace}Model");
            //var obj = (IProfileField) Activator.CreateInstance(_allFields[noSpace].GetType());

            //var local = new CheckInsModel();
            obj.AddData(data);

            //Helpers.Print($"adding {noSpace}", ConsoleColor.Yellow);
            _allFields[noSpace].Add(obj);
            //GetType().GetProperty(noSpace).PropertyType.GetMethod("Add").Invoke(this[noSpace], new object[] {obj});
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
    }
}