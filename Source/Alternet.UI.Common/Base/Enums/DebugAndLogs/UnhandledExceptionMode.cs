namespace Alternet.UI
{
    /// <summary>
    /// Defines where an application should send unhandled exceptions.
    /// </summary>
    public enum UnhandledExceptionMode
    {
        /// <summary>
        /// Always route exceptions to the event handler.
        /// If no event handler is assigned, application will
        /// show default error dialog
        /// with exception details, 'Continue' and 'Quit' buttons.
        /// </summary>
        CatchException,

        /// <summary>
        /// Never route exceptions to the event handler. Application will throw the exceptions.
        /// </summary>
        ThrowException,

        /// <summary>
        /// Route exceptions to the event handler.
        /// If exceptions are not handled there, application will
        /// show default error dialog
        /// with exception details, 'Continue' and 'Quit' buttons.
        /// </summary>
        CatchWithDialog,

        /// <summary>
        /// Route exceptions to the event handler.
        /// If exceptions are not handled there, application will
        /// show default error dialog
        /// with exception details, 'Continue' and 'Quit' buttons.
        /// If user presses 'Continue', exception will be shrown.
        /// </summary>
        CatchWithDialogAndThrow,

        /// <summary>
        /// Route exceptions to the event handler. If exceptions are not handled there
        /// they will be shrown.
        /// </summary>
        CatchWithThrow,
    }
}