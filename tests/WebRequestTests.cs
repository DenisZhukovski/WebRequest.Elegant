using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using WebRequest.Elegant;
using WebRequest.Elegant.Fakes;

namespace WebRequest.Tests
{
    public class WebRequestTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task MultipleJsonObjectsInPostMethod()
        {
            var fakeRequestHandler = new FkHttpMessageHandler("Test message");
            await new Elegant.WebRequest(
                "http://reqres.in/api/users",
                new HttpClient(fakeRequestHandler)
            )
            .WithMethod(HttpMethod.Post)
            .WithBody(new Dictionary<string, IJsonObject>
            {
                { "TestArgument1", new SimpleString("Hello World") },
                { "TestArgument2", new TestJsonObject() },
            }).GetResponseAsync();

            Assert.AreEqual(
                new FileContent("./TestData/MultiArgumentsPostBody.txt").ToString().Replace("\r", string.Empty),
                fakeRequestHandler.RequestsAsString[0].Replace("\r", string.Empty)
            );
        }

        [Test]
        public async Task UploadFile()
        {
            var fakeRequestHandler = new FkHttpMessageHandler("Test message");
            await new Elegant.WebRequest(
                "http://reqres.in/api/users",
                new HttpClient(fakeRequestHandler)
            ).UploadFileAsync("./TestData/TestUploadFile.txt");

            Assert.AreEqual(
                new FileContent("./TestData/TestUploadFile.txt").ToString().Replace("\r", string.Empty),
                fakeRequestHandler.RequestsAsString[0].Replace("\r", string.Empty).Split("PostBody: ")[1]
            );
        }

        [Test]
        public async Task SingleJsonObjectInPostMethod()
        {
            var fakeRequestHandler = new FkHttpMessageHandler("Test message");
            await new Elegant.WebRequest(
                "http://reqres.in/api/users",
                new HttpClient(fakeRequestHandler)
            )
            .WithMethod(HttpMethod.Post)
            .WithBody(new TestJsonObject()).GetResponseAsync();

            Assert.AreEqual(
                @"Request: http://reqres.in/api/users
PostBody: {
  ""FirstName"": ""Test First Name"",
  ""LastName"": ""Test Last Name""
}".Replace("\r", string.Empty),
                fakeRequestHandler.RequestsAsString[0].Replace("\r", string.Empty)
            );
        }

        [Test]
        public void WebRequestToString()
        {
            Assert.AreEqual(
                @"Uri: http://reqres.in:80/api/users
Token: 
Body: TestArgument1: Hello World
TestArgument2: {
  ""FirstName"": ""Test First Name"",
  ""LastName"": ""Test Last Name""
}".Replace("\r", string.Empty),
                new Elegant.WebRequest(
                    "http://reqres.in/api/users",
                    new HttpClient()
                )
                .WithMethod(HttpMethod.Post)
                .WithBody(new Dictionary<string, IJsonObject>
                {
                    { "TestArgument1", new SimpleString("Hello World") },
                    { "TestArgument2", new TestJsonObject() },
                }).ToString().Replace("\r", string.Empty)
            );
        }

        public class TestJsonObject : IJsonObject
        {
            public string ToJson()
            {
                return new JObject(
                    new JProperty("FirstName", "Test First Name"),
                    new JProperty("LastName", "Test Last Name")
                ).ToString();
            }
        }
    }
}