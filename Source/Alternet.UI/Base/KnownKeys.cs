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
        /// Gets or sets keys used to show 'Developer Tools' in debug mode.
        /// </summary>
        public static KeyInfo[] ShowDeveloperTools { get; set; } = [new(Keys.F12, ModifierKeys.ControlShiftAlt)];

        /// <summary>
        /// Gets or sets keys used to run test action.
        /// </summary>
        public static KeyInfo RunTest { get; set; } = new(Keys.T, ModifierKeys.ControlShift);

        /// <summary>
        /// Defines additional keys for the <see cref="RichTextBox"/>.
        /// </summary>
        public class RichEditKeys
        {
            /// <summary>
            /// Gets or sets keys used in the rich edit to decrease font size.
            /// </summary>
            public static KeyInfo[] DecFontSize { get; set; } =
            [
                 new(Keys.Comma, ModifierKeys.ControlShift, OperatingSystems.Any),
            ];

            /// <summary>
            /// Gets or sets keys used in the rich edit to increase font size.
            /// </summary>
            public static KeyInfo[] IncFontSize { get; set; } =
            [
                new(Keys.Period, ModifierKeys.ControlShift, OperatingSystems.Any),
            ];

            /// <summary>
            /// Gets or sets keys used in the rich edit to clear text formatting.
            /// </summary>
            public static KeyInfo[] ClearTextFormatting { get; set; } =
            [
                new(Keys.Backslash, ModifierKeys.Control, OperatingSystems.Any),
                new(Keys.Space, ModifierKeys.Control, OperatingSystems.WindowsOrLinux),
            ];

            /// <summary>
            /// Gets or sets keys used in the rich edit for toggling font strikethrough style.
            /// </summary>
            public static KeyInfo[] ToggleStrikethrough { get; set; } =
            [
                new(Keys.D5, ModifierKeys.AltShift, OperatingSystems.WindowsOrLinux),
                new(Keys.X, ModifierKeys.ControlShift, OperatingSystems.MacOs),
            ];

            /// <summary>
            /// Gets or sets keys used in the rich edit to select all text.
            /// </summary>
            public static KeyInfo[] SelectAll { get; set; } =
            [
                 new(Keys.A, ModifierKeys.Control, OperatingSystems.Any),
            ];

            /// <summary>
            /// Gets or sets keys used in the rich edit for toggling font bold style.
            /// </summary>
            public static KeyInfo[] ToggleBold { get; set; } =
            [
                 new(Keys.B, ModifierKeys.Control, OperatingSystems.Any),
            ];

            /// <summary>
            /// Gets or sets keys used in the rich edit for toggling font italic style.
            /// </summary>
            public static KeyInfo[] ToggleItalic { get; set; } =
            [
                new(Keys.I, ModifierKeys.Control, OperatingSystems.Any),
            ];

            /// <summary>
            /// Gets or sets keys used in the rich edit for toggling font underline style.
            /// </summary>
            public static KeyInfo[] ToggleUnderline { get; set; } =
            [
                 new(Keys.U, ModifierKeys.Control, OperatingSystems.Any),
            ];

            /// <summary>
            /// Gets or sets keys used in the rich edit for aligning text to the left.
            /// </summary>
            public static KeyInfo[] LeftAlign { get; set; } =
            [
                new(Keys.L, ModifierKeys.ControlShift, OperatingSystems.Any),
            ];

            /// <summary>
            /// Gets or sets keys used in the rich edit for aligning text to the center.
            /// </summary>
            public static KeyInfo[] CenterAlign { get; set; } =
            [
                new(Keys.E, ModifierKeys.ControlShift, OperatingSystems.Any),
            ];

            /// <summary>
            /// Gets or sets keys used in the rich edit for aligning text to the right.
            /// </summary>
            public static KeyInfo[] RightAlign { get; set; } =
            [
                new(Keys.R, ModifierKeys.ControlShift, OperatingSystems.Any),
            ];

            /// <summary>
            /// Gets or sets keys used in the rich edit to justify the text.
            /// </summary>
            public static KeyInfo[] Justify { get; set; } =
            [
                new(Keys.J, ModifierKeys.ControlShift, OperatingSystems.Any),
            ];
        }
    }
}