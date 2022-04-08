#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using Alternet.UI.Markup;
using System;
using System.ComponentModel;


namespace Alternet.UI
{
    /// <summary>
    ///     The RawModifierKeys enumeration describes a set of keys
    ///     used to modify other input operations, including macOS-specific keys.
    /// </summary>
    [TypeConverter(typeof(ModifierKeysConverter))]
    [ValueSerializer(typeof(ModifierKeysValueSerializer))]
    [Flags]
    public enum RawModifierKeys
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

        /// <summary>
        /// The "Command" key on Apple keyboard.
        /// </summary>
        MacCommand = 1 << 4,

        /// <summary>
        /// The "Option" key on Apple keyboard.
        /// </summary>
        MacOption = 1 << 5,

        /// <summary>
        /// The "Control" key on Apple keyboard.
        /// </summary>
        MacControl = 1 << 6,
    }
}


