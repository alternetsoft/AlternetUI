using System;
using System.Security;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the method that will handle the <see cref="Control.KeyPress" /> event
    /// of a <see cref="Control" />.</summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="KeyPressEventArgs" /> that contains the event data.</param>
    public delegate void KeyPressEventHandler(object? sender, KeyPressEventArgs e);

    /// <summary>
    /// This class is used in the <see cref="Control.KeyPress"/> event as EventArgs.
    /// </summary>
    public class KeyPressEventArgs : KeyboardEventArgs
    {
        private readonly char keyChar;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyPressEventArgs"/> class.
        /// </summary>
        public KeyPressEventArgs(
            object originalTarget,
            char keyChar,
            KeyboardDevice keyboardDevice)
            : base(originalTarget, keyboardDevice)
        {
            this.keyChar = keyChar;
        }

        /// <summary>
        ///     The Key referenced by the event.
        /// </summary>
        public char KeyChar
        {
            get { return keyChar; }
        }
    }
}