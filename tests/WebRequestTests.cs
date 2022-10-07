using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using WebRequest.Elegant;
using WebRequest.Elegant.Fakes;
using WebRequest.Elegant.Extensions;

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
                    fakeRequestHandler
                )
                .PostAsync(new Dictionary<string, IJsonObject>
                {
                    { "TestArgument1", new SimpleString("Hello World") },
                    { "TestArgument2", new TestJsonObject() },
                });

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
                fakeRequestHandler
            ).UploadFileAsync("./TestData/TestUploadFile.txt");

            Assert.AreEqual(
                new FileContent("./TestData/TestUploadFile.txt").ToString().Replace("\r", string.Empty),
                fakeRequestHandler.RequestsAsString[0].Replace("\r", string.Empty).Split("\n")[4].Remove(0, 1)
            );
        }

        [Test]
        public async Task SingleJsonObjectInPostMethod()
        {
            var fakeRequestHandler = new FkHttpMessageHandler("Test message");
            await new Elegant.WebRequest(
                    "http://reqres.in/api/users",
                    fakeRequestHandler
                )
                .PostAsync(new TestJsonObject());

            Assert.AreEqual(
                @"Request: http://reqres.in/api/users
                PostBody: {
                  ""FirstName"": ""Test First Name"",
                  ""LastName"": ""Test Last Name""
                }".NoNewLines(),
                fakeRequestHandler.RequestsAsString[0].NoNewLines()
            );
        }

        [Test]
        public async Task StringBodyInPostMethod()
        {
            var fakeRequestHandler = new FkHttpMessageHandler("Test message");
            await new Elegant.WebRequest(
                "http://reqres.in/api/users",
                fakeRequestHandler
            )
            .PostAsync(
                @"{
                  ""FirstName"": ""Test First Name"",
                  ""LastName"": ""Test Last Name""
                }");

            Assert.AreEqual(
                @"Request: http://reqres.in/api/users
                PostBody: {
                  ""FirstName"": ""Test First Name"",
                  ""LastName"": ""Test Last Name""
                }".NoNewLines(),
                fakeRequestHandler.RequestsAsString[0].NoNewLines()
            );
        }
        
        [Test]
        public async Task PostHttpContent()
        {
            var fakeRequestHandler = new FkHttpMessageHandler("Test message");
            await new Elegant.WebRequest(
                    "http://reqres.in/api/users",
                    fakeRequestHandler
                )
                .PostAsync(
                    new StringContent(
                        @"Hello world"
                    )
                );

            Assert.AreEqual(
                @"Request: http://reqres.in/api/users
                PostBody: Hello world".NoNewLines(),
                fakeRequestHandler.RequestsAsString[0].NoNewLines()
            );
        }
        
        [Test]
        public async Task GetAsync()
        {
            Assert.AreEqual(
                @"Test message",
                await (await new Elegant.WebRequest(
                        "http://reqres.in/api/users",
                        new FkHttpMessageHandler("Test message")
                     )
                     .GetAsync("/hello"))
                     .Content.ReadAsStringAsync()
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
                }".NoNewLines(),
                new Elegant.WebRequest("http://reqres.in/api/users")
                    .WithMethod(HttpMethod.Post)
                    .WithBody(new Dictionary<string, IJsonObject>
                    {
                        { "TestArgument1", new SimpleString("Hello World") },
                        { "TestArgument2", new TestJsonObject() },
                    }).ToString().NoNewLines()
            );
        }

        [Test]
        public void WebRequestWithTokenToString()
        {
            Assert.AreEqual(
                @"Uri: http://reqres.in:80/api/users
                Token: erYTo
                Body: test string".NoNewLines(),
                new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users"),
                    new HttpAuthenticationHeaderToken("erYTo"),
                    HttpMethod.Post,
                    new JsonBodyContent(new SimpleString("test string"))
                ).ToString().NoNewLines()
            );
        }

        [Test]
        public void WebRequestWithDoubleSlashRelativePath()
        {
            Assert.AreEqual(
                @"Uri: http://reqres.in:80/api/users
                Token: 
                Body: ".NoNewLines(),
                new Elegant.WebRequest("http://reqres.in/")
                    .WithRelativePath("/api/users")
                    .ToString().NoNewLines()
            );
        }

        [Test]
        public async Task WebRequestReceivesThroughtTheProxyHandler()
        {
            var proxy = new ProxyHttpMessageHandler();
            Assert.IsNotEmpty(
                await new Elegant.WebRequest(
                    new Uri("https://www.google.com/"),
                    proxy
                ).ReadAsStringAsync()
            );

            Assert.IsNotEmpty(proxy.RequestsContent);
            Assert.IsNotEmpty(proxy.ResponsesContent);
        }

        [Test]
        public void EqualTheSameUri()
        {
            Assert.AreEqual(
                new Elegant.WebRequest(new Uri("http://reqres.in/api/users")),
                new Elegant.WebRequest(new Uri("http://reqres.in/api/users"))
            );
        }

        [Test]
        public void NoEqualWhenDifferentUri()
        {
            Assert.AreNotEqual(
                new Elegant.WebRequest(new Uri("http://reqres.in/api/users")),
                new Elegant.WebRequest(new Uri("http://reqres.in/api/users2"))
            );
        }

        [Test]
        public void EqualToTheSameUri()
        {
            Assert.AreEqual(
                new Elegant.WebRequest(new Uri("http://reqres.in/api/users")),
                new Uri("http://reqres.in/api/users")
            );
        }

        [Test]
        public void NotEqualToDiffUri()
        {
            Assert.AreNotEqual(
                new Elegant.WebRequest(new Uri("http://reqres.in/api/users")),
                new Uri("http://reqres.in/api/users2")
            );
        }

        [Test]
        public void EqualToTheSameUriString()
        {
            Assert.AreEqual(
                new Elegant.WebRequest(new Uri("http://reqres.in/api/users")),
                "http://reqres.in/api/users"
            );
        }

        [Test]
        public void NotEqualToDiffUriString()
        {
            Assert.AreNotEqual(
                new Elegant.WebRequest(new Uri("http://reqres.in/api/users")),
                "http://reqres.in/api/users2"
            );
        }

        [Test]
        public void EqualToTheSameHttpMethod()
        {
            Assert.AreEqual(
                new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users")
                ).WithMethod(HttpMethod.Post),
                HttpMethod.Post
            );
        }

        [Test]
        public void EqualToDiffHttpMethod()
        {
            Assert.AreNotEqual(
                new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users")
                ).WithMethod(HttpMethod.Post),
                HttpMethod.Get
            );
        }

        [Test]
        public void IsPostRequest()
        {
            Assert.True(
                new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users")
                ).WithMethod(HttpMethod.Post)
                 .IsPost()
            );
        }

        [Test]
        public void EqualToSameToken()
        {
            Assert.AreEqual(
                new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users"),
                    new HttpAuthenticationHeaderToken()
                ),
                new HttpAuthenticationHeaderToken()
            );
        }

        [Test]
        public void NotEqualToDiffToken()
        {
            Assert.AreNotEqual(
                new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users"),
                    new HttpAuthenticationHeaderToken()
                ),
                new HttpAuthenticationHeaderToken("token1")
            );
        }

        [Test]
        public void EqualToTheSameBodyContent()
        {
            Assert.AreEqual(
                new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users"),
                    new HttpAuthenticationHeaderToken()
                ),
                new JsonBodyContent(new EmptyJsonObject())
            );
        }

        [Test]
        public void NotEqualToDiffBodyContent()
        {
            Assert.AreNotEqual(
                new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users"),
                    new HttpAuthenticationHeaderToken()
                ),
                new JsonBodyContent(new SimpleString("Hello"))
            );
        }

        [Test]
        public void EqualToTheSameQueryParams()
        {
            Assert.AreEqual(
                new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users")
                ).WithQueryParams(new Dictionary<string, string>()
                {
                    { "test", "test" }
                }),
                new Dictionary<string, string>
                {
                    { "test", "test" }
                }
            );
        }

        [Test]
        public void ThrowsInvalidOperationException_FromReadAsStringAsync_WhenServerException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users"),
                    new ExceptionHttpMessageHandler(new HttpRequestException("Not Host is found"))
                ).ReadAsStringAsync()
            );
        }

        [Test]
        public void ThrowsInvalidOperationException_FromEnsureSuccessAsync_WhenServerException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users"),
                    new ExceptionHttpMessageHandler(new HttpRequestException("Not Host is found"))
                ).EnsureSuccessAsync()
            );
        }

        [Test]
        public void ThrowsInvalidOperationException_FromUploadFile_WhenServerException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(() =>
               new Elegant.WebRequest(
                   "http://reqres.in/api/users",
                    new ExceptionHttpMessageHandler(new HttpRequestException("Not Host is found"))
               ).UploadFileAsync("./TestData/TestUploadFile.txt")
            );
        }

        [Test]
        public void EqualToDiffQueryParams()
        {
            Assert.AreNotEqual(
                new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users")
                ).WithQueryParams(new Dictionary<string, string>()
                {
                    { "test", "test" }
                }),
                new Dictionary<string, string>
                {
                    { "test", "test1" }
                }
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