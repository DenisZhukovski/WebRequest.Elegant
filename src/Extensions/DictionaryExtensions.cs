using System.Collections.Generic;

namespace WebRequest.Elegant
{
    public static class DictionaryExtensions
    {
        public static string AsJson(this Dictionary<string, IJsonObject> dictionary)
        {
            string[] results = new string[dictionary.Count];
            var count = 0;
            foreach(var key in dictionary.Keys)
            {
                results[count] = $"{key}: {dictionary[key].ToJson()}";
            }
            return string.Join("\n", results);
        }
    }
}
