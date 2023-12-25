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

        #region Input

        // Mouse          

        /// <summary>
        ///     An event reporting the left mouse button was pressed.
        /// </summary>
        event MouseEventHandler MouseLeftButtonDown;

        /// <summary>
        ///     An event reporting the left mouse button was released.
        /// </summary>
        event MouseEventHandler MouseLeftButtonUp;

        /// <summary>
        ///     An event reporting the right mouse button was pressed.
        /// </summary>
        event MouseEventHandler MouseRightButtonDown;

        /// <summary>
        ///     An event reporting the right mouse button was released.
        /// </summary>
        event MouseEventHandler MouseRightButtonUp;

        /// <summary>
        ///     An event reporting a mouse button was pressed.
        /// </summary>
        event MouseEventHandler MouseDown;

        /// <summary>
        ///     An event reporting a mouse button was released.
        /// </summary>
        event MouseEventHandler MouseUp;

        /// <summary>
        ///     An event reporting a mouse move.
        /// </summary>
        event MouseEventHandler MouseMove;

        /// <summary>
        ///     An event reporting a mouse wheel rotation.
        /// </summary>
        event MouseEventHandler MouseWheel;

        /// <summary>
        ///     A property indicating if the element is enabled or not.
        /// </summary>
        bool IsEnabled { get; }

        #endregion Input
    }
}

