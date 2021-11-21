using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebRequest.Elegant
{
    /// <summary>
    /// The web request token.
    /// </summary>
    public class HttpAuthenticationHeaderToken : IToken, IEqualityComparer<HttpAuthenticationHeaderToken>
    {
        private string _token;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpAuthenticationHeaderToken"/> class.
        /// </summary>
        public HttpAuthenticationHeaderToken()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpAuthenticationHeaderToken"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public HttpAuthenticationHeaderToken(string token)
        {
            _token = token;
        }

        /// <summary>
        /// Prolongates the token.
        /// </summary>
        /// <param name="httpResponseMessage">The http response message that contain new token.</param>
        public Task ProlongateFromAsync(HttpResponseMessage httpResponseMessage)
        {
            // TODO: Extract token from HttpResponseMessage
            _token = "";
            return Task.CompletedTask;
        }

        /// <summary>
        /// Inject token into http request.
        /// </summary>
        /// <param name="request">The http request message.</param>
        public Task InjectToAsync(HttpRequestMessage request)
        {
            if (!string.IsNullOrEmpty(_token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(_token);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Returns token entity as a string.
        /// </summary>
        /// <returns>Token as a string.</returns>
        public override string ToString()
        {
            return _token;
        }

        /// <summary>
        /// Check is two tokens are equal.
        /// </summary>
        /// <param name="x">The target token source.</param>
        /// <param name="y">The token to compare with.</param>
        /// <returns>True if tokens are equal.</returns>
        public bool Equals(HttpAuthenticationHeaderToken x, HttpAuthenticationHeaderToken y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x._token == y._token;
        }

        public override bool Equals(object obj)
        {
            return obj is HttpAuthenticationHeaderToken token
                && Equals(this, token);
        }

        /// <summary>
        /// Calculates token's hash code.
        /// </summary>
        /// <param name="obj">The token.</param>
        /// <returns>Token's hash code.</returns>
        public int GetHashCode(HttpAuthenticationHeaderToken obj)
        {
            return obj._token.GetHashCode();
        }

        public override int GetHashCode()
        {
            return GetHashCode(this);
        }
    }
}
