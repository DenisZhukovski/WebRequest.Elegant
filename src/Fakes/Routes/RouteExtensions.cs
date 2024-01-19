using System;
using System.Net;
using System.Net.Http;

namespace WebRequest.Elegant.Fakes
{
    public static class RouteExtensions
    {
        public static Route With(this Route route, string uri, string filePath)
        {
            return route.With(new Uri(uri), filePath);
        }
        
        public static HttpResponseMessage ToResponseMessage(this string data)
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(data),
            };
        }
    }
}
