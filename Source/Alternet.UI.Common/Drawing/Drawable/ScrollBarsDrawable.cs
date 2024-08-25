using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements vertical and horizontal scrollbars drawing.
    /// </summary>
    public class ScrollBarsDrawable : BaseDrawable
    {
        /// <summary>
        /// Gets or sets vertical scroll bar element.
        /// </summary>
        public ScrollBarDrawable? VertScrollBar;

        /// <summary>
        /// Gets or sets horizontal scroll bar element.
        /// </summary>
        public ScrollBarDrawable? HorzScrollBar;

        /// <summary>
        /// Gets or sets corner element.
        /// </summary>
        public RectangleDrawable? Corner;

        /// <inheritdoc/>
        public override void Draw(Control control, Graphics dc)
        {
            if (!Visible)
                return;

            var vertVisible = VertScrollBar?.Visible ?? false;
            var horzVisible = HorzScrollBar?.Visible ?? false;

            VertScrollBar?.Draw(control, dc);
            HorzScrollBar?.Draw(control, dc);

            if(vertVisible && horzVisible)
                Corner?.Draw(control, dc);
        }
    }
}
