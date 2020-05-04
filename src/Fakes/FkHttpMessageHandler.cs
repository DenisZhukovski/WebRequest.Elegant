using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebRequest.Elegant.Fakes
{
    /// <summary>
    /// Fake Http message handler.The purpose is to simulate the web request execution.
    /// </summary>
    public class FkHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _httpResponseMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="FkHttpMessageHandler"/> class.
        /// </summary>
        /// <param name="jsonResponse">Simulated response in json format.</param>
        public FkHttpMessageHandler(string jsonResponse)
            : this(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse),
            })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FkHttpMessageHandler"/> class.
        /// </summary>
        /// <param name="httpResponseMessage">Simulated response http response message.</param>
        public FkHttpMessageHandler(
            HttpResponseMessage httpResponseMessage)
        {
            _httpResponseMessage = httpResponseMessage;
        }

        /// <summary>
        /// Gets as a string the request that has been sent last.
        /// </summary>
        public List<string> RequestsAsString { get; private set; } = new List<string>();

        /// <summary>
        /// Simulates sending to the server.
        /// </summary>
        /// <param name="request">The request that has to be sent.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Simulated http response message.</returns>
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var postContent = string.Empty;
            if (request.Content != null)
            {
                postContent = await request.Content
                    .ReadAsStringAsync()
                    .ConfigureAwait(false);
            }

            RequestsAsString.Add($"Request: {request.RequestUri}\nPostBody: {postContent}");
            return _httpResponseMessage;
        }
    }
}
