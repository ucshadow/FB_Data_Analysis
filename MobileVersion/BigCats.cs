using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FB_Data_Analysis.Classes;
using FB_Data_Analysis.MobileVersion.MFBCategories;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.MobileVersion {
    public static class BigCats {

        private static readonly ChromeDriver Driver = SeleniumProvider.Driver;
        
        public static void GetBigCats(GenericProfileData profileData) {
            var all = Driver.FindElementsByXPath("//div[@class='_55wr _4g33 _52we _5b6o touchable _592p _25mv']");
            
            var cats = new Dictionary<string, Action<GenericProfileData, string>>();
            
            foreach (var cat in all) {
                var href = cat.FindElement(By.CssSelector("a")).GetAttribute("href");
                var name = cat.Text;

                switch (name) {
                    case "CHECK-INS": //done
                        //cats.Add(href, GetCheckIns);
                        break;
                    case "FRIENDS": // done
                        //cats.Add(href, GetFriends);
                        break;
                    case "SPORTS": // done
                        //cats.Add(href, GetSports);
                        break;
                    case "MUSIC":
                        cats.Add(href, GetMusic);
                        break;
                    case "MOVIES":
                        cats.Add(href, GetMovies);
                        break;
                    case "TV SHOWS":
                        cats.Add(href, GetTv);
                        break;
                    case "BOOKS":
                        cats.Add(href, GetBooks);
                        break;
                    case "LIKES":
                        cats.Add(href, GetLikes);
                        break;
                    case "REVIEWS":
                        cats.Add(href, GetReviews);
                        break;
                    case "EVENTS":
                        cats.Add(href, GetEvents);
                        break;
                    case "APPS AND GAMES":
                        cats.Add(href, GetGames);
                        break;
                    case "FITNESS":
                        cats.Add(href, GetFitness);
                        break;
                    default:
                        Print($"Unknow category found {name}", ConsoleColor.Red);
                        break;
                }
                
                Print($"{cat.Text} -> {href}", ConsoleColor.Magenta);
                
            }
            
            foreach (var kvp in cats) {
                kvp.Value(profileData, kvp.Key);
            }
        }

        private static void GetSports(GenericProfileData profileData, string url) {
            Print("Getting Sports", ConsoleColor.Yellow);
            Driver.Url = url;

            var cats = Driver.FindElementsByClassName("_25mv");
            
            var dic = new Dictionary<string, string>();
            
            foreach (var cat in cats) {
                var name = RemoveNumbers(cat.Text);
                var href = cat.FindElement(By.CssSelector("a")).GetAttribute("href");
                dic.Add(name, href);
            }
            
            foreach (var kvp in dic) {
                
                Driver.Url = kvp.Value;
                
                //scrap data
                var entries = Driver.FindElementsByClassName("_1a5p");
                for (var index = 0; index < entries.Count; index++) {
                    var entry = entries[index];
                    var name = entry.FindElement(By.CssSelector("span")).Text;

                    Print($"Adding {kvp.Key}: {name}, {entries.Count - index} remaining", ConsoleColor.Green);

                    profileData.AddData("Sports", new Sports() {Name = name, Type = kvp.Key});
                }
            }
        }
        
        
        private static void GetMusic(GenericProfileData profileData, string url) {
            
        }
        private static void GetMovies(GenericProfileData profileData, string url) {
            
        }
        private static void GetTv(GenericProfileData profileData, string url) {
            
        }
        private static void GetBooks(GenericProfileData profileData, string url) {
            
        }
        private static void GetLikes(GenericProfileData profileData, string url) {
            
        }
        private static void GetReviews(GenericProfileData profileData, string url) {
            
        }
        private static void GetEvents(GenericProfileData profileData, string url) {
            
        }
        private static void GetGames(GenericProfileData profileData, string url) {
            
        }
        private static void GetFitness(GenericProfileData profileData, string url) {
            
        }

        private static void GetCheckIns(GenericProfileData profileData, string url) {
            
            Print("Getting Check-Ins", ConsoleColor.Yellow);
            
            //OpenNewTabAndFocus();
            Driver.Url = url;

            //var allowed = new List<string>() {"RECENT"};
            var hrefs = new Dictionary<string, string>();
            
            Wait(2000, 1000);

            var cats = Driver.FindElementsByXPath("//div[@class='_55wr _4g33 _52we _5b6o touchable _592p _25mv']");
            
            foreach (var cat in cats) {
                //Print($"Cat text is {cat.Text}", ConsoleColor.DarkCyan);
                
                if (cat.Text.Contains("RECENT")) {
                    hrefs.Add("Recent", cat.FindElement(By.CssSelector("a")).GetAttribute("href"));
                }
            }
            
            foreach (var kv in hrefs) {
                GetSpecificChckIn(kv.Key, kv.Value, profileData);
            }
            
            //CloseAndSwitchToMainTab();
        }

        private static void GetSpecificChckIn(string key, string value, GenericProfileData profileData) {
            OpenNewTabAndFocus();
                
            Driver.Url = value;
            
            ScrollToBottom();

            var rows = Driver.FindElementsByClassName("content");

            for (var index = 1; index < rows.Count; index++) {
                var row = rows[index];
                var name = row.FindElement(By.ClassName("title")).Text;
                var data = row.FindElements(By.CssSelector("h4"));
                var location = data[0].Text;
                var time = data[1].Text;

                Print($"Adding place {name}, {rows.Count - index} remaining", ConsoleColor.Green);
                profileData.AddData("CheckIns", new CheckIns() {
                    Type = key,
                    Name = name,
                    Location = location,
                    Time = time
                });
            }

            CloseAndSwitchToMainTab();
        }

        private static void GetFriends(GenericProfileData profileData, string url) {
            Print("Getting Friends", ConsoleColor.Yellow);
            
            //OpenNewTabAndFocus();
            
            Driver.Url = url;
            Wait(1000, 500);
            ScrollToBottom(1000);
            
            var rows = Driver.FindElementsByClassName("_5pxb");

            var stop = Stopwatch.StartNew();

            for (var index = 0; index < rows.Count; index++) {
                var row = rows[index];
                var f = new Friend();

                var a = row.FindElement(By.TagName("a"));

                var name = a.Text;
                var workPlace = row.FindElement(By.ClassName("_30yn")).Text;
                var href = a.GetAttribute("href");

                f.Name = name;
                f.WorkPlace = workPlace;
                f.Url = href;

                Print($"Adding friend {name}, {rows.Count - index} remaining", ConsoleColor.Green);
                profileData.AddData("Friends", f);
            }

            stop.Stop();
            Print($"Added {rows.Count} in {stop.Elapsed}");
            
            //CloseAndSwitchToMainTab();
        }

        private static string RemoveNumbers(string s) {
            return new string(s.Where(char.IsLetter).ToArray());
        }
        
    }
}