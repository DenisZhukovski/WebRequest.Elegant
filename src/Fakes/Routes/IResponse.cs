using System.Net.Http;

namespace WebRequest.Elegant.Fakes
{
    public interface IResponse
    {
        HttpResponseMessage MessageFor(HttpRequestMessage request);
    }
}