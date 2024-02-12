using System;

namespace Alternet.UI
{
    /// <summary>
    /// </summary>
    public interface IInputElement
    {
        /// <summary>
        ///     Raise routed event with the given args
        /// </summary>
        /// <param name="e">
        ///     <see cref="RoutedEventArgs"/> for the event to be raised.
        /// </param>
        void RaiseEvent(RoutedEventArgs e);

        /// <summary>
        ///     Add an instance handler for the given RoutedEvent
        /// </summary>
        /// <param name="routedEvent"/>
        /// <param name="handler"/>
        void AddHandler(RoutedEvent routedEvent, Delegate handler);

        /// <summary>
        ///     Remove all instances of the given
        ///     handler for the given RoutedEvent
        /// </summary>
        /// <param name="routedEvent"/>
        /// <param name="handler"/>
        void RemoveHandler(RoutedEvent routedEvent, Delegate handler);
    }
}