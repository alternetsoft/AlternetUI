using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the method that will handle the selected tab change events.
    /// </summary>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    public delegate void SelectedTabPageChangingEventHandler(object sender, SelectedTabPageChangingEventArgs e);

    /// <summary>
    /// Provides data for the selected tab change routed events.
    /// </summary>
    public class SelectedTabPageChangingEventArgs : RoutedCancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedTabPageChangingEventArgs"/> class.
        /// </summary>
        public SelectedTabPageChangingEventArgs(RoutedEvent id, TabPage? oldValue, TabPage? newValue)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            RoutedEvent = id;
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
