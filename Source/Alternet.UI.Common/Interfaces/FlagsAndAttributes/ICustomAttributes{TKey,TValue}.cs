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

        /// <summary>
        /// Gets the attribute associated with the specified identifier, or adds a new attribute using the provided
        /// factory if none exists.
        /// </summary>
        /// <remarks>If an attribute does not exist for the given identifier, the method invokes the
        /// factory to create and add a new value. Subsequent calls with the same identifier will return the stored
        /// value.</remarks>
        /// <typeparam name="T2">The type of the attribute value. Must inherit from TValue.</typeparam>
        /// <param name="id">The identifier used to locate the attribute.</param>
        /// <param name="defaultValueFactory">A function that creates a new attribute value if one does not exist for the specified identifier.</param>
        /// <returns>The existing attribute value associated with the identifier, or the newly created value if none was found.</returns>
        T2 GetAttributeOrAdd<T2>(TKey id, Func<T2> defaultValueFactory)
            where T2 : TValue;
    }
}
