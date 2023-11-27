using System;

namespace Alternet.UI
{
    /// <summary>
    /// Displays a message window, also known as a dialog box, which presents a message to the user.
    /// It is a modal window, blocking other actions in the application until the user closes it.
    /// A <see cref="MessageBox"/> can contain text, buttons, and symbols that inform and instruct
    /// the user.
    /// </summary>
    /// <remarks>
    /// To display a message box, call the static method
    /// <see cref="Show(Window, object, string, MessageBoxButtons, MessageBoxIcon, MessageBoxDefaultButton)"/>.
    /// The title, message, buttons, and icons
    /// displayed in the message box are determined by parameters that you pass to this method.
    /// </remarks>
    public static class MessageBox
    {
        /// <summary>
        /// Displays a message box in front of the specified object and with the specified
        /// text, caption, buttons, icon, and default button.
        /// </summary>
        /// <param name="owner">A <see cref="Window"/> that will own the modal message box.</param>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons"/> values that
        /// specifies which buttons to display in the message box.</param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon"/> values that specifies
        /// which icon to display in the message box.</param>
        /// <param name="defaultButton">One of the <see cref="MessageBoxDefaultButton"/> values
        /// that specifies a default button for the message box.</param>
        /// <returns>One of the <see cref="MessageBoxResult"/> values.</returns>
        public static MessageBoxResult Show(
            Window? owner,
            object? text = null,
            string? caption = null,
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.Information,
            MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.OK)
        {
            if(buttons == MessageBoxButtons.YesNo || buttons == MessageBoxButtons.YesNoCancel)
            {
                if (defaultButton == MessageBoxDefaultButton.OK)
                    defaultButton = MessageBoxDefaultButton.Yes;
            }

            ValidateButtons(buttons, defaultButton);

            var nativeOwner = owner == null ? null :
                ((NativeWindowHandler)owner.Handler).NativeControl;
            return (MessageBoxResult)Native.MessageBox.Show(
                nativeOwner,
                text?.ToString() ?? string.Empty,
                caption,
                (Native.MessageBoxButtons)buttons,
                (Native.MessageBoxIcon)icon,
                (Native.MessageBoxDefaultButton)defaultButton);
        }

        /// <summary>
        /// Displays a message box in front of the specified object and with the specified
        /// text, caption, buttons, icon, and default button.
        /// </summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons"/> values that
        /// specifies which buttons to display in the message box.</param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon"/> values that specifies
        /// which icon to display in the message box.</param>
        /// <param name="defaultButton">One of the <see cref="MessageBoxDefaultButton"/> values
        /// that specifies a default button for the message box.</param>
        /// <returns>One of the <see cref="MessageBoxResult"/> values.</returns>
        public static MessageBoxResult Show(
            object? text = null,
            string? caption = null,
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.Information,
            MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.OK) =>
            Show(null, text, caption, buttons, icon, defaultButton);

        private static void ValidateButtons(
            MessageBoxButtons buttons,
            MessageBoxDefaultButton defaultButton)
        {
            var valid = defaultButton switch
            {
                MessageBoxDefaultButton.OK =>
                    buttons == MessageBoxButtons.OK || buttons == MessageBoxButtons.OKCancel,
                MessageBoxDefaultButton.Cancel => buttons == MessageBoxButtons.YesNoCancel ||
                                        buttons == MessageBoxButtons.OKCancel,
                MessageBoxDefaultButton.Yes => buttons == MessageBoxButtons.YesNoCancel ||
                                        buttons == MessageBoxButtons.YesNo,
                MessageBoxDefaultButton.No => buttons == MessageBoxButtons.YesNoCancel ||
                                        buttons == MessageBoxButtons.YesNo,
                _ => throw new InvalidOperationException(),
            };
            if (!valid)
            {
                throw new ArgumentException(
                    $"'{defaultButton}' cannot be used together with '{buttons}'");
            }
        }
    }
}