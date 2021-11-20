using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebRequest.Elegant.Fakes
{
    /// <summary>
    /// The purpose is to assist during the unit testing.
    /// </summary>
    public class RoutedHttpMessageHandler : HttpMessageHandler
    {
        private readonly IRoute _requestRoute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedHttpMessageHandler"/> class.
        /// </summary>
        /// <param name="requestRoute">Manages the responses for http requests.</param>
        public RoutedHttpMessageHandler(IRoute requestRoute)
        {
            _requestRoute = requestRoute;
        }

        /// <summary>
        /// Simulates sending to the server responding with prepared http messages.
        /// </summary>
        /// <param name="request">The request that has to be sent.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Simulated http response message.</returns>
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(_requestRoute.Response(request.RequestUri));
        }
    }
}
