#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Windows;
using System.Security;

using System;

namespace Alternet.UI
{
    internal sealed class NativeKeyboardDevice : KeyboardDevice
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputManager">
        /// </param>
        internal NativeKeyboardDevice(InputManager inputManager)
            : base(inputManager)
        {
        }

        /// <summary>
        ///     Gets the current state of the specified key from the device from the underlying system
        /// </summary>
        /// <param name="key">
        ///     Key to get the state of
        /// </param>
        /// <returns>                           
        ///     The state of the specified key
        /// </returns>
        protected override KeyStates GetKeyStatesFromSystem(Key key)
        {
            return (KeyStates)Application.Current.NativeKeyboard.GetKeyState((Native.Key)key);
        }
    }
}

