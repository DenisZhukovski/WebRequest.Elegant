using System;
using System.Net.Http;

namespace WebRequest.Elegant.Fakes
{
    public interface IResponse
    {
        HttpResponseMessage MessageFor(Uri uri);
    }
}