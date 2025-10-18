using System;
using System.Collections.Generic;
using System.Text;

#if ALTERNETUI
namespace Alternet.Skia
#else
namespace Alternet.Editor.Skia
#endif
{
    /// <summary>
    /// Specifies how the interior of a closed path is filled.
    /// </summary>
    public enum SkiaFillMode
    {
        /// <summary>
        /// Specifies the alternate (odd-even) fill mode. It means that "inside" is computed by
        /// an odd number of edge crossings.
        /// </summary>
        /// <remarks>
        /// To determine the interiors of closed figures in the alternate mode, draw a
        /// line from any arbitrary start
        /// point in the path to some point obviously outside the path. If the line crosses
        /// an odd number of path
        /// segments, the starting point is inside the closed region and is therefore part of
        /// the fill or clipping area.
        /// An even number of crossings means that the point is not in an area to be filled
        /// or clipped. An open figure
        /// is filled or clipped by using a line to connect the last point to the first point
        /// of the figure.
        /// </remarks>
        Alternate,

        /// <summary>
        /// Specifies the winding fill mode. It means that "inside" is computed by a non-zero sum
        /// of signed edge crossings.
        /// </summary>
        /// <remarks>
        /// The Winding mode considers the direction of the path segments at each intersection.
        /// It adds one for every
        /// clockwise intersection, and subtracts one for every counterclockwise intersection.
        /// If the result is nonzero,
        /// the point is considered inside the fill or clip area. A zero count means that the
        /// point lies outside the
        /// fill or clip area. A figure is considered clockwise or counterclockwise based on
        /// the order in which the segments of the figure are drawn.
        /// </remarks>
        Winding,
    }
}
