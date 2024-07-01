namespace Alternet.UI
{
    /// <summary>
    /// Defines where an application should send unhandled exceptions.
    /// </summary>
    public enum UnhandledExceptionMode
    {
        /// <summary>
        /// Always route exceptions to the event handler.
        /// </summary>
        CatchException,

        /// <summary>
        /// Never route exceptions to the event handler.
        /// </summary>
        ThrowException,
    }
}