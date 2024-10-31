﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Drawing context which raises <see cref="NotImplementedException"/> exeption
    /// on every method or property call.
    /// </summary>
    public class NotImplementedGraphics : Graphics
    {
        /// <inheritdoc/>
        public override bool IsOk
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc/>
        public override bool HasClip => throw new NotImplementedException();

        /// <inheritdoc/>
        public override Region? Clip
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc/>
        public override InterpolationMode InterpolationMode
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc/>
        public override object NativeObject
        {
            get
            {
                throw new NotImplementedException();
            }
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
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void Circle(Pen pen, Brush brush, PointD center, Coord radius)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DestroyClippingRegion()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawArc(Pen pen, PointD center, Coord radius, Coord startAngle, Coord sweepAngle)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawBezier(Pen pen, PointD startPoint, PointD controlPoint1, PointD controlPoint2, PointD endPoint)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawBeziers(Pen pen, PointD[] points)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawCircle(Pen pen, PointD center, Coord radius)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawEllipse(Pen pen, RectD bounds)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawImage(Image image, PointD origin)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawImage(Image image, RectD destinationRect)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawImage(Image image, RectD destinationRect, RectD sourceRect)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawImage(Image image, RectD destinationRect, RectD sourceRect, GraphicsUnit unit)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawLine(Pen pen, PointD a, PointD b)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawLines(Pen pen, PointD[] points)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawPath(Pen pen, GraphicsPath path)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawPie(Pen pen, PointD center, Coord radius, Coord startAngle, Coord sweepAngle)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawPoint(Pen pen, Coord x, Coord y)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawPolygon(Pen pen, PointD[] points)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawRectangle(Pen pen, RectD rectangle)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawRectangles(Pen pen, RectD[] rects)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawRoundedRectangle(Pen pen, RectD rect, Coord cornerRadius)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawText(string text, Font font, Brush brush, PointD origin)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawText(string text, PointD location, Font font, Color foreColor, Color backColor)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DrawText(string text, Font font, Brush brush, RectD bounds)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void Ellipse(Pen pen, Brush brush, RectD rectangle)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void FillCircle(Brush brush, PointD center, Coord radius)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void FillEllipse(Brush brush, RectD bounds)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void FillPath(Brush brush, GraphicsPath path)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void FillPie(Brush brush, PointD center, Coord radius, Coord startAngle, Coord sweepAngle)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void FillPolygon(Brush brush, PointD[] points, FillMode fillMode = FillMode.Alternate)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void FillRectangle(Brush brush, RectD rectangle)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void FillRectangle(Brush brush, RectD rectangle, GraphicsUnit unit)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void FillRectangles(Brush brush, RectD[] rects)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void FillRoundedRectangle(Brush brush, RectD rect, Coord cornerRadius)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void FloodFill(Brush brush, PointD point)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override RectD GetClippingBox()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override SizeI GetDPI()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override SizeD GetTextExtent(string text, Font font)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void Path(Pen pen, Brush brush, GraphicsPath path)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void Pie(Pen pen, Brush brush, PointD center, Coord radius, Coord startAngle, Coord sweepAngle)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void Polygon(Pen pen, Brush brush, PointD[] points, FillMode fillMode)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void Rectangle(Pen pen, Brush brush, RectD rectangle)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void RoundedRectangle(Pen pen, Brush brush, RectD rectangle, Coord cornerRadius)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void SetClippingRegion(RectD rect)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void SetPixel(PointD point, Pen pen)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void SetPixel(Coord x, Coord y, Pen pen)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void SetPixel(Coord x, Coord y, Color color)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        protected override void SetHandlerTransform(TransformMatrix matrix)
        {
            throw new NotImplementedException();
        }
    }
}
