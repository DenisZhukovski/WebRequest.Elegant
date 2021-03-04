using System.Net.Http;
using WebRequest.Elegant.Core;

namespace WebRequest.Elegant
{
    public class HttpBodyContent : IBodyContent
    {
        private readonly HttpContent _content;

        public HttpBodyContent(HttpContent content)
        {
            _content = content;
        }

        public void InjectTo(HttpRequestMessage request)
        {
            request.Content = _content;
        }

        public override string ToString()
        {
            return _content.ToString();
        }
    }
}
