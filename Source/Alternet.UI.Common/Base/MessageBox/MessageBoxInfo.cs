using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a delegate that handles the result of a message box interaction.
    /// </summary>
    /// <param name="info">
    /// An object containing details about the message box, including user response and options.
    /// </param>
    public delegate void MessageBoxResultDelegate(MessageBoxInfo info);

    /// <summary>
    /// Defines properties for the message box show handlers.
    /// </summary>
    public partial class MessageBoxInfo : BaseObjectWithAttr
    {
        /// Gets or sets a <see cref="Window"/> that will own the modal dialog box.
        public virtual Window? Owner { get; set; }

        /// Gets or sets the text to display in the message box.
        public virtual object? Text { get; set; }

        /// Gets or sets the text to display in the title bar of the message box.
        public virtual string? Caption { get; set; }

        /// Gets or sets one of the <see cref="MessageBoxButtons" /> values that specifies which
        /// buttons to display in the message box.
        public virtual MessageBoxButtons Buttons { get; set; }

        /// Gets or sets one of the <see cref="MessageBoxIcon" /> values that specifies which icon to
        /// display in the message box.
        public virtual MessageBoxIcon Icon { get; set; }

        /// Gets or sets one of the <see cref="MessageBoxDefaultButton" /> values that specifies the
        /// default button for the message box.
        public virtual MessageBoxDefaultButton DefaultButton { get; set; }

        /// Gets or sets one of the <see cref="MessageBoxOptions" /> values that specifies which display
        /// and association options will be used for the message box. You may pass in 0 if you wish
        /// to use the defaults.
        public virtual MessageBoxOptions Options { get; set; }

        /// <summary>
        /// Gets or sets whether to show help.
        /// Assign <see langword="true" /> to show the Help button; otherwise, <see langword="false" />.
        /// The default is <see langword="false" />.
        /// </summary>
        public virtual bool ShowHelp { get; set; }

        /// <summary>
        /// One of the <see cref="DialogResult" /> values. This is result of the message box dialog.
        /// </summary>
        public virtual DialogResult Result { get; set; } = DialogResult.None;

        /// <summary>
        /// Gets or sets information related to the help file.
        /// </summary>
        public virtual HelpInfo? HelpInfo { get; set; }

        /// <summary>
        /// Assign this property to false, if you need default message box event handler to be called.
        /// </summary>
        public virtual bool Handled { get; set; } = true;

        /// <summary>
        /// This action is called when dialog is closed.
        /// </summary>
        public virtual MessageBoxResultDelegate? OnClose { get; set; }
    }
}