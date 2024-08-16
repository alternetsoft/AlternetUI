using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with readonly attributes.
    /// </summary>
    /// <typeparam name="TKey">Type of the attribute identifier. Typically <see cref="string"/>
    /// or <see cref="int"/>.</typeparam>
    /// <typeparam name="TValue">Type of the attribute value.</typeparam>
    public interface IReadOnlyAttributes<TKey, TValue>
    {
        /// <summary>
        /// Calls <see cref="GetAttribute"/>.
        /// </summary>
        /// <param name="id">Attribute identifier.</param>
        /// <returns></returns>
        TValue? this[TKey id] { get; }

        /// <summary>
        /// Attribute with <paramref name="id"/> was added.
        /// </summary>
        /// <param name="id">Attribute identifier.</param>
        /// <returns><c>true</c> if attribute was added; <c>false</c> otherwise.</returns>
        bool HasAttribute(TKey id);

        /// <summary>
        /// Gets attribute value. Returns <c>null</c> if there is no such attribute.
        /// </summary>
        /// <param name="id">Attribute identifier.</param>
        TValue? GetAttribute(TKey id);

        /// <summary>
        /// Gets attribute value as <typeparamref name="TKey"/> type.
        /// </summary>
        /// <typeparam name="T2">Type of the result.</typeparam>
        /// <param name="id">Attribute identifier.</param>
        T2? GetAttribute<T2>(TKey id)
            where T2 : TValue;

        /// <summary>
        /// Gets attribute value as <typeparamref name="T2"/> type or <paramref name="defaultValue"/>
        /// if there is no such attribute.
        /// </summary>
        /// <typeparam name="T2">Type of the result.</typeparam>
        /// <param name="id">Attribute identifier.</param>
        /// <param name="defaultValue">Default value used ud there is no such attribute.</param>
        T2 GetAttribute<T2>(TKey id, T2 defaultValue)
            where T2 : TValue;
    }
}
