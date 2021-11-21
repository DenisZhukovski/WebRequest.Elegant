using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebRequest.Elegant.Fakes
{
    /// <summary>
    /// The purpose is to assist during the unit testing.
    /// The message handler can respond with different messages based on configured routes.
    /// If configured route can not find a match for the request then it delegates handling to base class.
    /// </summary>
    public class RoutedHttpMessageHandler : HttpClientHandler
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
        /// Simulates sending requests to the server responding with prepared http messages base on configured routes.
        /// </summary>
        /// <param name="request">The request that has to be sent.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Simulated http response message.</returns>
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (_requestRoute.Matches(request.RequestUri))
            {
                return Task.FromResult(_requestRoute.Response(request.RequestUri));
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}
