using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the method that will handle the selected tab change events.
    /// </summary>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    public delegate void TabPageChangedEventHandler(object? sender, TabPageChangedEventArgs e);

    /// <summary>
    /// Provides data for the selected tab change routed events.
    /// </summary>
    public class TabPageChangedEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabPageChangedEventArgs"/> class.
        /// </summary>
        public TabPageChangedEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabPageChangedEventArgs"/> class.
        /// </summary>
        public TabPageChangedEventArgs(AbstractControl? oldValue, AbstractControl? newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>
        /// Old selected tab page.
        /// </summary>
        public AbstractControl? OldValue { get; set; }

        /// <summary>
        /// New selected tab page.
        /// </summary>
        public AbstractControl? NewValue { get; set; }
    }
}
