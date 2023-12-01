using System;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies options on a <see cref="MessageBox" />.
    /// Currently these options are ignored and are added for the compatibility.
    /// </summary>
    [Flags]
    public enum MessageBoxOptions
    {
        /// <summary>
        /// The message box is displayed on the active desktop.
        /// </summary>
        /// <remarks>
        /// The caller is a service notifying the user of an event. Show displays a message box on the current
        /// active desktop, even if there is no user logged on to the computer.
        /// </remarks>
        ServiceNotification = 0x200000,

        /// <summary>
        /// The message box is displayed on the active desktop.
        /// </summary>
        /// <remarks>
        /// This constant is similar to ServiceNotification, except that the system displays the message
        /// box only on the default desktop of the interactive window station. The application that displayed
        /// the message box loses focus, and the message box is displayed without using visual styles.
        /// </remarks>
        DefaultDesktopOnly = 0x20000,

        /// <summary>
        /// The message box text is right-aligned.
        /// </summary>
        RightAlign = 0x80000,

        /// <summary>
        /// Specifies that the message box text is displayed with right to left reading order.
        /// </summary>
        RtlReading = 0x100000,
    }
}