using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines all flags used in the property grid and other property editing controls when property value
    /// is applied back to object instance.
    /// </summary>
    /// <remarks>
    /// This enumeration supports a bitwise combination of its member values.
    /// </remarks>
    [Flags]
    public enum PropertyGridApplyFlags
    {
        /// <summary>
        /// Raises 'PropertyChanged' event of the item when property is changed
        /// in the control.
        /// </summary>
        PropEvent = 0x01,

        /// <summary>
        /// Calls <see cref="PropertyInfo.SetValue(object?, object?)"/> when property is changed
        /// in the control. This requires 'Instance'
        /// and 'PropInfo' properties to be specified in the item.
        /// </summary>
        PropInfoSetValue = 0x02,

        /// <summary>
        /// Reloads property value again after set value (<see cref="PropInfoSetValue"/>)
        /// and updates the control.
        /// </summary>
        ReloadAfterSetValue = 0x04,

        /// <summary>
        /// Reloads all property values again after set value (<see cref="PropInfoSetValue"/>)
        /// and updates the control.
        /// </summary>
        ReloadAllAfterSetValue = 0x08,

        /// <summary>
        /// Flags <see cref="ReloadAfterSetValue"/> and <see cref="PropInfoSetValue"/>
        /// are turned on.
        /// </summary>
        SetValueAndReload = ReloadAfterSetValue | PropInfoSetValue,

        /// <summary>
        /// Flags <see cref="ReloadAllAfterSetValue"/> and <see cref="PropInfoSetValue"/>
        /// are turned on.
        /// </summary>
        SetValueAndReloadAll = ReloadAllAfterSetValue | PropInfoSetValue,

        /// <summary>
        /// Default value for the 'ApplyFlags' property.
        /// </summary>
        Default = PropertyGridApplyFlags.PropEvent,
    }
}
