using System;
using System.Security;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the method that will handle the <see cref="AbstractControl.KeyPress" /> event
    /// of a <see cref="AbstractControl" />.</summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="KeyPressEventArgs" /> that contains the event data.</param>
    public delegate void KeyPressEventHandler(object? sender, KeyPressEventArgs e);

    /// <summary>
    /// This class is used in the <see cref="AbstractControl.KeyPress"/> event as event arguments.
    /// </summary>
    public class KeyPressEventArgs : KeyboardEventArgs
    {
        private char keyChar;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyPressEventArgs"/> class with the specified key character.
        /// </summary>
        public KeyPressEventArgs(char keyChar)
        {
            this.keyChar = keyChar;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyPressEventArgs"/> class with the specified key character
        /// and original target.
        /// </summary>
        /// <param name="originalTarget">Original target object which received the event.</param>
        /// <param name="keyChar">The character referenced by the event.</param>
        public KeyPressEventArgs(object originalTarget, char keyChar)
            : base(originalTarget)
        {
            this.keyChar = keyChar;
        }

        /// <summary>
        /// Gets or sets the character referenced by the event.
        /// </summary>
        public virtual char KeyChar
        {
            get
            {
                return keyChar;
            }

            set
            {
                keyChar = value;
            }
        }
    }
}