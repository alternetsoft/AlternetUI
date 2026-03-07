using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines properties that specify the roles of dialog buttons, such as indicating which button acts as the default
    /// or cancel action within a dialog interface.
    /// </summary>
    /// <remarks>Implementations of this interface allow dialog controls to designate specific buttons as the
    /// default or cancel option, which can influence user interactions and keyboard shortcuts. Assigning these roles
    /// helps ensure a consistent and accessible user experience across dialog windows.</remarks>
    public interface IDialogButtonRoles
    {
        /// <summary>
        /// Gets or sets a value indicating whether the button is the default button of the dialog.
        /// </summary>
        bool IsDefault { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the button is the cancel button of the dialog.
        /// </summary>
        bool IsCancel { get; set; }

        /// <summary>
        /// Raises the Click event, notifying subscribers that a click action has occurred.
        /// </summary>
        /// <remarks>This method is typically called in response to user interactions, such as mouse
        /// clicks. Ensure that any necessary event handlers are properly subscribed to handle the Click
        /// event.</remarks>
        void RaiseClick();
    }
}
