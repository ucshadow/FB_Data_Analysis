using System;
using OpenQA.Selenium.Chrome;

namespace FB_Data_Analysis.Classes {
    public static class SeleniumProvider {
        
        private static ChromeDriver _driver;
        public static ChromeDriver Driver => _driver ?? GetDriver();

        private static ChromeDriver GetDriver() {
            var options = new ChromeOptions();
            options.AddArgument("--disable-notifications");
            _driver = new ChromeDriver(System.IO.Directory.GetCurrentDirectory(), options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
            return _driver;
        }
    }
}