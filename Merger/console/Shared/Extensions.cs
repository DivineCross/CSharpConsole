using System;
using System.Collections.Generic;

namespace ConsoleApplication
{
    public static class DictionaryExtensions
    {
        public static TValue Get<TKey, TValue>(
            this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
        {
            if (key == null)
                return defaultValue;

            return dict.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public static TValue Get<TKey, TValue>(
            this IReadOnlyDictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
        {
            if (key == null)
                return defaultValue;

            return dict.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public static void ShowKeyValues<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        {
            foreach (var pair in pairs)
            {
                var pairString = $@"{pair.Key}";
                if (pair.Value != null)
                    pairString += $@"=""{pair.Value}""";

                Console.WriteLine(pairString);
            }
        }
    }
}
