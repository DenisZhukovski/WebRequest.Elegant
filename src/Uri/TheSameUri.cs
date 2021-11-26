using System;
using WebRequest.Elegant.Core;

namespace WebRequest.Elegant
{
    public class TheSameUri
    {
        private readonly IUri _uri;
        private readonly object _obj;

        public TheSameUri(IUri uri, object obj)
        {
            _uri = uri;
            _obj = obj;
        }

        public bool ToBool()
        {
            if (_obj is IUri uri)
            {
                return _uri.Uri().ToString() == uri.Uri().ToString();
            }

            if (_obj is Uri dotNetUri)
            {
                return _uri.Uri().ToString() == dotNetUri.ToString();
            }

            if (_obj is string uriAsString)
            {
                return _uri.Uri().ToString() == uriAsString;
            }

            return false;
        }
    }
}

