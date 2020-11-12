using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using WebRequest.Elegant.Core;

namespace WebRequest.Elegant
{
    public class MultiArgumentsBodyContent : IBodyContent
    {
        private readonly Dictionary<string, IJsonObject> _bodyContent;

        public MultiArgumentsBodyContent(Dictionary<string, IJsonObject> bodyContent)
        {
            _bodyContent = bodyContent;
        }

        public void InjectTo(HttpRequestMessage request)
        {
            if (_bodyContent.Any())
            {
                var content = new MultipartFormDataContent("----WebRequestBoundary");
                foreach (var key in _bodyContent.Keys)
                {
                    content.Add(
                        ToContent(_bodyContent[key]),
                        key
                    );
                }
                request.Content = content;
            }
        }

        private HttpContent ToContent(IJsonObject jsonObject)
        {
            return new StringContent(
                jsonObject.ToJson(),
                Encoding.UTF8,
                "application/json"
            );
        }
    }
}
