using System;
using FB_Data_Analysis.Classes.UserFields;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.DesktopVersion.Models {
    public class ReviewsModel : IProfileField {

        public string Name;
        public string Url;
        public string Timestamp;
        public string Rating;
        public string RatingText;

        public void AddData(string[] data) {
            Name = data[0];
            Url = data[1];
            Timestamp = data[2];
            Rating = data[3];
            RatingText = data[4];
        }

        public void Log() {
            Print($"[{Name}] {Rating} -> {RatingText}. {Timestamp}", ConsoleColor.Green);
        }
        
    }
}