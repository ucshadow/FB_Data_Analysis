using System;
using System.Collections.Generic;

namespace FB_Data_Analysis.Classes.UserFields {
    public class CheckInsClass {

        public List<string> CheckIns;

        public CheckInsClass() {
            CheckIns = new List<string>();
        }

        public void PrintCheckIns() {
            Console.WriteLine();
            CheckIns.ForEach(e => Helpers.Print(e, ConsoleColor.DarkGreen));
        }

    }
}