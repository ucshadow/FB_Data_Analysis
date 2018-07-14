using System;
using System.Linq;
using FB_Data_Analysis.Classes;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FB_Data_Analysis.DesktopVersion.Util {
    public class Extractor {
        protected readonly ChromeDriver Driver;
        protected readonly User User;

        private readonly string[] _banned = {"Cities"};

        protected Extractor(User user) {
            Driver = SeleniumProvider.Driver;
            User = user;
        }

        private void Extract(string type, string miscCategoryName, string tabId, string title) {
            var d = Driver.FindElementById(tabId);
            GetFieldAndLink(d, type, miscCategoryName, title);
        }

        protected void ClickNavBarButton(int buttonIndex, IWebElement element) {
            var navButtons = element.FindElements(By.ClassName("_3sz"));
            
            if (navButtons.Count - 1 < buttonIndex) return;
            
            Helpers.Print($"Clicking on {navButtons[buttonIndex]?.Text}", ConsoleColor.Gray);
            
            Helpers.ScrollToElement(navButtons[buttonIndex]);
            
            Helpers.ScrollUpSome();
            
            navButtons[buttonIndex].Click();
            
            Helpers.Wait(1000, 200);
        }

        private void GetFieldAndLink(ISearchContext element, string type, string miscCategoryName, string title) {
            var dataBoxes = element.FindElements(By.ClassName("_3owb"));
            for (var i = 0; i < dataBoxes.Count; i++) {
                var webElement = dataBoxes[i];
                var row = webElement.FindElement(By.ClassName("_gx7"));
                var key = row?.Text;
                var value = row?.GetAttribute("href");
                if (!(key?.Trim().Length > 0)) continue;
                Helpers.Print($"( {dataBoxes.Count - i} )Adding {title}", ConsoleColor.Yellow);
                User.Misc.AddData(miscCategoryName, new[] {type, key, value});
            }
        }

        protected void Scrap(string title, string id, string name) {
            
            Helpers.Print($"Scrapping {name} -> {title}", ConsoleColor.DarkRed);
            
            //Driver.Url = url;

            Helpers.Wait(1000, 200);

            var butts = Helpers.GetTabButtons(id);

            for (var i = 0; i < butts.Count; i++) {
                var webElement = butts[i];

                if (name == "CheckIns") {
                    if(!CheckMapButtons(webElement.Text)) continue;
                }
                
                
                if(IsButtonBanned(webElement)) continue;
                
                ClickNavBarButton(i, SeleniumProvider.Driver.FindElementById(id));
                
                Helpers.ScrollToBottom();
                
                Extract(webElement.Text, name, id, title);
                Helpers.Print("------------------------------ ------------------ -------------");
                
                Helpers.ScrollToElement(id);
            }
        }

        private bool CheckMapButtons(string s) {
            return s == "Places" || s == "Recent";
        }

        private bool IsButtonBanned(IWebElement button) {
            return _banned.Contains(button.Text);
        }
        
        private void CustomMoviesExtract(string type, string miscCategoryName, string tabId) {
            var allMovies = Driver.
                FindElementsByXPath($"//div[@id='{tabId}']//div[@class='_gx6 _agv']");

            for (var i = 0; i < allMovies.Count; i++) {
                
                var name = allMovies[i].FindElement(By.CssSelector("a"))?.Text;
                var subText = "";

                if (type == "Watched" || type == "Read") {
                    subText = allMovies[i].FindElement(By.ClassName("timestampContent"))?.Text;
                }                
                Helpers.Print($"( {allMovies.Count - i} ) Adding {name}", ConsoleColor.Yellow);
                User.Misc.AddData(miscCategoryName, new[]{type, name, subText});
            }
        }

        protected void GeneralScrap(string title, string id) {

            Helpers.Print($"Scrapping -> {title}", ConsoleColor.DarkRed);

            //Driver.Url = url;

            Helpers.Wait(1000, 200);

            var butts = Helpers.GetTabButtons(id);

            for (var i = 0; i < butts.Count; i++) {
                var webElement = butts[i];
                ClickNavBarButton(i, SeleniumProvider.Driver.FindElementById(id));

                Helpers.ScrollToBottom();

                CustomMoviesExtract(webElement.Text, title, id);
                //Extract(webElement.Text, "Music", id);
                Helpers.Print("------------------------------ ------------------ -------------");

                Helpers.ScrollToElement(id);
            }
        }
    }
}