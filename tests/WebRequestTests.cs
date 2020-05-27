using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
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
        public void WebRequestCachingLogic()
        {
            //var request = new Elegant.WebRequest(
            //    "https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js",
            //    new System.Net.Http.HttpClient(
            //        new WebRequestHandler()
            //        {
            //            CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.CacheIfAvailable)
            //        }
            //    )
            //);
        }

        [Test]
        public async Task WebRequestDisposeForHttpClientCallsOnlyOnce()
        {
            var fakeResponse = new FkHttpMessageHandler("hello");
            using (var request = new Elegant.WebRequest(
                                    "https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js",
                                    new HttpClient(fakeResponse)
                                 )
                  )
            {
                using (var secondRequest = (Elegant.WebRequest)request.WithMethod(HttpMethod.Post))
                {
                    var secondResponse = await secondRequest.GetResponseAsync();
                }

                var firstResponse = request.GetResponseAsync();
            }

            Assert.AreEqual(2, fakeResponse.RequestsAsString.Count);
        }
    }
}