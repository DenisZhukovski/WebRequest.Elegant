using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebRequest.Elegant
{
    public static class HttpRequestMessageExtensions
    {
        public static async Task<bool> ContainsAsync(this HttpRequestMessage message, Func<string, bool> condition)
        {
            var stringContent = await message.Content.ReadAsStringAsync().ConfigureAwait(false);
            return condition(stringContent);
        }
    }
}
