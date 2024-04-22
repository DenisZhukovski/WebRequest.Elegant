using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebRequest.Elegant.Fakes
{
    public class StringResponse : IResponse
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _responseMessage;

        public StringResponse(string responseMessage)
            : this(_ => responseMessage.ToResponseMessage())
        {
        }

        public StringResponse(Func<string> responseMessage)
            : this(_ => responseMessage().ToResponseMessage())
        {
        }

        public StringResponse(Func<HttpRequestMessage, string> responseMessage)
            : this(request => responseMessage(request).ToResponseMessage())
        {
        }

        public StringResponse(Func<HttpRequestMessage, HttpResponseMessage> responseMessage)
        {
            _responseMessage = responseMessage;
        }

        public Task<bool> MatchesAsync(HttpRequestMessage request)
        {
            return Task.FromResult(true);
        }

        public Task<HttpResponseMessage> MessageForAsync(HttpRequestMessage request)
        {
            return Task.FromResult(_responseMessage(request));
        }
    }
}
