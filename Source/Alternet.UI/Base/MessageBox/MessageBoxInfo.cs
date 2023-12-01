namespace Alternet.UI
{
    /// <summary>
    /// Defines properties for the message box show handlers.
    /// </summary>
    public class MessageBoxInfo
    {
        /// Gets or sets a <see cref="Window"/> that will own the modal dialog box.
        public Window? Owner;

        /// Gets or sets the text to display in the message box.
        public object? Text;

        /// Gets or sets the text to display in the title bar of the message box.
        public string? Caption;

        /// Gets or sets one of the <see cref="MessageBoxButtons" /> values that specifies which
        /// buttons to display in the message box.
        public MessageBoxButtons Buttons;

        /// Gets or sets one of the <see cref="MessageBoxIcon" /> values that specifies which icon to
        /// display in the message box.
        public MessageBoxIcon Icon;

        /// Gets or sets one of the <see cref="MessageBoxDefaultButton" /> values that specifies the
        /// default button for the message box.
        public MessageBoxDefaultButton DefaultButton;

        /// Gets or sets one of the <see cref="MessageBoxOptions" /> values that specifies which display
        /// and association options will be used for the message box. You may pass in 0 if you wish to use the defaults.
        public MessageBoxOptions Options;

        /// <summary>
        /// Gets or sets whether to show help.
        /// Assign <see langword="true" /> to show the Help button; otherwise, <see langword="false" />.
        /// The default is <see langword="false" />.
        /// </summary>
        public bool ShowHelp;

        /// <summary>
        /// One of the <see cref="DialogResult" /> values. This is result of the message box dialog.
        /// </summary>
        public DialogResult Result = DialogResult.None;

        /// <summary>
        /// Gets or sets information related to the help file.
        /// </summary>
        public HelpInfo? HelpInfo;

        /// <summary>
        /// Assign this property to false, if you need default message box event handler to be called.
        /// </summary>
        public bool Handled = true;
    }
}