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
        /// Gets the bounds of the current clipping region of this <see cref="Graphics"/> object.
        /// </summary>
        public abstract RectD ClipBounds { get; }

        /// <summary>
        /// Clips the canvas to the specified rectangle.
        /// This is <see cref="Drawing2D.CombineMode.Intersect"/> operation with the current clipping region,
        /// so the resulting clipping region is the intersection of the current clipping region and the specified rectangle.
        /// </summary>
        /// <param name="rect">The rectangle to clip to.</param>
        /// <param name="antialiasing">Whether to apply anti-aliasing to the clip edge.</param>
        /// <remarks>
        /// <para>
        /// When <paramref name="antialiasing"/> is true: Graphics applies subpixel smoothing
        /// to the edges of the clipping rectangle.
        /// This helps avoid jagged or pixelated edges, especially when the clip region
        /// doesn't align perfectly with pixel boundaries.
        /// It’s most noticeable when clipping against curves or rotated or transformed rectangles.
        /// </para>
        /// <para>
        /// When <paramref name="antialiasing"/> is false: The clip edges are hard-edged, meaning pixels are either
        /// fully inside or outside the clip.
        /// This is faster and more predictable for pixel-perfect rendering, but may produce visible
        /// aliasing (jagged edges) on diagonal or curved boundaries.
        /// </para>
        /// </remarks>
        public abstract void ClipRect(RectD rect, bool antialiasing = false);

        /// <summary>
        /// Sets the clipping region of this <see cref="Graphics"/> object to the specified rectangle,
        /// using the specified combine mode and anti-aliasing option.
        /// </summary>
        /// <param name="rect">The rectangle to set as the clipping region.</param>
        /// <param name="combineMode">Specifies how the new clipping region is combined with the existing clipping region.</param>
        /// <param name="antialiasing">Whether to apply anti-aliasing to the clip edge.
        /// See more detailed description of this param in <see cref="ClipRect"/></param>
        public abstract void SetClip(RectD rect, Drawing2D.CombineMode combineMode, bool antialiasing = false);

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
