using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods which work with attributes.
    /// </summary>
    public interface ICustomAttributes
    {
        /// <summary>
        /// Attribute with <paramref name="name"/> was added.
        /// </summary>
        /// <param name="name">Attribute name.</param>
        /// <returns><c>true</c> if attribute was added; <c>false</c> otherwise.</returns>
        bool HasAttribute(string name);

        /// <summary>
        /// Removes attribute with <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Attribute name.</param>
        bool RemoveAttribute(string name);

        /// <summary>
        /// Sets attribute value.
        /// </summary>
        /// <param name="name">Attribute name.</param>
        /// <param name="value">Attribute value.</param>
        void SetAttribute(string name, object? value);

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
