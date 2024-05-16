using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Alternet.UI;
using Alternet.UI.Internal.ComponentModel;
using Alternet.UI.Localization;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines a drawing surface managed by WxWidgets library.
    /// </summary>
    public class WxGraphics : Graphics
    {
        private readonly bool dispose;
        private UI.Native.DrawingContext dc;

        static WxGraphics()
        {
            WxPlatform.Initialize();
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
                return new TransformMatrix(dc.Transform);
            }

            set
            {
                dc.Transform = (UI.Native.TransformMatrix)value.Handler;
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
                dc.Clip = (UI.Native.Region?)value?.NativeObject;
            }
        }

        /// <inheritdoc/>
        public override InterpolationMode InterpolationMode
        {
            get
            {
                return (InterpolationMode)dc.InterpolationMode;
            }

            set
            {
                dc.InterpolationMode = (UI.Native.InterpolationMode)value;
            }
        }

        /// <inheritdoc/>
        public override object NativeObject
        {
            get => dc;
        }

        /// <inheritdoc/>
        public override SizeD GetTextExtent(
            string text,
            Font font,
            out double descent,
            out double externalLeading,
            IControl? control = null)
        {
            var dc = (UI.Native.DrawingContext)NativeObject;

            var result = dc.GetTextExtent(
                text,
                (UI.Native.Font)font.Handler,
                WxPlatform.WxWidget(control));
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
                WxPlatform.WxWidget(control));
            return result;
        }

        /// <inheritdoc/>
        public override void DrawRotatedTextI(
            string text,
            PointI location,
            Font font,
            Color foreColor,
            Color backColor,
            double angle)
        {
            DebugTextAssert(text);
            DebugFontAssert(font);
            DebugColorAssert(foreColor);
            dc.DrawRotatedTextI(
                text,
                location,
                (UI.Native.Font)font.Handler,
                foreColor,
                backColor,
                angle);
        }

        /*/// <summary>
        /// If supported by the platform and the type of <see cref="Graphics"/>,
        /// fetches the contents
        /// of the graphics, or a subset of it, as an <see cref="Image"/>.
        /// </summary>
        /// <param name="subrect">Subset rectangle or <c>null</c> to get full image.
        /// Rectangle is specified in pixels.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Image GetAsBitmapI(RectI? subrect = default)
        {
            var result = dc.GetAsBitmapI(subrect ?? RectI.Empty);
            return new Bitmap(result);
        }*/

        /// <inheritdoc/>
        public override bool BlitI(
            PointI destPt,
            SizeI sz,
            Graphics source,
            PointI srcPt,
            RasterOperationMode rop = RasterOperationMode.Copy,
            bool useMask = false,
            PointI? srcPtMask = null)
        {
            srcPtMask ??= PointI.MinusOne;
            return dc.BlitI(
                        destPt,
                        sz,
                        (UI.Native.DrawingContext)source.NativeObject,
                        srcPt,
                        (int)rop,
                        useMask,
                        srcPtMask.Value);
        }

        /// <inheritdoc/>
        public override bool StretchBlitI(
            PointI dstPt,
            SizeI dstSize,
            Graphics source,
            PointI srcPt,
            SizeI srcSize,
            RasterOperationMode rop = RasterOperationMode.Copy,
            bool useMask = false,
            PointI? srcPtMask = null)
        {
            srcPtMask ??= PointI.MinusOne;
            return dc.StretchBlitI(
                dstPt,
                dstSize,
                (UI.Native.DrawingContext)source.NativeObject,
                srcPt,
                srcSize,
                (int)rop,
                useMask,
                srcPtMask.Value);
        }

        /// <inheritdoc/>
        public override void RoundedRectangle(Pen pen, Brush brush, RectD rectangle, double cornerRadius)
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
            double radius,
            double startAngle,
            double sweepAngle)
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
        public override void Circle(Pen pen, Brush brush, PointD center, double radius)
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
                (UI.Native.FillMode)fillMode);
        }

        /// <inheritdoc/>
        public override void FillRectangle(Brush brush, RectD rectangle)
        {
            DebugBrushAssert(brush);
            dc.FillRectangle((UI.Native.Brush)brush.Handler, rectangle);
        }

        /// <inheritdoc/>
        public override void FillRectangleI(Brush brush, RectI rectangle)
        {
            DebugBrushAssert(brush);
            dc.FillRectangleI((UI.Native.Brush)brush.Handler, rectangle);
        }

        /// <inheritdoc/>
        public override void DrawArc(
            Pen pen,
            PointD center,
            double radius,
            double startAngle,
            double sweepAngle)
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
        public override void DrawPoint(Pen pen, double x, double y)
        {
            DebugPenAssert(pen);
            dc.DrawPoint((UI.Native.Pen)pen.Handler, x, y);
        }

        /// <inheritdoc/>
        public override void FillPie(
            Brush brush,
            PointD center,
            double radius,
            double startAngle,
            double sweepAngle)
        {
            DebugBrushAssert(brush);
            dc.FillPie((UI.Native.Brush)brush.Handler, center, radius, startAngle, sweepAngle);
        }

        /// <inheritdoc/>
        public override void DrawPie(
            Pen pen,
            PointD center,
            double radius,
            double startAngle,
            double sweepAngle)
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
        public override void DrawCircle(Pen pen, PointD center, double radius)
        {
            DebugPenAssert(pen);
            dc.DrawCircle((UI.Native.Pen)pen.Handler, center, radius);
        }

        /// <inheritdoc/>
        public override void FillCircle(Brush brush, PointD center, double radius)
        {
            DebugBrushAssert(brush);
            dc.FillCircle((UI.Native.Brush)brush.Handler, center, radius);
        }

        /// <inheritdoc/>
        public override void DrawRoundedRectangle(Pen pen, RectD rect, double cornerRadius)
        {
            DebugPenAssert(pen);
            dc.DrawRoundedRectangle((UI.Native.Pen)pen.Handler, rect, cornerRadius);
        }

        /// <inheritdoc/>
        public override void FillRoundedRectangle(Brush brush, RectD rect, double cornerRadius)
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
                (UI.Native.FillMode)fillMode);
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
        public override void DrawImage(Image image, PointD origin, bool useMask = false)
        {
            DebugImageAssert(image);
            dc.DrawImageAtPoint(
                (UI.Native.Image)image.NativeObject,
                origin,
                useMask);
        }

        /// <inheritdoc/>
        public override void DrawImage(Image image, RectD destinationRect, bool useMask = false)
        {
            DebugImageAssert(image);
            dc.DrawImageAtRect(
                (UI.Native.Image)image.NativeObject,
                destinationRect,
                useMask);
        }

        /// <inheritdoc/>
        public override void DrawImage(Image image, RectD destinationRect, RectD sourceRect)
        {
            DebugImageAssert(image);
            dc.DrawImagePortionAtRect(
                (UI.Native.Image)image.NativeObject,
                destinationRect,
                sourceRect);
        }

        /// <inheritdoc/>
        public override void DrawImageI(Image image, RectI destinationRect, RectI sourceRect)
        {
            DebugImageAssert(image);
            dc.DrawImagePortionAtPixelRect(
                (UI.Native.Image)image.NativeObject,
                destinationRect,
                sourceRect);
        }

        /// <inheritdoc/>
        public override void SetPixel(PointD point, Pen pen)
        {
            DebugPenAssert(pen);
            dc.SetPixel(point, (UI.Native.Pen)pen.Handler);
        }

        /// <inheritdoc/>
        public override void SetPixel(double x, double y, Pen pen)
        {
            DebugPenAssert(pen);
            dc.SetPixel(new PointD(x, y), (UI.Native.Pen)pen.Handler);
        }

        /// <inheritdoc/>
        public override void SetPixel(double x, double y, Color color)
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
            if(unit != GraphicsUnit.Pixel)
            {
                var dpi = GetDPI();
                var graphicsType = GraphicsUnitConverter.GraphicsType.Undefined;
                destinationRect = GraphicsUnitConverter.ConvertRect(
                    unit,
                    GraphicsUnit.Pixel,
                    dpi,
                    destinationRect,
                    graphicsType);
                sourceRect = GraphicsUnitConverter.ConvertRect(
                    unit,
                    GraphicsUnit.Pixel,
                    dpi,
                    sourceRect,
                    graphicsType);
            }

            DrawImageI(image, destinationRect.ToRect(), sourceRect.ToRect());
        }

        /// <inheritdoc/>
        public override void DrawText(
            string text,
            Font font,
            Brush brush,
            PointD origin,
            TextFormat format)
        {
            DebugTextAssert(text);
            DebugFontAssert(font);
            DebugFormatAssert(format);
            dc.DrawTextAtPoint(
                text,
                origin,
                (UI.Native.Font)font.Handler,
                (UI.Native.Brush)brush.Handler);
        }

        /// <inheritdoc/>
        public override void DrawText(
            string text,
            Font font,
            Brush brush,
            RectD bounds,
            TextFormat format)
        {
            DebugTextAssert(text);
            DebugFontAssert(font);
            dc.DrawTextAtRect(
                text,
                bounds,
                (UI.Native.Font)font.Handler,
                (UI.Native.Brush)brush.Handler,
                (UI.Native.TextHorizontalAlignment)format.HorizontalAlignment,
                (UI.Native.TextVerticalAlignment)format.VerticalAlignment,
                (UI.Native.TextTrimming)format.Trimming,
                (UI.Native.TextWrapping)format.Wrapping);
        }

        /// <inheritdoc/>
        public override SizeD MeasureText(string text, Font font)
        {
            DebugTextAssert(text);
            DebugFontAssert(font);
            return dc.MeasureText(
                text,
                (UI.Native.Font)font.Handler,
                double.NaN,
                UI.Native.TextWrapping.None);
        }

        /// <inheritdoc/>
        public override SizeD MeasureText(string text, Font font, double maximumWidth)
        {
            DebugTextAssert(text);
            DebugFontAssert(font);
            return dc.MeasureText(
                text,
                (UI.Native.Font)font.Handler,
                maximumWidth,
                UI.Native.TextWrapping.Character);
        }

        /// <inheritdoc/>
        public override SizeD MeasureText(
            string text,
            Font font,
            double maximumWidth,
            TextFormat format)
        {
            DebugTextAssert(text);
            DebugFontAssert(font);
            DebugFormatAssert(format);
            return dc.MeasureText(
                text,
                (UI.Native.Font)font.Handler,
                maximumWidth,
                (UI.Native.TextWrapping)format.Wrapping);
        }

        /// <inheritdoc/>
        public override void Push()
        {
            dc.Push();
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
                (UI.Native.Image?)image?.NativeObject,
                rect,
                (int)alignment,
                indexAccel);
        }

        /// <inheritdoc/>
        public override void PushTransform(TransformMatrix transform)
        {
            Push();
            var currentTransform = Transform;
            currentTransform.Multiply(transform);
            Transform = currentTransform;
        }

        /// <inheritdoc/>
        public override SizeD GetDPI()
        {
            return dc.GetDpi();
        }

        /// <inheritdoc/>
        public override void Pop()
        {
            dc.Pop();
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
    }
}