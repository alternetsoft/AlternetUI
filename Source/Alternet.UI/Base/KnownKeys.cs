using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines different key and modifier combinations.
    /// </summary>
    public class KnownKeys
    {
        /// <summary>
        /// Gets or sets keys used to run test action.
        /// </summary>
        public static KeyInfo RunTest { get; set; } = new(Key.T, ModifierKeys.ControlShift);

        /// <summary>
        /// Defines additional keys for the <see cref="RichTextBox"/>.
        /// </summary>
        public class RichEditKeys
        {
            /// <summary>
            /// Gets or sets keys used in the rich edit to decrease font size.
            /// </summary>
            public static KeyInfo[] DecFontSize { get; set; } =
            {
                 new(Key.Comma, ModifierKeys.ControlShift, OperatingSystems.WindowsOrLinux),
                 new(Key.Comma, ModifierKeys.WindowsShift, OperatingSystems.MacOs),
            };

            /// <summary>
            /// Gets or sets keys used in the rich edit to increase font size.
            /// </summary>
            public static KeyInfo[] IncFontSize { get; set; } =
            {
                new(Key.Period, ModifierKeys.ControlShift, OperatingSystems.WindowsOrLinux),
                new(Key.Period, ModifierKeys.WindowsShift, OperatingSystems.MacOs),
            };

            /// <summary>
            /// Gets or sets keys used in the rich edit to clear text formatting.
            /// </summary>
            public static KeyInfo[] ClearTextFormatting { get; set; } =
            {
                new(Key.Backslash, ModifierKeys.Control, OperatingSystems.WindowsOrLinux),
                new(Key.Space, ModifierKeys.Control, OperatingSystems.WindowsOrLinux),
                new(Key.Backslash, ModifierKeys.Windows, OperatingSystems.MacOs),
            };

            /// <summary>
            /// Gets or sets keys used in the rich edit for toggling font strikethrough style.
            /// </summary>
            public static KeyInfo[] ToggleStrikethrough { get; set; } =
            {
                new(Key.D5, ModifierKeys.AltShift, OperatingSystems.WindowsOrLinux),
                new(Key.X, ModifierKeys.WindowsShift, OperatingSystems.MacOs),
            };

            /// <summary>
            /// Gets or sets keys used in the rich edit to select all text.
            /// </summary>
            public static KeyInfo[] SelectAll { get; set; } =
            {
                 new(Key.A, ModifierKeys.Control, OperatingSystems.WindowsOrLinux),
                 new(Key.A, ModifierKeys.Windows, OperatingSystems.MacOs),
            };

            /// <summary>
            /// Gets or sets keys used in the rich edit for toggling font bold style.
            /// </summary>
            public static KeyInfo[] ToggleBold { get; set; } =
            {
                 new(Key.B, ModifierKeys.Control, OperatingSystems.WindowsOrLinux),
                 new(Key.B, ModifierKeys.Windows, OperatingSystems.MacOs),
            };

            /// <summary>
            /// Gets or sets keys used in the rich edit for toggling font italic style.
            /// </summary>
            public static KeyInfo[] ToggleItalic { get; set; } =
            {
                new(Key.I, ModifierKeys.Control, OperatingSystems.WindowsOrLinux),
                new(Key.I, ModifierKeys.Windows, OperatingSystems.MacOs),
            };

            /// <summary>
            /// Gets or sets keys used in the rich edit for toggling font underline style.
            /// </summary>
            public static KeyInfo[] ToggleUnderline { get; set; } =
            {
                 new(Key.U, ModifierKeys.Control, OperatingSystems.WindowsOrLinux),
                 new(Key.U, ModifierKeys.Windows, OperatingSystems.MacOs),
            };

            /// <summary>
            /// Gets or sets keys used in the rich edit for aligning text to the left.
            /// </summary>
            public static KeyInfo[] LeftAlign { get; set; } =
            {
                new(Key.L, ModifierKeys.ControlShift, OperatingSystems.WindowsOrLinux),
                new(Key.L, ModifierKeys.WindowsShift, OperatingSystems.MacOs),
            };

            /// <summary>
            /// Gets or sets keys used in the rich edit for aligning text to the center.
            /// </summary>
            public static KeyInfo[] CenterAlign { get; set; } =
            {
                new(Key.E, ModifierKeys.ControlShift, OperatingSystems.WindowsOrLinux),
                new(Key.E, ModifierKeys.WindowsShift, OperatingSystems.MacOs),
            };

            /// <summary>
            /// Gets or sets keys used in the rich edit for aligning text to the right.
            /// </summary>
            public static KeyInfo[] RightAlign { get; set; } =
            {
                new(Key.R, ModifierKeys.ControlShift, OperatingSystems.WindowsOrLinux),
                new(Key.R, ModifierKeys.WindowsShift, OperatingSystems.MacOs),
            };

            /// <summary>
            /// Gets or sets keys used in the rich edit to justify the text.
            /// </summary>
            public static KeyInfo[] Justify { get; set; } =
            {
                new(Key.J, ModifierKeys.ControlShift, OperatingSystems.WindowsOrLinux),
                new(Key.J, ModifierKeys.WindowsShift, OperatingSystems.MacOs),
            };
        }
    }
}