using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebRequest.Elegant
{
    /// <summary>
    /// The web request entity.
    /// </summary>
    public interface IWebRequest
    {
        /// <summary>
        /// Gets the web request token.
        /// </summary>
        IToken Token { get; }

        /// <summary>
        /// Gets the web request Uri.
        /// </summary>
        Uri Uri { get; }

        /// <summary>
        ///     Get the web request http method.
        /// </summary>
        HttpMethod HttpMethod { get; }

        IJsonObject Body { get; }

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
        IWebRequest WithPath(Uri uri);

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
        /// <param name="postBody">The post body as json object.</param>
        /// <returns>New web request that is configured with specified post body parameter.</returns>
        IWebRequest WithBody(IJsonObject postBody);
    }
}
