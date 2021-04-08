using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using WebRequest.Elegant.Extensions;

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

        public static IWebRequest WithBody(this IWebRequest request, IJsonObject body)
        {
            return request.WithBody(new JsonBodyContent(body));
        }

        public static IWebRequest WithBody(this IWebRequest request, Dictionary<string, IJsonObject> body)
        {
            return request.WithBody(new MultiArgumentsBodyContent(body));
        }

        public static IWebRequest WithBody(this IWebRequest request, HttpContent content)
        {
            return request.WithBody(new HttpBodyContent(content));
        }

        private static bool HasDoubleSlash(IWebRequest request, string url)
        {
            return request.Uri.AbsoluteUri[request.Uri.AbsoluteUri.Length - 1] == '/'
                && url[0] == '/';
        }

        public static Task<HttpResponseMessage> UploadFileAsync(this IWebRequest webRequest, Stream fileStream, string fileName)
        {
            return webRequest
                .WithMethod(HttpMethod.Post)
                .WithBody(fileStream.ToStreamContent(fileName))
                .GetResponseAsync();
        }

        public static Task<HttpResponseMessage> UploadFileAsync(this IWebRequest webRequest, string filePath)
        {
            var fileStream = new FileStream(filePath, FileMode.Open);
            return webRequest.UploadFileAsync(fileStream, fileStream.Name);
        }

        public static async Task EnsureSuccessAsync(this IWebRequest request)
        {
            var response = await request
                .GetResponseAsync()
                .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }
    }
}
