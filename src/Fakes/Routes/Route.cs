using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace WebRequest.Elegant.Fakes
{
    public class Route : IRoute
    {
        private readonly IEnumerable<IResponse> _responses;

        public Route()
            : this(new Dictionary<Uri, Func<HttpResponseMessage>>())
        {
        }

        public Route(Dictionary<string, string> responses)
            : this(ToDictionary(responses))
        {
        }
        
        public Route(string uri, string response)
            : this(new RouteResponse(uri, response))
        {
        }
        
        public Route(Dictionary<string, Func<string>> responses)
            : this(ToDictionary(responses))
        {
        }

        public Route(Dictionary<string, HttpResponseMessage> responses)
            : this(ToDictionary(responses))
        {
        }

        public Route(Dictionary<Uri, Func<HttpResponseMessage>> responses)
            : this(ToResponses(responses))
        {
        }
        
        public Route(params IResponse[] responses)
            : this((IEnumerable<IResponse>)responses)
        {
        }
        
        public Route(IEnumerable<IResponse> responses)
        {
            _responses = responses;
        }

        public Route With(Uri uri, string filePath)
        {
            return With(
                new RouteResponse(
                    uri,
                    _ => File.ReadAllText(filePath).ToResponseMessage()
                )
            );
        }
        
        public Route With(IResponse response)
        {
            return new Route(
                new List<IResponse>(_responses) { response }
            );
        }

        public bool Matches(Uri uri)
        {
            if (_responses.Any(response => response.Equals(uri)))
            {
                return true;
            }

            var moreGenericUri = new Uri(uri.GetLeftPart(UriPartial.Path));
            return _responses.Any(response => response.Equals(moreGenericUri));
        }

        public HttpResponseMessage Response(Uri uri)
        {
            var response = _responses.FirstOrDefault(res => res.Equals(uri));
            if (response != null)
            {
                return response.MessageFor(uri);
            }

            var moreGenericUri = new Uri(uri.GetLeftPart(UriPartial.Path));
            response = _responses.First(res => res.Equals(moreGenericUri));
            return response.MessageFor(moreGenericUri);
        }

        private static Dictionary<string, Func<string>> ToDictionary(Dictionary<string, string> responses)
        {
            var messagesResponses = new Dictionary<string, Func<string>>();
            foreach (var key in responses.Keys)
            {
                messagesResponses.Add(key, () => responses[key]);
            }
            return messagesResponses;
        }
        
        private static Dictionary<Uri, Func<HttpResponseMessage>> ToDictionary(Dictionary<string, Func<string>> responses)
        {
            var messagesResponses = new Dictionary<Uri, Func<HttpResponseMessage>>();
            foreach (var key in responses.Keys)
            {
                messagesResponses.Add(new Uri(key), () => responses[key]().ToResponseMessage());
            }
            return messagesResponses;
        }

        private static Dictionary<Uri, Func<HttpResponseMessage>> ToDictionary(Dictionary<string, HttpResponseMessage> responses)
        {
            var uriResponses = new Dictionary<Uri, Func<HttpResponseMessage>>();
            foreach (var key in responses.Keys)
            {
                uriResponses.Add(new Uri(key), () => responses[key]);
            }
            return uriResponses;
        }
        
        private static IList<IResponse> ToResponses(Dictionary<Uri, Func<HttpResponseMessage>> responses)
        {
            var uriResponses = new List<IResponse>();
            foreach (var uri in responses.Keys)
            {
                uriResponses.Add(new RouteResponse(uri, responseUri => responses[responseUri]()));
            }
            return uriResponses;
        }
    }
}
