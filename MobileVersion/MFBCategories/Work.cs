using System;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.MobileVersion.MFBCategories {
    public struct Work : IProfileField {
        public string WorkPlace;
        public string Title;
        public string TimeSpan;
        public string Location;
        public string Comment;

        public void Log() {
            Print($"[Work] place {WorkPlace} " +
                  $" title {Title} timeSpan {TimeSpan} location {Location} " +
                  $"\n comment {Comment}", ConsoleColor.Green);
        }
    }
}