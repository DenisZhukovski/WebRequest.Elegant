namespace WebRequest.Elegant
{
    internal class EmptyJsonObject : IJsonObject
    {
        public string ToJson()
        {
            return string.Empty;
        }
    }
}
