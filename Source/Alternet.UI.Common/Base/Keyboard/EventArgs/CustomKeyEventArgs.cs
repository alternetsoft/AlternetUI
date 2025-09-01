using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Base class for <see cref="KeyEventArgs"/> and <see cref="PreviewKeyDownEventArgs"/> classes.
    /// </summary>
    public class CustomKeyEventArgs : KeyboardEventArgs
    {
        private ModifierKeys modifiers;
        private Key key;
        private Keys? keyData;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomKeyEventArgs"/> class.
        /// </summary>
        public CustomKeyEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomKeyEventArgs"/> class
        /// with the specified original target object.
        /// </summary>
        /// <param name="key">
        /// The key referenced by the event.
        /// </param>
        /// <param name="modifiers">The set of modifier keys pressed
        /// at the time when event was received.</param>
        /// <param name="originalTarget">Original target object which received the event.</param>
        public CustomKeyEventArgs(object originalTarget, Key key, ModifierKeys modifiers)
            : base(originalTarget)
        {
            this.key = key;
            this.modifiers = modifiers;
        }

        /// <summary>
        /// Gets or sets the key referenced by the event.
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
        /// Gets the modifier flags for a <see cref="AbstractControl.KeyDown" /> event.
        /// The flags indicate which combination of CTRL, SHIFT, and ALT keys was pressed.
        /// </summary>
        /// <returns>A <see cref="Keys" /> value representing one or more modifier flags.</returns>
        public virtual Keys Modifiers => KeyData & Keys.Modifiers;

        /// <summary>
        /// Gets or sets the set of modifier keys pressed at the time when event was received.
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
        /// Gets the keyboard code for a <see cref="AbstractControl.KeyDown"/> event.
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

        /// <summary>Gets the keyboard value for
        /// a <see cref="AbstractControl.KeyDown" /> event.</summary>
        /// <returns>The integer representation of the
        /// <see cref="KeyCode" /> property.</returns>
        public virtual int KeyValue => (int)(KeyData & Keys.KeyCode);

        /// <summary>
        /// Gets the key data for a <see cref="AbstractControl.KeyDown"/> event.
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
        /// Gets whether any modifiers are pressed.
        /// </summary>
        public virtual bool HasModifiers => modifiers != ModifierKeys.None;

        /// <summary>
        /// Gets whether CTRL and SHIFT keys are pressed.
        /// </summary>
        public bool ControlAndShift => Control && Shift;

        /// <summary>
        /// Get whether <see cref="Key"/> is plus or numpad plus character.
        /// </summary>
        public virtual bool IsAnyPlusKey => Key == Key.PlusSign || Key == Key.NumPadPlus;

        /// <summary>
        /// Get whether <see cref="Key"/> is minus or numpad minus character.
        /// </summary>
        public virtual bool IsAnyMinusKey => Key == Key.Minus || Key == Key.NumPadMinus;

        /// <summary>
        /// Gets a value indicating whether the SHIFT key was pressed.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the SHIFT key was pressed;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public virtual bool Shift => modifiers.HasFlag(ModifierKeys.Shift);

        /// <summary>
        /// Gets a value indicating whether the SHIFT key was pressed or
        /// no other modifiers were pressed.
        /// </summary>
        public virtual bool ShiftOrNone => Shift || !HasModifiers;

        /// <summary>
        /// Gets whether 'Escape' key is pressed without modifiers
        /// (<see cref="ModifierKeys"/> is empty).
        /// </summary>
        public virtual bool IsEscape => IsSimpleKey(Key.Escape);

        /// <summary>
        /// Gets whether 'Enter' key is pressed without modifiers
        /// (<see cref="ModifierKeys"/> is empty).
        /// </summary>
        public virtual bool IsEnter => IsSimpleKey(Key.Enter);

        /// <summary>
        /// Gets whether the specified key is pressed and <see cref="ModifierKeys"/> is empty.
        /// </summary>
        /// <param name="key">Key to test.</param>
        /// <returns></returns>
        public virtual bool IsSimpleKey(Key key)
        {
            return Key == key && ModifierKeys == UI.ModifierKeys.None;
        }

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
