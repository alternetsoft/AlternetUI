using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    public partial class Graphics
    {
        /// <summary>
        /// Clips the canvas to the specified rectangle.
        /// </summary>
        /// <param name="rect">The rectangle to clip to.</param>
        /// <param name="antialiasing">Whether to apply anti-aliasing to the clip edge.</param>
        /*
        When antialiasing = true: Graphics applies subpixel smoothing to the edges of
        the clipping rectangle.
        This helps avoid jagged or pixelated edges, especially when the clip region
        doesn't align perfectly with pixel boundaries.
        It’s most noticeable when clipping against curves or rotated/transformed rectangles.

        When antialiasing = false: The clip edges are hard-edged, meaning pixels are either
        fully inside or outside the clip.
        This is faster and more predictable for pixel-perfect rendering, but may produce visible
        aliasing (jagged edges) on diagonal or curved boundaries.
        */
        public abstract void ClipRect(RectD rect, bool antialiasing = false);

        /// <summary>
        /// Modify the current clip with the specified region.
        /// </summary>
        /// <param name="region">The region to clip to.</param>
        public abstract void ClipRegion(Region region);

        /// <summary>
        /// Calls the specified action inside temporary clipped rectangle, so painting outside
        /// this rectangle is ignored.
        /// </summary>
        /// <param name="isClipped">Whether to clip rectangle. Optional. Default is <c>true</c>.</param>
        /// <param name="rect">Rectangle region to set as clip object.</param>
        /// <param name="action">Action to call.</param>
        public virtual void DoInsideClipped(RectD rect, Action action, bool isClipped = true)
        {
            if(!isClipped)
            {
                action();
                return;
            }

            Save();

            try
            {
                ClipRect(rect);
                action();
            }
            finally
            {
                Restore();
            }
        }

        /// <summary>
        /// Calls the specified action inside temporary clipped region, so painting outside
        /// this region is ignored.
        /// </summary>
        /// <param name="isClipped">Whether to clip rectangle. Optional. Default is <c>true</c>.</param>
        /// <param name="region">The region to set as clip object.</param>
        /// <param name="action">Action to call.</param>
        public virtual void DoInsideClipped(Region region, Action action, bool isClipped = true)
        {
            if (!isClipped)
            {
                action();
                return;
            }

            try
            {
                Save();
                ClipRegion(region);
                action();
            }
            finally
            {
                Restore();
            }
        }
    }
}
