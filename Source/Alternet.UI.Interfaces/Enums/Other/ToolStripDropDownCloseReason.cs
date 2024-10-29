using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the reason that a drop down control was closed.
    /// </summary>
    public enum ToolStripDropDownCloseReason
    {
        /// <summary>
        /// Specifies that the drop down control was closed because another
        /// application has received the focus.
        /// </summary>
        AppFocusChange,

        /// <summary>
        /// Specifies that the drop down control was closed because an application
        /// was launched.
        /// </summary>
        AppClicked,

        /// <summary>
        /// Specifies that the drop down control was closed because one
        /// of its items was clicked.
        /// </summary>
        ItemClicked,

        /// <summary>
        /// Specifies that the drop down control was closed because of keyboard activity,
        /// such as the ESC key being pressed.
        /// </summary>
        Keyboard,

        /// <summary>
        /// Specifies that the drop down control was
        /// closed because the 'Close' method was called.
        /// </summary>
        CloseCalled,

        /// <summary>
        /// Specifies that the drop down control was
        /// closed because the other action was performed.
        /// </summary>
        Other,
    }
}
