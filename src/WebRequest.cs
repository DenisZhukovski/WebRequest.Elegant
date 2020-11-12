using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebRequest.Elegant
{
    public sealed class WebRequest : IWebRequest
    {
        private readonly Dictionary<string, string> _queryParams;
        private readonly HttpClient _httpClient;
        private readonly Dictionary<string, IJsonObject> _bodies;

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
                new Dictionary<string, IJsonObject> { { string.Empty, new EmptyJsonObject() } },
                new Dictionary<string, string>(),
                httpClient
            )
        {
        }

        public WebRequest(
            IToken token,
            Uri uri,
            HttpMethod method,
            Dictionary<string, IJsonObject> bodies,
            Dictionary<string, string> queryParams,
            HttpClient httpClient)
        {
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
            Token = token ?? throw new ArgumentNullException(nameof(token));
            HttpMethod = method ?? throw new ArgumentNullException(nameof(method));
            _bodies = bodies ?? throw new ArgumentNullException(nameof(bodies));
            _queryParams = queryParams ?? throw new ArgumentNullException(nameof(queryParams));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public IToken Token { get; }

        public Uri Uri { get; }

        public HttpMethod HttpMethod { get; }

        public Dictionary<string, IJsonObject> Body => new Dictionary<string, IJsonObject>(_bodies);

        public async Task<HttpResponseMessage> GetResponseAsync()
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

        public IWebRequest WithPath(Uri uri)
        {
            return new WebRequest(
                Token,
                uri,
                HttpMethod,
                _bodies,
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
                _bodies,
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
                _bodies,
                parameters,
                _httpClient
            );
        }

        public IWebRequest WithBody(Dictionary<string, IJsonObject> bodies)
        {
            return new WebRequest(
                Token,
                Uri,
                HttpMethod,
                bodies,
                _queryParams,
                _httpClient
            );
        }

        public override string ToString()
        {
            var bodies = _bodies.Count == 1
                ? _bodies.Values.First().ToJson()
                : _bodies.AsJson();
            return $"Uri: {new QueryParamsAsString(_queryParams).With(Uri)}\n" +
                   $"Token: {Token}\n" +
                   $"Body: {bodies}";
        }

        private HttpRequestMessage RequestMessage(HttpMethod method)
        {
            var request = new HttpRequestMessage(
                method,
                new QueryParamsAsString(_queryParams).With(Uri)
            );

            if (_bodies.Count == 1)
            {
                var bodyJsonString = _bodies.Values.First().ToJson();
                if (!string.IsNullOrEmpty(bodyJsonString))
                {
                    request.Content = new StringContent(bodyJsonString, Encoding.UTF8, "application/json");
                }
            }
            else
            {
                var content = new MultipartFormDataContent();
                foreach(var key in _bodies.Keys)
                {
                    content.Add(
                        new StringContent(
                            _bodies[key].ToJson(),
                            Encoding.UTF8,
                            "application/json"
                        ),
                        key
                    );
                }
                request.Content = content;
            }

            Token.InjectTo(request);
            return request;
        }
    }
}
