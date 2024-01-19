using System;
using System.Net;
using System.Net.Http;

namespace WebRequest.Elegant.Fakes
{
    public class RouteResponse : IResponse
    {
        private readonly Uri _uri;
        private readonly Func<Uri, HttpResponseMessage> _message;
        
        public RouteResponse(string uri, string message)
            : this(uri, _ => message.ToResponseMessage())
        {
        }
        
        public RouteResponse(string uri, Func<string> message)
            : this(uri, _ => message().ToResponseMessage())
        {
        }
        
        public RouteResponse(string uri, Func<Uri, string> message)
            : this(uri, uri => message(uri).ToResponseMessage())
        {
        }
        
        public RouteResponse(string uri, Func<Uri, HttpResponseMessage> message)
            : this(new Uri(uri), message)
        {
        }
        
        public RouteResponse(Uri uri, Func<Uri, HttpResponseMessage> message)
        {
            _uri = uri;
            _message = message;
        }
        
        public HttpResponseMessage MessageFor(Uri uri)
        {
            return _message(uri);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) 
                || (obj is Uri uri && uri.Equals(_uri))
                || (obj is string uriAsString && uriAsString == _uri.ToString());
        }

        public override int GetHashCode()
        {
            return _uri.GetHashCode();
        }
    }
}