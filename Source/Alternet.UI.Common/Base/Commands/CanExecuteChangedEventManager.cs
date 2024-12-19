using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Manager for the <see cref="ICommand.CanExecuteChanged"/> event.
    /// </summary>
    internal class CanExecuteChangedEventManager
    {
        public delegate void CanExecuteDelegate(ICommand source, EventHandler<EventArgs> handler);

        public static event CanExecuteDelegate? GlobalAddHandler;

        public static event CanExecuteDelegate? GlobalRemoveHandler;

        /// <summary>
        /// Adds a handler for the given source's event.
        /// </summary>
        public static void AddHandler(ICommand source, EventHandler<EventArgs> handler)
        {
            GlobalAddHandler?.Invoke(source, handler);
        }

        /// <summary>
        /// Remove a handler for the given source's event.
        /// </summary>
        public static void RemoveHandler(ICommand source, EventHandler<EventArgs> handler)
        {
            GlobalRemoveHandler?.Invoke(source, handler);
        }
    }
}