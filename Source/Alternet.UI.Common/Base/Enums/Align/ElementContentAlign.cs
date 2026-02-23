using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the alignment options for content displayed within a button or similar control.
    /// </summary>
    /// <remarks>Use this enumeration to control the horizontal or vertical positioning of a content. The
    /// available values allow content to be aligned to the left, right, top, bottom, or use the default alignment as
    /// determined by the control or system settings.</remarks>
    public enum ElementContentAlign
    {
        /// <summary>
        /// Default alignment.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Content is horizontally aligned on the left.
        /// </summary>
        Left = 0x0010,

        /// <summary>
        /// Content is horizontally aligned on the right.
        /// </summary>
        Right = 0x0020,

        /// <summary>
        /// Content is vertically aligned at the top.
        /// </summary>
        Top = 0x0040,

        /// <summary>
        /// Content is vertically aligned at the bottom.
        /// </summary>
        Bottom = 0x0080,
    }
}
