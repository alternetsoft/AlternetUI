using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
