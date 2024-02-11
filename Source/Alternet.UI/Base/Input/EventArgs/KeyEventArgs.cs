// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI
{
    /// <summary>
    ///     The delegate to use for handlers that receive KeyboardEventArgs.
    /// </summary>
    /// <ExternalAPI Inherit="true"/>
    public delegate void KeyEventHandler(object sender, KeyEventArgs e);

    /// <summary>
    ///     The KeyEventArgs class contains information about key states.
    /// </summary>
    public class KeyEventArgs : KeyboardEventArgs
    {
        private readonly ModifierKeys modifiers;
        private readonly Key key;
        private readonly bool isRepeat;
        private readonly KeyStates keyStates;
        private Keys? keyData;
        private bool suppressKeyPress;

        /// <summary>
        /// Constructs an instance of the KeyEventArgs class.
        /// </summary>
        /// <param name="key">
        /// The key referenced by the event.
        /// </param>
        /// <param name="isRepeat">Whether the key pressed is a repeated key or not.</param>
        /// <param name="originalTarget"></param>
        internal KeyEventArgs(
            Control originalTarget,
            Key key,
            bool isRepeat)
            : base(originalTarget)
        {
            this.key = key;
            this.isRepeat = isRepeat;
            keyStates = this.KeyboardDevice.GetKeyStates(key);
            modifiers = KeyboardDevice.Modifiers;
        }

        /// <summary>
        /// The Key referenced by the event, if the key is not being handled specially.
        /// </summary>
        public Key Key
        {
            get
            {
                return key;
            }
        }

        /// <summary>
        /// Gets the modifier flags for a <see cref="Control.KeyDown" /> event.
        /// The flags indicate which combination of CTRL, SHIFT, and ALT keys was pressed.
        /// </summary>
        /// <returns>A <see cref="Keys" /> value representing one or more modifier flags.</returns>
        public Keys Modifiers => KeyData & Keys.Modifiers;

        /// <summary>
        /// Returns the set of modifier keys currently pressed.
        /// </summary>
        public ModifierKeys ModifierKeys
        {
            get
            {
                return modifiers;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the key event should be passed on to
        /// the underlying control.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the key event should not be sent
        /// to the control; otherwise, <see langword="false" />.
        /// </returns>
        public bool SuppressKeyPress
        {
            get
            {
                return suppressKeyPress;
            }

            set
            {
                suppressKeyPress = value;
                Handled = value;
            }
        }

        /// <summary>
        /// Gets the keyboard code for a <see cref="Control.KeyDown"/> event.
        /// </summary>
        /// <returns>
        /// A <see cref="Keys" /> value that is the key code for the event.
        /// </returns>
        public Keys KeyCode
        {
            get
            {
                Keys keys = KeyData & Keys.KeyCode;
                if (!Enum.IsDefined(typeof(Keys), (int)keys))
                {
                    return Keys.None;
                }

                return keys;
            }
        }

        /// <summary>Gets the keyboard value for a <see cref="Control.KeyDown" /> event.</summary>
        /// <returns>The integer representation of the
        /// <see cref="KeyEventArgs.KeyCode" /> property.</returns>
        public int KeyValue => (int)(KeyData & Keys.KeyCode);

        /// <summary>
        /// Gets the key data for a <see cref="Control.KeyDown"/> event.
        /// </summary>
        /// <returns>
        /// A <see cref="Keys"/> representing
        /// the key code for the key that was pressed, combined
        /// with modifier flags that indicate which combination
        /// of CTRL, SHIFT, and ALT keys was pressed at the same time.
        /// </returns>
        public Keys KeyData => keyData ??= key.ToKeys(modifiers);

        /// <summary>
        /// Gets a value indicating whether the ALT key was pressed.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the ALT key was pressed;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public virtual bool Alt => modifiers.HasFlag(ModifierKeys.Alt);

        /// <summary>
        /// Gets a value indicating whether the CTRL key was pressed.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the CTRL key was pressed;
        /// otherwise, <see langword="false" />.
        /// </returns>
        public virtual bool Control => modifiers.HasFlag(ModifierKeys.Control);

        /// <summary>
        /// Gets a value indicating whether the SHIFT key was pressed.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the SHIFT key was pressed;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public virtual bool Shift => modifiers.HasFlag(ModifierKeys.Shift);

        /// <summary>
        /// The state of the key referenced by the event.
        /// </summary>
        public KeyStates KeyStates
        {
            get
            {
                return keyStates;
            }
        }

        /// <summary>
        /// Whether the key pressed is a repeated key or not.
        /// </summary>
        public bool IsRepeat
        {
            get
            {
                return isRepeat;
            }
        }

        /// <summary>
        /// Whether or not the key referenced by the event is down.
        /// </summary>
        public bool IsDown => keyStates == KeyStates.Down;

        /// <summary>
        /// Whether or not the key referenced by the event is up.
        /// </summary>
        public bool IsUp => !IsDown;

        /// <summary>
        /// Whether or not the key referenced by the event is toggled.
        /// </summary>
        public bool IsToggled => keyStates == KeyStates.Toggled;
    }
}