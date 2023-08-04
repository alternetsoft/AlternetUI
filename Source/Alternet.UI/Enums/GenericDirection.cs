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
        /// Up direction is specified.
        /// </summary>
        Up = 0x0040,

        /// <summary>
        /// Content is vertically aligned at the bottom.
        /// Down direction is specified.
        /// </summary>
        Down = 0x0080,

        /// <summary>
        /// Content is vertically aligned at the top.
        /// Top direction is specified.
        /// </summary>
        Top = Up,

        /// <summary>
        /// Content is vertically aligned at the bottom.
        /// Bottom direction is specified.
        /// </summary>
        Bottom = Down,

        /// <summary>
        /// Same as Up direction.
        /// </summary>
        North = Up,

        /// <summary>
        /// Same as Down direction.
        /// </summary>
        South = Down,

        /// <summary>
        /// Same as Left direction.
        /// </summary>
        West = Left,

        /// <summary>
        /// Same as East direction.
        /// </summary>
        Eeast = Right,

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

        /// <summary>
        /// Value with all direction flags specified.
        /// </summary>
        All = Up | Down | Right | Left,

        /// <summary>
        /// A mask to extract direction or content alignmnent from the combination
        /// of flags.
        /// </summary>
        DirectionMask = All,
    }
}

/*
BottomCenter    512	
Content is vertically aligned at the bottom, and horizontally aligned at the center.


MiddleCenter	32	
Content is vertically aligned in the middle, and horizontally aligned at the center.

MiddleLeft	16	
Content is vertically aligned in the middle, and horizontally aligned on the left.

MiddleRight	64	
Content is vertically aligned in the middle, and horizontally aligned on the right.

TopCenter	2	
Content is vertically aligned at the top, and horizontally aligned at the center.

*/