using System;

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
            if (IsDoubleDash(request, url))
            {
                url = url.TrimStart('/');
            }
            return request.Path(new Uri(request.Uri.AbsoluteUri + url));
        }

        private static bool IsDoubleDash(IWebRequest request, string url)
        {
            return request.Uri.AbsoluteUri[request.Uri.AbsoluteUri.Length - 1] == '/' 
                && url[0] == '/';
        }
    }
}
