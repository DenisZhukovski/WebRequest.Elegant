using System.Net.Http;
using System.Text;
using WebRequest.Elegant.Core;

namespace WebRequest.Elegant
{
    public class JsonBodyContent : IBodyContent
    {
        private readonly IJsonObject _jsonObject;

        public JsonBodyContent(IJsonObject jsonObject)
        {
            _jsonObject = jsonObject;
        }

        public override bool Equals(object obj)
        {
            return obj is IBodyContent content
                && content.ToString() == ToString();
        }

        public override int GetHashCode()
        {
            return _jsonObject.GetHashCode();
        }

        public void InjectTo(HttpRequestMessage request)
        {
            var jsonAsString = _jsonObject.ToJson();
            if (!string.IsNullOrEmpty(jsonAsString))
            {
                request.Content = new StringContent(
                    _jsonObject.ToJson(),
                    Encoding.UTF8,
                    "application/json"
                );
            }
        }

        public override string ToString()
        {
            return _jsonObject.ToJson();
        }
    }
}
