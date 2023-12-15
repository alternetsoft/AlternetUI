using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the type of scroll bars to display in a control.</summary>
    public enum RichTextBoxScrollBars
    {
        /// <summary>
        /// No scroll bars are displayed.
        /// </summary>
        None = 0,

        /// <summary>
        /// Display a horizontal scroll bar only when text is longer than the width
        /// of the control.
        /// </summary>
        Horizontal = 1,

        /// <summary>
        /// Display a vertical scroll bar only when text is longer than the height of
        /// the control.
        /// </summary>
        Vertical = 2,

        /// <summary>
        /// Display both a horizontal and a vertical scroll bar when needed.
        /// </summary>
        Both = 3,

        /// <summary>
        /// Always display a horizontal scroll bar.
        /// </summary>
        ForcedHorizontal = 17,

        /// <summary>
        /// Always display a vertical scroll bar.
        /// </summary>
        ForcedVertical = 18,

        /// <summary>
        /// Always display both a horizontal and a vertical scroll bar.
        /// </summary>
        ForcedBoth = 19,
    }
}
