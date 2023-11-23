using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines all flags used in <see cref="PropertyGrid"/> when property value
    /// is applied back to object instance.
    /// </summary>
    [Flags]
    public enum PropertyGridApplyFlags
    {
        /// <summary>
        /// Raises <see cref="IPropertyGridItem.PropertyChanged"/> event when property is changed
        /// in the <see cref="PropertyGrid"/>.
        /// </summary>
        PropEvent = 0x01,

        /// <summary>
        /// Calls <see cref="PropertyInfo.SetValue(object?, object?)"/> when property is changed
        /// in the <see cref="PropertyGrid"/>. This requires <see cref="IPropInfoAndInstance.Instance"/>
        /// and <see cref="IPropInfoAndInstance.PropInfo"/> to be specified in the
        /// <see cref="IPropertyGridItem"/>.
        /// </summary>
        PropInfoSetValue = 0x02,

        /// <summary>
        /// Reloads property value again after set value (<see cref="PropInfoSetValue"/>)
        /// and updates <see cref="PropertyGrid"/>.
        /// </summary>
        ReloadAfterSetValue = 0x04,

        /// <summary>
        /// Reloads all property value again after set value (<see cref="PropInfoSetValue"/>)
        /// and updates <see cref="PropertyGrid"/>.
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
        /// Default value for the <see cref="PropertyGrid.ApplyFlags"/> property.
        /// </summary>
        Default = PropertyGridApplyFlags.PropEvent,
    }
}
