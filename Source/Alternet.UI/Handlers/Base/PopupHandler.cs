namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific <see cref="Popup"/> behavior and appearance.
    /// </summary>
    internal abstract class PopupHandler : ControlHandler
    {
        /// <summary>
        /// Gets a <see cref="Popup"/> this handler provides the implementation for.
        /// </summary>
        public new Popup Control => (Popup)base.Control;

        /*/// <inheritdoc/>
        protected override bool VisualChildNeedsNativeControl => true;*/

        /// <summary>
        /// Changes size of the popup to fit the size of its content.
        /// </summary>
        public abstract void SetSizeToContent(WindowSizeToContentMode mode);
    }
}