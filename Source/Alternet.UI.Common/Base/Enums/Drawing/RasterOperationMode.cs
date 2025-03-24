using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines logical raster operations which can be used with blit, stretch-blit
    /// and some other functions.
    /// </summary>
    /// <remarks>
    /// The description of the enum values refer to how a generic src source pixel
    /// and the corresponding dst destination pixel gets combined together to produce
    /// the final pixel. E.g. <see cref="Clear"/> and <see cref="Set"/> completely ignore
    /// the source and the destination pixel and always put zeroes or ones in the final surface.
    /// </remarks>
    public enum RasterOperationMode
    {
        /// <summary>
        /// dst = 0.
        /// </summary>
        Clear = 0,

        /// <summary>
        /// dst = src XOR dst.
        /// </summary>
        Xor,

        /// <summary>
        /// dst = NOT dst.
        /// </summary>
        Invert,

        /// <summary>
        /// dst = src OR (NOT dst).
        /// </summary>
        OrReverse,

        /// <summary>
        /// dst = src AND (NOT dst).
        /// </summary>
        AndReverse,

        /// <summary>
        /// dst = src.
        /// </summary>
        Copy,

        /// <summary>
        /// dst = src AND dst.
        /// </summary>
        And,

        /// <summary>
        /// dst = (NOT src) AND dst..
        /// </summary>
        AndInvert,

        /// <summary>
        /// dst = dst (no operation).
        /// </summary>
        NoOp,

        /// <summary>
        /// dst = (NOT src) AND (NOT dst).
        /// </summary>
        Nor,

        /// <summary>
        /// dst = (NOT src) XOR dst.
        /// </summary>
        Equiv,

        /// <summary>
        /// dst = (NOT src).
        /// </summary>
        SrcInvert,

        /// <summary>
        /// dst = (NOT src) OR dst.
        /// </summary>
        OrInvert,

        /// <summary>
        /// dst = (NOT src) OR (NOT dst).
        /// </summary>
        Nand,

        /// <summary>
        /// dst = src OR dst.
        /// </summary>
        Or,

        /// <summary>
        /// dst = 1.
        /// </summary>
        Set,
    }
}