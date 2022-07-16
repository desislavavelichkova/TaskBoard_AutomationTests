using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;

namespace TaskBoard.WebDriverTests
{
    public class UITests
    {
        private const string url = "https://taskboard.nakov.repl.co";
        //private const string url = "http://localhost:8080";
        private WebDriver driver;

        [SetUp]
        public void OpenBrowser()
        {
            this.driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }


        [Test]
        public void Test_ListTasks_CheckFirstTaskFromDone()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Task Board")).Click();                     
            
            var taskTitle = driver.FindElements(By.CssSelector("tr.title > td"));
            var result = "";

            for (int i = 0; i < taskTitle.Count; i++)
            {
                if (taskTitle[i].Text == "Project skeleton")
                {
                    result = "Project skeleton";
                }
            }                
            Assert.That(result, Is.EqualTo("Project skeleton"));
            
        }

        [Test]
        public void Test_SearchTasks_CheckFirstResults()
        {            
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Search")).Click();
           
            var searchField = driver.FindElement(By.Id("keyword"));
            searchField.SendKeys("home");

            driver.FindElement(By.Id("search")).Click();

            var title = driver.FindElement(By.CssSelector(".title > td")).Text;            

            Assert.That(title, Is.EqualTo("Home page"));
           
        }

        [Test]
        public void Test_SearchTask_EmptyResult()
        {            
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Search")).Click();
                        
            var searchField = driver.FindElement(By.Id("keyword"));
            searchField.SendKeys("missing12345");
            driver.FindElement(By.Id("search")).Click();

            var resultLabel = driver.FindElement(By.Id("searchResult")).Text;

            Assert.That(resultLabel, Is.EqualTo("No tasks found."));
        }

        [Test]
        public void Test_CreateTask_WithInvalidData()
        {            
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Create")).Click();            
           
            var buttonCreate = driver.FindElement(By.Id("create"));
            buttonCreate.Click();

            var errorMessage = driver.FindElement(By.CssSelector(".err")).Text;
            Assert.That(errorMessage, Is.EqualTo("Error: Title cannot be empty!"));
        }

        [Test]
        public void Test_CreateTask_WithValidData()
        {            
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Task Board")).Click();

            var oldTasks = driver.FindElements(By.CssSelector("tr.title > td"));
            var oldTaskCount = oldTasks.Count();

            driver.FindElement(By.LinkText("Create")).Click();

            var title = "TaskTitle" + DateTime.Now.Ticks;
            var taskDescription = "New Text";
                       
            driver.FindElement(By.Id("title")).SendKeys(title);
            driver.FindElement(By.Id("description")).SendKeys(taskDescription);

            driver.FindElement(By.Id("create")).Click();

            var taskTitle = driver.FindElements(By.CssSelector("tr.title > td"));
            var deskription = driver.FindElements(By.CssSelector("tr.description > td"));

            var currTaskTitle = "";
            var currTaskdescription = "";

            for (int i = 0; i < taskTitle.Count; i++)
            {
                if (taskTitle[i].Text == title && deskription[i].Text == taskDescription )
                {
                    currTaskTitle = title;
                    currTaskdescription = taskDescription;
                }
            }
            Assert.That(currTaskTitle, Is.EqualTo(title));
            Assert.That(currTaskdescription, Is.EqualTo(taskDescription));
            Assert.That(taskTitle.Count(), Is.EqualTo(oldTaskCount + 1));
           
        }

        [TearDown]
        public void CloseBrowser()
        {
            this.driver.Quit();
        }
    }
}