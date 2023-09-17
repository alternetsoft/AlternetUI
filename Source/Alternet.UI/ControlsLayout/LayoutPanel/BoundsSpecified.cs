using System;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the bounds of the object to use when defining a
    /// its size and/or position. Supports a
    /// bitwise combination of its member values.
    /// </summary>
    [Flags]
    public enum BoundsSpecified
    {
        /// <summary>
        /// No bounds are specified.
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// Specifies that the left edge of the object is defined.
        /// </summary>
        X = 0x00000001,

        /// <summary>
        /// Specifies that the top edge of the object is defined. 
        /// </summary>
        Y = 0x00000002,

        /// <summary>
        /// Specifies that both the X and Y coordinates are defined.
        /// </summary>
        Location = 0x00000003,

        /// <summary>
        /// Specifies that the width of the object is defined.
        /// </summary>
        Width = 0x00000004,

        /// <summary>
        /// Specifies that the height of the object is defined.
        /// </summary>
        Height = 0x00000008,

        /// <summary>
        /// Specifies that both the width and height are defined.
        /// </summary>
        Size = 0x0000000c,

        /// <summary>
        /// Specifies that both the location and size are defined.
        /// </summary>
        All = 0x0000000f,
    }
}
