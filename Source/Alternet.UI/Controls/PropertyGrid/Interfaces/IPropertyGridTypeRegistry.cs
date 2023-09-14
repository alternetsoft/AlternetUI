using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains <see cref="PropertyGrid"/> settings related to <see cref="Type"/>.
    /// </summary>
    public interface IPropertyGridTypeRegistry
    {
        /// <summary>
        /// Gets or sets <see cref="IPropertyGridItem"/> create function.
        /// </summary>
        PropertyGridItemCreate? CreateFunc { get; set; }

        /// <summary>
        /// Gets <see cref="Type"/> associated with this <see cref="IPropertyGridTypeRegistry"/> item.
        /// </summary>
        Type InstanceType { get; }

        /// <summary>
        /// Gets <see cref="PropertyGrid"/> settings related to <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="propInfo">Property information.</param>
        IPropertyGridPropInfoRegistry GetPropRegistry(PropertyInfo propInfo);

        /// <summary>
        /// Gets <see cref="PropertyGrid"/> settings related to the specified property name.
        /// </summary>
        /// <param name="propName">Property name.</param>
        IPropertyGridPropInfoRegistry GetPropRegistry(string propName);

        /// <summary>
        /// Gets <see cref="PropertyGrid"/> settings related to <see cref="PropertyInfo"/> if
        /// its available, otherwise returns <c>null</c>.
        /// </summary>
        /// <param name="propInfo">Property information.</param>
        IPropertyGridPropInfoRegistry? GetPropRegistryOrNull(PropertyInfo propInfo);
    }
}
