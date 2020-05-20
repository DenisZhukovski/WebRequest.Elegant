using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebRequest.Elegant
{
    public sealed class WebRequest : IWebRequest, IDisposable
    {
        private readonly IJsonObject _postBody;
        private readonly Dictionary<string, string> _queryParams;
        private readonly HttpClient _httpClient;
        private static readonly Dictionary<HttpClient, int> _objectUsageCount = new Dictionary<HttpClient, int>();

        public WebRequest(
            string uriString,
            HttpClient httpClient
        ) : this(new Uri(uriString), httpClient)
        {
        }

        public WebRequest(
            Uri uri,
            HttpClient httpClient
        ) : this(uri, new HttpAuthenticationHeaderToken(), httpClient)
        {
        }

        public WebRequest(
            Uri uri,
            IToken token,
            HttpClient httpClient
        ) : this(
                token,
                uri,
                HttpMethod.Get,
                new EmptyJsonObject(),
                new Dictionary<string, string>(),
                httpClient
            )
        {
        }

        public WebRequest(
            IToken token,
            Uri uri,
            HttpMethod method,
            IJsonObject postBody,
            Dictionary<string, string> queryParams,
            HttpClient httpClient)
        {
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
            Token = token ?? throw new ArgumentNullException(nameof(token));
            HttpMethod = method ?? throw new ArgumentNullException(nameof(method));
            _postBody = postBody ?? throw new ArgumentNullException(nameof(postBody));
            _queryParams = queryParams ?? throw new ArgumentNullException(nameof(queryParams));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            if (!_objectUsageCount.ContainsKey(_httpClient))
            {
                _objectUsageCount.Add(_httpClient, 0);
            }
            _objectUsageCount[_httpClient]++;
        }

        public IToken Token { get; }

        public Uri Uri { get; }

        public HttpMethod HttpMethod { get; }

        public async Task<HttpResponseMessage> GetAsync()
        {
            var requestMessage = RequestMessage(HttpMethod);
            try
            {
                return await _httpClient.SendAsync(requestMessage);
            }
            finally
            {
                requestMessage.Dispose();
            }
        }

        public IWebRequest Path(Uri uri)
        {
            return new WebRequest(
                Token,
                uri,
                HttpMethod,
                _postBody,
                _queryParams,
                _httpClient
            );
        }

        public IWebRequest Method(HttpMethod method)
        {
            return new WebRequest(
                Token, 
                Uri, 
                method, 
                _postBody, 
                _queryParams, 
                _httpClient
            );
        }

        public IWebRequest QueryParams(Dictionary<string, string> parameters)
        {
            return new WebRequest(
                Token, 
                Uri,
                HttpMethod, 
                _postBody, 
                parameters, 
                _httpClient
            );
        }

        public IWebRequest Body(IJsonObject postBody)
        {
            return new WebRequest(Token, Uri, HttpMethod, postBody, _queryParams, _httpClient);
        }

        public override string ToString()
        {
            return $"Uri: {new QueryParamsAsString(_queryParams).With(Uri)}\n" +
                   $"Token: {Token}\n" +
                   $"PostBody: {_postBody.ToJson()}";
        }

        private HttpRequestMessage RequestMessage(HttpMethod method)
        {
            var request = new HttpRequestMessage(
                method,
                new QueryParamsAsString(_queryParams).With(Uri)
            );
            var postBodyString = _postBody.ToJson();
            if (!string.IsNullOrEmpty(postBodyString))
            {
                request.Content = new StringContent(postBodyString, Encoding.UTF8, "application/json");
            }
            Token.InjectTo(request);
            return request;
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _objectUsageCount[_httpClient]--;
                    if (_objectUsageCount[_httpClient] == 0)
                    {
                        _httpClient.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        #endregion
    }
}
