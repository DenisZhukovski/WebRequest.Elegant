using System;
using WebRequest.Elegant;

namespace WebRequest.Elegant.Extensions
{
    public static class WebRequestExtensions
    {
        public static IWebRequest Path(this IWebRequest request, string url)
        {
            return request.Path(new Uri(url));
        }

        public static IWebRequest RelativePath(this IWebRequest request, string url)
        {
            return request.Path(new Uri(request.Uri.AbsoluteUri + url));
        }
    }
}
