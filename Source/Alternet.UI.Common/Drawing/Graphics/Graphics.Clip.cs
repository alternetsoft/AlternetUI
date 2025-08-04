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
