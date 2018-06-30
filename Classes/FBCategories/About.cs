using System;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.Classes.FBCategories {
    public class About {
        private readonly ChromeDriver _driver;
        private User _user;
        private string _tabClass = "_4qm1";

        public About(User user) {
            _user = user;
            _driver = SeleniumProvider.Driver;
            
            // to be removed later
            
            Scrap();
        }

        private void Scrap() {
            GetWorkAndEducation();
        }

        private void GetPlacesLived() {
            
            // male
            if (ElementIsPresent(_driver, By.XPath("//*[@title='Places He's Lived']"))) {
                _driver.FindElementByXPath("//*[@title='Places He's Lived']").Click();
            }
            else { // female
                _driver.FindElementByXPath("//*[@title='Places She's Lived']").Click();
            }
            var allFields = _driver.FindElementsByClassName(_tabClass);
        }

        private void GetWorkAndEducation() {
            _driver.FindElementByXPath("//*[@title='Work and Education']").Click();

            // all fields in the work and education tab.
            var allFields = _driver.FindElementsByClassName(_tabClass);
            foreach (var webElement in allFields) {
                //var title = webElement.FindElement(By.XPath("//a[@role = 'heading']")).Text.ToLower();
                
                var title = webElement.FindElement(By.ClassName("_h72")).Text.ToLower();
                
                Print($"Found title: {title}");
                
                switch (title) {
                    case "work":
                        GetWork(webElement);
                        break;
                    case "professional skills":
                        GetSkills(webElement);
                        break;
                    case "education":
                        GetEducation(webElement);
                        break;
                    default:
                        NewTitleFound(title);
                        break;
                }
            }
        }

        private void GetWork(ISearchContext container) {
            
            Print("Getting work...", ConsoleColor.DarkBlue);
            
            var fields = container.FindElements(By.ClassName("_2tdc"));
            foreach (var webElement in fields) {
                var elementUrl = GetElementUrl(webElement);
                var elementTitle = GetElementTitle(webElement);
                
                Print($"Found job {elementUrl?.Text} working as {elementTitle?.Text}", ConsoleColor.Cyan);
                
                _user.WorkAndEducation.Work.Add(elementUrl?.Text, elementTitle?.Text);
            }
        }

        private void GetEducation(IWebElement container) {
            Print("Getting education...", ConsoleColor.DarkBlue);
            
            var fields = container.FindElements(By.ClassName("_2tdc"));
            foreach (var webElement in fields) {
                var elementUrl = GetElementUrl(webElement);
                var elementTitle = GetElementTitle(webElement);
                
                Print($"Found education {elementUrl?.Text} in {elementTitle?.Text}", ConsoleColor.Cyan);
                
                _user.WorkAndEducation.Education.Add(elementUrl?.Text, elementTitle?.Text);
            }
        }

        private void GetSkills(IWebElement container) {
            
            Print("Getting skills...", ConsoleColor.DarkBlue);
            
            var fields = container.FindElement(By.ClassName("fbProfileEditExperiences")).
                FindElements(By.CssSelector("a"));
            foreach (var webElement in fields) {
                var elementUrl = webElement;
                
                Print($"Found skill {elementUrl?.Text}", ConsoleColor.Cyan);
                
                _user.WorkAndEducation.Skills.Add(elementUrl?.Text);
            }
        }

        private void NewTitleFound(string title) {
            Print($"new title found: {title} for {_driver.Url}");
        }

        private IWebElement GetElementUrl(ISearchContext webElement) {

            if (ElementIsPresent(webElement, By.ClassName("_2lzr"))) {
                return webElement.FindElement(By.ClassName("_2lzr"))
                    .FindElement(By.CssSelector("a"));
            }
            Print($"{webElement} has no url", ConsoleColor.DarkRed);
            return null;
        }
        
        private IWebElement GetElementTitle(ISearchContext webElement) {

            if (ElementIsPresent(webElement, By.ClassName("fsm"))) {
                return webElement.FindElement(By.ClassName("fsm"));
            }
            Print($"{webElement} has no title.", ConsoleColor.DarkRed);
            return null;
        }
    }
}