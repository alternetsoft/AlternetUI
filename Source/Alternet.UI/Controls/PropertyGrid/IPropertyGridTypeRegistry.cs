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
        /// Gets <see cref="PropertyGrid"/> settings related to <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="propInfo">Property information.</param>
        IPropertyGridPropInfoRegistry GetPropRegistry(PropertyInfo propInfo);

        /// <summary>
        /// Gets <see cref="PropertyGrid"/> settings related to <see cref="PropertyInfo"/> if
        /// its available, otherwise returns <c>null</c>.
        /// </summary>
        /// <param name="propInfo">Property information.</param>
        IPropertyGridPropInfoRegistry? GetPropRegistryOrNull(PropertyInfo propInfo);
    }
}
