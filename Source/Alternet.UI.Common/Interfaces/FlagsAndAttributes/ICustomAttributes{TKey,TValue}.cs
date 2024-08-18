using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with attributes.
    /// </summary>
    /// <typeparam name="TKey">Type of the attribute identifier.</typeparam>
    /// <typeparam name="TValue">Type of the attribute value.</typeparam>
    public interface ICustomAttributes<TKey, TValue> : IReadOnlyAttributes<TKey, TValue>
    {
        /// <summary>
        /// Calls <see cref="SetAttribute"/> and <see cref="IReadOnlyAttributes{TKey,TValue}.GetAttribute"/>.
        /// </summary>
        /// <param name="id">Attribute identifier.</param>
        /// <returns></returns>
        new TValue? this[TKey id] { get; set; }

        /// <summary>
        /// Removes attribute with <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Attribute identifier.</param>
        bool RemoveAttribute(TKey id);

        /// <summary>
        /// Sets attribute value.
        /// </summary>
        /// <param name="id">Attribute identifier.</param>
        /// <param name="value">Attribute value.</param>
        void SetAttribute(TKey id, TValue? value);

        /// <summary>
        /// Sets attribute value.
        /// </summary>
        /// <param name="id">Attribute identifier.</param>
        /// <param name="value">Attribute value.</param>
        void SetAttribute<T2>(TKey id, T2 value)
            where T2 : TValue;
    }
}
