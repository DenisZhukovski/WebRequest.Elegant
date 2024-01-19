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
            Assert.That(
                httpRequestMessage.Headers.Authorization.Scheme,
                Is.EqualTo(token)
            );
        }

        [Test]
        public async Task ProlongateFrom()
        {
            var token = new HttpAuthenticationHeaderToken("123");
            var httpRequestMessage = new System.Net.Http.HttpRequestMessage();

            await token.ProlongateFromAsync(new System.Net.Http.HttpResponseMessage()).ConfigureAwait(false);
            await token.InjectToAsync(httpRequestMessage).ConfigureAwait(false);

            Assert.That(
                httpRequestMessage.Headers.Authorization,
                Is.Null
            );
        }

        [Test]
        public void TheSameTokensIsEqual()
        {
            Assert.That(
                new HttpAuthenticationHeaderToken("eyJ0e"),
                Is.EqualTo(new HttpAuthenticationHeaderToken("eyJ0e"))
            );
        }

        [Test]
        public void DifferentTokensIsNotEqual()
        {
            Assert.That(
                new HttpAuthenticationHeaderToken("123"),
                Is.Not.EqualTo(new HttpAuthenticationHeaderToken("987"))
            );
        }

        [Test]
        public void NullIsNotEqualToken()
        {
            Assert.That(
                new HttpAuthenticationHeaderToken("eyJ0e"),
                Is.Not.EqualTo(null)
            );
        }

        [Test]
        public void TheSameTokensHasSameHashcodes()
        {
            Assert.That(
                new HttpAuthenticationHeaderToken("eyJ0e").GetHashCode(),
                Is.EqualTo(new HttpAuthenticationHeaderToken("eyJ0e").GetHashCode())
            );
        }
    }
}
