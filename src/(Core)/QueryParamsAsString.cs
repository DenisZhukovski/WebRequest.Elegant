using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebRequest.Elegant
{
    internal class QueryParamsAsString
    {
        private readonly Dictionary<string, string> _queryParams;

        public QueryParamsAsString(Dictionary<string, string> queryParams)
        {
            _queryParams = queryParams;
        }

        public string With(Uri origin)
        {
            return new UriBuilder(origin)
            {
                Query = ToString(),
            }.ToString();
        }

        public override string ToString()
        {
            if (_queryParams.Any())
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append("?");
                foreach (var queryParamsKey in _queryParams.Keys)
                {
                    var value = _queryParams[queryParamsKey];
                    if (!string.IsNullOrEmpty(value))
                    {
                        stringBuilder.Append($"{queryParamsKey}={_queryParams[queryParamsKey]}&");
                    }
                    else
                    {
                        stringBuilder.Append($"{queryParamsKey}&");
                    }
                }

                stringBuilder.Remove(stringBuilder.Length - 1, 1); // remove last &
                return stringBuilder.ToString();
            }

            return string.Empty;
        }
    }
}
