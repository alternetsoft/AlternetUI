using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    public partial class Graphics
    {
        /// <summary>
        /// Gets or sets clipping region.
        /// </summary>
        public abstract Region? Clip { get; set; }

        /// <summary>
        /// Gets whether <see cref="Clip"/> property is assigned.
        /// </summary>
        public abstract bool HasClip { get; }

        /// <summary>
        /// Destroys the current clipping region so that none of the DC is clipped.
        /// </summary>
        public abstract void DestroyClippingRegion();

        /// <summary>
        /// Sets the clipping region for this device context to the intersection of the
        /// given region described by the parameters of this method and the previously
        /// set clipping region.
        /// </summary>
        /// <param name="rect">Clipping rectangle.</param>
        /// <remarks>
        /// The clipping region is an area to which drawing is restricted. Possible uses
        /// for the clipping region are for clipping text or for speeding up
        /// window redraws when only a known area of the screen is damaged.
        /// </remarks>
        /// <remarks>
        /// Calling this function can only make the clipping region
        /// smaller, never larger.
        /// You need to call <see cref="DestroyClippingRegion"/> first if you want
        /// to set the clipping
        /// region exactly to the region specified.
        /// If resulting clipping region is empty, then all drawing on the DC is
        /// clipped out (all changes made by drawing operations are masked out).
        /// </remarks>
        public abstract void SetClippingRegion(RectD rect);

        /// <summary>
        /// Gets the rectangle surrounding the current clipping region.
        /// If no clipping region is set this function returns the extent of the device context.
        /// </summary>
        /// <returns>
        /// <see cref="RectD"/> filled in with the logical coordinates of the clipping region
        /// on success, or <see cref="RectD.Empty"/> otherwise.
        /// </returns>
        public abstract RectD GetClippingBox();

        /// <summary>
        /// Calls the specified action inside temprorary clipped rectangle, so painting outside
        /// this rectangle is ignored.
        /// </summary>
        /// <param name="isClipped">Whether to clip rectangle. Optional. Default is <c>true</c>.</param>
        /// <param name="rect">Rectangle region to set as clip object.</param>
        /// <param name="action">Action to call.</param>
        public virtual void DoInsideClipped(RectD rect, Action action, bool isClipped = true)
        {
            if (isClipped)
            {
                try
                {
                    PushClip();
                    Clip = new Region(rect);
                    action();
                }
                finally
                {
                    PopClip();
                }
            }
            else
                action();
        }

        /// <summary>
        /// Pops a stored clip region state from the stack and sets the current clip region
        /// to that state.
        /// </summary>
        public virtual void PopClip()
        {
            clipStack ??= new();
            Clip = clipStack.Pop();
        }

        /// <summary>
        /// Pushes the current state of the clip region on a stack.
        /// </summary>
        public virtual void PushClip()
        {
            clipStack ??= new();

            if (HasClip)
                clipStack.Push(Clip);
            else
                clipStack.Push(null);
        }
    }
}
