using System.Net.Http;

namespace WebRequest.Elegant.Core
{
    public interface IBodyContent
    {
        void InjectTo(HttpRequestMessage request);
    }
}
