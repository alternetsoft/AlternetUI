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
    public interface IPropertyGridTypeRegistry : IObjectToStringOptions
    {
        /// <summary>
        /// Occurs when button is clicked in the property editor.
        /// </summary>
        /// <remarks>
        /// This property is not used.
        /// </remarks>
        event EventHandler? ButtonClick;

        /// <summary>
        /// Gets or sets whether property editor has ellipsis button.
        /// </summary>
        /// <remarks>
        /// This property is not used.
        /// </remarks>
        bool? HasEllipsis { get; set; }

        /// <summary>
        /// Gets or sets <see cref="IPropertyGridItem"/> create function.
        /// </summary>
        PropertyGridItemCreate? CreateFunc { get; set; }

        /// <summary>
        /// Gets <see cref="Type"/> associated with this <see cref="IPropertyGridTypeRegistry"/> item.
        /// </summary>
        Type InstanceType { get; }

        /// <summary>
        /// Gets <see cref="PropertyGrid"/> settings related
        /// to base type of <see cref="InstanceType"/>.
        /// </summary>
        IPropertyGridTypeRegistry? BaseTypeRegistry { get; }

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

        /// <summary>
        /// Gets <see cref="PropertyGrid"/> settings related to the specified property name if
        /// its available, otherwise returns <c>null</c>.
        /// </summary>
        /// <param name="propName">Property name.</param>
        IPropertyGridPropInfoRegistry? GetPropRegistryOrNull(string propName);

        /// <summary>
        /// Adds simple named action which can be used for any purpose.
        /// </summary>
        /// <param name="name">Action name.</param>
        /// <param name="action">Action method.</param>
        void AddSimpleAction(string name, Action action);

        /// <summary>
        /// Gets list of simple actions which were previously added with <see cref="AddSimpleAction"/>.
        /// </summary>
        /// <returns><c>null</c> if no actions were added; list of actions otherwise.</returns>
        IEnumerable<(string, Action)>? GetSimpleActions();

        /// <summary>
        /// Raises <see cref="ButtonClick"/> event.
        /// </summary>
        void RaiseButtonClick(IPropertyGridItem item);
    }
}
