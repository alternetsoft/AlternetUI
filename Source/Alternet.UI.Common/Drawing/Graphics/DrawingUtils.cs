﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        /// Creates <see cref="Image"/> with the size specified by <paramref name="sizeAndDrawFunc"/>
        /// and with pixels filled with <paramref name="drawAction"/>
        /// (or <paramref name="sizeAndDrawFunc"/> if it is null).
        /// </summary>
        /// <param name="scaleFactor">Scale factor to use when draw action is called.</param>
        /// <param name="sizeAndDrawFunc">Function which calculates drawable element. Called with measure
        /// <see cref="Graphics"/> which can measure text size.</param>
        /// <param name="drawAction">Action which draws an element. If Null,
        /// <paramref name="sizeAndDrawFunc"/>
        /// is used for drawing with <see cref="Graphics"/> created on the <see cref="Bitmap"/>.
        /// <paramref name="sizeAndDrawFunc"/>Optional. </param>
        /// <returns></returns>
        public static Image ImageFromAction(
            Coord scaleFactor,
            Func<Graphics, SizeD> sizeAndDrawFunc,
            Action<Graphics>? drawAction = null)
        {
            drawAction ??= (graphics) =>
            {
                sizeAndDrawFunc(graphics);
            };

            var measureCanvas = SkiaUtils.CreateMeasureCanvas(scaleFactor);
            var size = sizeAndDrawFunc(measureCanvas);
            var canvas = SkiaUtils.CreateBitmapCanvas(size, scaleFactor);

            drawAction(canvas);

            var image = (Image)canvas.Bitmap!;
            return image;
        }

        /// <summary>
        /// Creates image from the specified text with html bold tags.
        /// </summary>
        /// <param name="text">String with html bold tags to draw.</param>
        /// <param name="font"><see cref="Font"/> that defines the font of the string.</param>
        /// <param name="scaleFactor">Scale factor.</param>
        /// <param name="foreColor">Foreground color of the text.</param>
        /// <param name="backColor">Background color of the text. Optional. Default is Null.
        /// If Null, background is transparent.</param>
        /// <returns></returns>
        public static Image ImageFromTextWithBoldTag(
            string text,
            Coord scaleFactor,
            Font font,
            Color foreColor,
            Color? backColor = null)
        {
            return ImageFromAction(
                scaleFactor,
                (canvas) =>
                {
                    return canvas.DrawTextWithBoldTags(
                        text,
                        PointD.Empty,
                        font,
                        foreColor,
                        backColor);
                });
        }

        /// <summary>
        /// Sets background of the control's parents to Red, Green, Blue and
        /// Yellow colors.
        /// </summary>
        /// <param name="control">Specifies the control which parent's background
        /// is changed</param>
        public static void SetDebugBackgroundToParents(AbstractControl? control)
        {
            static AbstractControl? SetParentBackground(AbstractControl? control, Brush brush)
            {
                if (control == null)
                    return null;
                AbstractControl? parent = control?.Parent;
                if (parent != null)
                    parent.Background = brush;
                return parent;
            }

            control = SetParentBackground(control, Brushes.Red);
            control = SetParentBackground(control, Brushes.Green);
            control = SetParentBackground(control, Brushes.Blue);
            SetParentBackground(control, Brushes.Yellow);
        }

        /// <inheritdoc cref="IControlPainterHandler.GetCheckBoxSize"/>
        public static SizeD GetCheckBoxSize(
            AbstractControl control,
            CheckState checkState,
            VisualControlState controlState)
        {
            return ControlPainter.Handler.GetCheckBoxSize(control, checkState, controlState);
        }

        /// <inheritdoc cref="IControlPainterHandler.DrawCheckBox"/>
        public static void DrawCheckBox(
            this Graphics canvas,
            AbstractControl control,
            RectD rect,
            CheckState checkState,
            VisualControlState controlState)
        {
            ControlPainter.Handler.DrawCheckBox(
                control,
                canvas,
                rect,
                checkState,
                controlState);
        }

        /// <summary>
        /// Fills rectangle background and draws its border using the specified border settings.
        /// </summary>
        /// <param name="dc"><see cref="Graphics"/> where to draw.</param>
        /// <param name="rect">Rectangle.</param>
        /// <param name="brush">Brush to fill the rectangle.</param>
        /// <param name="border">Border settings.</param>
        /// <param name="hasBorder">Whether border is painted.</param>
        /// <param name="control">Control in which border is painted. Optional.</param>
        public static void FillBorderRectangle(
            this Graphics dc,
            RectD rect,
            Brush? brush,
            BorderSettings? border,
            bool hasBorder = true,
            AbstractControl? control = null)
        {
            if (brush is null && border is null)
                return;

            var radius = border?.GetUniformCornerRadius(rect);

            if (radius is not null && brush is not null)
            {
                var color = border?.Color;
                if (border is null || color is null || !hasBorder)
                {
                    dc.FillRoundedRectangle(brush, rect.InflatedBy(-1, -1), radius.Value);
                }
                else
                {
                    dc.RoundedRectangle(
                        color.AsPen,
                        brush,
                        rect.InflatedBy(-1, -1),
                        radius.Value);
                }

                return;
            }

            if (brush != null)
            {
                dc.FillRectangle(brush, rect);
            }

            if (hasBorder && border is not null)
            {
                DrawBorder(control, dc, rect, border);
            }
        }

        /// <summary>
        /// Draws border in the specified rectangle of the drawing context.
        /// </summary>
        /// <param name="control">Control in which drawing is performed.</param>
        /// <param name="dc">Drawing context.</param>
        /// <param name="rect">Rectangle.</param>
        /// <param name="border">Border settings.</param>
        public static void DrawBorder(
            AbstractControl? control,
            Graphics dc,
            RectD rect,
            BorderSettings? border)
        {
            if (border is null)
                return;

            border.InvokePaint(dc, rect);

            if (!border.DrawDefaultBorder)
                return;

            var radius = border.GetUniformCornerRadius(rect);
            var defaultColor = ColorUtils.GetDefaultBorderColor(control);

            if (radius != null)
            {
                dc.DrawRoundedRectangle(
                    border.Top.GetPen(defaultColor),
                    rect.InflatedBy(-1, -1),
                    radius.Value);
                return;
            }

            var topColor = border.Top.Color ?? defaultColor;
            var bottomColor = border.Bottom.Color ?? defaultColor;
            var leftColor = border.Left.Color ?? defaultColor;
            var rightColor = border.Right.Color ?? defaultColor;

            if (border.Top.Width > 0 && border.ColorIsOk(topColor))
            {
                dc.FillRectangle(topColor.AsBrush, border.GetTopRectangle(rect));
            }

            if (border.Bottom.Width > 0 && border.ColorIsOk(bottomColor))
            {
                dc.FillRectangle(bottomColor.AsBrush, border.GetBottomRectangle(rect));
            }

            if (border.Left.Width > 0 && border.ColorIsOk(leftColor))
            {
                dc.FillRectangle(leftColor.AsBrush, border.GetLeftRectangle(rect));
            }

            if (border.Right.Width > 0 && border.ColorIsOk(rightColor))
            {
                dc.FillRectangle(rightColor.AsBrush, border.GetRightRectangle(rect));
            }
        }

        /// <summary>
        /// Calculates distance between two points.
        /// </summary>
        /// <param name="p1">First point.</param>
        /// <param name="p2">Second point.</param>
        /// <returns>Distance between point <paramref name="p1"/>
        /// and point <paramref name="p2"/>.</returns>
        public static Coord GetDistance(PointD p1, PointD p2)
        {
            return MathUtils.GetDistance(p1.X, p1.Y, p2.X, p2.Y);
        }

        /// <summary>
        /// Compares distance between two points and specified value.
        /// </summary>
        /// <param name="p1">First point.</param>
        /// <param name="p2">Second point.</param>
        /// <param name="value">Value to compare distance with.</param>
        /// <returns><c>true</c> if distance between two points is less
        /// than <paramref name="value"/>; <c>false</c> otherwise.</returns>
        public static bool DistanceIsLess(PointD p1, PointD p2, Coord value)
        {
            return (Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2)) < Math.Pow(value, 2);
        }

        /// <summary>
        /// Gets squared distance between two points.
        /// </summary>
        /// <param name="p1">First point.</param>
        /// <param name="p2">Second point.</param>
        /// <returns></returns>
        public static Coord GetDistanceSquared(PointD p1, PointD p2) =>
            ((Coord)(p1.X - p2.X) * (Coord)(p1.X - p2.X))
            + ((Coord)(p1.Y - p2.Y) * (Coord)(p1.Y - p2.Y));

        /// <summary>
        /// Gets whether point is inside the circle.
        /// </summary>
        /// <param name="p">Point to test.</param>
        /// <param name="center">Circle center.</param>
        /// <param name="radius">Circle radius.</param>
        /// <returns></returns>
        public static bool IsPointInCircle(PointD p, PointD center, Coord radius)
        {
            return GetDistanceSquared(p, center) <= radius * radius;
        }

        /// <summary>
        /// Gets point on circle.
        /// </summary>
        /// <param name="center">Circle center.</param>
        /// <param name="radius">Circle radius.</param>
        /// <param name="angle">Angle.</param>
        /// <returns></returns>
        public static PointD GetPointOnCircle(PointD center, Coord radius, Coord angle)
        {
            return new(
                center.X + (radius * Math.Cos(angle * MathUtils.DegToRad)),
                center.Y + (radius * Math.Sin(angle * MathUtils.DegToRad)));
        }

        /// <summary>
        /// Gets rectangle of the top border edge with the specified width.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        /// <param name="width">Border side width.</param>
        public static RectD GetTopLineRect(this RectD rect, Coord width)
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
        public static RectD GetCenterLineHorz(this RectD rect)
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
        public static RectD GetCenterLineVert(this RectD rect)
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
        public static RectD GetBottomLineRect(this RectD rect, Coord width)
        {
            var point = new PointD(rect.Left, rect.Bottom - width);
            var size = new SizeD(rect.Width, width);
            return new RectD(point, size);
        }

        /// <summary>
        /// Draws horizontal line using <see cref="Graphics.FillRectangle(Brush,RectD)"/>.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <param name="brush">Brush to draw line.</param>
        /// <param name="point">Starting point.</param>
        /// <param name="length">Line length.</param>
        /// <param name="width">Line width.</param>
        public static void DrawHorzLine(
            this Graphics dc,
            Brush brush,
            PointD point,
            Coord length,
            Coord width)
        {
            var rect = new RectD(point, new SizeD(length, width));
            dc.FillRectangle(brush, rect);
        }

        /// <summary>
        /// Draws vertical line using <see cref="Graphics.FillRectangle(Brush,RectD)"/>.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <param name="brush">Brush to draw line.</param>
        /// <param name="point">Starting point.</param>
        /// <param name="length">Line length.</param>
        /// <param name="width">Line width.</param>
        public static void DrawVertLine(
            this Graphics dc,
            Brush brush,
            PointD point,
            Coord length,
            Coord width)
        {
            var rect = new RectD(point, new SizeD(width, length));
            dc.FillRectangle(brush, rect);
        }

        /// <summary>
        /// Draws inner and outer border with the specified colors.
        /// </summary>
        /// <param name="canvas"><see cref="Graphics"/> where drawing is performed.</param>
        /// <param name="rect"><see cref="RectD"/> where drawing is performed.</param>
        /// <param name="innerColor">Inner border color.</param>
        /// <param name="outerColor">Outer border color.</param>
        /// <returns>
        /// Value of the <paramref name="rect"/> parameter deflated by number of the painted
        /// borders (0, 1 or 2).
        /// </returns>
        /// <remarks>
        /// If border color is <see cref="Color.Empty"/> it is not painted.
        /// </remarks>
        public static RectD DrawDoubleBorder(
            this Graphics canvas,
            RectD rect,
            Color innerColor,
            Color outerColor)
        {
            var result = rect;
            var hasOuterBorder = outerColor != Color.Empty;
            var hasInnerBorder = innerColor != Color.Empty;
            if (hasOuterBorder)
            {
                DrawingUtils.FillRectangleBorder(canvas, outerColor.AsBrush, result);
                result.Deflate();
            }

            if (hasInnerBorder)
            {
                DrawingUtils.FillRectangleBorder(canvas, innerColor.AsBrush, result);
                result.Deflate();
            }

            return result;
        }

        /// <summary>
        /// Draws rectangle border using <see cref="Graphics.FillRectangle(Brush,RectD)"/>.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <param name="brush">Brush to draw border.</param>
        /// <param name="rect">Border rectangle.</param>
        /// <param name="borderWidth">Border width.</param>
        public static void FillRectangleBorder(
            this Graphics dc,
            Brush brush,
            RectD rect,
            Coord borderWidth = 1)
        {
            dc.FillRectangle(brush, GetTopLineRect(rect, borderWidth));
            dc.FillRectangle(brush, GetBottomLineRect(rect, borderWidth));
            dc.FillRectangle(brush, GetLeftLineRect(rect, borderWidth));
            dc.FillRectangle(brush, GetRightLineRect(rect, borderWidth));
        }

        /// <summary>
        /// Draws rectangle border using <see cref="Graphics.FillRectangle(Brush,RectD)"/>.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <param name="brush">Brush to draw border.</param>
        /// <param name="rect">Border rectangle.</param>
        /// <param name="borderWidth">Border width.</param>
        public static void FillRectangleBorder(
            this Graphics dc,
            Brush brush,
            RectD rect,
            Thickness borderWidth)
        {
            if (borderWidth.Top > 0)
                dc.FillRectangle(brush, GetTopLineRect(rect, borderWidth.Top));
            if (borderWidth.Bottom > 0)
                dc.FillRectangle(brush, GetBottomLineRect(rect, borderWidth.Bottom));
            if (borderWidth.Left > 0)
                dc.FillRectangle(brush, GetLeftLineRect(rect, borderWidth.Left));
            if (borderWidth.Right > 0)
                dc.FillRectangle(brush, GetRightLineRect(rect, borderWidth.Right));
        }

        /// <summary>
        /// Draws rectangles border using <see cref="Graphics.FillRectangle(Brush,RectD)"/>.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <param name="brush">Brush to draw border.</param>
        /// <param name="rects">Border rectangles.</param>
        /// <param name="borders">Border width.</param>
        public static void FillRectanglesBorder(
            this Graphics dc,
            Brush brush,
            RectD[] rects,
            Thickness[]? borders = null)
        {
            for (int i = 0; i < rects.Length; i++)
            {
                FillRectangleBorder(dc, brush, rects[i], borders?[i] ?? 1);
            }
        }

        /// <summary>
        /// Gets rectangle of the left border edge with the specified width.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        /// <param name="width">Border side width.</param>
        public static RectD GetLeftLineRect(this RectD rect, Coord width)
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
        public static RectD GetRightLineRect(this RectD rect, Coord width)
        {
            var point = new PointD(rect.Right - width, rect.Top);
            var size = new SizeD(width, rect.Height);
            return new RectD(point, size);
        }

        /// <summary>
        /// Performs word wrapping of the text.
        /// </summary>
        /// <param name="text">Text to wrap.</param>
        /// <param name="maxWidth">Width of the text in device-independent units.</param>
        /// <param name="font">Text font.</param>
        /// <param name="canvas">Drawing context.</param>
        /// <returns></returns>
        public static string WrapTextToMultipleLines(
            string text,
            Coord maxWidth,
            Font font,
            Graphics canvas)
        {
            var list = WrapTextToList(text, maxWidth, font, canvas);
            if (list.Count == 0)
                return string.Empty;
            if (list.Count == 1)
                return list[0];
            StringBuilder result = new();
            bool firstLine = true;
            foreach (var s in list)
            {
                if (!firstLine)
                    result.AppendLine();
                result.Append(s);
                firstLine = false;
            }

            return result.ToString();
        }

        /// <summary>
        /// Calculates total size of the text strings.
        /// </summary>
        /// <param name="text">Collection of the strings.</param>
        /// <param name="font">Font.</param>
        /// <param name="canvas">Drawing context.</param>
        /// <returns></returns>
        /// <param name="lineDistance">Distance between lines of text. Optional. Default is 0.</param>
        public static SizeD MeasureText(
            IEnumerable text,
            Font font,
            Graphics canvas,
            Coord lineDistance = 0)
        {
            Coord width = 0;
            Coord height = 0;

            foreach(var s in text)
            {
                var size = canvas.GetTextExtent(s.ToString(), font);
                width = Math.Max(width, size.Width);
                height += size.Height + lineDistance;
            }

            return (width, height);
        }

        /// <summary>
        /// Performs word wrapping of the multiple text lines which can contain new line characters.
        /// </summary>
        /// <param name="text">Text to wrap.</param>
        /// <param name="maxWidth">Max width of the text in device-independent units.</param>
        /// <param name="font">Text font.</param>
        /// <param name="canvas">Graphics context.</param>
        /// <returns></returns>
        public static List<string> WrapTextToList(
            string text,
            Coord? maxWidth,
            Font font,
            Graphics canvas)
        {
            List<string> result = new();

            var splitted = StringUtils.Split(text, false);

            if(maxWidth is null)
            {
                result.AddRange(splitted);
                return result;
            }

            foreach(var s in splitted)
            {
                var wrappedLine = WrapTextLineToList(s, maxWidth.Value, font, canvas);
                result.AddRange(wrappedLine);
            }

            return result;
        }

        /// <summary>
        /// Performs word wrapping of the single text line which doesn't contain new line characters.
        /// </summary>
        /// <param name="text">Text to wrap.</param>
        /// <param name="maxWidth">Max width of the text in device-independent units.</param>
        /// <param name="font">Text font.</param>
        /// <param name="canvas">Graphics context.</param>
        /// <returns></returns>
        public static List<string> WrapTextLineToList(
            string text,
            Coord maxWidth,
            Font font,
            Graphics canvas)
        {
            List<string> wrappedLines = new();

            if (string.IsNullOrEmpty(text))
                return wrappedLines;

            var spaceWidth = canvas.MeasureText(StringUtils.OneSpace, font).Width;

            string[] originalLines = text.Split(' ');

            StringBuilder actualLine = new();
            Coord actualWidth = 0;

            foreach (var item in originalLines)
            {
                var measureResult = canvas.MeasureText(item, font);
                measureResult.Width += spaceWidth;
                var w = measureResult.Width;
                actualWidth += w;

                if (actualWidth > maxWidth)
                {
                    wrappedLines.Add(actualLine.ToString());
                    actualLine.Clear();
                    actualWidth = w;
                }

                actualLine.Append(item + StringUtils.OneSpace);
            }

            if (actualLine.Length > 0)
                wrappedLines.Add(actualLine.ToString());

            return wrappedLines;
        }

        /// <summary>
        /// Draws sliced image with the specified
        /// <see cref="NinePatchImagePaintParams"/> parameters. This method can be used,
        /// for example, for drawing complex button bakgrounds using predefined templates.
        /// </summary>
        /// <param name="canvas"><see cref="Graphics"/> where to draw.</param>
        /// <param name="e">Draw parameters.</param>
        /// <remarks>
        /// Source image is sliced into 9 pieces. All parts of the image except corners
        /// (top-left, top-right, bottom-right, bottom-left parts) are used
        /// by <see cref="TextureBrush"/> to fill larger destination rectangle.
        /// </remarks>
        /// <remarks>
        /// Issue with details is here:
        /// <see href="https://github.com/alternetsoft/AlternetUI/issues/115"/>.
        /// </remarks>
        internal static void DrawImageSliced(this Graphics canvas, NinePatchImagePaintParams e)
        {
            var src = e.SourceRect;
            var dst = e.DestRect;
            var patchSrc = e.PatchRect;

            var offsetX = patchSrc.X - src.X;
            var offsetY = patchSrc.Y - src.Y;

            RectI patchDst = patchSrc;

            NineRects srcNine = new(src, patchSrc);

            patchDst.X = dst.X + offsetX;
            patchDst.Y = dst.Y + offsetY;
            patchDst.Width = dst.Width - (src.Width - patchSrc.Width);
            patchDst.Height = dst.Height - (src.Height - patchSrc.Height);

            NineRects dstNine = new(dst, patchDst);

            CopyRect(srcNine.Center, dstNine.Center);
            CopyRect(srcNine.TopCenter, dstNine.TopCenter);
            CopyRect(srcNine.BottomCenter, dstNine.BottomCenter);
            CopyRect(srcNine.CenterLeft, dstNine.CenterLeft);
            CopyRect(srcNine.CenterRight, dstNine.CenterRight);

            canvas.DrawImage(e.Image, dstNine.TopLeft, srcNine.TopLeft, GraphicsUnit.Pixel);
            canvas.DrawImage(e.Image, dstNine.TopRight, srcNine.TopRight, GraphicsUnit.Pixel);
            canvas.DrawImage(e.Image, dstNine.BottomLeft, srcNine.BottomLeft, GraphicsUnit.Pixel);
            canvas.DrawImage(e.Image, dstNine.BottomRight, srcNine.BottomRight, GraphicsUnit.Pixel);

            void CopyRect(RectI srcRect, RectI dstRect)
            {
                if (e.Tile)
                {
                    var subImage = e.Image.GetSubBitmap(srcRect);
                    var brush = subImage.AsBrush;
                    canvas.FillRectangle(brush, dstRect, GraphicsUnit.Pixel);
                }
                else
                {
                    canvas.DrawImage(e.Image, dstRect, srcRect, GraphicsUnit.Pixel);
                }
            }
        }
    }
}