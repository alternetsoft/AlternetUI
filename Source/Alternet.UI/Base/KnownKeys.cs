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
        /// Gets or sets keys used in the rich edit for toggling font bold style.
        /// </summary>
        public static KeyInfo RichEditToggleBold { get; set; } = new(Key.B, ModifierKeys.Control);

        /// <summary>
        /// Gets or sets keys used in the rich edit for toggling font italic style.
        /// </summary>
        public static KeyInfo RichEditToggleItalic { get; set; } = new(Key.I, ModifierKeys.Control);

        /// <summary>
        /// Gets or sets keys used in the rich edit for toggling font underline style.
        /// </summary>
        public static KeyInfo RichEditToggleUnderline { get; set; } = new(Key.U, ModifierKeys.Control);

        /// <summary>
        /// Gets or sets keys used in the rich edit for aligning text to the left.
        /// </summary>
        public static KeyInfo RichEditLeftAlign { get; set; } = new(Key.L, ModifierKeys.ControlShift);

        /// <summary>
        /// Gets or sets keys used in the rich edit for aligning text to the center.
        /// </summary>
        public static KeyInfo RichEditCenterAlign { get; set; } = new(Key.E, ModifierKeys.ControlShift);

        /// <summary>
        /// Gets or sets keys used in the rich edit for aligning text to the right.
        /// </summary>
        public static KeyInfo RichEditRightAlign { get; set; } = new(Key.R, ModifierKeys.ControlShift);

        /// <summary>
        /// Gets or sets keys used in the rich edit to justify the text.
        /// </summary>
        public static KeyInfo RichEditJustify { get; set; } = new(Key.J, ModifierKeys.ControlShift);
    }
}