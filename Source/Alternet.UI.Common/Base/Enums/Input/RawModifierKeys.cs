using System;
using System.ComponentModel;

using Alternet.UI.Markup;

namespace Alternet.UI
{
    /// <summary>
    ///     The RawModifierKeys enumeration describes a set of keys
    ///     used to modify other input operations, including macOS-specific keys.
    /// </summary>
    /// <remarks>
    /// This enumeration supports a bitwise combination of its member values.
    /// </remarks>
    [TypeConverter("Alternet.UI.ModifierKeysConverter")]
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
        Alt = 1,

        /// <summary>
        /// A "Control" key on Windows and Linux or "Command" key on macOS. Value is 0x0002.
        /// </summary>
        Control = 1 << 1,

        /// <summary>
        /// A shift key. Value is 0x0004.
        /// </summary>
        Shift = 1 << 2,

        /// <summary>
        /// The Microsoft "Windows" key on Windows or "Control" key
        /// on macOS or "Meta" key on Linux. Value is 0x0008.
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