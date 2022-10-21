using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;

namespace WebRequest.Elegant.Fakes
{
    public class Route : IRoute
    {
        private readonly Dictionary<Uri, Func<HttpResponseMessage>> _responses;

        public Route()
            : this(new Dictionary<Uri, Func<HttpResponseMessage>>())
        {
        }

        public Route(Dictionary<string, string> responses)
            : this(ToDictionary(responses))
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
        {
            _responses = responses;
        }

        public Route With(Uri uri, string filePath)
        {
            return new Route(
                new Dictionary<Uri, Func<HttpResponseMessage>>(_responses)
                {
                    { uri, () => ToResponseMessage(File.ReadAllText(filePath)) }
                }
            );
        }

        public bool Matches(Uri uri)
        {
            if  (_responses.ContainsKey(uri))
            {
                return true;
            }

            return _responses.ContainsKey(new Uri(uri.GetLeftPart(UriPartial.Path)));
        }

        public HttpResponseMessage Response(Uri uri)
        {
            if (_responses.TryGetValue(uri, out Func<HttpResponseMessage> response))
            {
                return response();
            }

            return _responses[new Uri(uri.GetLeftPart(UriPartial.Path))]();
        }

        private static HttpResponseMessage ToResponseMessage(string data)
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(data),
            };
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
                messagesResponses.Add(new Uri(key), () => ToResponseMessage(responses[key]()));
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
    }
}
