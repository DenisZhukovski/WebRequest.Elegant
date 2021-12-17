using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebRequest.Elegant.Fakes
{
    public class ExceptionHttpMessageHandler : HttpMessageHandler
    {
        private readonly Exception _serverException;

        public ExceptionHttpMessageHandler(Exception serverException)
        {
            _serverException = serverException;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            throw _serverException;
        }
    }
}
