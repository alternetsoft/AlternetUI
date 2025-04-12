using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents different ways a user can close a dialog.
    /// </summary>
    public enum DialogCloseAction
    {
        /// <summary>
        /// Represents cases where none of the defined close actions were taken.
        /// </summary>
        None,

        /// <summary>
        /// The user closed the dialog using the OK button.
        /// </summary>
        OkButton,

        /// <summary>
        /// The user closed the dialog by pressing the Enter key.
        /// </summary>
        EnterKey,

        /// <summary>
        /// The user canceled the dialog using the Cancel button.
        /// </summary>
        CancelButton,

        /// <summary>
        /// The user dismissed the dialog using the Escape key.
        /// </summary>
        EscapeKey,

        /// <summary>
        /// The user closed the dialog using the title bar close button.
        /// </summary>
        CloseButtonInTitleBar,

        /// <summary>
        /// Represents an unspecified or custom close action.
        /// </summary>
        Other,
    }
}
