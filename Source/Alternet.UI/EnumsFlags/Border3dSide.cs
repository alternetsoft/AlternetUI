using System;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the sides of a rectangle to apply a three-dimensional border to.
    /// </summary>
    [Flags]
    public enum Border3DSide
    {
        /// <summary>
        /// A 3D border on the left edge of the rectangle.
        /// </summary>
        Left = 1,

        /// <summary>
        /// A 3D border on the top edge of the rectangle.
        /// </summary>
        Top = 2,

        /// <summary>
        /// A 3D border on the right side of the rectangle.
        /// </summary>
        Right = 4,

        /// <summary>
        /// A 3D border on the bottom side of the rectangle.
        /// </summary>
        Bottom = 8,

        /// <summary>
        /// The interior of the rectangle is filled with the color defined for 3D controls
        /// instead of the background color for the form.
        /// </summary>
        Middle = 0x800,

        /// <summary>
        /// A 3D border on all sides of the rectangle.
        /// The middle of the rectangle is filled with the color defined for 3D controls.</summary>
        All = 0x80F,
    }
}
