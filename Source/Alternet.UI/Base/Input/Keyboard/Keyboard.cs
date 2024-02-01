#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the keyboard device.
    /// </summary>
    public static class Keyboard
    {
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
    }
}

