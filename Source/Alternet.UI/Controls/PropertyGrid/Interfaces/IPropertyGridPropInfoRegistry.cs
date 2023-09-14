using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains <see cref="PropertyGrid"/> settings related to <see cref="PropertyInfo"/>.
    /// </summary>
    public interface IPropertyGridPropInfoRegistry
    {
        /// <summary>
        /// Gets or sets customization parameters used when new <see cref="IPropertyGridItem"/>
        /// instances are created in <see cref="PropertyGrid"/>.
        /// </summary>
        IPropertyGridNewItemParams NewItemParams { get; set; }

        /// <summary>
        /// Gets whether customization parameters are specified in
        /// the <see cref="NewItemParams"/> property.
        /// </summary>
        bool HasNewItemParams { get; }

        /// <summary>
        /// Gets <see cref="PropertyInfo"/> associated with this
        /// <see cref="IPropertyGridPropInfoRegistry"/> instance.
        /// </summary>
        PropertyInfo PropInfo { get; }

        /// <summary>
        /// Gets or sets type of the data source provider for the collection editor.
        /// </summary>
        public Type? ListEditSourceType { get; set; }
    }
}