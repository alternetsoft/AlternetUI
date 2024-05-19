using System;

using ApiCommon;


namespace NativeApi.Api
{
    /// <summary>
    ///     The ModifierKeys enumeration describes a set of common keys
    ///     used to modify other input operations.
    /// </summary>
    [Flags]
    [ManagedExternName("Alternet.UI.ModifierKeys")]
    [ManagedName("Alternet.UI.ModifierKeys")]
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


