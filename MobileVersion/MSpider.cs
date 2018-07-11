using System;
using System.Linq;
using FB_Data_Analysis.Classes;
using FB_Data_Analysis.Classes.Util;
using FB_Data_Analysis.MobileVersion.MFBCategories;
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
            GoToPageAndLogin(email, password);
            //var url = "https://m.facebook.com/CristinaUngurean97/about";
            var url = "https://m.facebook.com/antonio.silva.7923/about";
            _driver.Url = url;
            Wait(2000, 1000);
            GetWork();
        }

        private void GetWork() {
            Print($"Getting rows", ConsoleColor.Blue);
            var rows = _driver.FindElementsByXPath("//div[@id='work']//div[@class='_5cds _2lcw']");
            Print($"{rows.Count}", ConsoleColor.Cyan);

            foreach (var row in rows) {
                IProfileField work = new Work();

                // work place

                var qq = ElementIsPresent(row, By.ClassName("_52jd"), true);
                work.GetType().GetField("WorkPlace").SetValue(work, qq?.Text);

                // work title

                qq = ElementIsPresent(row, By.ClassName("_52ja"), true);
                work.GetType().GetField("Title").SetValue(work, qq?.Text);
//                
                // work location

                var all = ElementIsPresent(row, By.ClassName("_52j9"), 0);
                foreach (var r in all) {
                    var text = r.Text;

                    Print($"Found text {text}", ConsoleColor.DarkRed);

                    if (IsDate(text)) {
                        work.GetType().GetField("TimeSpan").SetValue(work, text);
                    }
                    else {
                        work.GetType().GetField("Location").SetValue(work, text);
                    }
                }

                // comment
                qq = ElementIsPresent(row, By.ClassName("_5p1r"), true);
                work.GetType().GetField("Comment").SetValue(work, qq?.Text);

                work.Log();
            }
        }

        private bool IsDate(string text) {
            var qq = text.ToCharArray().Count(s => int.TryParse(s.ToString(), out var x));

            Print($"{text} had {qq} numbers");
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
            Print($"Mobile linkl: {x}");
        }


        private void GoToPageAndLogin(string email, string password) {
            _driver.Url = "https://m.facebook.com";
            _driver.FindElementById("m_login_email").SendKeys(email);
            _driver.FindElementById("m_login_password").SendKeys(password);
            _driver.FindElementById("u_0_5").Click();
            Wait(2000, 1000);
        }
    }
}