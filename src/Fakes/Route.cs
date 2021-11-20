using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;

namespace WebRequest.Elegant.Fakes
{
    public class Route : IRoute
    {
        private readonly Dictionary<Uri, HttpResponseMessage> _responses;

        public Route(Dictionary<string, HttpResponseMessage> responses)
            : this(ToDictionary(responses))
        {
        }

        public Route(Dictionary<Uri, HttpResponseMessage> responses)
        {
            _responses = responses;
        }

        public Route With(Uri uri, string filePath)
        {
            var newRoutes = new Dictionary<Uri, HttpResponseMessage>(_responses);
            newRoutes.Add(uri, new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(File.ReadAllText(filePath)),
            });
            return new Route(newRoutes);
        }

        public HttpResponseMessage Response(Uri uri)
        {
            return _responses[uri];
        }

        private static Dictionary<Uri, HttpResponseMessage> ToDictionary(Dictionary<string, HttpResponseMessage> responses)
        {
            var uriResponses = new Dictionary<Uri, HttpResponseMessage>();
            foreach (var key in responses.Keys)
            {
                uriResponses.Add(new Uri(key), responses[key]);
            }
            return uriResponses;
        }
    }
}
