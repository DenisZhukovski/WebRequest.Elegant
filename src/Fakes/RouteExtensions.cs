using System;

namespace WebRequest.Elegant.Fakes
{
    public static class RouteExtensions
    {
        public static Route With(this Route route, string uri, string filePath)
        {
            return route.With(new Uri(uri), filePath);
        }
    }
}
