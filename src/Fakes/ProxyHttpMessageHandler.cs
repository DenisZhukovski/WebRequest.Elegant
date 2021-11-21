using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebRequest.Elegant.Fakes
{
    /// <summary>
    /// The purpose is to assist during the unit testing.
    /// </summary>
    public class ProxyHttpMessageHandler : DelegatingHandler
    {
        public ProxyHttpMessageHandler()
            : this(new HttpClientHandler())
        {
        }

        public ProxyHttpMessageHandler(HttpMessageHandler messageHandler)
        {
            InnerHandler = messageHandler;
        }

        /// <summary>
        /// Gets as a <see cref="HttpRequestMessage"/> the requests that have been sent during the collaboration.
        /// </summary>
        public List<HttpRequestMessage> Requests
        {
            get;
            private set;
        } = new List<HttpRequestMessage>();

        /// <summary>
        /// Gets as a string the requests that have been sent during the collaboration.
        /// </summary>
        public List<string> RequestsContent
        {
            get;
            private set;
        } = new List<string>();

        /// <summary>
        /// Gets as a string the responses that have been received during the collaboration.
        /// </summary>
        public List<string> ResponsesContent
        {
            get;
            private set;
        } = new List<string>();

        /// <summary>
        /// Gets as a <see cref="HttpResponseMessage"/> the responses that have been received during the collaboration.
        /// </summary>
        public List<HttpResponseMessage> Responses
        {
            get;
            private set;
        } = new List<HttpResponseMessage>();

        /// <summary>
        /// Works as a proxy sending requests to the server and receiving the responses.
        /// </summary>
        /// <param name="request">The request that has to be sent.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Simulated http response message.</returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests.Add(request);
            var postContent = string.Empty;
            if (request.Content != null)
            {
                postContent = await request.Content
                    .ReadAsStringAsync()
                    .ConfigureAwait(false);
            }

            RequestsContent.Add(postContent);
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            Responses.Add(response);
            if (response.Content != null)
            {
                ResponsesContent.Add(
                    await response.Content.ReadAsStringAsync().ConfigureAwait(false)
                );
            }

            return response;
        }
    }
}
