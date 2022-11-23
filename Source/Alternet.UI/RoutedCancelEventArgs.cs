namespace Alternet.UI
{
    /// <summary>
    /// Provides data for a cancelable routed event.
    /// </summary>
    public class RoutedCancelEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedCancelEventArgs"/> class.
        /// </summary>
        public RoutedCancelEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedCancelEventArgs"/> class.
        /// </summary>
        public RoutedCancelEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether the event should be canceled.
        /// </summary>
        public bool Cancel { get; set; }
    }
}