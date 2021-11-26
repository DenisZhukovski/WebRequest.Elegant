using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebRequest.Elegant.Core;

namespace WebRequest.Elegant
{
    /// <summary>
    /// The web request entity.
    /// </summary>
    public interface IWebRequest
    {
        /// <summary>
        /// Gets the web request Uri.
        /// </summary>
        IUri Uri { get; }

        /// <summary>
        /// Executes the web request.
        /// </summary>
        /// <returns>Return response.</returns>
        Task<HttpResponseMessage> GetResponseAsync();

        /// <summary>
        /// Creates a new web request with specified path.
        /// </summary>
        /// <param name="uri">Target web uri.</param>
        /// <returns>New web request that configured with specified path.</returns>
        IWebRequest WithPath(IUri uri);

        /// <summary>
        /// Creates a new web request with specified http method.
        /// </summary>
        /// <param name="method">Http method.</param>
        /// <returns>New web request that configured with specified http method.</returns>
        IWebRequest WithMethod(HttpMethod method);

        /// <summary>
        /// Creates a new web request with specified query parameters.
        /// </summary>
        /// <param name="parameters">Dictionary with query parameters.</param>
        /// <returns>New web request that configured with specified query parameters.</returns>
        IWebRequest WithQueryParams(Dictionary<string, string> parameters);

        /// <summary>
        /// Creates a new web request with specified post body.
        /// </summary>
        /// <param name="postBody">The body content that will be sent to the server as POST body.</param>
        /// <returns>New web request that is configured with specified post body parameter.</returns>
        IWebRequest WithBody(IBodyContent body);
    }
}
