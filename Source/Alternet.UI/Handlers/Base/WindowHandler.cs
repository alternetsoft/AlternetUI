namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific <see cref="Window"/> behavior and appearance.
    /// </summary>
    public abstract class WindowHandler : ControlHandler
    {
        /// <summary>
        /// Gets a <see cref="Window"/> this handler provides the implementation for.
        /// </summary>
        public new Window Control => (Window)base.Control;

        /// <summary>
        /// Changes size of the window to fit the size of its content.
        /// </summary>
        public abstract void SetSizeToContent(WindowSizeToContentMode mode);
    }
}