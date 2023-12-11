#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using System;
using System.ComponentModel;
using System.Security; 

namespace Alternet.UI
{
    /// <summary>
    ///     The KeyEventArgs class contains information about key states.
    /// </summary>
    public class KeyEventArgs : KeyboardEventArgs
    {
        private readonly ModifierKeys modifiers;
        private readonly Keys keyData;
        private readonly Key key;
        private readonly bool isRepeat;
        private readonly KeyStates keyStates;

        /// <summary>
        /// Constructs an instance of the KeyEventArgs class.
        /// </summary>
        /// <param name="keyboard">
        /// The logical keyboard device associated with this event.
        /// </param>
        /// <param name="timestamp">
        /// The time when the input occurred.
        /// </param>
        /// <param name="key">
        /// The key referenced by the event.
        /// </param>
        /// <param name="isRepeat">Whether the key pressed is a repeated key or not.</param>
        public KeyEventArgs(KeyboardDevice keyboard, long timestamp, Key key, bool isRepeat)
            : base(keyboard, timestamp)
        {
            if (!Keyboard.IsValidKey(key))
                throw new InvalidEnumArgumentException(nameof(key), (int)key, typeof(Key));
            this.key = key;
            this.isRepeat = isRepeat;
            modifiers = KeyboardDevice.Modifiers;
            keyStates = this.KeyboardDevice.GetKeyStates(key);
            keyData = key.ToKeys(modifiers);
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
        /// Gets the modifier flags for a <see cref="UIElement.KeyDown" /> event.
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
        /// Gets the keyboard code for a <see cref="UIElement.KeyDown"/> event.
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

        /// <summary>Gets the keyboard value for a <see cref="UIElement.KeyDown" /> event.</summary>
        /// <returns>The integer representation of the
        /// <see cref="KeyEventArgs.KeyCode" /> property.</returns>
        public int KeyValue => (int)(KeyData & Keys.KeyCode);

        /// <summary>
        /// Gets the key data for a <see cref="UIElement.KeyDown"/> event.
        /// </summary>
        /// <returns>
        /// A <see cref="Keys"/> representing
        /// the key code for the key that was pressed, combined
        /// with modifier flags that indicate which combination
        /// of CTRL, SHIFT, and ALT keys was pressed at the same time.
        /// </returns>
        public Keys KeyData => keyData;

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

        /// <summary>
        /// The mechanism used to call the type-specific handler on the target.
        /// </summary>
        /// <param name="genericHandler">
        /// The generic handler to call in a type-specific way.
        /// </param>
        /// <param name="genericTarget">The target to call the handler on.</param>
        protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
        {
            KeyEventHandler handler = (KeyEventHandler) genericHandler;
            
            handler(genericTarget, this);
        }
    }
}

