using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebRequest.Elegant
{
    public interface IToken
    {
        /// <summary>
        /// Prolongates the token.
        /// </summary>
        /// <param name="httpResponseMessage">The http response message that contain new token.</param>
        Task ProlongateFromAsync(HttpResponseMessage httpResponseMessage, CancellationToken token = default);

        /// <summary>
        /// Inject token into http request.
        /// </summary>
        /// <param name="request">The http request message.</param>
        Task InjectToAsync(HttpRequestMessage request, CancellationToken token = default);
    }
}
