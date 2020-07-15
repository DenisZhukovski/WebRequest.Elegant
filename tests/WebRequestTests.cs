using NUnit.Framework;

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
    }
}