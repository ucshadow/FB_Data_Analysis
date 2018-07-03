using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using static FB_Data_Analysis.Classes.Helpers;

namespace FB_Data_Analysis.Classes.FBCategories {
    public class ProfileAbout {
        private readonly ChromeDriver _driver;
        private User _user;
        private string _tabClass = "_4qm1";

        public ProfileAbout(User user) {
            _user = user;
            _driver = SeleniumProvider.Driver;
                
            Scrap();
        }

        private void Scrap() {
            
            if (!TabIsPresent("About")) return;

            ScrollToBottom();

            //_driver.ExecuteScript("scrollBy(0,-document.body.scrollHeight)");

            ScrollToElement("pagelet_timeline_medley_about");
            
            var tabList = _driver.FindElementsByClassName("_47_-");


            GetSpecificOverviewTab(1, tabList, "Work and Education");
            GetSpecificOverviewTab(2, tabList, "Places Lived");
            GetSpecificOverviewTab(3, tabList, "Contact And Basic Info");
            GetSpecificOverviewTab(4, tabList, "Family And Relationships");
            GetSpecificOverviewTab(5, tabList, "Details About");
            GetSpecificOverviewTab(6, tabList, "Life Events");
            //_user.PrintUser();
            //GetPlacesLived();
        }

        /// <summary>
        /// Gets al specific overview tabs
        /// ex: Conatct and Basic Info Tab may have
        /// Contact Information, Websites and Social Links, Basic Information
        /// className = _4qm1
        /// </summary>
        /// <param name="index"></param>
        /// <param name="tabs"></param>
        /// <param name="term"></param>
        private void GetSpecificOverviewTab(int index, IReadOnlyList<IWebElement> tabs, string term) {

            Console.WriteLine();
            Console.WriteLine($"-- {term} -----------------------------------------------------------------------");
            Console.WriteLine();
            
            // all fields in the specific tab.
            
            tabs[index].Click();
            Wait(2000, 200);
            
            // _4qm1
            var allFields = _driver.FindElementsByClassName(_tabClass);
            
            foreach (var webElement in allFields) {
                //var title = webElement.FindElement(By.XPath("//a[@role = 'heading']")).Text.ToLower();
                
                GetTitle(webElement, term, index);

                //var noSpace = Regex.Replace(title, @"\s+", "");
                //var capitalised = noSpace.First().ToString().ToUpper() + noSpace.Substring(1);
            }
        }

        /// <summary>
        /// Gets the title of the profileEditExperiences tab
        /// className = _h72
        /// </summary>
        private void GetTitle(ISearchContext element, string term, int index) {
            var title = element.FindElement(By.ClassName("_h72")).Text.ToLower();
                
            Print($"Found title: {title}");
            
            var pascalCategoryName = Regex.Replace(term, @"\s+", "");
            var info = CultureInfo.CurrentCulture.TextInfo;
            var pascalAttributeName = info.ToTitleCase(title.ToLower()).Replace(" ", string.Empty);
            Print($"Pascaled {title} into {pascalAttributeName}");
            
            GetExperiencesTabs(element, index, pascalCategoryName, pascalAttributeName);
                
            //GetExperiencesTabs(element);
            //GetTabData(pascalCategoryName, pascalAttributeName, element);
        }

        /// <summary>
        /// Gets all the tabs in the profileEditExperiences tab with a specific title
        /// tab className = fbProfileEditExperiences
        /// </summary>
        private void GetExperiencesTabs(ISearchContext element, int index, string pascalCategoryName, 
            string pascalAttributeName) {
            var tabs = element.FindElements(By.ClassName("fbProfileEditExperiences"));
            switch (index) {
                case 1:
                case 2:
                    foreach (var webElement in tabs) {
                        GetExperienceTabRows1And2(webElement, pascalCategoryName, pascalAttributeName);
                    }

                    break;
                case 3:
                    // contact and basic info
                    foreach (var webElement in tabs) {
                        GetContactAndBasicInfo(webElement, pascalCategoryName, pascalAttributeName);
                    }

                    break;
                case 4:
                case 5:
                    foreach (var webElement in tabs) {
                        GetFamilyAndDetails(webElement, pascalCategoryName, pascalAttributeName);
                    }

                    break;
                
                case 6:
                    foreach (var webElement in tabs) {
                        GetLifeEvents(webElement, pascalCategoryName, "AboutPerson");
                    }

                    break;
                default:
                    // ToDo: parse the other category tabs
                    break;
            }
        }
        
        private void GetLifeEvents(ISearchContext element, 
            string pascalCategoryName, string pascalAttributeName) {
            
            Print($"Getting row data for {pascalCategoryName} known as attribute {pascalAttributeName}", ConsoleColor.DarkBlue);

            var allRows = element.FindElements(By.ClassName("_ikh"));

            ScrapLifeEvents(allRows, pascalAttributeName);
        }
        
        private void ScrapLifeEvents(IReadOnlyList<IWebElement> allRows, string pascalAttributeName) {
            for (var index = 0; index < allRows.Count; index++) {
                var year = allRows[index].FindElement(By.ClassName("_50f8"));
                var webElement = allRows[index];
                var value = ExtractTextValueFromElement(webElement, true);

                Print($"Found key {pascalAttributeName} and value {value}", ConsoleColor.DarkCyan);
                _user.About.AddData(pascalAttributeName, $"{index}_{pascalAttributeName}", $"{year.Text}: {value}");
            }
        }
        
        
        private void GetFamilyAndDetails(ISearchContext element, 
            string pascalCategoryName, string pascalAttributeName) {
            
            Print($"Getting row data for {pascalCategoryName} known as attribute {pascalAttributeName}", ConsoleColor.DarkBlue);

            var allRows = element.FindElements(By.ClassName("_4bl9"));

            ScrapFamAndDet(allRows, pascalAttributeName);
        }

        private void ScrapFamAndDet(IReadOnlyList<IWebElement> allRows, string pascalAttributeName) {
            for (var index = 0; index < allRows.Count; index++) {
                var webElement = allRows[index];
                var value = ExtractTextValueFromElement(webElement, true);

                Print($"Found key {pascalAttributeName} and value {value}", ConsoleColor.DarkCyan);
                _user.About.AddData(pascalAttributeName, $"{index}_{pascalAttributeName}", value);
            }
        }

        /// <summary>
        /// Gets all the rows from a specific experience tab
        /// Works with Contacts and Basic Info tab
        /// className = _ikh
        /// </summary>
        /// <param name="element"></param>
        /// <param name="pascalCategoryName"></param>
        /// <param name="pascalAttributeName"></param>
        private void GetContactAndBasicInfo(ISearchContext element, 
            string pascalCategoryName, string pascalAttributeName) {
            
            Print($"Getting row data for {pascalCategoryName} known as attribute {pascalAttributeName}", ConsoleColor.DarkBlue);

            var allRows = element.FindElements(By.ClassName("_ikh"));
            
            foreach (var webElement in allRows) {
                var key = webElement.FindElement(By.ClassName("_5kx5"));
                var value = webElement.FindElement(By.ClassName("_pt5"));

                var keyText = key.Text;
                var valueText = ExtractTextValueFromElement(value, false); // dont scrap divs, only spans and hrefs
                
                Print($"Found key {keyText} and value {valueText}", ConsoleColor.DarkCyan);

                if (keyText.Length > 0) {
                    _user.About.AddData(pascalAttributeName, keyText, valueText);
                }
            }
        }

        /// <summary>
        /// Gets all the rows from a specific experience tab
        /// className = _42ef
        /// only works for 'work and education' and 'places she lived'
        /// 1 and 2 are the indexes of the tabs this works for
        /// </summary>
        private void GetExperienceTabRows1And2(ISearchContext element, 
            string pascalCategoryName, string pascalAttributeName) {
            
            Print($"Getting row data for {pascalCategoryName} known as attribute {pascalAttributeName}", ConsoleColor.DarkBlue);
            
            var fields = element.FindElements(By.ClassName("_42ef"));

            for (var index = 0; index < fields.Count; index++) {
                var webElement = fields[index];
                var elementUrlTitle = GetElementUrl(webElement);
                var elementSubtitle = GetElementSubtitle(webElement);

                Print($"Found title {elementUrlTitle?.Text} and subtitle {elementSubtitle?.Text}", ConsoleColor.Cyan);

                if (elementSubtitle?.Text.Length > 0) {
                    _user.About.AddData(pascalAttributeName, $"{index}_" + elementUrlTitle?.Text, elementSubtitle?.Text);
                }
                else {
                    _user.About.AddData(pascalAttributeName, $"{index}_" + pascalAttributeName, elementUrlTitle?.Text);
                }
            }
            
        }

        private IWebElement GetElementUrl(ISearchContext webElement) {

            if (ElementIsPresent(webElement, By.CssSelector("a"))) {
                return webElement.FindElement(By.CssSelector("a"));
            }
            Print($"warning !!! {webElement} has no url", ConsoleColor.DarkRed);
            return null;
        }
        
        private IWebElement GetElementSubtitle(ISearchContext webElement) {

            if (ElementIsPresent(webElement, By.ClassName("fsm"))) {
                return webElement.FindElement(By.ClassName("fsm"));
            }
            Print($"{webElement} has no subtitle.", ConsoleColor.DarkRed);
            return null;
        }
        
        
        
        
        
//        private void GetSpecialTabData(string pascalTabName, string pascalAttributeName, ISearchContext container) {
//            
//            Print("Getting special tab data...", ConsoleColor.Blue);
//            
//            var fields = container.FindElements(By.ClassName("fbProfileEditExperiences"));
//            foreach (var webElement in fields) {
//                
//                var localFields = webElement.FindElements(By.CssSelector("a"));
//                
//                for (var index = 0; index < localFields.Count; index++) {
//                    var webElementLocal = localFields[index];
//                    var elementUrlLocal = webElementLocal;
//                    
//                    Print($"Found special tab data {elementUrlLocal?.Text}", ConsoleColor.Cyan);
//                    _user.About.AddData(pascalAttributeName, pascalAttributeName + index, elementUrlLocal?.Text);
//
//                    //_user.WorkAndEducation.Skills.Add(elementUrl?.Text);
//                }
//            }
//        }
//        
//        private void GetTabData(string pascalTabName, string pascalAttributeName, ISearchContext element) {
//            Print($"Getting tab data for {pascalTabName} known as attribute {pascalAttributeName}", ConsoleColor.DarkBlue);
//            
//            var fields = element.FindElements(By.ClassName("_2tdc"));
//            if (fields.Count == 0) {
//                GetSpecialTabData(pascalTabName, pascalAttributeName, element);
//            }
//
//            for (var index = 0; index < fields.Count; index++) {
//                var webElement = fields[index];
//                var elementUrlTitle = GetElementUrl(webElement);
//                var elementSubtitle = GetElementSubtitle(webElement);
//
//                Print($"Found title {elementUrlTitle?.Text} and subtitle {elementSubtitle?.Text}", ConsoleColor.Cyan);
//
//                if (elementSubtitle?.Text.Length > 0) {
//                    _user.About.AddData(pascalAttributeName, elementUrlTitle?.Text, elementSubtitle?.Text);
//                }
//                else {
//                    if (elementSubtitle?.FindElements(By.CssSelector("a")).Count > 0) {
//                        // the subtitle is an actual link
//                        var txt = elementSubtitle?.FindElement(By.CssSelector("a")).Text;
//                        _user.About.AddData(pascalAttributeName, elementUrlTitle?.Text, txt);
//                    }
//                    else {
//                        _user.About.AddData(pascalAttributeName, pascalAttributeName + index, elementUrlTitle?.Text);
//                    }
//                    
//                }
//            }
//        }
    }
}