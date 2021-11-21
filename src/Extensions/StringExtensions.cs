namespace WebRequest.Elegant.Extensions
{
    public static class StringExtensions
    {
        public static string NoNewLines(this string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return data;
            }
            return data
                .Replace("\\r\\n", string.Empty)
                .Replace("\r\n", string.Empty)
                .Replace("\\n", string.Empty)
                .Replace("\\t", string.Empty)
                .Replace("\t", string.Empty)
                .Replace("  ", string.Empty); // in some editor tabs replaced with spaces
        }
    }
}
