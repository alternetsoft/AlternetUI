using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies content alignment or general direction.
    /// </summary>
    [Flags]
    public enum GenericDirection
    {
        /// <summary>
        /// Content alignment or direction is not specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// Content alignment or direction is not specified.
        /// </summary>
        Default = None,

        /// <summary>
        /// Content is horizontally aligned on the left.
        /// Left direction is specified.
        /// </summary>
        Left = 0x0010,

        /// <summary>
        /// Content is horizontally aligned on the right.
        /// Right direction is specified.
        /// </summary>
        Right = 0x0020,

        /// <summary>
        /// Content is vertically aligned at the top.
        /// Top direction is specified.
        /// </summary>
        Top = 0x0040,

        /// <summary>
        /// Content is vertically aligned at the bottom.
        /// Bottom direction is specified.
        /// </summary>
        Bottom = 0x0080,

        /// <summary>
        /// Content is vertically aligned at the bottom, and horizontally
        /// aligned on the left.
        /// Bottom and Left directions are specified.
        /// </summary>
        BottomLeft = Bottom | Left,

        /// <summary>
        /// Content is vertically aligned at the bottom, and horizontally
        /// aligned on the right.
        /// Bottom and Right directions are specified.
        /// </summary>
        BottomRight = Bottom | Right,

        /// <summary>
        /// Content is vertically aligned at the top, and horizontally
        /// aligned on the left.
        /// Top and Left directions are specified.
        /// </summary>
        TopLeft = Top | Left,

        /// <summary>
        /// Content is vertically aligned at the top, and horizontally
        /// aligned on the right.
        /// Top and Right directions are specified.
        /// </summary>
        TopRight = Top | Right,
    }
}