using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using WebRequest.Elegant;
using WebRequest.Elegant.Fakes;
using WebRequest.Elegant.Extensions;
using NUnit.Framework.Internal;

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

            Assert.That(
                fakeRequestHandler.RequestsAsString[0].Replace("\r", string.Empty),
                Is.EqualTo(
                    new FileContent("./TestData/MultiArgumentsPostBody.txt").ToString().Replace("\r", string.Empty)
                )
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

            Assert.That(
                fakeRequestHandler.RequestsAsString[0].Replace("\r", string.Empty).Split("\n")[4].Remove(0, 1),
                Is.EqualTo(new FileContent("./TestData/TestUploadFile.txt").ToString().Replace("\r", string.Empty))
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

            Assert.That(
                fakeRequestHandler.RequestsAsString[0].NoNewLines(),
                Is.EqualTo(
                    @"Request: http://reqres.in/api/users
                    PostBody: {
                      ""FirstName"": ""Test First Name"",
                      ""LastName"": ""Test Last Name""
                    }".NoNewLines()
                )
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

            Assert.That(
                fakeRequestHandler.RequestsAsString[0].NoNewLines(),
                Is.EqualTo(
                    @"Request: http://reqres.in/api/users
                    PostBody: {
                      ""FirstName"": ""Test First Name"",
                      ""LastName"": ""Test Last Name""
                    }".NoNewLines()
                )
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

            Assert.That(
                fakeRequestHandler.RequestsAsString[0].NoNewLines(),
                Is.EqualTo(
                    @"Request: http://reqres.in/api/users
                    PostBody: Hello world".NoNewLines()
                )
            );
        }
        
        [Test]
        public async Task GetAsync()
        {
            Assert.That(
                await (await new Elegant.WebRequest(
                        "http://reqres.in/api/users",
                        new FkHttpMessageHandler("Test message")
                    )
                    .GetAsync("/hello")
                ).Content.ReadAsStringAsync(),
                Is.EqualTo("Test message")
            );
        }

        [Test]
        public void WebRequestToString()
        {
            Assert.That(
                new Elegant.WebRequest("http://reqres.in/api/users")
                    .WithMethod(HttpMethod.Post)
                    .WithBody(new Dictionary<string, IJsonObject>
                    {
                        { "TestArgument1", new SimpleString("Hello World") },
                        { "TestArgument2", new TestJsonObject() },
                    }).ToString().NoNewLines(),
                Is.EqualTo(
                    @"Uri: http://reqres.in:80/api/users
                    Token: 
                    Body: TestArgument1: Hello World
                    TestArgument2: {
                      ""FirstName"": ""Test First Name"",
                      ""LastName"": ""Test Last Name""
                    }".NoNewLines()
                )
            );
        }

        [Test]
        public void WebRequestWithTokenToString()
        {
            Assert.That(
                new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users"),
                    new HttpAuthenticationHeaderToken("erYTo"),
                    HttpMethod.Post,
                    new JsonBodyContent(new SimpleString("test string"))
                ).ToString().NoNewLines(),
                Is.EqualTo(
                    @"Uri: http://reqres.in:80/api/users
                    Token: erYTo
                    Body: test string".NoNewLines()
                )
            );
        }

        [Test]
        public void WebRequestWithDoubleSlashRelativePath()
        {
            Assert.That(
                new Elegant.WebRequest("http://reqres.in/")
                    .WithRelativePath("/api/users")
                    .ToString().NoNewLines(),
                Is.EqualTo(
                    @"Uri: http://reqres.in:80/api/users
                    Token: 
                    Body: ".NoNewLines()
                )
            );
        }

        [Test]
        public async Task WebRequestReceivesThroughtTheProxyHandler()
        {
            var proxy = new ProxyHttpMessageHandler();
            Assert.That(
                await new Elegant.WebRequest(
                    new Uri("https://www.google.com/"),
                    proxy
                ).ReadAsStringAsync(),
                Is.Not.Empty
            );

            Assert.That(proxy.RequestsContent, Is.Not.Empty);
            Assert.That(proxy.ResponsesContent, Is.Not.Empty);
        }

        [Test]
        public void EqualTheSameUri()
        {
            Assert.That(
                new Elegant.WebRequest(new Uri("http://reqres.in/api/users")),
                Is.EqualTo(new Elegant.WebRequest(new Uri("http://reqres.in/api/users")))
            );
        }

        [Test]
        public void NoEqualWhenDifferentUri()
        {
            Assert.That(
                new Elegant.WebRequest(new Uri("http://reqres.in/api/users")),
                Is.Not.EqualTo(new Elegant.WebRequest(new Uri("http://reqres.in/api/users2")))
            );
        }

        [Test]
        public void EqualToTheSameUri()
        {
            Assert.That(
                new Elegant.WebRequest(new Uri("http://reqres.in/api/users")),
                Is.EqualTo(new Uri("http://reqres.in/api/users"))
            );
        }

        [Test]
        public void NotEqualToDiffUri()
        {
            Assert.That(
                new Elegant.WebRequest(new Uri("http://reqres.in/api/users")),
                Is.Not.EqualTo(new Uri("http://reqres.in/api/users2"))
            );
        }

        [Test]
        public void EqualToTheSameUriString()
        {
            Assert.That(
                new Elegant.WebRequest(new Uri("http://reqres.in/api/users")),
                Is.EqualTo("http://reqres.in/api/users")
            );
        }

        [Test]
        public void NotEqualToDiffUriString()
        {
            Assert.That(
                new Elegant.WebRequest(new Uri("http://reqres.in/api/users")),
                Is.Not.EqualTo("http://reqres.in/api/users2")
            );
        }

        [Test]
        public void EqualToTheSameHttpMethod()
        {
            Assert.That(
                new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users")
                ).WithMethod(HttpMethod.Post),
                Is.EqualTo(HttpMethod.Post)
            );
        }

        [Test]
        public void EqualToDiffHttpMethod()
        {
            Assert.That(
                new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users")
                ).WithMethod(HttpMethod.Post),
                Is.Not.EqualTo(HttpMethod.Get)
            );
        }

        [Test]
        public void IsPostRequest()
        {
            Assert.That(
                new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users")
                ).WithMethod(HttpMethod.Post)
                 .IsPost(),
                Is.True
            );
        }

        [Test]
        public void EqualToSameToken()
        {
            Assert.That(
                new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users"),
                    new HttpAuthenticationHeaderToken()
                ),
                Is.EqualTo(new HttpAuthenticationHeaderToken())
            );
        }

        [Test]
        public void NotEqualToDiffToken()
        {
            Assert.That(
                new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users"),
                    new HttpAuthenticationHeaderToken()
                ),
                Is.Not.EqualTo(new HttpAuthenticationHeaderToken("token1"))
            );
        }

        [Test]
        public void EqualToTheSameBodyContent()
        {
            Assert.That(
                new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users"),
                    new HttpAuthenticationHeaderToken()
                ),
                Is.EqualTo(new JsonBodyContent(new EmptyJsonObject()))
            );
        }

        [Test]
        public void NotEqualToDiffBodyContent()
        {
            Assert.That(
                new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users"),
                    new HttpAuthenticationHeaderToken()
                ),
                Is.Not.EqualTo(new JsonBodyContent(new SimpleString("Hello")))
            );
        }

        [Test]
        public void EqualToTheSameQueryParams()
        {
            Assert.That(
                new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users")
                ).WithQueryParams(new Dictionary<string, string>
                {
                    { "test", "test" }
                }),
                Is.EqualTo(new Dictionary<string, string>
                {
                    { "test", "test" }
                })
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
            Assert.That(
                new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users")
                ).WithQueryParams(new Dictionary<string, string>
                {
                    { "test", "test" }
                }),
                Is.Not.EqualTo(new Dictionary<string, string>
                {
                    { "test", "test1" }
                })
            );
        }

        [Test]
        public void QueryParamWithoutValue()
        {
            Assert.That(
                new Elegant.WebRequest(new Uri("http://reqres.in/api/users/"))
                    .WithQueryParams("test")
                    .ToString()
                    .NoNewLines(),
                Is.EqualTo(
                    @"Uri: http://reqres.in:80/api/users/?test
                    Token: 
                    Body: ".NoNewLines()
                )
            );
        }
        
        [Test]
        public void CancelLoading()
        {
            Assert.ThrowsAsync<TaskCanceledException>(() =>
            {
                var cts = new CancellationTokenSource();
                cts.Cancel();
                return new Elegant.WebRequest(
                    "http://reqres.in/api/users"
                ).EnsureSuccessAsync(cts.Token);
            });
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