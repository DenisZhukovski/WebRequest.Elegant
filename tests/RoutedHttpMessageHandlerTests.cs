using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using WebRequest.Elegant;
using WebRequest.Elegant.Fakes;

namespace WebRequest.Tests
{
    public class RoutedHttpMessageHandlerTests
    {
        [Test]
        public async Task RouteHandlerWithStaticResponse()
        {
            Assert.AreEqual(
                "Hello world",
                await new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users"),
                    new RoutedHttpMessageHandler(
                        new Route(new Dictionary<string, string>
                        {
                            { "http://reqres.in/api/users","Hello world" }
                        })
                    )
                ).ReadAsStringAsync()
            );
        }
        
        [Test]
        public async Task RouteHandlerWithDelegate()
        {
            var count = 23;
            Assert.AreEqual(
                "Hello world 23",
                await new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users"),
                    new RoutedHttpMessageHandler(
                        new Route(new Dictionary<string, Func<string>>
                        {
                            { "http://reqres.in/api/users", () => "Hello world " + count }
                        })
                    )
                ).ReadAsStringAsync()
            );
        }

        [Test]
        public async Task RouteHandlerAndDataFile()
        {
            Assert.AreEqual(
                "Hello world testing test.",
                await new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users"),
                    new RoutedHttpMessageHandler(
                        new Route().With(
                            "http://reqres.in/api/users",
                            "./TestData/TestUploadFile.txt"
                        )
                    )
                ).ReadAsStringAsync()
            );
        }

        [Test]
        public async Task ByFullMatch()
        {
            Assert.AreEqual(
                "Hello world testing test.",
                await new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users"),
                    new RoutedHttpMessageHandler(
                        new Route(new Dictionary<string, string>
                        {
                            { "http://reqres.in/api/users", "Hello world testing test." }
                        })
                    )
                ).ReadAsStringAsync()
            );
        }

        [Test]
        public async Task ByUriMatchButIgnoreQueryParameters()
        {
            Assert.AreEqual(
                "Hello world testing test.",
                await new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users"),
                    new RoutedHttpMessageHandler(
                        new Route(new Dictionary<string, string>
                        {
                            { "http://reqres.in/api/users", "Hello world testing test." }
                        })
                    )
                ).WithQueryParams(
                    new Dictionary<string, string>
                    {
                        { "params1", "1" },
                        { "param2", "2" }
                    }
                )
                .ReadAsStringAsync()
            );
        }

        [Test]
        public async Task ByFullMatchPriority()
        {
            Assert.AreEqual(
                "Hello full match.",
                await new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users"),
                    new RoutedHttpMessageHandler(
                        new Route(new Dictionary<string, string>
                        {
                            { "http://reqres.in/api/users", "Hello world testing test." },
                            { "http://reqres.in/api/users?params1=1&param2=2", "Hello full match." }
                        })
                    )
                ).WithQueryParams(
                    new Dictionary<string, string>
                    {
                        { "params1", "1" },
                        { "param2", "2" }
                    }
                )
                .ReadAsStringAsync()
            );
        }
    }
}
