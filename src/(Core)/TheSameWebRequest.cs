using System.Collections.Generic;

namespace WebRequest.Elegant.Core
{
    public class TheSameWebRequest
    {
        private readonly WebRequest _webRequest;
        private readonly object _obj;

        public TheSameWebRequest(WebRequest webRequest, object obj)
        {
            _webRequest = webRequest;
            _obj = obj;
        }

        public bool ToBool()
        {
            if (_obj is WebRequest webRequest)
            {
                return new TheSameUri(_webRequest.Uri, webRequest.Uri).ToBool()
                    && _webRequest._token.Equals(webRequest._token)
                    && _webRequest._httpMethod == webRequest._httpMethod
                    && _webRequest._body.Equals(webRequest._body)
                    && TheSameQueryParameters(webRequest._queryParams);
            }

            return false;
        }

        private bool TheSameQueryParameters(object obj)
        {
            if (obj is Dictionary<string, string> parameters)
            {
                return new TheSameDictionary<string, string>(
                   _webRequest._queryParams,
                    parameters
                ).ToBool();
            }

            return false;
        }
    }
}
