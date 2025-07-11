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
    internal partial class WxGraphics : Graphics
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
        public override bool HasClip
        {
            get
            {
                if (dc.Clip == null)
                    return false;
                return true;
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

        /// <inheritdoc/>
        public override void RoundedRectangle(
            Pen pen,
            Brush brush,
            RectD rectangle,
            Coord cornerRadius)
        {
            dc.RoundedRectangle(
                (UI.Native.Pen)pen.Handler,
                (UI.Native.Brush)brush.Handler,
                rectangle,
                cornerRadius);
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

        protected override void SetHandlerTransform(TransformMatrix matrix)
        {
            dc.SetTransformValues(
                matrix.M11,
                matrix.M12,
                matrix.M21,
                matrix.M22,
                GraphicsFactory.PixelFromDip(matrix.DX, ScaleFactor),
                GraphicsFactory.PixelFromDip(matrix.DY, ScaleFactor));
        }
    }
}