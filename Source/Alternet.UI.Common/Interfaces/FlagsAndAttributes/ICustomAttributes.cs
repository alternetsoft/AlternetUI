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
    public interface ICustomAttributes : IReadOnlyCustomAttributes
    {
        /// <summary>
        /// Calls <see cref="SetAttribute"/> and <see cref="IReadOnlyCustomAttributes.GetAttribute"/>.
        /// </summary>
        /// <param name="name">Attribute name.</param>
        /// <returns></returns>
        new object? this[string name] { get; set; }

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
        /// Sets attribute value.
        /// </summary>
        /// <param name="name">Attribute name.</param>
        /// <param name="value">Attribute value.</param>
        void SetAttribute<T>(string name, T value);
    }
}
