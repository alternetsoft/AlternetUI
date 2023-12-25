#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Alternet.UI
{
    /// <summary>
    ///     The Keyboard class represents the mouse device to the
    ///     members of a context.
    /// </summary>
    /// <remarks>
    ///     The static members of this class simply delegate to the primary
    ///     keyboard device of the calling thread's input manager.
    /// </remarks>
    public static class Keyboard
    {
        /// <summary>
        ///     KeyDown
        /// </summary>
        public static readonly RoutedEvent KeyDownEvent = EventManager.RegisterRoutedEvent("KeyDown", RoutingStrategy.Bubble, typeof(KeyEventHandler), typeof(Keyboard));

        /*/// <summary>
        ///     TextInput
        /// </summary>
        public static readonly RoutedEvent KeyPressEvent
            = EventManager.RegisterRoutedEvent(
                "KeyPress",
                RoutingStrategy.Bubble,
                typeof(KeyPressEventHandler),
                typeof(Keyboard));*/

        /// <summary>
        ///     KeyUp
        /// </summary>
        public static readonly RoutedEvent KeyUpEvent = EventManager.RegisterRoutedEvent("KeyUp", RoutingStrategy.Bubble, typeof(KeyEventHandler), typeof(Keyboard));

        /// <summary>
        ///     The set of modifier keys currently pressed.
        /// </summary>
        public static ModifierKeys Modifiers
        {
            get
            {
                return Keyboard.PrimaryDevice.Modifiers;
            }
        }

        /// <summary>
        ///     The set of raw modifier keys currently pressed.
        /// </summary>
        public static RawModifierKeys RawModifiers
        {
            get
            {
                return Keyboard.PrimaryDevice.RawModifiers;
            }
        }

        /// <summary>
        ///     Returns whether or not the specified key is down.
        /// </summary>
        public static bool IsKeyDown(Key key)
        {
            return Keyboard.PrimaryDevice.IsKeyDown(key);
        }

        /// <summary>
        ///     Returns whether or not the specified key is up.
        /// </summary>
        public static bool IsKeyUp(Key key)
        {
            return Keyboard.PrimaryDevice.IsKeyUp(key);
        }

        /// <summary>
        ///     Returns whether or not the specified key is toggled.
        /// </summary>
        public static bool IsKeyToggled(Key key)
        {
            return Keyboard.PrimaryDevice.IsKeyToggled(key);
        }

        /// <summary>
        ///     Returns the state of the specified key.
        /// </summary>
        public static KeyStates GetKeyStates(Key key)
        {
            return Keyboard.PrimaryDevice.GetKeyStates(key);
        }

        /// <summary>
        ///     The primary keyboard device.
        /// </summary>
        public static KeyboardDevice PrimaryDevice
        {
            get
            {
                KeyboardDevice keyboardDevice = InputManager.UnsecureCurrent.PrimaryKeyboardDevice;
                return keyboardDevice;
            }
        }

        // Check for Valid enum, as any int can be casted to the enum.
        internal static bool IsValidKey(Key key)
        {
            return ((int)key >= (int)Key.None/* && (int)key <= (int)Key.OemClear*/);
        }

        /*internal static bool IsFocusable(DependencyObject element)
        {
            // This should really be its own property, but it is hard to do efficiently.
            if (element == null)
            {
                return false;
            }

            var c = element as Control;
            if (c != null)
            {
                if (!c.Visible || !c.Enabled)
                    return false;
            }

            // There are too many conflicting desires for whether or not
            // an element is focusable.  We need to differentiate between
            // a false default value, and the user specifying false
            // explicitly.
            bool hasModifiers = false;
            BaseValueSourceInternal valueSource = element.GetValueSource(UIElement.FocusableProperty, null, out hasModifiers);
            bool focusable = (bool) element.GetValue(UIElement.FocusableProperty);

            if(!focusable && valueSource == BaseValueSourceInternal.Default && !hasModifiers)
            {
                // The Focusable property was not explicitly set to anything.
                // The default value is generally false, but true in a few cases.

                if(FocusManager.GetIsFocusScope(element))
                {
                    // Focus scopes are considered focusable, even if
                    // the Focusable property is false.
                    return true;
                }
                else if(c != null && c.Parent == null)
                {
                    if (Window.GetParentWindow(c) != null) // yezo
                    {
                        // A UIElements that is the root of a PresentationSource is considered focusable.
                        return true;
                    }
                }
            }

            return focusable;
        }*/
    }
}

