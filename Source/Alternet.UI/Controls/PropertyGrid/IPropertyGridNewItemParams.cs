using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
    }
}
