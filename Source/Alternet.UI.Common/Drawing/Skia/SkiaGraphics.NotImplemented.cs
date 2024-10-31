using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;
using Alternet.UI.Extensions;

namespace Alternet.Drawing
{
    public partial class SkiaGraphics
    {
        /// <inheritdoc/>
        public override bool HasClip
        {
            get
            {
                return !canvas.IsClipEmpty;
            }
        }

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
        public override void DrawRotatedText(
            string text,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor,
            Coord angle,
            GraphicsUnit unit = GraphicsUnit.Dip)
        {
            DebugFontAssert(font);
            ToDip(ref location, unit);
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool Blit(
            PointD destPt,
            SizeD sz,
            Graphics source,
            PointD srcPt,
            RasterOperationMode rop = RasterOperationMode.Copy,
            bool useMask = false,
            PointD? srcPtMask = null,
            GraphicsUnit unit = GraphicsUnit.Dip)
        {
            return false;
        }

        /// <inheritdoc/>
        public override bool StretchBlit(
            PointD dstPt,
            SizeD dstSize,
            Graphics source,
            PointD srcPt,
            SizeD srcSize,
            RasterOperationMode rop = RasterOperationMode.Copy,
            bool useMask = false,
            PointD? srcPtMask = null,
            GraphicsUnit unit = GraphicsUnit.Dip)
        {
            return false;
        }

        /// <inheritdoc/>
        public override void Path(Pen pen, Brush brush, GraphicsPath path)
        {
            var paint = GetFillAndStrokePaint(pen, brush);
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void Polygon(Pen pen, Brush brush, PointD[] points, FillMode fillMode)
        {
            var paint = GetFillAndStrokePaint(pen, brush);
            var skiaPoints = points.ToSkia();
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
        public override void FillPolygon(
            Brush brush,
            PointD[] points,
            FillMode fillMode = FillMode.Alternate)
        {
            DebugBrushAssert(brush);
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void FloodFill(Brush brush, PointD point)
        {
            DebugBrushAssert(brush);
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
        public override void DestroyClippingRegion()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void SetClippingRegion(RectD rect)
        {
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
        public override void Pie(
            Pen pen,
            Brush brush,
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle)
        {
            var paint = GetFillAndStrokePaint(pen, brush);
            throw new NotImplementedException();
        }
    }
}
