using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using FB_Data_Analysis.Classes;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FB_Data_Analysis.DesktopVersion.FBCategories {
    public class ProfileAbout : IPageTab {
        private readonly ChromeDriver _driver;
        private User _user;
        private string _tabClass = "_4qm1";

        public ProfileAbout(User user) {
            _user = user;
            _driver = SeleniumProvider.Driver;
        }

        public void Scrap(string title) {
            
            if (!Helpers.TabIsPresent("About")) return;
            
            Helpers.Print($"Scrapping -> {title}", ConsoleColor.DarkRed);

            Helpers.ScrollToBottom();

            //_driver.ExecuteScript("scrollBy(0,-document.body.scrollHeight)");

            Helpers.ScrollToElement("pagelet_timeline_medley_about");
            
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
            Helpers.Wait(2000, 200);
            
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
                
            Helpers.Print($"Found title: {title}");
            
            var pascalCategoryName = Regex.Replace(term, @"\s+", "");
            var info = CultureInfo.CurrentCulture.TextInfo;
            var pascalAttributeName = info.ToTitleCase(title.ToLower()).Replace(" ", string.Empty);
            Helpers.Print($"Pascaled {title} into {pascalAttributeName}");
            
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
                        GetLifeEvents(webElement, pascalCategoryName, pascalAttributeName);
                    }

                    break;
                default:
                    // ToDo: parse the other category tabs
                    break;
            }
        }
        
        private void GetLifeEvents(ISearchContext element, 
            string pascalCategoryName, string pascalAttributeName) {
            
            Helpers.Print($"Getting row data for {pascalCategoryName} known as attribute {pascalAttributeName}", ConsoleColor.DarkBlue);

            var allRows = element.FindElements(By.ClassName("_ikh"));

            ScrapLifeEvents(allRows, pascalAttributeName, pascalCategoryName);
        }
        
        private void ScrapLifeEvents(IEnumerable<IWebElement> allRows, string pascalAttributeName, string pascalCategoryName) {
            foreach (var t in allRows) {
                var year = t.FindElement(By.ClassName("_50f8"));
                
                var values = t.FindElements(By.ClassName("_c24"));
                
                foreach (var value in values) {
                    //toDo can add Url to specific event here if needed, but not needed right now.
                    Helpers.Print($"Found key {pascalAttributeName} and value {value}", ConsoleColor.DarkCyan);
                    _user.About.AddData(pascalAttributeName, new[] {year.Text, value.Text});
                }
                
            }
        }
        
        
        private void GetFamilyAndDetails(ISearchContext element, 
            string pascalCategoryName, string pascalAttributeName) {
            
            Helpers.Print($"Getting row data for {pascalCategoryName} known as attribute {pascalAttributeName}", ConsoleColor.DarkBlue);

            var allRows = element.FindElements(By.ClassName("_4bl9"));
            if (allRows.Count == 0) {
                allRows = element.FindElements(By.ClassName("_2pi4"));
            }
            if (allRows.Count == 0) {
                allRows = element.FindElements(By.ClassName("_3twh"));
            }

            if (pascalAttributeName.Contains("About")) pascalAttributeName = "AboutPerson";
            
            ScrapFamAndDet(allRows, pascalAttributeName);
        }

        private void ScrapFamAndDet(IEnumerable<IWebElement> allRows, string pascalAttributeName) {
            foreach (var webElement in allRows) {
                var value = webElement.Text;
                var href = Helpers.ElementIsPresent(webElement, By.CssSelector("a"), true)?.GetAttribute("href");

                Helpers.Print($"Found key {pascalAttributeName} and value {value}", ConsoleColor.DarkCyan);
                _user.About.AddData(pascalAttributeName, new[] {pascalAttributeName, value, href});
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
            
            Helpers.Print($"Getting row data for {pascalCategoryName} known as attribute {pascalAttributeName}", ConsoleColor.DarkBlue);

            var allRows = element.FindElements(By.ClassName("_ikh"));
            
            foreach (var webElement in allRows) {
                // toDo: this MAY throw no such element error, although it should not
                var key = webElement.FindElement(By.ClassName("_5kx5"));
                var value = webElement.FindElement(By.ClassName("_pt5"));

                var keyText = key.Text;
                var valueText = value.Text;
//                var valueText = Helpers.ExtractTextValueFromElement(value, false); // dont scrap divs, only spans and hrefs
                
                Helpers.Print($"Found key {keyText} and value {valueText}", ConsoleColor.DarkCyan);

                if (keyText.Length > 0) {
                    _user.About.AddData(pascalAttributeName, new[]{keyText, valueText});
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
            
            // toDo: Maybe fix the education tab having the same class as the class mates
            
            Helpers.Print($"Getting row data for {pascalCategoryName} known as attribute {pascalAttributeName}", ConsoleColor.DarkBlue);
            
            var fields = element.FindElements(By.ClassName("_42ef"));

            foreach (var webElement in fields) {
                var elementTitle = GetElementTitleFromA(webElement);
                var href = elementTitle?.GetAttribute("href");
                var elementSubtitle = GetElementSubtitle(webElement);

                Helpers.Print($"Found title {elementTitle?.Text} and subtitle {elementSubtitle?.Text}", ConsoleColor.Cyan);
                _user.About.AddData(pascalAttributeName, new[] {elementTitle?.Text, href, elementSubtitle?.Text});
            }
            
        }

        private static IWebElement GetElementTitleFromA(ISearchContext webElement) {
            return Helpers.ElementIsPresent(webElement, By.CssSelector("a"), true);
        }
        
        private static IWebElement GetElementSubtitle(ISearchContext webElement) {
            return Helpers.ElementIsPresent(webElement, By.ClassName("fsm"), true);
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