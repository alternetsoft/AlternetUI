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
        public virtual Region? Clip
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
                TransformRectToNative(rectangle),
                TransformSizeToNative(cornerRadius).Width);
        }

        /// <inheritdoc/>
        public override void Rectangle(Pen pen, Brush brush, RectD rectangle)
        {
            DebugBrushAssert(brush);
            DebugPenAssert(pen);
            dc.Rectangle(
                (UI.Native.Pen)pen.Handler,
                (UI.Native.Brush)brush.Handler,
                TransformRectToNative(rectangle));
        }

        /// <inheritdoc/>
        public override void Ellipse(Pen pen, Brush brush, RectD rectangle)
        {
            DebugBrushAssert(brush);
            DebugPenAssert(pen);
            dc.Ellipse(
                (UI.Native.Pen)pen.Handler,
                (UI.Native.Brush)brush.Handler,
                TransformRectToNative(rectangle));
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
                TransformPointToNative(center),
                TransformSizeToNative(radius).Width,
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
                TransformPointToNative(center),
                TransformSizeToNative(radius).Width);
        }

        /// <inheritdoc/>
        public override void Polygon(Pen pen, Brush brush, PointD[] points, FillMode fillMode)
        {
            DebugBrushAssert(brush);
            DebugPenAssert(pen);
            dc.Polygon(
                (UI.Native.Pen)pen.Handler,
                (UI.Native.Brush)brush.Handler,
                TransformPointsToNative(points),
                fillMode);
        }

        /// <inheritdoc/>
        public override void FillRectangle(Brush brush, RectD rectangle)
        {
            DebugBrushAssert(brush);
            dc.FillRectangle(
                (UI.Native.Brush)brush.Handler,
                TransformRectToNative(rectangle));
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
                TransformPointToNative(center),
                TransformSizeToNative(radius).Width,
                startAngle,
                sweepAngle);
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
            dc.FillPie(
                (UI.Native.Brush)brush.Handler,
                TransformPointToNative(center),
                TransformSizeToNative(radius).Width,
                startAngle,
                sweepAngle);
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
            dc.DrawPie(
                (UI.Native.Pen)pen.Handler,
                TransformPointToNative(center),
                TransformSizeToNative(radius).Width,
                startAngle,
                sweepAngle);
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
                TransformPointToNative(startPoint),
                TransformPointToNative(controlPoint1),
                TransformPointToNative(controlPoint2),
                TransformPointToNative(endPoint));
        }

        /// <inheritdoc/>
        public override void DrawBeziers(Pen pen, PointD[] points)
        {
            DebugPenAssert(pen);
            DebugBezierPointsAssert(points);
            dc.DrawBeziers(
                (UI.Native.Pen)pen.Handler,
                TransformPointsToNative(points));
        }

        /// <inheritdoc/>
        public override void DrawCircle(Pen pen, PointD center, Coord radius)
        {
            DebugPenAssert(pen);
            dc.DrawCircle(
                (UI.Native.Pen)pen.Handler,
                TransformPointToNative(center),
                TransformSizeToNative(radius).Width);
        }

        /// <inheritdoc/>
        public override void FillCircle(Brush brush, PointD center, Coord radius)
        {
            DebugBrushAssert(brush);
            dc.FillCircle(
                (UI.Native.Brush)brush.Handler,
                TransformPointToNative(center),
                TransformSizeToNative(radius).Width);
        }

        /// <inheritdoc/>
        public override void DrawRoundedRectangle(Pen pen, RectD rect, Coord cornerRadius)
        {
            DebugPenAssert(pen);
            dc.DrawRoundedRectangle(
                (UI.Native.Pen)pen.Handler,
                TransformRectToNative(rect),
                TransformSizeToNative(cornerRadius).Width);
        }

        /// <inheritdoc/>
        public override void FillRoundedRectangle(Brush brush, RectD rect, Coord cornerRadius)
        {
            DebugBrushAssert(brush);
            dc.FillRoundedRectangle(
                (UI.Native.Brush)brush.Handler,
                TransformRectToNative(rect),
                TransformSizeToNative(cornerRadius).Width);
        }

        /// <inheritdoc/>
        public override void DrawPolygon(Pen pen, PointD[] points)
        {
            DebugPenAssert(pen);
            dc.DrawPolygon(
                (UI.Native.Pen)pen.Handler,
                TransformPointsToNative(points));
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
                TransformPointsToNative(points),
                fillMode);
        }

        /// <inheritdoc/>
        public override void FillEllipse(Brush brush, RectD bounds)
        {
            DebugBrushAssert(brush);
            dc.FillEllipse(
                (UI.Native.Brush)brush.Handler,
                TransformRectToNative(bounds));
        }

        /// <inheritdoc/>
        public override void DrawRectangle(Pen pen, RectD rectangle)
        {
            DebugPenAssert(pen);
            dc.DrawRectangle(
                (UI.Native.Pen)pen.Handler,
                TransformRectToNative(rectangle));
        }

        /// <inheritdoc/>
        public override void DrawPath(Pen pen, GraphicsPath path)
        {
            DebugPenAssert(pen);
            dc.DrawPath(
                (UI.Native.Pen)pen.Handler,
                (UI.Native.GraphicsPath)path.Handler);
        }

        /// <inheritdoc/>
        public override void FillPath(
            Brush brush,
            GraphicsPath path)
        {
            DebugBrushAssert(brush);
            dc.FillPath(
                (UI.Native.Brush)brush.Handler,
                (UI.Native.GraphicsPath)path.Handler);
        }

        /// <inheritdoc/>
        public override void DrawLine(Pen pen, PointD a, PointD b)
        {
            DebugPenAssert(pen);
            dc.DrawLine(
                (UI.Native.Pen)pen.Handler,
                TransformPointToNative(a),
                TransformPointToNative(b));
        }

        /// <inheritdoc/>
        public override void DrawLines(Pen pen, PointD[] points)
        {
            DebugPenAssert(pen);
            dc.DrawLines(
                (UI.Native.Pen)pen.Handler,
                TransformPointsToNative(points));
        }

        /// <inheritdoc/>
        public override void DrawEllipse(Pen pen, RectD bounds)
        {
            DebugPenAssert(pen);
            dc.DrawEllipse(
                (UI.Native.Pen)pen.Handler,
                TransformRectToNative(bounds));
        }

        /// <inheritdoc/>
        public override void DrawImage(Image image, PointD origin)
        {
            DebugImageAssert(image);
            dc.DrawImageAtPoint(
                (UI.Native.Image)image.Handler,
                TransformPointToNative(origin),
                false);
        }

        /// <inheritdoc/>
        public override void DrawImage(Image image, RectD destinationRect)
        {
            DebugImageAssert(image);
            dc.DrawImageAtRect(
                (UI.Native.Image)image.Handler,
                TransformRectToNative(destinationRect),
                false);
        }


        /// <inheritdoc/>
        public override void Save()
        {
            base.Save();
            dc.Save();
        }

        /// <inheritdoc/>
        public override void Restore()
        {
            dc.Restore();
            PopTransform();
        }

        /// <inheritdoc/>
        public override SizeI GetDPI()
        {
            var result = dc.GetDpi();
            return (result.Width < 96 || result.Height < 96) ? 96 : result;
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
                dc.FillRectangleI(
                    (UI.Native.Brush)brush.Handler,
                    rectangle.ToRect());
                return;
            }

            ToDip(ref rectangle, unit);
            FillRectangle(brush, rectangle);
        }

        /// <inheritdoc/>
        public override void ClipRect(RectD rect, bool antialiasing = true)
        {
            dc.SetClippingRect(TransformRectToNative(rect));
        }

        /// <inheritdoc/>
        public override void ClipRegion(Region region)
        {
            var nativeRegion = (UI.Native.Region)region.Handler;
            dc.SetClippingRegion(nativeRegion);
        }

        protected override void SetHandlerTransform(TransformMatrix matrix)
        {
            if (!GetNoTransformToNative())
                return;

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