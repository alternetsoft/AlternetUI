// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    ///     The Win32MouseDevice class implements the platform specific
    ///     MouseDevice features for the Win32 platform
    /// </summary>
    internal sealed class NativeMouseDevice : MouseDevice
    {
        internal NativeMouseDevice()
        {
        }

        /// <summary>
        ///     Gets the current state of the specified button from
        ///     the device from the underlying system.
        /// </summary>
        /// <param name="mouseButton">
        ///     The mouse button to get the state of
        /// </param>
        /// <returns>
        ///     The state of the specified mouse button
        /// </returns>
        internal override MouseButtonState GetButtonStateFromSystem(MouseButton mouseButton)
        {
            return (MouseButtonState)Application.Current.NativeMouse.GetButtonState(
                (Native.MouseButton)mouseButton);
        }

        internal override PointD GetScreenPositionFromSystem()
        {
            return Application.Current.NativeMouse.GetPosition();
        }
    }
}
