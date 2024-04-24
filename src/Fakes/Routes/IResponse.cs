using System.Net.Http;
using System.Threading.Tasks;

namespace WebRequest.Elegant.Fakes
{
    public interface IResponse
    {
        Task<bool> MatchesAsync(HttpRequestMessage request);

        Task<HttpResponseMessage> MessageForAsync(HttpRequestMessage request);
    }
}
