using System;
using System.Net.Http;

namespace WebRequest.Elegant.Fakes
{
    public interface IRoute
    {
        bool Matches(HttpRequestMessage uri);

        HttpResponseMessage Response(HttpRequestMessage request);
    }
}
