using System;
using System.ComponentModel;
using Alternet.UI.Markup;

namespace Alternet.UI
{
    /// <summary>
    /// Defines the modifier keys that can be pressed simultaneously with another key.
    /// </summary>
    /// <remarks>
    /// This enumeration supports a bitwise combination of its member values.
    /// </remarks>
    [TypeConverter("Alternet.UI.ModifierKeysConverter")]
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
        /// The Microsoft "Windows" key on Windows or "Control"
        /// key on macOS or "Meta" key on Linux.
        /// </summary>
        Windows = 1 << 3,

        /// <summary>
        /// Both Control and Shift modifiers are pressed.
        /// </summary>
        ControlShift = Control | Shift,

        /// <summary>
        /// Control+Shift+Alt pressed.
        /// </summary>
        ControlShiftAlt = Control | Shift | Alt,

        /// <summary>
        /// Both Control and Alt modifiers are pressed.
        /// </summary>
        ControlAlt = Control | Alt,

        /// <summary>
        /// Both Alt and Shift modifiers are pressed.
        /// </summary>
        AltShift = Alt | Shift,

        /// <summary>
        /// Both Windows and Shift modifiers are pressed.
        /// </summary>
        WindowsShift = Windows | Shift,
    }
}