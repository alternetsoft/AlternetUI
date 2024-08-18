using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Default implementation of the flags and attributes used in the library.
    /// </summary>
    /// <typeparam name="TKey">The type of the identifiers.</typeparam>
    /// <typeparam name="TValue">The type of the values.</typeparam>
    public class FlagsAndAttributes<TKey, TValue>
        : AdvDictionary<TKey, TValue>, IFlagsAndAttributes<TKey, TValue>,
        ICustomFlags<TKey>, ICustomAttributes<TKey, TValue>
        where TKey : notnull
    {
        /// <inheritdoc/>
        public ICustomFlags<TKey> Flags => this;

        /// <inheritdoc/>
        public ICustomAttributes<TKey, TValue> Attr => this;

        bool ICustomFlags<TKey>.this[TKey id]
        {
            get => HasFlag(id);
            set => SetFlag(id, value);
        }

        bool IReadOnlyFlags<TKey>.this[TKey id]
        {
            get => HasFlag(id);
        }

        TValue? ICustomAttributes<TKey, TValue>.this[TKey id]
        {
            get => GetAttribute(id);
            set => SetAttribute(id, value);
        }

        /// <inheritdoc/>
        public bool HasFlag(TKey id) => HasAttribute(id);

        /// <inheritdoc/>
        public void AddFlag(TKey id) => TryAdd(id, default!);

        /// <inheritdoc/>
        public void RemoveFlag(TKey id) => RemoveAttribute(id);

        /// <inheritdoc/>
        public void ToggleFlag(TKey id)
        {
            if (HasFlag(id))
                RemoveFlag(id);
            else
                AddFlag(id);
        }

        /// <inheritdoc/>
        public void SetFlag(TKey id, bool value)
        {
            if (value)
                AddFlag(id);
            else
                RemoveFlag(id);
        }

        /// <inheritdoc/>
        public bool HasAttribute(TKey id)
        {
            return ContainsKey(id);
        }

        /// <inheritdoc/>
        public bool RemoveAttribute(TKey id)
        {
            return Remove(id);
        }

        /// <inheritdoc/>
        public void SetAttribute<T2>(TKey id, T2? value)
            where T2 : TValue
        {
            RemoveAttribute(id);
            if (value is not null)
                this[id] = value;
        }

        /// <inheritdoc/>
        public void SetAttribute(TKey id, TValue? value)
        {
            RemoveAttribute(id);
            if(value is not null)
                this[id] = value;
        }

        /// <inheritdoc/>
        public TValue? GetAttribute(TKey id)
        {
            if (TryGetValue(id, out var result))
                return result;
            return default;
        }

        /// <inheritdoc/>
        public T2? GetAttribute<T2>(TKey id)
            where T2 : TValue
        {
            return (T2?)GetAttribute(id);
        }

        /// <inheritdoc/>
        public T2 GetAttribute<T2>(TKey id, T2 defaultValue)
            where T2 : TValue
        {
            if (TryGetValue(id, out var result))
                return (T2)result!;
            return defaultValue;
        }
    }
}
