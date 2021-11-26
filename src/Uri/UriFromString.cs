using System;
using System.Collections.Generic;
using WebRequest.Elegant.Core;

namespace WebRequest.Elegant.Core
{
    public class UriFromString : IUri
    {
        private readonly Uri _uri;

        public UriFromString(string uri)
            : this(new Uri(uri))
        {
        }

        public UriFromString(Uri uri)
        {
            _uri = uri;
        }

        public Uri Uri()
        {
            return _uri;
        }

        public override string ToString()
        {
            return _uri.ToString();
        }

        public override bool Equals(object obj)
        {
            return object.ReferenceEquals(this, obj)
                || new TheSameUri(this, obj).ToBool();
        }

        public override int GetHashCode()
        {
#if NETSTANDARD2_1
            return HashCode.Combine(_uri);
#else
            return -338061152 + EqualityComparer<string>.Default.GetHashCode(_uri.ToString());
#endif
        }
    }
}

