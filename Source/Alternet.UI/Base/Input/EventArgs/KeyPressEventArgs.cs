#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using System;
using System.Security; 

namespace Alternet.UI
{
    /// <summary>
    /// Represents the method that will handle the <see cref="UIElement.KeyPress" /> event
    /// of a <see cref="Control" />.</summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="KeyPressEventArgs" /> that contains the event data.</param>
    public delegate void KeyPressEventHandler(object? sender, KeyPressEventArgs e);

    /// <summary>
    /// This class is used in the <see cref="UIElement.KeyPress"/> event as EventArgs.
    /// </summary>
    public class KeyPressEventArgs : KeyboardEventArgs
    {
        private char keyChar;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyPressEventArgs"/> class.
        /// </summary>
        public KeyPressEventArgs(KeyboardDevice keyboard, long timestamp, char keyChar)
            : base(keyboard, timestamp)
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

        /// <summary>
        ///     The mechanism used to call the type-specific handler on the
        ///     target.
        /// </summary>
        /// <param name="genericHandler">
        ///     The generic handler to call in a type-specific way.
        /// </param>
        /// <param name="genericTarget">
        ///     The target to call the handler on.
        /// </param>
        /// <ExternalAPI/> 
        protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
        {
            KeyPressEventHandler handler = (KeyPressEventHandler) genericHandler;
            
            handler(genericTarget, this);
        }
    }
}

