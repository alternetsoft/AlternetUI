namespace Alternet.UI
{
    /// <summary>
    /// Specifies result of user interaction with a modal window shown with <see cref="Window.ShowModal()"/>.
    /// </summary>
    public enum ModalResult
    {
        /// <summary>
        /// ModalResult is not provided.
        /// </summary>
        None,

        /// <summary>
        /// The activity was canceled.
        /// </summary>
        Canceled,

        /// <summary>
        /// The activity was accepted.
        /// </summary>
        Accepted
    }
}