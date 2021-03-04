using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebRequest.Elegant.Extensions
{
    public static class StreamExtensions
    {
        public static StreamContent ToStreamContent(this Stream fileStream, string fileName)
        {
            var content = new StreamContent(fileStream);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "fileToUpload",
                FileName = fileName
            };
            return content;
        }
    }
}
