using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the keyboard edit mode for a control.
    /// </summary>
    public enum KeyboardEditMode
    {
        /// <summary>
        /// No keyboard edit mode is specified.
        /// </summary>
        None,

        /// <summary>
        /// The control enters edit mode when the Enter key is pressed.
        /// </summary>
        EditOnEnter,

        /// <summary>
        /// The control enters edit mode when the F2 key is pressed.
        /// </summary>
        EditOnF2,
    }
}