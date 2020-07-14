using System;

namespace WebRequest.Elegant
{
    public static class WebRequestExtensions
    {
        public static IWebRequest WithPath(this IWebRequest request, string url)
        {
            return request.WithPath(new Uri(url));
        }

        public static IWebRequest WithRelativePath(this IWebRequest request, string url)
        {
            if (HasDoubleSlash(request, url))
            {
                url = url.TrimStart('/');
            }
            return request.WithPath(new Uri(request.Uri.AbsoluteUri + url));
        }

        private static bool HasDoubleSlash(IWebRequest request, string url)
        {
            return request.Uri.AbsoluteUri[request.Uri.AbsoluteUri.Length - 1] == '/'
                && url[0] == '/';
        }
    }
}
