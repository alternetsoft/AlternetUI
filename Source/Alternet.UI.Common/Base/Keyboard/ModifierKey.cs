using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides constants for commonly used modifier keys.
    /// </summary>
    public static class ModifierKey
    {
        /// <summary>
        /// Represents the "Control" modifier key.
        /// </summary>
        public const ModifierKeys Control = ModifierKeys.Control;

        /// <summary>
        /// Represents the "Shift" modifier key.
        /// </summary>
        public const ModifierKeys Shift = ModifierKeys.Shift;

        /// <summary>
        /// Represents the "Alt" modifier key.
        /// </summary>
        public const ModifierKeys Alt = ModifierKeys.Alt;

        /// <summary>
        /// Represents the "Windows" modifier key.
        /// </summary>
        public const ModifierKeys Windows = ModifierKeys.Windows;

        /// <summary>
        /// Represents no modifier keys.
        /// </summary>
        public const ModifierKeys None = ModifierKeys.None;

        /// <summary>
        /// Represents both Control and Shift modifiers.
        /// </summary>
        public const ModifierKeys ControlShift = ModifierKeys.ControlShift;

        /// <summary>
        /// Represents Control, Shift, and Alt modifiers.
        /// </summary>
        public const ModifierKeys ControlShiftAlt = ModifierKeys.ControlShiftAlt;

        /// <summary>
        /// Represents both Control and Alt modifiers.
        /// </summary>
        public const ModifierKeys ControlAlt = ModifierKeys.ControlAlt;

        /// <summary>
        /// Represents both Alt and Shift modifiers.
        /// </summary>
        public const ModifierKeys AltShift = ModifierKeys.AltShift;

        /// <summary>
        /// Represents both Windows and Shift modifiers.
        /// </summary>
        public const ModifierKeys WindowsShift = ModifierKeys.WindowsShift;
    }
}
