using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods related to drawing.
    /// </summary>
    public static class DrawingUtils
    {
        /// <summary>
        /// Gets rectangle of the top border edge with the specified width.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        /// <param name="width">Border side width.</param>
        public static RectD GetTopLineRect(RectD rect, double width)
        {
            var point = rect.TopLeft;
            var size = new SizeD(rect.Width, width);
            return new RectD(point, size);
        }

        /// <summary>
        /// Gets rectangle of the horizontal center line of the rectangle.
        /// </summary>
        /// <param name="rect">Rectangle.</param>
        /// <returns></returns>
        public static RectD GetCenterLineHorz(RectD rect)
        {
            var size = new SizeD(rect.Width, 1);
            var point = new PointD(rect.Left, (int)rect.Center.Y);
            return new RectD(point, size);
        }

        /// <summary>
        /// Gets rectangle of the vertical center line of the rectangle.
        /// </summary>
        /// <param name="rect">Rectangle.</param>
        /// <returns></returns>
        public static RectD GetCenterLineVert(RectD rect)
        {
            var size = new SizeD(1, rect.Height);
            var point = new PointD((int)rect.Center.X, rect.Top);
            return new RectD(point, size);
        }

        /// <summary>
        /// Gets rectangle of the bottom border edge with the specified width.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        /// <param name="width">Border side width.</param>
        public static RectD GetBottomLineRect(RectD rect, double width)
        {
            var point = new PointD(rect.Left, rect.Bottom - width);
            var size = new SizeD(rect.Width, width);
            return new RectD(point, size);
        }

        /// <summary>
        /// Draws horizontal line using <see cref="Graphics.FillRectangle"/>.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <param name="brush">Brush to draw line.</param>
        /// <param name="point">Starting point.</param>
        /// <param name="length">Line length.</param>
        /// <param name="width">Line width.</param>
        public static void DrawHorzLine(
            Graphics dc,
            Brush brush,
            PointD point,
            double length,
            double width)
        {
            var rect = new RectD(point, new SizeD(length, width));
            dc.FillRectangle(brush, rect);
        }

        /// <summary>
        /// Draws vertical line using <see cref="Graphics.FillRectangle"/>.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <param name="brush">Brush to draw line.</param>
        /// <param name="point">Starting point.</param>
        /// <param name="length">Line length.</param>
        /// <param name="width">Line width.</param>
        public static void DrawVertLine(
            Graphics dc,
            Brush brush,
            PointD point,
            double length,
            double width)
        {
            var rect = new RectD(point, new SizeD(width, length));
            dc.FillRectangle(brush, rect);
        }

        /// <summary>
        /// Draws rectangle border using <see cref="Graphics.FillRectangle"/>.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <param name="brush">Brush to draw border.</param>
        /// <param name="rect">Border rectangle.</param>
        /// <param name="borderWidth">Border width.</param>
        public static void FillRectangleBorder(
            Graphics dc,
            Brush brush,
            RectD rect,
            double borderWidth)
        {
            dc.FillRectangle(brush, GetTopLineRect(rect, borderWidth));
            dc.FillRectangle(brush, GetBottomLineRect(rect, borderWidth));
            dc.FillRectangle(brush, GetLeftLineRect(rect, borderWidth));
            dc.FillRectangle(brush, GetRightLineRect(rect, borderWidth));
        }

        /// <summary>
        /// Gets rectangle of the left border edge with the specified width.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        /// <param name="width">Border side width.</param>
        public static RectD GetLeftLineRect(RectD rect, double width)
        {
            var point = rect.TopLeft;
            var size = new SizeD(width, rect.Height);
            return new RectD(point, size);
        }

        /// <summary>
        /// Gets rectangle of the right border edge with the specified width.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        /// <param name="width">Border side width.</param>
        public static RectD GetRightLineRect(RectD rect, double width)
        {
            var point = new PointD(rect.Right - width, rect.Top);
            var size = new SizeD(width, rect.Height);
            return new RectD(point, size);
        }
    }
}
