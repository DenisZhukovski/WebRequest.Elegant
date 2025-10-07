using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebRequest.Elegant.Core;
using WebRequest.Elegant.Extensions;

namespace WebRequest.Elegant
{
    public static class WebRequestExtensions
    {
        public static bool IsPost(this IWebRequest request)
        {
            return request.Equals<HttpMethod>(HttpMethod.Post);
        }

        public static bool Equals<T>(this IWebRequest webRequest, T other)
        {
            if (webRequest is IEquatable<T> equatable)
            {
                return equatable.Equals(other);
            }
            return webRequest.Equals(other);
        }

        public static IWebRequest WithPath(this IWebRequest request, Uri uri)
        {
            return request.WithPath(new UriFromString(uri));
        }

        public static IWebRequest WithPath(this IWebRequest request, string url)
        {
            return request.WithPath(new UriFromString(url));
        }

        public static IWebRequest WithRelativePath(this IWebRequest request, string url)
        {
            if (HasDoubleSlash(request, url))
            {
                url = url.TrimStart('/');
            }
            return request.WithPath(new Uri(request.Uri.Uri().AbsoluteUri + url));
        }

        public static Task PostAsync(this IWebRequest request, IJsonObject body, CancellationToken token = default)
        {
            return request
                .WithMethod(HttpMethod.Post)
                .WithBody(body)
                .EnsureSuccessAsync(token);
        }

        public static Task PostAsync(this IWebRequest request, string body, CancellationToken token = default)
        {
            return request
                .WithMethod(HttpMethod.Post)
                .WithBody(body)
                .EnsureSuccessAsync(token);
        }

        public static Task PostAsync(this IWebRequest request, Dictionary<string, IJsonObject> body, CancellationToken token = default)
        {
            return request
                .WithMethod(HttpMethod.Post)
                .WithBody(body)
                .EnsureSuccessAsync(token);
        }

        public static Task PostAsync(this IWebRequest request, HttpContent content, CancellationToken token = default)
        {
            return request
                .WithMethod(HttpMethod.Post)
                .WithBody(content)
                .EnsureSuccessAsync(token);
        }

        public static Task<HttpResponseMessage> GetAsync(this IWebRequest request, string relativePath, CancellationToken token = default)
        {
            return request
                .WithMethod(HttpMethod.Get)
                .WithRelativePath(relativePath)
                .GetResponseAsync(token);
        }

        public static IWebRequest WithBody(this IWebRequest request, string body)
        {
            return request.WithBody(new StringContent(body));
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

        public static Task<HttpResponseMessage> UploadFileAsync(
            this IWebRequest webRequest,
            string filePath,
            CancellationToken token = default)
        {
            var fileStream = new FileStream(filePath, FileMode.Open);
            return webRequest.UploadFileAsync(fileStream, fileStream.Name, token);
        }

        public static async Task<HttpResponseMessage> UploadFileAsync(
            this IWebRequest webRequest,
            Stream fileStream,
            string fileName,
            CancellationToken token = default)
        {
            try
            {
                return await webRequest
                    .WithMethod(HttpMethod.Post)
                    .WithBody(fileStream.ToStreamContent(fileName))
                    .GetResponseAsync(token);
            }
            catch (Exception ex)
            {
                if (ex is TaskCanceledException)
                {
                    throw;
                }
                
                throw new InvalidOperationException(
                   $"Web Request error occured for {webRequest}",
                   ex
                );
            }
        }

        public static async Task EnsureSuccessAsync(this IWebRequest request, CancellationToken token = default)
        {
            try
            {
                var response = await request
                    .GetResponseAsync(token)
                    .ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                if (ex is TaskCanceledException)
                {
                    throw;
                }
                
                throw new InvalidOperationException(
                   $"Web Request error occured for {request}",
                   ex
                );
            }
        }

        public static async Task<string> ReadAsStringAsync(this IWebRequest request, CancellationToken token = default)
        {
            try
            {
                var response = await request
                   .GetResponseAsync(token)
                   .ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                return await response.Content
                   .ReadAsStringAsync()
                   .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (ex is TaskCanceledException)
                {
                    throw;
                }
                
                throw new InvalidOperationException(
                   $"Web Request error occured for {request}",
                   ex
                );
            }
        }

        private static bool HasDoubleSlash(IWebRequest request, string url)
        {
            var uri = request.Uri.Uri();
            return uri.AbsoluteUri[uri.AbsoluteUri.Length - 1] == '/'
                && url[0] == '/';
        }
    }
}
