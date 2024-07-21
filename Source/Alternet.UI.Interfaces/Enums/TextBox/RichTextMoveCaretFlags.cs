using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates flags used in move caret methods of the rich text box control.
    /// </summary>
    /// <remarks>
    /// This enumeration supports a bitwise combination of its member values.
    /// </remarks>
    [Flags]
    public enum RichTextMoveCaretFlags
    {
        /// <summary>
        /// Shift key is pressed.
        /// </summary>
        ShiftDown = 0x01,

        /// <summary>
        /// Control key is pressed.
        /// </summary>
        ControlDown = 0x02,

        /// Alt key is pressed.
        AltDown = 0x04,
    }
}