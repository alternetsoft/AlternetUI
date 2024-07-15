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
        private ModifierKeys modifiers;
        private Key key;
        private KeyStates keyStates;
        private Keys? keyData;
        private bool suppressKeyPress;
        private uint repeatCount;

        /// <summary>
        /// Constructs an instance of the <see cref="KeyEventArgs"/> class.
        /// </summary>
        public KeyEventArgs()
        {
        }

        /// <summary>
        /// Constructs an instance of the <see cref="KeyEventArgs"/> class.
        /// </summary>
        /// <param name="key">
        /// The key referenced by the event.
        /// </param>
        /// <param name="repeatCount">Number of repeated key presses.</param>
        /// <param name="originalTarget"></param>
        public KeyEventArgs(object originalTarget, Key key, uint repeatCount)
            : base(originalTarget)
        {
            this.key = key;
            this.repeatCount = repeatCount;
            keyStates = Keyboard.GetKeyStates(key);
            modifiers = Keyboard.Modifiers;
        }

        /// <summary>
        /// The Key referenced by the event, if the key is not being handled specially.
        /// </summary>
        public virtual Key Key
        {
            get
            {
                return key;
            }

            set
            {
                key = value;
                keyData = null;
            }
        }

        /// <summary>
        /// Gets the modifier flags for a <see cref="Control.KeyDown" /> event.
        /// The flags indicate which combination of CTRL, SHIFT, and ALT keys was pressed.
        /// </summary>
        /// <returns>A <see cref="Keys" /> value representing one or more modifier flags.</returns>
        public virtual Keys Modifiers => KeyData & Keys.Modifiers;

        /// <summary>
        /// Returns the set of modifier keys currently pressed.
        /// </summary>
        public virtual ModifierKeys ModifierKeys
        {
            get
            {
                return modifiers;
            }

            set
            {
                modifiers = value;
                keyData = null;
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
        public virtual bool SuppressKeyPress
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
        /// Contains key code for the key that was pressed
        /// without modifier flags.
        /// </summary>
        /// <returns>
        /// A <see cref="Keys" /> value that is the key code for the event.
        /// </returns>
        public virtual Keys KeyCode
        {
            get
            {
                Keys keys = KeyData & Keys.KeyCode;
                if (keys < 0 || keys > Keys.OemClear)
                {
                    return Keys.None;
                }

                return keys;
            }
        }

        /// <summary>Gets the keyboard value for a <see cref="Control.KeyDown" /> event.</summary>
        /// <returns>The integer representation of the
        /// <see cref="KeyEventArgs.KeyCode" /> property.</returns>
        public virtual int KeyValue => (int)(KeyData & Keys.KeyCode);

        /// <summary>
        /// Gets the key data for a <see cref="Control.KeyDown"/> event.
        /// Contains key code for the key that was pressed, combined
        /// with modifier flags that indicate which combination
        /// of CTRL, SHIFT, and ALT keys was pressed at the same time.
        /// </summary>
        /// <returns>
        /// A <see cref="Keys"/> representing
        /// the key code for the key that was pressed, combined
        /// with modifier flags that indicate which combination
        /// of CTRL, SHIFT, and ALT keys was pressed at the same time.
        /// </returns>
        public virtual Keys KeyData => keyData ??= key.ToKeys(modifiers);

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
        public virtual KeyStates KeyStates
        {
            get
            {
                return keyStates;
            }

            set
            {
                keyStates = value;
                keyData = null;
            }
        }

        /// <summary>
        /// Gets or sets key repeat count.
        /// </summary>
        public virtual uint RepeatCount
        {
            get => repeatCount;
            set => repeatCount = value;
        }

        /// <summary>
        /// Whether the key pressed is a repeated key or not.
        /// </summary>
        public virtual bool IsRepeat
        {
            get
            {
                return repeatCount > 0;
            }
        }

        /// <summary>
        /// Whether or not the key referenced by the event is down.
        /// </summary>
        public virtual bool IsDown => keyStates == KeyStates.Down;

        /// <summary>
        /// Whether or not the key referenced by the event is up.
        /// </summary>
        public virtual bool IsUp => !IsDown;

        /// <summary>
        /// Whether or not the key referenced by the event is toggled.
        /// </summary>
        public virtual bool IsToggled => keyStates == KeyStates.Toggled;

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            KeyInfo keyInfo = new(Key, ModifierKeys);
            return $"{{{base.ToString()}: {keyInfo}}}";
        }
    }
}