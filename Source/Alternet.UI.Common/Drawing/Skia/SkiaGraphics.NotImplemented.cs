using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;
using Alternet.UI.Extensions;

using SkiaSharp;

namespace Alternet.Drawing
{
    public partial class SkiaGraphics
    {
        /// <inheritdoc/>
        public override Region? Clip
        {
            get
            {
                return null;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc/>
        public override void Path(Pen pen, Brush brush, GraphicsPath path)
        {
            /* var (fill, stroke) = GetFillAndStrokePaint(pen, brush);*/
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawPath(Pen pen, GraphicsPath path)
        {
            DebugPenAssert(pen);
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void FillPath(Brush brush, GraphicsPath path)
        {
            DebugBrushAssert(brush);
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void FillPie(
            Brush brush,
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle)
        {
            DebugBrushAssert(brush);
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawPie(
            Pen pen,
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle)
        {
            DebugPenAssert(pen);
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawText(string text, Font font, Brush brush, RectD bounds)
        {
            DebugFontAssert(font);
            DebugBrushAssert(brush);
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void Pie(
            Pen pen,
            Brush brush,
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle)
        {
            /*
            var paint = GetFillAndStrokePaint(pen, brush);
            */
            throw new NotImplementedException();
        }
    }
}
