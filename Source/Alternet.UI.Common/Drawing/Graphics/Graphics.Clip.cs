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
                Save();
                Clip = new Region(rect);
                action();
            }
            finally
            {
                Restore();
            }
        }
    }
}
