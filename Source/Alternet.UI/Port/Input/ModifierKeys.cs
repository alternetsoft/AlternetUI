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
    ///     The ModifierKeys enumeration describes a set of common keys
    ///     used to modify other input operations.
    /// </summary>
    [TypeConverter(typeof(ModifierKeysConverter))]
    [ValueSerializer(typeof(ModifierKeysValueSerializer))]
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
        /// A control key.
        /// </summary>
        Control = 1 << 1,

        /// <summary>
        /// A shift key.
        /// </summary>
        Shift = 1 << 2,

        /// <summary>
        /// The Microsoft "Windows" key on Windows or "Command" key on macOS or "Meta" key on Linux.
        /// </summary>
        Windows = 1 << 3,

        /// <summary>
        /// The "Command" key on Apple keyboard.
        /// </summary>
        Command = 1 << 4,

        /// <summary>
        /// The "Option" key on Apple keyboard.
        /// </summary>
        Option = 1 << 5,
    }
}


