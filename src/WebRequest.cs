using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebRequest.Elegant
{
    public sealed class WebRequest : IWebRequest
    {
        private readonly HttpMethod _method;
        private readonly IJsonObject _postBody;
        private readonly Dictionary<string, string> _queryParams;
        private readonly HttpClient _httpClient;

        public WebRequest(
            string uriString,
            HttpClient httpClient)
            : this(new Uri(uriString), httpClient)
        {
        }

        public WebRequest(
            Uri uri,
            HttpClient httpClient)
            : this(
                new HttpAuthenticationHeaderToken(),
                uri,
                HttpMethod.Get,
                new EmptyJsonObject(),
                new Dictionary<string, string>(),
                httpClient)
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
            _method = method ?? throw new ArgumentNullException(nameof(method));
            _postBody = postBody ?? throw new ArgumentNullException(nameof(postBody));
            _queryParams = queryParams ?? throw new ArgumentNullException(nameof(queryParams));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public IToken Token { get; }

        public Uri Uri { get; }

        public async Task<HttpResponseMessage> GetAsync()
        {
            var requestMessage = RequestMessage(_method);
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
                _method,
                _postBody,
                _queryParams,
                _httpClient);
        }

        public IWebRequest Method(HttpMethod method)
        {
            return new WebRequest(Token, Uri, method, _postBody, _queryParams, _httpClient);
        }

        public IWebRequest QueryParams(Dictionary<string, string> parameters)
        {
            return new WebRequest(Token, Uri, _method, _postBody, parameters, _httpClient);
        }

        public IWebRequest Body(IJsonObject postBody)
        {
            return new WebRequest(Token, Uri, _method, postBody, _queryParams, _httpClient);
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
                new QueryParamsAsString(_queryParams).With(Uri));

            var postBodyString = _postBody.ToJson();
            if (!string.IsNullOrEmpty(postBodyString))
            {
                request.Content = new StringContent(postBodyString);
            }

            Token.InjectTo(request);
            return request;
        }
    }
}
