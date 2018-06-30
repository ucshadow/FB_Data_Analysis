using System;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.Classes.FBCategories {
    public class About {
        private readonly ChromeDriver _driver;

        public About() {
            _driver = SeleniumProvider.Driver;
            
            
            Scrap();
        }

        private void Scrap() {
            GetOverview();
            GetWorkAndEducation();
        }

        /// <summary>
        /// Scraps the Overview tab, focused by default
        /// </summary>
        private void GetOverview() {
            
        }

        private void GetWorkAndEducation() {
            var button = _driver.FindElementByXPath("//*[@title='Work and Education']");
            button.Click();
            //Helpers.Wait(_driver, 10000, 5000);

            // all fields in the work and education tab.
            var allFields = _driver.FindElementsByClassName("_4qm1");
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
            Wait(50000, 10000);
            _driver.Close();
        }

        private void GetWork(ISearchContext container) {
            
            Print("Getting work...", ConsoleColor.DarkBlue);
            
            var fields = container.FindElements(By.ClassName("_2tdc"));
            foreach (var webElement in fields) {
                var jobUrl = GetJobUrl(webElement);
                var jobTitle = GetJobTitle(webElement);
                
                Print($"Found job {jobUrl?.Text} working as {jobTitle?.Text}", ConsoleColor.Cyan);
            }
        }

        private void GetEducation(IWebElement container) {
            
        }

        private void GetSkills(IWebElement container) {
            
        }

        private void NewTitleFound(string title) {
            Print($"new title found: {title} for {_driver.Url}");
        }

        private IWebElement GetJobUrl(ISearchContext webElement) {

            if (ElementIsPresent(webElement, By.ClassName("_2lzr"))) {
                return webElement.FindElement(By.ClassName("_2lzr"))
                    .FindElement(By.CssSelector("a"));
            }
            Print($"{webElement} has no job url");
            return null;
        }
        
        private IWebElement GetJobTitle(ISearchContext webElement) {

            if (ElementIsPresent(webElement, By.ClassName("fsm"))) {
                return webElement.FindElement(By.ClassName("fsm"));
            }
            Print($"{webElement} has no job title.");
            return null;
        }
    }
}