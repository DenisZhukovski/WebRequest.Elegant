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

        //[Test]
        //public void WebRequestCachingLogic()
        //{
        //    var request = new Elegant.WebRequest(
        //        "https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js",
        //        new System.Net.Http.HttpClient(
        //            new SocketsHttpHandler()
        //            {
        //                CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.CacheIfAvailable)
        //            }
        //        )
        //    );
        //}

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