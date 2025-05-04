using System.Collections;
using System.Collections.Generic;

namespace Source.Utilities
{
    public class ImmutableDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private readonly Dictionary<TKey, TValue> _dictionary;
        
        public int Count => _dictionary.Count;
        
        public ImmutableDictionary(Dictionary<TKey, TValue> dictionary)
        {
            _dictionary = dictionary;
        }
        
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);
        
        public bool ContainsValue(TValue value) => _dictionary.ContainsValue(value);
        
        public TValue this[TKey key] => _dictionary[key];
    }
}