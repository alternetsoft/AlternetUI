using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates some of the generic control related actions.
    /// </summary>
    public enum GenericControlAction
    {
        /// <summary>
        /// No action is performed.
        /// </summary>
        None,

        /// <summary>
        /// Shows on-screen keyboard.
        /// </summary>
        ShowKeyboard,

        /// <summary>
        /// Hides on-screen keyboard.
        /// </summary>
        HideKeyboard,

        /// <summary>
        /// Shows on-screen keyboard if no hardware keyboard is present.
        /// </summary>
        ShowKeyboardIfNoHardware,

        /// <summary>
        /// Shows on-screen keyboard if no hardware keyboard is present or it's unknown
        /// whether hardware keyboard is present.
        /// </summary>
        ShowKeyboardIfUnknown,
    }
}
