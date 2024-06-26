﻿namespace Alternet.UI
{
    /// <summary>
    /// Specifies identifiers to indicate the default button of a message box.
    /// </summary>
    public enum MessageBoxDefaultButton
    {
        /// <summary>
        /// The first button on the message box is the default button.
        /// </summary>
        Button1,

        /// <summary>
        /// The message box default button is 'OK'.
        /// </summary>
        OK = Button1,

        /// <summary>
        /// The message box default button is 'Cancel'.
        /// </summary>
        Cancel,

        /// <summary>
        /// The message box default button is 'Yes'.
        /// </summary>
        Yes,

        /// <summary>
        /// The message box default button is 'No'.
        /// </summary>
        No,

        /// <summary>
        /// The second button on the message box is the default button.
        /// </summary>
        Button2 = 0x100,

        /// <summary>
        /// The third button on the message box is the default button.
        /// </summary>
        Button3 = 0x200,
    }
}