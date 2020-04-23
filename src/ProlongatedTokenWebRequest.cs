using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebRequest.Elegant
{
    /// <inheritdoc />
    public class ProlongatedTokenWebRequest : IWebRequest
    {
        private readonly IWebRequest _origin;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProlongatedTokenWebRequest"/> class.
        /// </summary>
        /// <param name="origin">The origin web request.</param>
        public ProlongatedTokenWebRequest(IWebRequest origin)
        {
            _origin = origin;
        }

        /// <inheritdoc />
        public IToken Token => _origin.Token;

        /// <inheritdoc />
        public Uri Uri => _origin.Uri;

        /// <inheritdoc />
        public async Task<HttpResponseMessage> GetAsync()
        {
            var response = await _origin.GetAsync().ConfigureAwait(false);
            _origin.Token.ProlongateFrom(response);
            return response;
        }

        /// <inheritdoc />
        public IWebRequest Path(Uri uri)
        {
            return new ProlongatedTokenWebRequest(_origin.Path(uri));
        }

        /// <inheritdoc />
        public IWebRequest Method(HttpMethod method)
        {
            return new ProlongatedTokenWebRequest(_origin.Method(method));
        }

        /// <inheritdoc />
        public IWebRequest QueryParams(Dictionary<string, string> parameters)
        {
            return new ProlongatedTokenWebRequest(_origin.QueryParams(parameters));
        }

        /// <inheritdoc />
        public IWebRequest Body(IJsonObject postBody)
        {
            return new ProlongatedTokenWebRequest(_origin.Body(postBody));
        }
    }
}
