#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Alternet.UI.Threading;
using System;
using System.Collections;
using System.Security;

#pragma warning disable 1634, 1691  // suppressing PreSharp warnings

namespace Alternet.UI
{
    /// <summary>
    ///     The KeyboardDevice class represents the mouse device to the
    ///     members of a context.
    /// </summary>
    public abstract class KeyboardDevice : InputDevice
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected KeyboardDevice(InputManager inputManager)
        {
        }

        /// <summary>
        ///     Gets the current state of the specified key from the device from
        ///     the underlying system
        /// </summary>
        /// <param name="key">
        ///     Key to get the state of
        /// </param>
        /// <returns>
        ///     The state of the specified key
        /// </returns>
        protected abstract KeyStates GetKeyStatesFromSystem(Key key);

        /// <summary>
        ///     Returns the set of modifier keys currently pressed as determined
        ///     by querying our keyboard state cache
        /// </summary>
        public ModifierKeys Modifiers
        {
            get
            {
                ModifierKeys modifiers = ModifierKeys.None;
                if (IsKeyDown_private(Key.Alt))
                {
                    modifiers |= ModifierKeys.Alt;
                }

                if (IsKeyDown_private(Key.Control))
                {
                    modifiers |= ModifierKeys.Control;
                }

                if (IsKeyDown_private(Key.Shift))
                {
                    modifiers |= ModifierKeys.Shift;
                }

                if (IsKeyDown_private(Key.Windows))
                {
                    modifiers |= ModifierKeys.Windows;
                }

                return modifiers;
            }
        }

        /// <summary>
        ///     Returns the set of raw modifier keys currently pressed as determined by querying our keyboard state cache
        /// </summary>
        public RawModifierKeys RawModifiers
        {
            get
            {
                RawModifierKeys modifiers = RawModifierKeys.None;
                if (IsKeyDown_private(Key.Alt))
                {
                    modifiers |= RawModifierKeys.Alt;
                }
                if (IsKeyDown_private(Key.Control))
                {
                    modifiers |= RawModifierKeys.Control;
                }
                if (IsKeyDown_private(Key.Shift))
                {
                    modifiers |= RawModifierKeys.Shift;
                }
                if (IsKeyDown_private(Key.Windows))
                {
                    modifiers |= RawModifierKeys.Windows;
                }
                if (IsKeyDown_private(Key.MacCommand))
                {
                    modifiers |= RawModifierKeys.MacCommand;
                }

                if (IsKeyDown_private(Key.MacOption))
                {
                    modifiers |= RawModifierKeys.MacOption;
                }

                if (IsKeyDown_private(Key.MacControl))
                {
                    modifiers |= RawModifierKeys.MacControl;
                }

                return modifiers;
            }
        }

        /// <summary>
        /// There is a proscription against using Enum.IsDefined().  (it is slow)
        /// so we write these PRIVATE validate routines instead.
        /// </summary>
        private bool ValidateKey(Key key)
        {
            if (256 <= (int)key || (int)key <= 0)
                return false;
            return true;
                /*throw new System.ComponentModel.InvalidEnumArgumentException("key", (int)key, typeof(Key));*/
        }

        /// <summary>
        /// This is the core private method that returns whether or not the specified key
        ///  is down.  It does it without the extra argument validation and context checks.
        /// </summary>
        private bool IsKeyDown_private(Key key)
        {
            return ((GetKeyStatesFromSystem(key) & KeyStates.Down) == KeyStates.Down);
        }

        /// <summary>
        ///     Returns whether or not the specified key is down.
        /// </summary>
        public bool IsKeyDown(Key key)
        {
            if (!ValidateKey(key))
                return false;
            return IsKeyDown_private(key);
        }

        /// <summary>
        ///     Returns whether or not the specified key is up.
        /// </summary>
        public bool IsKeyUp(Key key)
        {
            if (!ValidateKey(key))
                return false;
            return (!IsKeyDown_private(key));
        }

        /// <summary>
        ///     Returns whether or not the specified key is toggled.
        /// </summary>
        public bool IsKeyToggled(Key key)
        {
            if (!ValidateKey(key))
                return false;
            return ((GetKeyStatesFromSystem(key) & KeyStates.Toggled) == KeyStates.Toggled);
        }

        /// <summary>
        ///     Returns the state of the specified key.
        /// </summary>
        public KeyStates GetKeyStates(Key key)
        {
            if (!ValidateKey(key))
                return KeyStates.None;
            return GetKeyStatesFromSystem(key);
        }
   }
}