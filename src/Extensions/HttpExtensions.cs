using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebRequest.Elegant
{
    public static class HttpExtensions
    {
        private static readonly List<HttpStatusCode> AllowedStatuses = new List<HttpStatusCode>
        {
             HttpStatusCode.OK,
             HttpStatusCode.Accepted,
             HttpStatusCode.Created,
        };

        public static async Task ThrowIfNotOkOrAcceptedStatusAsync(this HttpResponseMessage response, Uri request = null)
        {
            if (!AllowedStatuses.Contains(response.StatusCode))
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new InvalidOperationException($"URI:{request};\nResponse Code: {response.StatusCode};\n Data: {content}");
            }
        }
    }
}
