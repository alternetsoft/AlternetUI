using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Defines customization parameters used when new <see cref="IPropertyGridItem"/>
    /// instances are created in <see cref="PropertyGrid"/>.
    /// </summary>
    public interface IPropertyGridNewItemParams
    {
        /// <summary>
        /// Gets or sets property label.
        /// </summary>
        /// <remarks>
        /// This setting is used to specify localized or user friendly property label
        /// which will be used in <see cref="PropertyGrid"/> instead of property name.
        /// </remarks>
        string? Label { get; set; }

        /// <summary>
        /// Gets or sets whether to configure <see cref="IPropertyGridItem"/> as nullable property.
        /// </summary>
        bool? IsNullable { get; set; }

        /// <summary>
        /// Gets or sets <see cref="PropertyInfo"/> associated with this
        /// <see cref="IPropertyGridNewItemParams"/> instance.
        /// </summary>
        PropertyInfo? PropInfo { get; set; }

        /// <summary>
        /// Gets or sets kind of the <see cref="Color"/> editor.
        /// </summary>
        /// <remarks>
        /// Used in <see cref="PropertyGrid.CreateColorItemWithKind"/>.
        /// </remarks>
        PropertyGridEditKindColor? EditKindColor { get; set; }

        /// <summary>
        /// Gets or sets kind of the <see cref="string"/> editor.
        /// </summary>
        /// <remarks>
        /// Used in <see cref="PropertyGrid.CreateStringItemWithKind"/>.
        /// </remarks>
        PropertyGridEditKindString? EditKindString { get; set; }

        /// <summary>
        /// Gets or sets whether property editor has ellipsis button.
        /// </summary>
        bool? HasEllipsis { get; set; }

        /// <summary>
        /// Gets or sets whether property editor is readonly.
        /// </summary>
        /// <remarks>
        /// This doesn't change readonly status of ellipsis button. If you need to
        /// change readonly status of all parts of property editor, use
        /// <see cref="PropertyGrid.SetPropertyReadOnly"/>.
        /// </remarks>
        bool? TextReadOnly { get; set; }

        /// <summary>
        /// Gets or sets whether to make ellipsis button readonly if
        /// property is not writable.
        /// </summary>
        /// <remarks>
        /// If <c>true</c>, only text editor part is set readonly
        /// when property specified in <see cref="PropInfo"/> is not writable.
        /// </remarks>
        bool? OnlyTextReadOnly { get; set; }
    }
}
