using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebRequest.Elegant.Core;

namespace WebRequest.Elegant
{
    public sealed class WebRequest : IWebRequest
    {
        private readonly Dictionary<string, string> _queryParams;
        private readonly HttpClient _httpClient;

        public WebRequest(
            string uriString
        ) : this(uriString, new HttpClient())
        {
        }

        public WebRequest(
            string uriString,
            HttpClient httpClient
        ) : this(new Uri(uriString), httpClient)
        {
        }

        public WebRequest(
            string uriString,
            HttpMessageHandler messageHandler
        ) : this(new Uri(uriString), messageHandler)
        {
        }

        public WebRequest(
            Uri uri,
            HttpMessageHandler messageHandler
        ) : this(uri, new HttpClient(messageHandler))
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
                new JsonBodyContent(new EmptyJsonObject()),
                new Dictionary<string, string>(),
                httpClient
            )
        {
        }

        public WebRequest(
            Uri uri,
            IToken token,
            HttpMethod method,
            IBodyContent body
        ) : this(
                token,
                uri,
                method,
                body,
                new Dictionary<string, string>(),
                new HttpClient()
            )
        {
        }

        public WebRequest(
            IToken token,
            Uri uri,
            HttpMethod method,
            IBodyContent body,
            Dictionary<string, string> queryParams,
            HttpClient httpClient)
        {
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
            Token = token ?? throw new ArgumentNullException(nameof(token));
            HttpMethod = method ?? throw new ArgumentNullException(nameof(method));
            Body = body ?? throw new ArgumentNullException(nameof(body));
            _queryParams = queryParams ?? throw new ArgumentNullException(nameof(queryParams));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public IToken Token { get; }

        public Uri Uri { get; }

        public HttpMethod HttpMethod { get; }

        public IBodyContent Body { get; }

        public async Task<HttpResponseMessage> GetResponseAsync()
        {
            var requestMessage = await RequestMessageAsync(HttpMethod).ConfigureAwait(false);
            try
            {
                return await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);
            }
            finally
            {
                requestMessage.Dispose();
            }
        }

        public IWebRequest WithPath(Uri uri)
        {
            return new WebRequest(
                Token,
                uri,
                HttpMethod,
                Body,
                _queryParams,
                _httpClient
            );
        }

        public IWebRequest WithMethod(HttpMethod method)
        {
            return new WebRequest(
                Token,
                Uri,
                method,
                Body,
                _queryParams,
                _httpClient
            );
        }

        public IWebRequest WithQueryParams(Dictionary<string, string> parameters)
        {
            return new WebRequest(
                Token,
                Uri,
                HttpMethod,
                Body,
                parameters,
                _httpClient
            );
        }

        public IWebRequest WithBody(IBodyContent body)
        {
            return new WebRequest(
                Token,
                Uri,
                HttpMethod,
                body,
                _queryParams,
                _httpClient
            );
        }

        public override string ToString()
        {
            return $"Uri: {new QueryParamsAsString(_queryParams).With(Uri)}\n" +
                   $"Token: {Token}\n" +
                   $"Body: {Body}";
        }

        private async Task<HttpRequestMessage> RequestMessageAsync(HttpMethod method)
        {
            var request = new HttpRequestMessage(
                method,
                new QueryParamsAsString(_queryParams).With(Uri)
            );
            Body.InjectTo(request);
            await Token.InjectToAsync(request).ConfigureAwait(false);
            return request;
        }
    }
}
