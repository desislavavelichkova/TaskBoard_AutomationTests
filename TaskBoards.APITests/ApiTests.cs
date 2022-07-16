using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace TaskBoard.APITests
{
    public class ApiTests
    {

        private const string url = "https://taskboard.nakov.repl.co/api";
        //private const string url = "http://localhost:8080/api";
        
        private RestClient client;
        private RestRequest request;

        [SetUp]
        public void Setup()
        {
            this.client = new RestClient();
        }

        [Test]
        public void Test_GetAllBoards_CheckFirstTask_Done()
        {
            this.request = new RestRequest(url + "/tasks/1");
            var response = this.client.Execute(request, Method.Get);

            var task = JsonSerializer.Deserialize<Task>(response.Content);            

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(task.id, Is.EqualTo(1));
            Assert.That(task.Title, Is.EqualTo("Project skeleton"));
        }

        [Test]
        public void Test_SearchTask_CheckFirstResult()
        {
            this.request = new RestRequest(url + "/tasks/search/{keyword}");
            request.AddUrlSegment("keyword", "home");
            
            var response = this.client.Execute(request, Method.Get);

            var tasks = JsonSerializer.Deserialize<List<Task>>(response.Content);
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(tasks.Count, Is.GreaterThan(0));

            Assert.That(tasks[0].Title, Is.EqualTo("Home page"));
        
        }

        [Test]
        public void Test_SearchTasks_EmptyResults()
        {
            this.request = new RestRequest(url + "/tasks/search/{keyword}");
            request.AddUrlSegment("keyword", "missing1234");

            var response = this.client.Execute(request, Method.Get);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content, Is.EqualTo("[]"));

            
        }

        [Test]
        public void Test_CreateTask_InvalidData()
        {

            this.request = new RestRequest(url + "/tasks");

            var body = new Task
            {
                Description = "description",
               
            };
            request.AddJsonBody(body);

            var response = this.client.Execute(request, Method.Post);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            var err = "{\"errMsg\":\"Title cannot be empty!\"}";
            Assert.That(response.Content, Is.EqualTo(err));
        }

        [Test]
        public void Test_CreateContact_ValidData()
        {
            this.request = new RestRequest(url + "/tasks");

            var body = new
            {
                title = "newTitle" + DateTime.Now.Ticks,
                description = "descriprion",   
                
            };

            request.AddJsonBody(body);

            var response = this.client.Execute(request, Method.Post);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var allTasks = this.client.Execute(request, Method.Get);

            var tasks = JsonSerializer.Deserialize<List<Task>>(allTasks.Content);
            var lastTask = tasks.Last();

            Assert.That(lastTask.Title, Is.EqualTo(body.title));
            Assert.That(lastTask.Description, Is.EqualTo(body.description));

            }
        }
}