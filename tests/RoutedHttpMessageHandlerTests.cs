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
            Assert.That(
                await new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users"),
                    new RoutedHttpMessageHandler(
                        new Route(new Dictionary<string, string>
                        {
                            { "http://reqres.in/api/users","Hello world" }
                        })
                    )
                ).ReadAsStringAsync(),
                Is.EqualTo("Hello world")
            );
        }
        
        [Test]
        public async Task RouteHandlerWithDelegate()
        {
            var count = 23;
            Assert.That(
                await new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users"),
                    new RoutedHttpMessageHandler(
                        new Route(new Dictionary<string, Func<string>>
                        {
                            { "http://reqres.in/api/users", () => "Hello world " + count }
                        })
                    )
                ).ReadAsStringAsync(),
                Is.EqualTo("Hello world 23")
            );
        }

        [Test]
        public async Task RouteHandlerAndDataFile()
        {
            Assert.That(
                await new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users"),
                    new RoutedHttpMessageHandler(
                        new Route().With(
                            "http://reqres.in/api/users",
                            "./TestData/TestUploadFile.txt"
                        )
                    )
                ).ReadAsStringAsync(),
                Is.EqualTo("Hello world testing test.")
            );
        }

        [Test]
        public async Task ByFullMatch()
        {
            Assert.That(
                await new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users"),
                    new Route(
                        "http://reqres.in/api/users",
                        "Hello world testing test."
                    )
                ).ReadAsStringAsync(),
                Is.EqualTo("Hello world testing test.")
            );
        }
        
        [Test]
        public async Task DynamicRoute()
        {
            int counter = 0;
            var webRequest = new Elegant.WebRequest(
                new Uri("http://reqres.in/api/users"),
                new Route(
                    new RouteResponse(
                        "http://reqres.in/api/users",
                        () => "Hello world testing test." + counter++
                    )
                )
            );
            await webRequest.ReadAsStringAsync();
            Assert.That(
                await webRequest.ReadAsStringAsync(),
                Is.EqualTo("Hello world testing test.1")
            );
        }
        
        [Test]
        public async Task DynamicRouteForRequest()
        {
            int counter = 0;
            var webRequest = new Elegant.WebRequest(
                new Uri("http://reqres.in/api/users"),
                new Route(
                    new RouteResponse(
                        "http://reqres.in/api/users",
                        (request) => "Hello world testing test." + counter++
                    )
                )
            );
            await webRequest.ReadAsStringAsync();
            Assert.That(
                await webRequest.ReadAsStringAsync(),
                Is.EqualTo("Hello world testing test.1")
            );
        }

        [Test]
        public async Task ByUriMatchButIgnoreQueryParameters()
        {
            Assert.That(
                await new Elegant.WebRequest(
                        new Uri("http://reqres.in/api/users"),
                        new Route("http://reqres.in/api/users", "Hello world testing test.")
                    ).WithQueryParams(
                        new Dictionary<string, string>
                        {
                            { "params1", "1" },
                            { "param2", "2" }
                        }
                    )
                    .ReadAsStringAsync(),
                Is.EqualTo("Hello world testing test.")
            );
        }

        [Test]
        public async Task ByFullMatchPriority()
        {
            Assert.That(
                await new Elegant.WebRequest(
                    new Uri("http://reqres.in/api/users"),
                    new RoutedHttpMessageHandler(
                        new Route(new Dictionary<string, string>
                        {
                            { "http://reqres.in/api/users", "Hello world testing test." },
                            { "http://reqres.in/api/users?params1=1&param2=2", "Hello full match." }
                        })
                    )
                ).WithQueryParams(new Dictionary<string, string>
                {
                    { "params1", "1" },
                    { "param2", "2" }
                })
                .ReadAsStringAsync(),
                Is.EqualTo("Hello full match.")
            );
        }
    }
}
