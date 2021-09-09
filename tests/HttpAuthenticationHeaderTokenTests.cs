using System.Threading.Tasks;
using NUnit.Framework;
using WebRequest.Elegant;

namespace WebRequest.Tests
{
    public class HttpAuthenticationHeaderTokenTests
    {
        [Test]
        public async Task InjectTo()
        {
            var token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9";
            var httpRequestMessage = new System.Net.Http.HttpRequestMessage();
            await new HttpAuthenticationHeaderToken(token).InjectToAsync(httpRequestMessage).ConfigureAwait(false);
            Assert.AreEqual(
                token,
                httpRequestMessage.Headers.Authorization.Scheme
            );
        }

        [Test]
        public async Task TheSameTokensIsEqual()
        {
            Assert.AreEqual(
                new HttpAuthenticationHeaderToken("eyJ0e"),
                new HttpAuthenticationHeaderToken("eyJ0e")
            );
        }

        [Test]
        public async Task IsEqual()
        {
            Assert.AreEqual(
                "eyJ0e",
                "eyJ0e"
            );
        }

        [Test]
        public async Task DifferentTokensIsNotEqual()
        {
            Assert.AreNotEqual(
                new HttpAuthenticationHeaderToken("123"),
                new HttpAuthenticationHeaderToken("987")
            );
        }

        [Test]
        public async Task NullIsNotEqualToken()
        {
            Assert.AreNotEqual(
                new HttpAuthenticationHeaderToken("eyJ0e"),
                null
            );
        }

        [Test]
        public async Task TheSameTokensHasSameHashcodes()
        {
            Assert.AreEqual(
                new HttpAuthenticationHeaderToken("eyJ0e").GetHashCode(),
                new HttpAuthenticationHeaderToken("eyJ0e").GetHashCode()
            );
        }
    }
}
