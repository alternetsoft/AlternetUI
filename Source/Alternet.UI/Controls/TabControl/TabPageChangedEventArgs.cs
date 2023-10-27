using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the method that will handle the selected tab change events.
    /// </summary>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    public delegate void TabPageChangedEventHandler(object sender, TabPageChangedEventArgs e);

    /// <summary>
    /// Provides data for the selected tab change routed events.
    /// </summary>
    public class TabPageChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabPageChangedEventArgs"/> class.
        /// </summary>
        public TabPageChangedEventArgs(TabPage? oldValue, TabPage? newValue)
            : base()
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>
        /// Old selected tab page.
        /// </summary>
        public TabPage? OldValue { get; }

        /// <summary>
        /// New selected tab page.
        /// </summary>
        public TabPage? NewValue { get; }
    }
}
