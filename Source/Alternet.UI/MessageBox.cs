using System;

namespace Alternet.UI
{
    /// <summary>
    /// Displays a message window, also known as a dialog box, which presents a message to the user.
    /// It is a modal window, blocking other actions in the application until the user closes it.
    /// A <see cref="MessageBox"/> can contain text, buttons, and symbols that inform and instruct the user.
    /// </summary>
    /// <remarks>
    /// To display a message box, call the static method <see cref="Show(string, string)"/>. The title, message, buttons, and icons
    /// displayed in the message box are determined by parameters that you pass to this method.
    /// </remarks>
    public static class MessageBox
    {
        /// <summary>
        /// Displays a message box with specified text and caption.
        /// </summary>
        public static void Show(string text, string caption)
        {
            if (text is null)
                throw new ArgumentNullException(nameof(text));

            Native.MessageBox.Show(text, caption);
        }

        /// <summary>
        /// Displays a message box with specified text.
        /// </summary>
        public static void Show(string text)
        {
            if (text is null)
                throw new ArgumentNullException(nameof(text));

            Native.MessageBox.Show(text, " ");
        }
    }
}