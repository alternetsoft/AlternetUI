using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies content alignment.
    /// </summary>
    [Flags]
    public enum GenericAlignment
    {
        /// <summary>
        /// A symbolic name for an invalid alignment value which can be assumed to be
        /// different from anything else.
        /// </summary>
        Invalid = -1,

        /// <summary>
        /// Content is horizontally aligned on the left or vertically aligned at the top.
        /// </summary>
        Not = 0x0000,

        /// <summary>
        /// Content is horizontally aligned on the center.
        /// </summary>
        CenterHorizontal = 0x0100,

        /// <summary>
        /// Content is horizontally aligned on the left.
        /// </summary>
        Left = Not,

        /// <summary>
        /// Content is vertically aligned at the top.
        /// </summary>
        Top = Not,

        /// <summary>
        /// Content is horizontally aligned on the right.
        /// </summary>
        Right = 0x0200,

        /// <summary>
        /// Content is vertically aligned at the bottom.
        /// </summary>
        Bottom = 0x0400,

        /// <summary>
        /// Content is vertically aligned at the center.
        /// </summary>
        CenterVertical = 0x0800,

        /// <summary>
        /// Content is aligned at the center.
        /// </summary>
        Center = CenterHorizontal | CenterVertical,

        /// <summary>
        /// Content is vertically aligned at the bottom, and horizontally
        /// aligned on the left.
        /// </summary>
        BottomLeft = Bottom | Left,

        /// <summary>
        /// Content is vertically aligned at the bottom, and horizontally
        /// aligned on the right.
        /// </summary>
        BottomRight = Bottom | Right,

        /// <summary>
        /// Content is vertically aligned at the top, and horizontally
        /// aligned on the left.
        /// </summary>
        TopLeft = Top | Left,

        /// <summary>
        /// Content is vertically aligned at the top, and horizontally
        /// aligned on the right.
        /// </summary>
        TopRight = Top | Right,

        /// <summary>
        /// Content is vertically aligned at the center, and horizontally
        /// aligned on the right.
        /// </summary>
        CenterRight = CenterVertical | Right,
    }
}