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
        /// <summary>
        /// Attempts to add the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. It can be null.</param>
        /// <returns>true if the key/value pair was added to the dictionary successfully;
        /// otherwise, false.</returns>
        public bool TryAdd(TKey key, TValue value)
        {
            if (ContainsKey(key))
                return false;
            Add(key, value);
            return true;
        }
#endif

        /// <summary>
        /// Gets the value associated with the specified key.
        /// If the <paramref name="key"/> is not found in the dictionary,
        /// it is added there with the value provided by <paramref name="func"/>.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="func">Value provider function.</param>
        public TValue GetOrCreate(TKey key, Func<TValue> func)
        {
            if (TryGetValue(key, out var result))
                return result;
            result = func();
            Add(key, result);
            return result;
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// Returns <paramref name="defaultValue"/> if the key is not found.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="defaultValue">Default value.</param>
        public TValue GetValueOrDefault(TKey key, TValue defaultValue = default!)
        {
            return TryGetValue(key, out var value) ? value : defaultValue;
        }

        /// <summary>
        /// Gets the value associated with the specified <paramref name="key"/>.
        /// Returns a result of <paramref name="func"/> function call
        /// if the <paramref name="key"/> is not found.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="func">Default value provider function.</param>
        public TValue GetValueOrDefault(TKey key, Func<TValue> func)
        {
            return TryGetValue(key, out var value) ? value : func();
        }
    }
}