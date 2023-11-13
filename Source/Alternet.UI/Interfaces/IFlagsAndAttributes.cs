using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods which work with flags and attributes.
    /// </summary>
    /// <remarks>
    /// Both flags and attributes share the same dictionary, so you can't have
    /// flag and attribute with the same name.
    /// </remarks>
    internal interface IFlagsAndAttributes
    {
        /// <summary>
        /// Flag with <paramref name="name"/> was added.
        /// </summary>
        /// <param name="name">Flag name.</param>
        /// <returns><c>true</c> if flag was added; <c>false</c> otherwise.</returns>
        bool HasFlag(string name);

        /// <summary>
        /// Adds flag with <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Flag name.</param>
        void AddFlag(string name);

        /// <summary>
        /// Removes flag with <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Flag name.</param>
        void RemoveFlag(string name);

        /// <summary>
        /// If flag with <paramref name="name"/> was previously added, it is removed.
        /// And if there is no such flag, it is added.
        /// </summary>
        /// <param name="name">Flag name.</param>
        void ToggleFlag(string name);

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

        void SetAttribute(string name, object? value);

        object? GetAttribute(string name);

        T? GetAttribute<T>(string name);

        T GetAttribute<T>(string name, T defaultValue);
    }
}
