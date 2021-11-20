using System;
using System.Net.Http;

namespace WebRequest.Elegant.Fakes
{
    public interface IRoute
    {
        HttpResponseMessage Response(Uri uri);
    }
}
