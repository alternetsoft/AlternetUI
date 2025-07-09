using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines the scope and behavior for propagating the checked state among sibling controls
    /// when a control becomes checked.
    /// </summary>
    public enum CheckedSpreadMode
    {
        /// <summary>
        /// No propagation. The checked state applies only to the current control,
        /// leaving all siblings unaffected.
        /// </summary>
        None,

        /// <summary>
        /// Ensures exclusive selection among immediate siblings. When this control is checked,
        /// all other sibling controls are unchecked.
        /// </summary>
        SingleSibling,

        /// <summary>
        /// Enforces exclusivity across the logical group. When this control is checked,
        /// all other checked controls within the same group are automatically unchecked.
        /// </summary>
        /// <remarks>
        /// Sibling controls are determined using <see cref="AbstractControl.GroupIndex"/> and
        /// <see cref="AbstractControl.GetGroup"/>. The group is retrieved by calling
        /// <see cref="AbstractControl.GetGroup"/> on the control's parent.
        /// </remarks>
        SingleInGroup,
    }
}
