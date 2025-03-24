namespace Alternet.UI
{
    /// <summary>
    /// Specifies identifiers to indicate the return value of a dialog box.
    /// </summary>
    public enum DialogResult
    {
        /// <summary>
        /// The dialog box return value is 'OK'.
        /// </summary>
        OK,

        /// <summary>
        /// The dialog box return value is 'Cancel'.
        /// </summary>
        Cancel,

        /// <summary>
        /// The dialog box return value is 'Yes'.
        /// </summary>
        Yes,

        /// <summary>
        /// The dialog box return value is 'No'.
        /// </summary>
        No,

        /// <summary>
        /// The dialog box return value is 'Abort'
        /// (usually sent from a button labeled Abort).
        /// </summary>
        Abort,

        /// <summary>
        /// The dialog box return value is 'Retry'
        /// (usually sent from a button labeled Retry).
        /// </summary>
        Retry,

        /// <summary>The dialog box return value is 'Ignore'
        /// (usually sent from a button labeled Ignore).
        /// </summary>
        Ignore,

        /// <summary>
        /// <see langword="Nothing" /> is returned from the dialog box.
        /// This means that the modal dialog continues running.
        /// </summary>
        None,
    }
}
