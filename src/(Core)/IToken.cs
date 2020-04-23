using System.Net.Http;

namespace WebRequest.Elegant
{
    public interface IToken
    {
        /// <summary>
        /// Prolongates the token.
        /// </summary>
        /// <param name="httpResponseMessage">The http response message that contain new token.</param>
        void ProlongateFrom(HttpResponseMessage httpResponseMessage);

        /// <summary>
        /// Inject token into http request.
        /// </summary>
        /// <param name="request">The http request message.</param>
        void InjectTo(HttpRequestMessage request);
    }
}
