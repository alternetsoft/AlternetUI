using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the method used to show a control.
    /// </summary>
    public enum ShowMethod
    {
        /// <summary>
        /// Indicates that the control should be shown using the default method,
        /// which is determined by the control's implementation.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Indicates that the control should not be shown.
        /// </summary>
        None = 1,
    }
}
