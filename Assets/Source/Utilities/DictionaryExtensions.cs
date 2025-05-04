using System.Collections.Generic;

namespace Source.Utilities
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Converts the specified dictionary to an <see cref="ImmutableDictionary{TKey, TValue}"/>.
        /// </summary>
        public static ImmutableDictionary<TKey, TValue> ToImmutable<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            return new ImmutableDictionary<TKey, TValue>(dictionary);
        }
    }
}