using System;
using System.Net.Http;

namespace WebRequest.Elegant.Fakes
{
    public interface IRoute
    {
        bool Matches(Uri uri);

        HttpResponseMessage Response(Uri uri);
    }
}
