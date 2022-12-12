namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific <see cref="Popup"/> behavior and appearance.
    /// </summary>
    public abstract class PopupHandler : ControlHandler
    {
        /// <inheritdoc/>
        public new Popup Control => (Popup)base.Control;

        /// <summary>
        /// Changes size of the popup to fit the size of its content.
        /// </summary>
        public abstract void SetSizeToContent(WindowSizeToContentMode mode);

        /// <inheritdoc/>
        protected override bool VisualChildNeedsNativeControl => true;
    }
}