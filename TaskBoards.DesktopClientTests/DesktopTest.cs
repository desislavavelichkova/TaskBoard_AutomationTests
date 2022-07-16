using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading;

namespace ContactBook.DesktopClientTests
{
    public class DesktopTest
    {
        private const string AppiumUrl = "http://127.0.0.1:4723/wd/hub";
        //private const string url = "https://taskboard.nakov.repl.co/api";
        private const string url = "http://localhost:8080/api";
        private const string appLocation = @"C:\Exam_19.06.2022\TaskBoard.DesktopClient-v1.0\TaskBoard.DesktopClient.exe";

        private WindowsDriver<WindowsElement> driver;
        private AppiumOptions options;

        [SetUp]
        public void StartApp()
        {
            options = new AppiumOptions() { PlatformName = "Windows" };
            options.AddAdditionalCapability("app", appLocation);

            driver = new WindowsDriver<WindowsElement>(new Uri(AppiumUrl), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }


       
        [Test]
        public void Test_SearchTask_WithGivenName()
        {            
            var textBoxApiUrl = driver.FindElementByAccessibilityId("textBoxApiUrl");
            textBoxApiUrl.Clear(); 
            textBoxApiUrl.SendKeys(url);

            var buttonConnect = driver.FindElementByAccessibilityId("buttonConnect");
            buttonConnect.Click();

            string windowsName = driver.WindowHandles[0];
            driver.SwitchTo().Window(windowsName);

            var textBoxSearch = driver.FindElementByAccessibilityId("textBoxSearchText");
            textBoxSearch.SendKeys("Project skeleton");
                        
            driver.FindElementByAccessibilityId("buttonSearch").Click();
             
            var listItems = driver.FindElementsByAccessibilityId("listViewTasks");
            
            Assert.That(listItems, Is.Not.Null);
        }
        [Test]
        public void Test_AddNewTask()
        {
            string title = "Title" + DateTime.Now;
            var textBoxApiUrl = driver.FindElementByAccessibilityId("textBoxApiUrl");
            textBoxApiUrl.Clear();
            textBoxApiUrl.SendKeys(url);

            var buttonConnect = driver.FindElementByAccessibilityId("buttonConnect");
            buttonConnect.Click();

            string windowsName = driver.WindowHandles[0];
            driver.SwitchTo().Window(windowsName);

            var buttonAdd = driver.FindElementByAccessibilityId("buttonAdd");
            buttonAdd.Click();

            var textBoxAddTitle = driver.FindElementByAccessibilityId("textBoxTitle");
            textBoxAddTitle.SendKeys(title);


            var textBoxDescription = driver.FindElementByAccessibilityId("textBoxDescription");
            
            textBoxDescription.SendKeys("New Description");

            var buttonCreate = driver.FindElementByAccessibilityId("buttonCreate");
            buttonCreate.Click();

            var textBoxSearch = driver.FindElementByAccessibilityId("textBoxSearchText");
            textBoxSearch.SendKeys(title);

            driver.FindElementByAccessibilityId("buttonSearch").Click();

            var listItems = driver.FindElementsByAccessibilityId("listViewTasks");

            Assert.That(listItems, Is.Not.Null);

        }

        [TearDown]
        public void CloseApp()
        {
            driver.Quit();
        }

    }
}