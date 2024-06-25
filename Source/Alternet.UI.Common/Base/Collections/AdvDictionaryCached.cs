using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements dictionary with <see cref="GetValueOrDefaultCached"/>
    /// and <see cref="GetOrCreateCached"/> methods. These methods speed up
    /// work with dictionary if same item is accessed more than once continiously.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public class AdvDictionaryCached<TKey, TValue> : AdvDictionary<TKey, TValue>
        where TKey : notnull
    {
        private (TKey Key, TValue Data)? last;

        /// <summary>
        /// Gets last used item in <see cref="GetOrCreateCached"/> method.
        /// </summary>
        public (TKey Key, TValue Value)? LastItem => last;

        /// <summary>
        /// Gets the value associated with the specified key.
        /// Returns <paramref name="defaultValue"/> if the key is not found.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <remarks>
        /// This function uses value of the <see cref="LastItem"/> property to speed up its work.
        /// </remarks>
        public TValue GetValueOrDefaultCached(TKey key, TValue defaultValue = default!)
        {
            if (last?.Key?.Equals(key) ?? false)
                return last.Value.Data;
            return GetValueOrDefault(key, defaultValue);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// If the <paramref name="key"/> is not found in the dictionary,
        /// it is added there with the value provided by <paramref name="func"/>.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="func">Value provider function.</param>
        /// <remarks>
        /// This function uses value of the <see cref="LastItem"/> property to speed up its work.
        /// </remarks>
        public TValue GetOrCreateCached(TKey key, Func<TValue> func)
        {
            if (last?.Key?.Equals(key) ?? false)
                return last.Value.Data;
            var result = GetOrCreate(key, func);
            last = (key, result);
            return result;
        }

        /// <summary>
        /// Removes the value with the specified key from the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        /// <c>true</c> if the element is successfully found and removed; otherwise, <c>false</c>. This
        /// method returns <c>false</c> if key is not found in the dictionary.
        /// </returns>
        public new bool Remove(TKey key)
        {
            last = default;
            return base.Remove(key);
        }

        /// <summary>
        /// Removes all keys and values from the dictionary.
        /// </summary>
        public new void Clear()
        {
            last = default;
            base.Clear();
        }
    }
}
