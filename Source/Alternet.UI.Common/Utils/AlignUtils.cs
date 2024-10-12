using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to the layout and align.
    /// </summary>
    public static class AlignUtils
    {
        /// <summary>
        /// Aligns the specified rectangle in the container using horizontal and vertical
        /// alignment options.
        /// </summary>
        /// <param name="rect">Rectangle to align.</param>
        /// <param name="container">Container rectangle.</param>
        /// <param name="horz">Horizontal alignment.</param>
        /// <param name="vert">Vertical alignment.</param>
        /// <param name="shrinkSize">Whether to shrink size of the rectangle
        /// to fit in the container. Optional. Default is <c>true</c>.</param>
        /// <returns></returns>
        public static RectD AlignRectInRect(
            RectD rect,
            RectD container,
            HorizontalAlignment? horz,
            VerticalAlignment? vert,
            bool shrinkSize = true)
        {
            if(shrinkSize)
                rect.Size = rect.Size.Shrink(container.Width, container.Height);

            if(horz is not null)
            {
                AlignCoord(false, (CoordAlignment)horz.Value);
            }

            if (vert is not null)
            {
                AlignCoord(true, (CoordAlignment)vert.Value);
            }

            void AlignCoord(bool vert, CoordAlignment alignment)
            {
                switch (alignment)
                {
                    case CoordAlignment.Near:
                        rect.SetLocation(vert, container.GetLocation(vert));
                        break;
                    case CoordAlignment.Center:
                        rect.SetCenter(vert, container.GetCenter(vert));
                        break;
                    case CoordAlignment.Far:
                        rect.SetFarLocation(vert, container.GetFarLocation(vert));
                        break;
                    case CoordAlignment.Stretch:
                    case CoordAlignment.Fill:
                        rect.SetLocation(vert, container.GetLocation(vert));
                        rect.SetSize(vert, container.GetSize(vert));
                        break;
                }
            }

            return rect;
        }
    }
}
