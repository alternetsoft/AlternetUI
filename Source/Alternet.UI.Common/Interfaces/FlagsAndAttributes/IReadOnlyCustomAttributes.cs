using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with readonly attributes.
    /// </summary>
    public interface IReadOnlyCustomAttributes
    {
        /// <summary>
        /// Calls <see cref="GetAttribute"/>.
        /// </summary>
        /// <param name="name">Attribute name.</param>
        /// <returns></returns>
        object? this[string name] { get; }

        /// <summary>
        /// Attribute with <paramref name="name"/> was added.
        /// </summary>
        /// <param name="name">Attribute name.</param>
        /// <returns><c>true</c> if attribute was added; <c>false</c> otherwise.</returns>
        bool HasAttribute(string name);

        /// <summary>
        /// Gets attribute value. Returns <c>null</c> if there is no such attribute.
        /// </summary>
        /// <param name="name">Attribute name.</param>
        object? GetAttribute(string name);

        /// <summary>
        /// Gets attribute value as <typeparamref name="T"/> type.
        /// </summary>
        /// <typeparam name="T">Type of the result.</typeparam>
        /// <param name="name">Attribute name.</param>
        T? GetAttribute<T>(string name);

        /// <summary>
        /// Gets attribute value as <typeparamref name="T"/> type or <paramref name="defaultValue"/>
        /// if there is no such attribute.
        /// </summary>
        T GetAttribute<T>(string name, T defaultValue);
    }
}
