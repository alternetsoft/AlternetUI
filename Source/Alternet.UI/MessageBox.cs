using System;

namespace Alternet.UI
{
    /// <summary>
    /// Displays a message window, also known as a dialog box, which presents a message to the user.
    /// It is a modal window, blocking other actions in the application until the user closes it.
    /// A <see cref="MessageBox"/> can contain text, buttons, and symbols that inform and instruct the user.
    /// </summary>
    /// <remarks>
    /// To display a message box, call the static method
    /// <see cref="Show(Window, string, string, MessageBoxButtons, MessageBoxIcon, MessageBoxDefaultButton)"/>.
    /// The title, message, buttons, and icons
    /// displayed in the message box are determined by parameters that you pass to this method.
    /// </remarks>
    public static class MessageBox
    {
        /// <summary>
        /// Displays a message box in front of the specified object and with the specified text, caption, buttons, icon, and default button.
        /// </summary>
        /// <param name="owner">A <see cref="Window"/> that will own the modal message box.</param>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons"/> values that specifies which buttons to display in the message box.</param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon"/> values that specifies which icon to display in the message box.</param>
        /// <param name="defaultButton">One of the <see cref="MessageBoxDefaultButton"/> values that specifies a default button for the message box.</param>
        /// <returns>One of the <see cref="MessageBoxResult"/> values.</returns>
        public static MessageBoxResult Show(
            Window? owner,
            string text,
            string? caption = null,
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.Information,
            MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.OK)
        {
            ValidateButtons(buttons, defaultButton);

            var nativeOwner = owner == null ? null : ((NativeWindowHandler)owner.Handler).NativeControl;
            return (MessageBoxResult)Native.MessageBox.Show(
                nativeOwner,
                text,
                caption,
                (Native.MessageBoxButtons)buttons,
                (Native.MessageBoxIcon)icon,
                (Native.MessageBoxDefaultButton)defaultButton);
        }

        private static void ValidateButtons(MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            bool valid;

            switch (defaultButton)
            {
                case MessageBoxDefaultButton.OK:
                    valid = buttons == MessageBoxButtons.OK || buttons == MessageBoxButtons.OKCancel;
                    break;
                case MessageBoxDefaultButton.Cancel:
                    valid = buttons == MessageBoxButtons.YesNoCancel || buttons == MessageBoxButtons.OKCancel;
                    break;
                case MessageBoxDefaultButton.Yes:
                    valid = buttons == MessageBoxButtons.YesNoCancel || buttons == MessageBoxButtons.YesNo;
                    break;
                case MessageBoxDefaultButton.No:
                    valid = buttons == MessageBoxButtons.YesNoCancel || buttons == MessageBoxButtons.YesNo;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            if (!valid)
                throw new ArgumentException($"MessageBoxDefaultButton.{defaultButton} cannot be used together with MessageBoxButtons.{buttons}");
        }

        /// <summary>
        /// Displays a message box in front of the specified object and with the specified text, caption, buttons, icon, and default button.
        /// </summary>
        /// <param name="text">The text to display in the message box.</param>
        /// <param name="caption">The text to display in the title bar of the message box.</param>
        /// <param name="buttons">One of the <see cref="MessageBoxButtons"/> values that specifies which buttons to display in the message box.</param>
        /// <param name="icon">One of the <see cref="MessageBoxIcon"/> values that specifies which icon to display in the message box.</param>
        /// <param name="defaultButton">One of the <see cref="MessageBoxDefaultButton"/> values that specifies a default button for the message box.</param>
        /// <returns>One of the <see cref="MessageBoxResult"/> values.</returns>
        public static MessageBoxResult Show(
            string text,
            string? caption = null,
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.Information,
            MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.OK) =>
            Show(null, text, caption, buttons, icon, defaultButton);
    }

    /// <summary>
    /// Specifies identifiers to indicate the return value of a <see cref="MessageBox"/>.
    /// </summary>
    public enum MessageBoxResult
    {
        /// <summary>
        /// The <see cref="MessageBox"/> box return value is OK.
        /// </summary>
        OK,

        /// <summary>
        /// The <see cref="MessageBox"/> box return value is Cancel.
        /// </summary>
        Cancel,

        /// <summary>
        /// The <see cref="MessageBox"/> box return value is Yes.
        /// </summary>
        Yes,

        /// <summary>
        /// The <see cref="MessageBox"/> box return value is No.
        /// </summary>
        No
    }

    /// <summary>
    /// Specifies identifiers to indicate the default button of a <see cref="MessageBox"/>.
    /// </summary>
    public enum MessageBoxDefaultButton
    {
        /// <summary>
        /// The <see cref="MessageBox"/> box default button is OK.
        /// </summary>
        OK,

        /// <summary>
        /// The <see cref="MessageBox"/> box default button is Cancel.
        /// </summary>
        Cancel,

        /// <summary>
        /// The <see cref="MessageBox"/> box default button is Yes.
        /// </summary>
        Yes,

        /// <summary>
        /// The <see cref="MessageBox"/> box default button is No.
        /// </summary>
        No
    }

    /// <summary>
    /// Specifies constants defining which buttons to display on a <see cref="MessageBox"/>.
    /// </summary>
    public enum MessageBoxButtons
    {
        /// <summary>
        /// The message box contains an OK button.
        /// </summary>
        OK,

        /// <summary>
        /// The message box contains OK and Cancel buttons.
        /// </summary>
        OKCancel,

        /// <summary>
        /// The message box contains Yes, No and Cancel buttons.
        /// </summary>
        YesNoCancel,

        /// <summary>
        /// The message box contains Yes and No buttons.
        /// </summary>
        YesNo,
    }

    /// <summary>
    /// Specifies an icon to display in a <see cref="MessageBox"/>.
    /// </summary>
    public enum MessageBoxIcon
    {
        /// <summary>
        /// The message box contains no icon where possible.
        /// </summary>
        None,

        /// <summary>
        /// The message box contains an Information icon.
        /// </summary>
        Information,

        /// <summary>
        /// The message box contains a Warning icon.
        /// </summary>
        Warning,

        /// <summary>
        /// The message box contains an Error icon.
        /// </summary>
        Error
    }
}