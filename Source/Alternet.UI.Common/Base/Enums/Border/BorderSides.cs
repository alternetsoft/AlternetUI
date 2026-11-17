using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the sides of a border that can be drawn.
    /// </summary>
    [Flags]
    public enum BorderSides
    {
        /// <summary>
        /// No border sides are drawn.
        /// </summary>
        None = 0,

        /// <summary>
        /// The left border side is drawn.
        /// </summary>
        Left = 1 << 0,

        /// <summary>
        /// The top border side is drawn.
        /// </summary>
        Top = 1 << 1,

        /// <summary>
        /// The right border side is drawn.
        /// </summary>
        Right = 1 << 2,

        /// <summary>
        /// The bottom border side is drawn.
        /// </summary>
        Bottom = 1 << 3,

        /// <summary>
        /// All border sides are drawn.
        /// </summary>
        All = Left | Top | Right | Bottom,

        /// <summary>
        /// The horizontal border sides (left and right) are drawn.
        /// </summary>
        Horizontal = Left | Right,

        /// <summary>
        /// The vertical border sides (top and bottom) are drawn.
        /// </summary>
        Vertical = Top | Bottom,
    }

    internal static class BorderSidesHelper
    {
        public static BorderSides Without(this BorderSides sides, BorderSides sidesToRemove)
        {
            return sides & ~sidesToRemove;
        }
    }
}
