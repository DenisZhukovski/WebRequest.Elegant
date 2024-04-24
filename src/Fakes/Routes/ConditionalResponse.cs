using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebRequest.Elegant.Fakes
{
    public class ConditionalResponse : IResponse
    {
        private readonly Func<HttpRequestMessage, Task<bool>> _condition;
        private readonly IResponse _origin;

        public ConditionalResponse(Func<HttpRequestMessage, bool> condition, IResponse origin)
            : this(message => Task.FromResult(condition(message)), origin)
        {
        }

        public ConditionalResponse(Func<HttpRequestMessage, Task<bool>> condition, IResponse origin)
        {
            _condition = condition;
            _origin = origin;
        }

        public Task<bool> MatchesAsync(HttpRequestMessage request)
        {
            return _condition(request);
        }

        public Task<HttpResponseMessage> MessageForAsync(HttpRequestMessage request)
        {
            return _origin.MessageForAsync(request);
        }

        public override bool Equals(object? obj)
        {
            return _origin.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _origin.GetHashCode();
        }

        public override string ToString()
        {
            return _origin.ToString();
        }
    }
}
