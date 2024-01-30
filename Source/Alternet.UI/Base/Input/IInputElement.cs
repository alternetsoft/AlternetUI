#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Alternet.UI
{
    /// <summary>
    /// </summary>
    public interface IInputElement 
    {
        #region Events
    
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

        #endregion Events    
    }
}

