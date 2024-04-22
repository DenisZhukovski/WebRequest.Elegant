using System.Net.Http;
using System.Threading.Tasks;

namespace WebRequest.Elegant.Fakes
{
    public interface IRoute
    {
        Task<bool> MatchesAsync(HttpRequestMessage uri);

        Task<HttpResponseMessage> ResponseAsync(HttpRequestMessage request);
    }
}
