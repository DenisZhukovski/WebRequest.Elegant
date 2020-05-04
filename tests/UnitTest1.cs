using System.Net.Cache;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WebRequest.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {
            var request = new Elegant.WebRequest(
                "https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js",
                new System.Net.Http.HttpClient(
                    new WebRequestHandler()
                    {
                        CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.CacheIfAvailable)
                    }
                )
            );
        }
    }
}