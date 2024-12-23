using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Adds additional functionality to the <see cref="ConcurrentDictionary{TKey,TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public class AdvDictionary<TKey, TValue> : ConcurrentDictionary<TKey, TValue>
        where TKey : notnull
    {
        /// <summary>
        /// Gets the value associated with the specified key.
        /// If the <paramref name="key"/> is not found in the dictionary,
        /// it is added there with the value provided by <paramref name="func"/>.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="func">Value provider function.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TValue GetOrCreate(TKey key, Func<TValue> func)
        {
            return GetOrAdd(key, (k) => func());
        }

        /// <summary>
        /// Adds the specified key and value pair to the dictionary or updates value if
        /// key is already added.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add.
        /// The value can be null for reference types.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(TKey key, TValue value)
        {
            AddOrUpdate(key, (k) => value, (k, v) => value);
        }

        /// <summary>
        /// Attempts to remove and return the value that has the specified key from the ditionary.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>True if the object was removed successfully; otherwise, False.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Remove(TKey key)
        {
            return TryRemove(key, out _);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// Returns <paramref name="defaultValue"/> if the key is not found.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="defaultValue">Default value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TValue GetValueOrDefault(TKey key, Func<TValue> func)
        {
            return TryGetValue(key, out var value) ? value : func();
        }
    }
}