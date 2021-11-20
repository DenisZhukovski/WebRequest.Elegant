namespace WebRequest.Elegant
{
    public class SimpleString : IJsonObject
    {
        private readonly string _value;

        public SimpleString(string value)
        {
            _value = value;
        }
        public string ToJson()
        {
            return _value;
        }
    }
}
