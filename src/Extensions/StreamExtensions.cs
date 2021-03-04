using System.IO;
using System.Net.Http;

namespace WebRequest.Elegant.Extensions
{
    public static class StreamExtensions
    {
        public static HttpContent ToStreamContent(this Stream fileStream, string fileName)
        {
            var requestContent = new MultipartFormDataContent();
            requestContent.Add(new StreamContent(fileStream), "application/octet-stream", fileName);
            return requestContent;
        }
    }
}
