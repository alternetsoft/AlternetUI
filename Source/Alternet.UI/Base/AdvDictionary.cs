using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Adds additional functionality to the <see cref="Dictionary{TKey,TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    internal class AdvDictionary<TKey, TValue> : Dictionary<TKey, TValue>
        where TKey : notnull
    {
#if NETFRAMEWORK
        public bool TryAdd(TKey key, TValue value)
        {
            if (ContainsKey(key))
                return false;
            Add(key, value);
            return true;
        }
#endif

        public TValue GetOrCreate(TKey key, Func<TValue> func)
        {
            if (TryGetValue(key, out var result))
                return result;
            result = func();
            Add(key, result);
            return result;
        }

        public TValue GetValueOrDefault(TKey key, TValue defaultValue = default!)
        {
            return TryGetValue(key, out var value) ? value : defaultValue;
        }

        public TValue GetValueOrDefault(TKey key, Func<TValue> defaultValueProvider)
        {
            return TryGetValue(key, out var value) ? value : defaultValueProvider();
        }
    }
}