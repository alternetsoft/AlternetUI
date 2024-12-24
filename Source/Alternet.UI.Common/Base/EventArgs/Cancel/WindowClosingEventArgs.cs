using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    ///  Provides data for the <see cref='Window.Closing'/> event.
    /// </summary>
    /// <remarks>The <see cref="Window.Closing"/> event occurs just before a window is closed,
    /// either by the user, through the user interface (UI), or programmatically.</remarks>
    public class WindowClosingEventArgs : BaseCancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowClosingEventArgs"/> class.
        /// </summary>
        public WindowClosingEventArgs()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowClosingEventArgs"/> class
        /// with the specified argument.
        /// </summary>
        /// <param name="cancel"><c>true</c> to cancel the event; otherwise, <c>false</c>.</param>
        public WindowClosingEventArgs(bool cancel)
            : base(cancel)
        {
        }
    }
}