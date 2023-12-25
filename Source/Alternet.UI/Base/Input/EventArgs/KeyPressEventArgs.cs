#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


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
        private char keyChar;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyPressEventArgs"/> class.
        /// </summary>
        public KeyPressEventArgs(KeyboardDevice keyboard, char keyChar)
            : base(keyboard)
        {
            this.keyChar = keyChar;
        }

        /// <summary>
        ///     The Key referenced by the event, if the key is not being 
        ///     handled specially.
        /// </summary>
        public char KeyChar
        {
            get {return keyChar;}
        }
    }
}

