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
        /// in the <see cref="PropertyGrid"/>. This requires <see cref="IPropertyGridItem.Instance"/>
        /// and <see cref="IPropertyGridItem.PropInfo"/> to be specified.
        /// </summary>
        PropInfoSetValue = 0x02,

        /// <summary>
        /// Default value for the <see cref="PropertyGrid.ApplyFlags"/> property.
        /// </summary>
        Default = PropInfoSetValue | PropEvent,
    }
}
