namespace Alternet.UI
{
    /// <summary>
    /// Implements a visual theme. <see cref="ControlHandlerFactory"/> property is used to create handlers
    /// for the UI controls when the <see cref="VisualTheme"/> is applied.
    /// </summary>
    public class VisualTheme
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisualTheme"/> class.
        /// </summary>
        public VisualTheme(IControlHandlerFactory controlHandlerFactory)
        {
            ControlHandlerFactory = controlHandlerFactory;
        }

        /// <summary>
        /// Gets a <see cref="ControlHandlerFactory"/> instance which is used to create handlers
        /// for the UI controls when this <see cref="VisualTheme"/> is applied.
        /// </summary>
        public IControlHandlerFactory ControlHandlerFactory { get; }
    }
}