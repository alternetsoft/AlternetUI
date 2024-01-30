// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.



using Alternet.Drawing;
using System;
using System.Security;

namespace Alternet.UI
{
    /// <summary>
    ///     The Mouse class represents the mouse device to the
    ///     members of a context.
    /// </summary>
    /// <remarks>
    ///     The static members of this class simply delegate to the primary
    ///     mouse device of the calling thread's input manager.
    /// </remarks>
    public static class Mouse
    {
        /// <summary>
        ///     MouseMove
        /// </summary>
        public static readonly RoutedEvent MouseMoveEvent = EventManager.RegisterRoutedEvent("MouseMove", RoutingStrategy.Bubble, typeof(MouseEventHandler), typeof(Mouse));

        /// <summary>
        ///     Adds a handler for the MouseMove attached event
        /// </summary>
        /// <param name="element">UIElement or ContentElement that listens to this event</param>
        /// <param name="handler">Event Handler to be added</param>
        public static void AddMouseMoveHandler(DependencyObject element, MouseEventHandler handler)
        {
            UIElement.AddHandler(element, MouseMoveEvent, handler);
        }

        /// <summary>
        ///     Removes a handler for the MouseMove attached event
        /// </summary>
        /// <param name="element">UIElement or ContentElement that removedto this event</param>
        /// <param name="handler">Event Handler to be removed</param>
        public static void RemoveMouseMoveHandler(DependencyObject element, MouseEventHandler handler)
        {
            UIElement.RemoveHandler(element, MouseMoveEvent, handler);
        }

        /// <summary>
        ///     MouseDown
        /// </summary>
        public static readonly RoutedEvent MouseDownEvent
            = EventManager.RegisterRoutedEvent("MouseDown", RoutingStrategy.Bubble, typeof(MouseEventHandler), typeof(Mouse));

        /// <summary>
        ///     Adds a handler for the MouseDown attached event
        /// </summary>
        /// <param name="element">UIElement or ContentElement that listens to this event</param>
        /// <param name="handler">Event Handler to be added</param>
        public static void AddMouseDownHandler(DependencyObject element, MouseEventHandler handler)
        {
            UIElement.AddHandler(element, MouseDownEvent, handler);
        }

        /// <summary>
        ///     Removes a handler for the MouseDown attached event
        /// </summary>
        /// <param name="element">UIElement or ContentElement that listens to this event</param>
        /// <param name="handler">Event Handler to be removed</param>
        public static void RemoveMouseDownHandler(DependencyObject element, MouseEventHandler handler)
        {
            UIElement.RemoveHandler(element, MouseDownEvent, handler);
        }

        /// <summary>
        ///     MouseDoubleClick
        /// </summary>
        public static readonly RoutedEvent MouseDoubleClickEvent
            = EventManager.RegisterRoutedEvent("MouseDoubleClick", RoutingStrategy.Bubble, typeof(MouseEventHandler), typeof(Mouse));

        /// <summary>
        ///     Adds a handler for the MouseDoubleClick attached event
        /// </summary>
        /// <param name="element">UIElement or ContentElement that listens to this event</param>
        /// <param name="handler">Event Handler to be added</param>
        public static void AddMouseDoubleClickHandler(DependencyObject element, MouseEventHandler handler)
        {
            UIElement.AddHandler(element, MouseDoubleClickEvent, handler);
        }

        /// <summary>
        ///     Removes a handler for the MouseDoubleClick attached event
        /// </summary>
        /// <param name="element">UIElement or ContentElement that listens to this event</param>
        /// <param name="handler">Event Handler to be removed</param>
        public static void RemoveMouseDoubleClickHandler(DependencyObject element, MouseEventHandler handler)
        {
            UIElement.RemoveHandler(element, MouseDoubleClickEvent, handler);
        }

        /// <summary>
        ///     MouseUp
        /// </summary>
        public static readonly RoutedEvent MouseUpEvent
            = EventManager.RegisterRoutedEvent("MouseUp", RoutingStrategy.Bubble, typeof(MouseEventHandler), typeof(Mouse));

        /// <summary>
        ///     Adds a handler for the MouseUp attached event
        /// </summary>
        /// <param name="element">UIElement or ContentElement that listens to this event</param>
        /// <param name="handler">Event Handler to be added</param>
        public static void AddMouseUpHandler(DependencyObject element, MouseEventHandler handler)
        {
            UIElement.AddHandler(element, MouseUpEvent, handler);
        }

        /// <summary>
        ///     Removes a handler for the MouseUp attached event
        /// </summary>
        /// <param name="element">UIElement or ContentElement that listens to this event</param>
        /// <param name="handler">Event Handler to be removed</param>
        public static void RemoveMouseUpHandler(DependencyObject element, MouseEventHandler handler)
        {
            UIElement.RemoveHandler(element, MouseUpEvent, handler);
        }

        /// <summary>
        ///     MouseWheel
        /// </summary>
        public static readonly RoutedEvent MouseWheelEvent
            = EventManager.RegisterRoutedEvent("MouseWheel", RoutingStrategy.Bubble, typeof(MouseEventHandler), typeof(Mouse));

        /// <summary>
        ///     Adds a handler for the MouseWheel attached event
        /// </summary>
        /// <param name="element">UIElement or ContentElement that listens to this event</param>
        /// <param name="handler">Event Handler to be added</param>
        public static void AddMouseWheelHandler(DependencyObject element, MouseEventHandler handler)
        {
            UIElement.AddHandler(element, MouseWheelEvent, handler);
        }

        /// <summary>
        ///     Removes a handler for the MouseWheel attached event
        /// </summary>
        /// <param name="element">UIElement or ContentElement that listens to this event</param>
        /// <param name="handler">Event Handler to be removed</param>
        public static void RemoveMouseWheelHandler(DependencyObject element, MouseEventHandler handler)
        {
            UIElement.RemoveHandler(element, MouseWheelEvent, handler);
        }

        /// <summary>
        ///     MouseEnter
        /// </summary>
        public static readonly RoutedEvent MouseEnterEvent = EventManager.RegisterRoutedEvent("MouseEnter", RoutingStrategy.Direct, typeof(MouseEventHandler), typeof(Mouse));

        /// <summary>
        ///     Adds a handler for the MouseEnter attached event
        /// </summary>
        /// <param name="element">UIElement or ContentElement that listens to this event</param>
        /// <param name="handler">Event Handler to be added</param>
        public static void AddMouseEnterHandler(DependencyObject element, MouseEventHandler handler)
        {
            UIElement.AddHandler(element, MouseEnterEvent, handler);
        }

        /// <summary>
        ///     Removes a handler for the MouseEnter attached event
        /// </summary>
        /// <param name="element">UIElement or ContentElement that listens to this event</param>
        /// <param name="handler">Event Handler to be removed</param>
        public static void RemoveMouseEnterHandler(DependencyObject element, MouseEventHandler handler)
        {
            UIElement.RemoveHandler(element, MouseEnterEvent, handler);
        }

        /// <summary>
        ///     MouseLeave
        /// </summary>
        public static readonly RoutedEvent MouseLeaveEvent = EventManager.RegisterRoutedEvent("MouseLeave", RoutingStrategy.Direct, typeof(MouseEventHandler), typeof(Mouse));

        /// <summary>
        ///     Adds a handler for the MouseLeave attached event
        /// </summary>
        /// <param name="element">UIElement or ContentElement that listens to this event</param>
        /// <param name="handler">Event Handler to be added</param>
        public static void AddMouseLeaveHandler(DependencyObject element, MouseEventHandler handler)
        {
            UIElement.AddHandler(element, MouseLeaveEvent, handler);
        }

        /// <summary>
        ///     Removes a handler for the MouseLeave attached event
        /// </summary>
        /// <param name="element">UIElement or ContentElement that listens to this event</param>
        /// <param name="handler">Event Handler to be removed</param>
        public static void RemoveMouseLeaveHandler(DependencyObject element, MouseEventHandler handler)
        {
            UIElement.RemoveHandler(element, MouseLeaveEvent, handler);
        }

        /// <summary>
        ///     The state of the left button.
        /// </summary>
        public static MouseButtonState LeftButton
        {
            get
            {
                return Mouse.PrimaryDevice.LeftButton;
            }
        }

        /// <summary>
        ///     The state of the right button.
        /// </summary>
        public static MouseButtonState RightButton
        {
            get
            {
                return Mouse.PrimaryDevice.RightButton;
            }
        }

        /// <summary>
        ///     The state of the middle button.
        /// </summary>
        public static MouseButtonState MiddleButton
        {
            get
            {
                return Mouse.PrimaryDevice.MiddleButton;
            }
        }

        /// <summary>
        ///     The state of the first extended button.
        /// </summary>
        public static MouseButtonState XButton1
        {
            get
            {
                return Mouse.PrimaryDevice.XButton1;
            }
        }

        /// <summary>
        ///     The state of the second extended button.
        /// </summary>
        public static MouseButtonState XButton2
        {
            get
            {
                return Mouse.PrimaryDevice.XButton2;
            }
        }

        /// <summary>
        ///     Calculates the position of the mouse relative to
        ///     a particular element.
        /// </summary>
        public static PointD GetPosition(Control relativeTo)
        {
            return Mouse.PrimaryDevice.GetPosition(relativeTo);
        }

        /// <summary>
        ///     The number of units the mouse wheel should be rotated to scroll one line.
        /// </summary>
        /// <remarks>
        ///     The delta was set to 120 to allow Microsoft or other vendors to
        ///     build finer-resolution wheels in the future, including perhaps
        ///     a freely-rotating wheel with no notches. The expectation is
        ///     that such a device would send more messages per rotation, but
        ///     with a smaller value in each message. To support this
        ///     possibility, you should either add the incoming delta values
        ///     until MouseWheelDeltaForOneLine amount is reached (so for a
        ///     delta-rotation you get the same response), or scroll partial
        ///     lines in response to the more frequent messages. You could also
        ///     choose your scroll granularity and accumulate deltas until it
        ///     is reached.
        /// </remarks>
        public const int MouseWheelDeltaForOneLine = 120;

        /// <summary>
        ///     The primary mouse device.
        /// </summary>
        public static MouseDevice PrimaryDevice
        {
            get
            {
                MouseDevice mouseDevice;
                //there is a link demand on the Current property
                mouseDevice = InputManager.UnsecureCurrent.PrimaryMouseDevice;
                return mouseDevice;
            }
        }
    }
}
