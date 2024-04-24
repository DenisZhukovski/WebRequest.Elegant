using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebRequest.Elegant.Fakes
{
    public class RouteResponse : IResponse
    {
        private readonly Uri _uri;
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _message;
        
        public RouteResponse(string uri, string message)
            : this(uri, _ => message.ToResponseMessage())
        {
        }
        
        public RouteResponse(string uri, Func<string> message)
            : this(uri, _ => message().ToResponseMessage())
        {
        }
        
        public RouteResponse(string uri, Func<HttpRequestMessage, string> message)
            : this(uri, uri => message(uri).ToResponseMessage())
        {
        }
        
        public RouteResponse(string uri, Func<HttpRequestMessage, HttpResponseMessage> message)
            : this(new Uri(uri), message)
        {
        }
        
        public RouteResponse(Uri uri, Func<HttpRequestMessage, HttpResponseMessage> message)
        {
            _uri = uri;
            _message = message;
        }

        public Task<bool> MatchesAsync(HttpRequestMessage request)
        {
            return Task.FromResult(
                SameUri(request.RequestUri)
            );
        }

        public Task<HttpResponseMessage> MessageForAsync(HttpRequestMessage request)
        {
            return Task.FromResult(_message(request));
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) 
                || (obj is Uri uri && SameUri(uri))
                || (obj is string uriAsString && uriAsString == _uri.ToString())
                || (obj is HttpRequestMessage request && SameUri(request.RequestUri));
        }

        public override int GetHashCode()
        {
            return _uri.GetHashCode();
        }

        public override string ToString()
        {
            return _uri.ToString();
        }

        private bool SameUri(Uri uri)
        {
            return uri.Equals(_uri) || new Uri(uri.GetLeftPart(UriPartial.Path)).Equals(_uri);
        }
    }
}