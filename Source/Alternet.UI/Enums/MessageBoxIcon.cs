namespace Alternet.UI
{
    /// <summary>
    /// Specifies an icon to display in a <see cref="MessageBox"/> or any other places.
    /// </summary>
    public enum MessageBoxIcon
    {
        /// <summary>
        /// The message box contains no icon where possible.
        /// </summary>
        None,

        /// <summary>
        /// The message box contains an 'Information' icon.
        /// </summary>
        Information,

        /// <summary>
        /// The message box contains a 'Warning' icon.
        /// </summary>
        Warning,

        /// <summary>
        /// The message box contains an 'Error' icon.
        /// </summary>
        Error,

        /// <summary>
        /// Displays a question mark symbol. This style is not supported for message dialogs under
        /// Windows when a task dialog is used to implement them (i.e. when running under
        /// Windows Vista or later) because Microsoft guidelines indicate that no icon
        /// should be used for routine confirmations. If it is specified, no icon will be displayed.
        /// Currently not implemented, shows now icon in this case.
        /// </summary>
        Question,

        /// <summary>
        /// The message box contains a symbol consisting of a white X in a circle with a red background.
        /// Currently not implemented, shows now icon in this case.
        /// </summary>
        Hand,

        /// <summary>
        /// The message box contains a symbol consisting of an exclamation point in a
        /// triangle with a yellow background.
        /// Currently not implemented, shows now icon in this case.
        /// </summary>
        Exclamation,

        /// <summary>
        /// The message box contains a symbol consisting of
        /// a lowercase letter i in a circle.
        /// Currently not implemented, shows now icon in this case.
        /// </summary>
        Asterisk,

        /// <summary>
        /// The message box contains a symbol consisting
        /// of white X in a circle with a red background.
        /// Currently not implemented, shows now icon in this case.
        /// </summary>
        Stop,
    }
}