using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Item of the <see cref="PropertyGrid"/>.
    /// </summary>
    public interface IPropertyGridItem
    {
        /// <summary>
        /// Gets objects instance in which property is contained.
        /// </summary>
        /// <remarks>
        /// This is used when object properties are added to <see cref="PropertyGrid"/>.
        /// </remarks>
        object? Instance { get; }

        /// <summary>
        /// Gets property information.
        /// </summary>
        /// <remarks>
        /// This is used when object properties are added to <see cref="PropertyGrid"/>.
        /// </remarks>
        PropertyInfo? PropInfo { get; }

        /// <summary>
        /// Gets list of children properties.
        /// </summary>
        /// <remarks>
        /// You can change list of children properties before item is
        /// added to <see cref="PropertyGrid"/>.
        /// </remarks>
        IList<IPropertyGridItem> Children { get; }

        /// <summary>
        /// Gets parent property.
        /// </summary>
        IPropertyGridItem? Parent { get; }

        /// <summary>
        /// Gets whether <see cref="Children"/> has items.
        /// </summary>
        bool HasChildren { get; }

        /// <summary>
        /// Gets type of the property editor.
        /// </summary>
        string PropertyEditorKind { get; }

        /// <summary>
        /// Item handle.
        /// </summary>
        IntPtr Handle { get; }

        /// <summary>
        /// Item name.
        /// </summary>
        string DefaultName { get; }

        /// <summary>
        /// Item label.
        /// </summary>
        string DefaultLabel { get; }

        /// <summary>
        /// Item default value.
        /// </summary>
        object? DefaultValue { get; }

        /// <summary>
        /// Item is category.
        /// </summary>
        bool IsCategory { get; }
    }
}
