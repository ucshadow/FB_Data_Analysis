using System;
using System.IO;
using System.Linq;
using FB_Data_Analysis.Classes;
using FB_Data_Analysis.Classes.Util;
using FB_Data_Analysis.MobileVersion.MFBCategories;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.MobileVersion {
    public class MSpider {
        private ChromeDriver _driver;

        public MSpider() {
            _driver = SeleniumProvider.Driver;
        }

        public void MainLoop(string groupUrl, string email, string password) {
            GoToPageAndLogin(email, password);
            Wait(2000, 500);
            GetGroupUsers(groupUrl);
        }

        public void SingleUSerTest(string email, string password) {
            var profileData = new GenericProfileData();

            GoToPageAndLogin(email, password);
            
            var url = "https://m.facebook.com/zuck/about";

            _driver.Url = url;
            Wait(2000, 1000);


            //GetWork(profileData);
            //GetEducation(profileData);
            //GetSkills(profileData);
            //GetPlacesLived(profileData);

            //GetContactInfo(profileData);
            //GetBasicInfo(profileData);
            
            GetFamilyMembers(profileData);
            Jsonise(profileData);
        }

        private void GetFamilyMembers(GenericProfileData profileData) {
            Print($"Getting rows", ConsoleColor.Blue);
            var rows = _driver.FindElementsByXPath("//div[@id='family']//div[@class='_4g34']");
            
            foreach (var row in rows) {
                var family = new FamilyMembers();
                
                var name = ElementIsPresent(row, By.ClassName("_52jb"), true)?.Text;
                var relation = ElementIsPresent(row, By.ClassName("_52jg"), true)?.Text;
                var url = ElementIsPresent(row, By.CssSelector("a"), true)?.GetAttribute("href");
                
                Print($"Adding name {name}, relation {relation}, url {url}", ConsoleColor.DarkGreen);
                
                family.Name = name;
                family.Relation = relation;
                family.Url = url;
                
                profileData.AddData("FamilyMembers", family);
            }
        }
        
        private void GetBasicInfo(GenericProfileData profileData) {
            Print($"Getting rows", ConsoleColor.Blue);
            var rows = _driver.FindElementsByXPath("//div[@id='basic-info']//div[@class='lr']");
            
            var basic = new BasicInfo();
            
            foreach (var row in rows) {
                
                var key = ElementIsPresent(row, By.ClassName("_52jd"), true)?.Text;
                var value = ElementIsPresent(row, By.ClassName("_5cdv"), true)?.Text;
                
                Print($"Adding basic info {key}: {value}", ConsoleColor.DarkGreen);
                basic.Data.Add(key, value);
            }
            Print($"Adding all basic info...", ConsoleColor.DarkGreen);
            profileData.AddData("BasicInfo", basic);
        }

        private void GetContactInfo(GenericProfileData profileData) {
            Print($"Getting rows", ConsoleColor.Blue);
            var rows = _driver.FindElementsByXPath("//div[@id='contact-info']//div[@class='lr']");
            
            var contact = new ContactInfo();
            foreach (var row in rows) {
                
                var key = ElementIsPresent(row, By.ClassName("_52jd"), true)?.Text;
                var value = ElementIsPresent(row, By.ClassName("_5cdv"), true)?.Text;
                
                Print($"Adding contact info {key}: {value}", ConsoleColor.DarkGreen);
                contact.Data.Add(key, value);
            }
            Print($"Adding all contacts...", ConsoleColor.DarkGreen);
            profileData.AddData("ContactInfo", contact);
        }

        private void GetPlacesLived(GenericProfileData profileData) {
            Print($"Getting rows", ConsoleColor.Blue);
            var rows = _driver.FindElementsByXPath("//div[@id='living']//div[@class='_5xu4']");

            foreach (var row in rows) {
                var lives = new Living();


                var headers = row.FindElements(By.CssSelector("h4"));
                if (headers.Count == 2) {
                    lives.Location = headers[0]?.Text;
                    lives.Info = headers[1]?.Text;
                    Print($"Adding living places {lives}", ConsoleColor.DarkGreen);
                    profileData.AddData("Living", lives);
                }
                else if (headers.Count == 1) {
                    lives.Location = headers[0]?.Text;
                    Print($"Adding living places {lives}", ConsoleColor.DarkGreen);
                    profileData.AddData("Living", lives);
                }
            }
        }

        private void GetWork(GenericProfileData profileData) {
            Print($"Getting rows", ConsoleColor.Blue);
            var rows = _driver.FindElementsByXPath("//div[@id='work']//div[@class='_5cds _2lcw']");
            //Print($"{rows.Count}", ConsoleColor.Cyan);

            foreach (var row in rows) {
                var work = new Work();

                // work place
                var qq = ElementIsPresent(row, By.ClassName("_52jd"), true);
                work.WorkPlace = qq?.Text;

                // work title
                qq = ElementIsPresent(row, By.ClassName("_52ja"), true);
                work.Title = qq?.Text;
//                
                // work location
                var all = ElementIsPresent(row, By.ClassName("_52j9"), 0);
                foreach (var r in all) {
                    var text = r.Text;

                    //Print($"Found text {text}", ConsoleColor.DarkRed);

                    if (IsDate(text)) {
                        work.TimeSpan = text;
                    }
                    else {
                        work.Location = text;
                    }
                }

                // comment
                qq = ElementIsPresent(row, By.ClassName("_5p1r"), true);
                work.Comment = qq?.Text;

                //work.Log();
                Print($"Adding work {work}", ConsoleColor.DarkGreen);
                profileData.AddData("Work", work);
            }
        }

        private void GetEducation(GenericProfileData profileData) {
            Print($"Getting rows", ConsoleColor.Blue);
            var rows = _driver.FindElementsByXPath("//div[@id='education']//div[@class='_5cds _2lcw']");

            foreach (var row in rows) {
                var education = new Education();

                // institution
                var qq = ElementIsPresent(row, By.ClassName("_3-8_"), true);
                education.Institution = qq?.Text;

                // type
                qq = ElementIsPresent(row, By.ClassName("_52ja"), true);
                if (qq.FindElements(By.CssSelector("span")).Count == 0) {
                    education.Type = qq?.Text;
                }

                // profile
                qq = ElementIsPresent(row, By.ClassName("concs"), true);
                education.Profile = qq?.Text;

                // time
                qq = ElementIsPresent(row, By.ClassName("_52j9"), true);
                education.GetType().GetField("Time").SetValue(education, qq?.Text);

                // comment
                qq = ElementIsPresent(row, By.ClassName("_5p1r"), true);
                education.Comment = qq?.Text;

                Print($"Adding education {education}", ConsoleColor.DarkGreen);
                profileData.AddData("Education", education);
            }
        }

        private void GetSkills(GenericProfileData profileData) {
            Print($"Getting rows", ConsoleColor.Blue);
            var rows = _driver.FindElementsByClassName("skills");

            foreach (var row in rows) {
                var skill = new Skills {Skill = row?.Text};

                Print($"Adding skill {skill}", ConsoleColor.DarkGreen);
                profileData.AddData("Skill", skill);
            }
        }

        private bool IsDate(string text) {
            var qq = text.ToCharArray().Count(s => int.TryParse(s.ToString(), out var x));

            //Print($"{text} had {qq} numbers");
            return qq >= 2;
        }

        private void GetGroupUsers(string groupUrl) {
            var x = new GroupParser(groupUrl);
            x.GetEm();
            var list = x.UserHrefs;
            list.ForEach(AddMobile);
        }

        private void AddMobile(string url) {
            var x = url.Replace("www", "m");
            //Print($"Mobile linkl: {x}");
        }


        private void GoToPageAndLogin(string email, string password) {
            _driver.Url = "https://m.facebook.com";
            _driver.FindElementById("m_login_email").SendKeys(email);
            _driver.FindElementById("m_login_password").SendKeys(password);
            _driver.FindElementById("u_0_5").Click();
            Wait(2000, 1000);
        }

        private void Jsonise(GenericProfileData data) {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            var path = $"{Directory.GetCurrentDirectory()}";
            Print($"Writing to {path}\\test_.json");
            File.WriteAllText($"{path}\\test_.json", json);
        }
    }
}