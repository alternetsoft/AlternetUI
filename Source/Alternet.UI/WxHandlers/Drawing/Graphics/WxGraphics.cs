using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Alternet.UI;
using Alternet.UI.Localization;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines a drawing surface managed by WxWidgets library.
    /// </summary>
    internal partial class WxGraphics : Graphics, IWxGraphics
    {
        private readonly bool dispose;
        private UI.Native.DrawingContext dc;

        static WxGraphics()
        {
        }

        internal WxGraphics(UI.Native.DrawingContext dc, bool dispose = true)
        {
            this.dc = dc;
            this.dispose = dispose;
        }

        /// <inheritdoc/>
        public override bool IsOk => dc.IsOk;

        /// <inheritdoc/>
        public override TransformMatrix Transform
        {
            get
            {
                var matrix = dc.Transform;
                return new TransformMatrix(
                    matrix.M11,
                    matrix.M12,
                    matrix.M21,
                    matrix.M22,
                    matrix.DX,
                    matrix.DY);
            }

            set
            {
                var matrix = new UI.Native.TransformMatrix();
                matrix.Initialize(value.M11, value.M12, value.M21, value.M22, value.DX, value.DY);
                dc.Transform = matrix;
            }
        }

        /// <inheritdoc/>
        public override Region? Clip
        {
            get
            {
                var clip = dc.Clip;
                if (clip == null)
                    return null;

                return new Region(clip);
            }

            set
            {
                dc.Clip = (UI.Native.Region?)value?.Handler;
            }
        }

        /// <inheritdoc/>
        public override InterpolationMode InterpolationMode
        {
            get
            {
                return dc.InterpolationMode;
            }

            set
            {
                dc.InterpolationMode = value;
            }
        }

        /// <inheritdoc/>
        public override object NativeObject
        {
            get => dc;
        }

        public SizeD GetTextExtent(
            string text,
            Font font,
            out Coord? descent,
            out Coord? externalLeading,
            IControl? control = null)
        {
            var dc = (UI.Native.DrawingContext)NativeObject;

            var result = dc.GetTextExtent(
                text,
                (UI.Native.Font)font.Handler,
                WxApplicationHandler.WxWidget(control));
            descent = result.X;
            externalLeading = result.Y;
            return result.Size;
        }

        /// <inheritdoc/>
        public override SizeD GetTextExtent(
            string text,
            Font font,
            IControl? control)
        {
            var dc = (UI.Native.DrawingContext)NativeObject;
            var result = dc.GetTextExtentSimple(
                text,
                (UI.Native.Font)font.Handler,
                WxApplicationHandler.WxWidget(control));
            return result;
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
            DebugTextAssert(text);
            DebugFontAssert(font);
            DebugColorAssert(foreColor);

            if(unit == GraphicsUnit.Pixel)
            {
                dc.DrawRotatedTextI(
                    text,
                    location.ToPoint(),
                    (UI.Native.Font)font.Handler,
                    foreColor,
                    backColor,
                    angle);
                return;
            }

            ToDip(ref location, unit);

            dc.DrawRotatedText(
                text,
                location,
                (UI.Native.Font)font.Handler,
                foreColor,
                backColor,
                angle);
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
            var srcPtMaskValue = srcPtMask ?? PointI.MinusOne;

            if (unit == GraphicsUnit.Pixel)
            {
                var result = dc.BlitI(
                            destPt.ToPoint(),
                            sz.ToSize(),
                            (UI.Native.DrawingContext)source.NativeObject,
                            srcPt.ToPoint(),
                            (int)rop,
                            useMask,
                            srcPtMaskValue.ToPoint());
                return result;
            }

            ToDip(ref destPt, unit);
            ToDip(ref sz, unit);
            ToDip(ref srcPt, unit);

            if (srcPtMaskValue != PointI.MinusOne)
                ToDip(ref srcPtMaskValue, unit);

            return dc.Blit(
                        destPt,
                        sz,
                        (UI.Native.DrawingContext)source.NativeObject,
                        srcPt,
                        (int)rop,
                        useMask,
                        srcPtMaskValue);
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
            var srcPtMaskValue = srcPtMask ?? PointI.MinusOne;

            if (unit == GraphicsUnit.Pixel)
            {
                var result = dc.StretchBlitI(
                    dstPt.ToPoint(),
                    dstSize.ToSize(),
                    (UI.Native.DrawingContext)source.NativeObject,
                    srcPt.ToPoint(),
                    srcSize.ToSize(),
                    (int)rop,
                    useMask,
                    srcPtMaskValue.ToPoint());
                return result;
            }

            ToDip(ref dstPt, unit);
            ToDip(ref dstSize, unit);
            ToDip(ref srcPt, unit);
            ToDip(ref srcSize, unit);

            if (srcPtMaskValue != PointI.MinusOne)
                ToDip(ref srcPtMaskValue, unit);

            return dc.StretchBlit(
                dstPt,
                dstSize,
                (UI.Native.DrawingContext)source.NativeObject,
                srcPt,
                srcSize,
                (int)rop,
                useMask,
                srcPtMaskValue);
        }

        /// <inheritdoc/>
        public override void RoundedRectangle(Pen pen, Brush brush, RectD rectangle, Coord cornerRadius)
        {
            dc.RoundedRectangle(
                (UI.Native.Pen)pen.Handler,
                (UI.Native.Brush)brush.Handler,
                rectangle,
                cornerRadius);
        }

        /// <inheritdoc/>
        public override SizeD GetTextExtent(string text, Font font)
        {
            var result = dc.GetTextExtentSimple(
                text,
                (UI.Native.Font)font.Handler,
                default);
            return result;
        }

        /// <inheritdoc/>
        public override void Rectangle(Pen pen, Brush brush, RectD rectangle)
        {
            DebugBrushAssert(brush);
            DebugPenAssert(pen);
            dc.Rectangle(
                (UI.Native.Pen)pen.Handler,
                (UI.Native.Brush)brush.Handler,
                rectangle);
        }

        /// <inheritdoc/>
        public override void Ellipse(Pen pen, Brush brush, RectD rectangle)
        {
            DebugBrushAssert(brush);
            DebugPenAssert(pen);
            dc.Ellipse(
                (UI.Native.Pen)pen.Handler,
                (UI.Native.Brush)brush.Handler,
                rectangle);
        }

        /// <inheritdoc/>
        public override void Path(Pen pen, Brush brush, GraphicsPath path)
        {
            DebugBrushAssert(brush);
            DebugPenAssert(pen);
            dc.Path(
                (UI.Native.Pen)pen.Handler,
                (UI.Native.Brush)brush.Handler,
                (UI.Native.GraphicsPath)path.Handler);
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
            DebugBrushAssert(brush);
            DebugPenAssert(pen);
            dc.Pie(
                (UI.Native.Pen)pen.Handler,
                (UI.Native.Brush)brush.Handler,
                center,
                radius,
                startAngle,
                sweepAngle);
        }

        /// <inheritdoc/>
        public override void Circle(Pen pen, Brush brush, PointD center, Coord radius)
        {
            DebugBrushAssert(brush);
            DebugPenAssert(pen);
            dc.Circle(
                (UI.Native.Pen)pen.Handler,
                (UI.Native.Brush)brush.Handler,
                center,
                radius);
        }

        /// <inheritdoc/>
        public override void Polygon(Pen pen, Brush brush, PointD[] points, FillMode fillMode)
        {
            DebugBrushAssert(brush);
            DebugPenAssert(pen);
            dc.Polygon(
                (UI.Native.Pen)pen.Handler,
                (UI.Native.Brush)brush.Handler,
                points,
                fillMode);
        }

        /// <inheritdoc/>
        public override void FillRectangle(Brush brush, RectD rectangle)
        {
            DebugBrushAssert(brush);
            dc.FillRectangle((UI.Native.Brush)brush.Handler, rectangle);
        }

        /// <inheritdoc/>
        public override void DrawArc(
            Pen pen,
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle)
        {
            DebugPenAssert(pen);
            dc.DrawArc(
                (UI.Native.Pen)pen.Handler,
                center,
                radius,
                startAngle,
                sweepAngle);
        }

        /// <inheritdoc/>
        public override void DrawPoint(Pen pen, Coord x, Coord y)
        {
            DebugPenAssert(pen);
            dc.DrawPoint((UI.Native.Pen)pen.Handler, x, y);
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
            dc.FillPie((UI.Native.Brush)brush.Handler, center, radius, startAngle, sweepAngle);
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
            dc.DrawPie((UI.Native.Pen)pen.Handler, center, radius, startAngle, sweepAngle);
        }

        /// <inheritdoc/>
        public override void DrawBezier(
            Pen pen,
            PointD startPoint,
            PointD controlPoint1,
            PointD controlPoint2,
            PointD endPoint)
        {
            DebugPenAssert(pen);
            dc.DrawBezier(
                (UI.Native.Pen)pen.Handler,
                startPoint,
                controlPoint1,
                controlPoint2,
                endPoint);
        }

        /// <inheritdoc/>
        public override void DrawBeziers(Pen pen, PointD[] points)
        {
            DebugPenAssert(pen);
            DebugBezierPointsAssert(points);
            dc.DrawBeziers((UI.Native.Pen)pen.Handler, points);
        }

        /// <inheritdoc/>
        public override void DrawCircle(Pen pen, PointD center, Coord radius)
        {
            DebugPenAssert(pen);
            dc.DrawCircle((UI.Native.Pen)pen.Handler, center, radius);
        }

        /// <inheritdoc/>
        public override void FillCircle(Brush brush, PointD center, Coord radius)
        {
            DebugBrushAssert(brush);
            dc.FillCircle((UI.Native.Brush)brush.Handler, center, radius);
        }

        /// <inheritdoc/>
        public override void DrawRoundedRectangle(Pen pen, RectD rect, Coord cornerRadius)
        {
            DebugPenAssert(pen);
            dc.DrawRoundedRectangle((UI.Native.Pen)pen.Handler, rect, cornerRadius);
        }

        /// <inheritdoc/>
        public override void FillRoundedRectangle(Brush brush, RectD rect, Coord cornerRadius)
        {
            DebugBrushAssert(brush);
            dc.FillRoundedRectangle((UI.Native.Brush)brush.Handler, rect, cornerRadius);
        }

        /// <inheritdoc/>
        public override void DrawPolygon(Pen pen, PointD[] points)
        {
            DebugPenAssert(pen);
            dc.DrawPolygon((UI.Native.Pen)pen.Handler, points);
        }

        /// <inheritdoc/>
        public override void FillPolygon(
            Brush brush,
            PointD[] points,
            FillMode fillMode = FillMode.Alternate)
        {
            DebugBrushAssert(brush);
            dc.FillPolygon(
                (UI.Native.Brush)brush.Handler,
                points,
                fillMode);
        }

        /// <inheritdoc/>
        public override void DrawRectangles(Pen pen, RectD[] rects)
        {
            DebugPenAssert(pen);
            dc.DrawRectangles((UI.Native.Pen)pen.Handler, rects);
        }

        /// <inheritdoc/>
        public override void FillRectangles(Brush brush, RectD[] rects)
        {
            DebugBrushAssert(brush);
            dc.FillRectangles((UI.Native.Brush)brush.Handler, rects);
        }

        /// <inheritdoc/>
        public override void FillEllipse(Brush brush, RectD bounds)
        {
            DebugBrushAssert(brush);
            dc.FillEllipse((UI.Native.Brush)brush.Handler, bounds);
        }

        /// <inheritdoc/>
        public override void FloodFill(Brush brush, PointD point)
        {
            DebugSolidBrushAssert(brush);
            dc.FloodFill((UI.Native.Brush)brush.Handler, point);
        }

        /// <inheritdoc/>
        public override void DrawRectangle(Pen pen, RectD rectangle)
        {
            DebugPenAssert(pen);
            dc.DrawRectangle((UI.Native.Pen)pen.Handler, rectangle);
        }

        /// <inheritdoc/>
        public override void DrawPath(Pen pen, GraphicsPath path)
        {
            DebugPenAssert(pen);
            dc.DrawPath((UI.Native.Pen)pen.Handler, (UI.Native.GraphicsPath)path.Handler);
        }

        /// <inheritdoc/>
        public override void FillPath(Brush brush, GraphicsPath path)
        {
            DebugBrushAssert(brush);
            dc.FillPath((UI.Native.Brush)brush.Handler, (UI.Native.GraphicsPath)path.Handler);
        }

        /// <inheritdoc/>
        public override void DrawLine(Pen pen, PointD a, PointD b)
        {
            DebugPenAssert(pen);
            dc.DrawLine((UI.Native.Pen)pen.Handler, a, b);
        }

        /// <inheritdoc/>
        public override void DrawLines(Pen pen, PointD[] points)
        {
            DebugPenAssert(pen);
            dc.DrawLines((UI.Native.Pen)pen.Handler, points);
        }

        /// <inheritdoc/>
        public override void DrawEllipse(Pen pen, RectD bounds)
        {
            DebugPenAssert(pen);
            dc.DrawEllipse((UI.Native.Pen)pen.Handler, bounds);
        }

        /// <inheritdoc/>
        public override void DrawImage(Image image, PointD origin)
        {
            DebugImageAssert(image);
            dc.DrawImageAtPoint(
                (UI.Native.Image)image.Handler,
                origin,
                false);
        }

        /// <inheritdoc/>
        public override void DrawImage(Image image, RectD destinationRect)
        {
            DebugImageAssert(image);
            dc.DrawImageAtRect(
                (UI.Native.Image)image.Handler,
                destinationRect,
                false);
        }

        /// <inheritdoc/>
        public override void SetPixel(PointD point, Pen pen)
        {
            DebugPenAssert(pen);
            dc.SetPixel(point, (UI.Native.Pen)pen.Handler);
        }

        /// <inheritdoc/>
        public override void SetPixel(Coord x, Coord y, Pen pen)
        {
            DebugPenAssert(pen);
            dc.SetPixel(new PointD(x, y), (UI.Native.Pen)pen.Handler);
        }

        /// <inheritdoc/>
        public override void SetPixel(Coord x, Coord y, Color color)
        {
            DebugColorAssert(color);
            dc.SetPixel(new PointD(x, y), (UI.Native.Pen)color.AsPen.Handler);
        }

        /// <inheritdoc/>
        public override Color GetPixel(PointD point)
        {
            return dc.GetPixel(point);
        }

        /// <inheritdoc/>
        public override void DrawImage(
            Image image,
            RectD destinationRect,
            RectD sourceRect,
            GraphicsUnit unit)
        {
            if(unit == GraphicsUnit.Pixel)
            {
                dc.DrawImagePortionAtPixelRect(
                    (UI.Native.Image)image.Handler,
                    destinationRect.ToRect(),
                    sourceRect.ToRect());
                return;
            }

            ToDip(ref destinationRect, unit);
            ToDip(ref sourceRect, unit);

            DrawImage(image, destinationRect, sourceRect);
        }

        /// <inheritdoc/>
        public override void DrawImage(Image image, RectD destinationRect, RectD sourceRect)
        {
            DebugImageAssert(image);
            dc.DrawImagePortionAtRect(
                (UI.Native.Image)image.Handler,
                destinationRect,
                sourceRect);
        }


        /// <inheritdoc/>
        public override void DrawText(string text, Font font, Brush brush, RectD bounds)
        {
            DrawText(text, font, brush, bounds, TextFormat.Default);
        }

        /// <inheritdoc/>
        public override void DrawText(
            string text,
            Font font,
            Brush brush,
            PointD origin)
        {
            DebugTextAssert(text);
            DebugFontAssert(font);
            dc.DrawTextAtPoint(
                text,
                origin,
                (UI.Native.Font)font.Handler,
                (UI.Native.Brush)brush.Handler);
        }

        /// <inheritdoc/>
        public override void DrawText(
            string text,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor)
        {
            DebugTextAssert(text);
            DebugFontAssert(font);
            DebugColorAssert(foreColor);
            dc.DrawText(
                text,
                location,
                (UI.Native.Font)font.Handler,
                foreColor,
                backColor);
        }

        /// <inheritdoc/>
        public override RectD DrawLabel(
            string text,
            Font font,
            Color foreColor,
            Color backColor,
            Image? image,
            RectD rect,
            GenericAlignment alignment = GenericAlignment.TopLeft,
            int indexAccel = -1)
        {
            DebugTextAssert(text);
            DebugFontAssert(font);
            DebugColorAssert(foreColor, nameof(foreColor));
            return dc.DrawLabel(
                text,
                (UI.Native.Font)font.Handler,
                foreColor,
                backColor,
                (UI.Native.Image?)image?.Handler,
                rect,
                (int)alignment,
                indexAccel);
        }

        /// <inheritdoc/>
        public override SizeI GetDPI()
        {
            return dc.GetDpi();
        }

        /// <inheritdoc/>
        public override void DestroyClippingRegion()
        {
            dc.DestroyClippingRegion();
        }

        /// <inheritdoc/>
        public override void SetClippingRegion(RectD rect)
        {
            dc.SetClippingRegion(rect);
        }

        /// <inheritdoc/>
        public override RectD GetClippingBox()
        {
            return dc.GetClippingBox();
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            if (dispose)
                dc.Dispose();
            dc = null!;
        }

        public override void FillRectangle(Brush brush, RectD rectangle, GraphicsUnit unit)
        {
            if (unit == GraphicsUnit.Pixel)
            {
                dc.FillRectangleI((UI.Native.Brush)brush.Handler, rectangle.ToRect());
                return;
            }

            ToDip(ref rectangle, unit);
            FillRectangle(brush, rectangle);
        }
    }
}