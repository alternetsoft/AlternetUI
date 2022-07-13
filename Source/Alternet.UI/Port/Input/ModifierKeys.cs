#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

#if !ALTERNET_UI_INTEGRATION_REMOTING
using Alternet.UI.Markup;
#endif

using System.ComponentModel;

#if ALTERNET_UI_INTEGRATION_REMOTING
namespace Alternet.UI.Integration.Remoting
#else
namespace Alternet.UI
#endif
{
    /// <summary>
    ///     The ModifierKeys enumeration describes a set of common keys
    ///     used to modify other input operations.
    /// </summary>
#if !ALTERNET_UI_INTEGRATION_REMOTING
    [TypeConverter(typeof(ModifierKeysConverter))]
    [ValueSerializer(typeof(ModifierKeysValueSerializer))]
#endif
    [Flags]
    public enum ModifierKeys
    {
        /// <summary>
        /// No modifiers are pressed.
        /// </summary>
        None = 0,

        /// <summary>
        /// The "Alt" key on Windows and Linux or "Option" key on macOS.
        /// </summary>
        Alt = 1 << 0,
        
        /// <summary>
        /// A "Control" key on Windows and Linux or "Command" key on macOS.
        /// </summary>
        Control = 1 << 1,

        /// <summary>
        /// A shift key.
        /// </summary>
        Shift = 1 << 2,

        /// <summary>
        /// The Microsoft "Windows" key on Windows or "Control" key on macOS or "Meta" key on Linux.
        /// </summary>
        Windows = 1 << 3,
    }
}


