using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

namespace SecretSanta.Web.Tests
{
    [TestClass]
    public class UserTests
    {
        [NotNull]
        public TestContext? TestContext { get; set; }
        [NotNull]
        private IWebDriver? Driver { get; set; }
        string AppUrl { get; } = "https://localhost:44394/Users";

        [TestInitialize]
        public void TestInitialize()
        {
            Driver = new ChromeDriver();
            Driver.Manage().Timeouts().ImplicitWait = new System.TimeSpan(0, 0, 10);
        }

        [TestMethod]
        [TestCategory("Chrome")]
        public void ValidateLinks_GiftsListPage()
        {
            Driver.Navigate().GoToUrl(new Uri(AppUrl));
            IReadOnlyCollection<IWebElement> links = Driver.FindElements(By.TagName("a"));

            foreach (var link in links)
            {
                if (link.Displayed) //there is a null/empty link to create the hamburger dropdown mobile menu
                {
                    string url = link.GetAttribute("href");
                    Assert.IsTrue(Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute));
                }
            }
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            Driver.Quit();
        }

    }
}
