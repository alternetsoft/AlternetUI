using System;
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
        /// Gets or sets space characters used to split strings when they are word wrapped.
        /// </summary>
        public static char[] SpaceCharsUsedToSplit = new char[] { ' ', '\u00A0' };

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
        /// Creates image from the specified array of <see cref="TextAndFontStyle"/>.
        /// </summary>
        /// <param name="text">Array of strings with font styles.</param>
        /// <param name="font"><see cref="Font"/> that defines the font of the string.</param>
        /// <param name="scaleFactor">Scale factor.</param>
        /// <param name="foreColor">Foreground color of the text.</param>
        /// <param name="backColor">Background color of the text. Optional. Default is Null.
        /// If Null, background is transparent.</param>
        /// <returns></returns>
        /// <returns></returns>
        public static Image ImageFromTextWithFontStyle(
            TextAndFontStyle[] text,
            Coord scaleFactor,
            Font font,
            Color foreColor,
            Color? backColor = null)
        {
            return ImageFromAction(
                scaleFactor,
                (canvas) =>
                {
                    return canvas.DrawTextWithFontStyle(
                        text,
                        PointD.Empty,
                        font,
                        foreColor,
                        backColor);
                });
        }

        /// <summary>
        /// Creates debug image from the specified array of <see cref="TextAndFontStyle"/>.
        /// </summary>
        /// <param name="text">Array of strings with font styles.</param>
        public static Image DebugImageFromTextWithFontStyle(TextAndFontStyle[] text)
        {
            return ImageFromTextWithFontStyle(
                        text,
                        Display.MaxScaleFactor,
                        Control.DefaultFont,
                        Color.Black,
                        Color.White);
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
            return ControlPaint.Handler.GetCheckBoxSize(control, checkState, controlState);
        }

        /// <inheritdoc cref="IControlPainterHandler.DrawCheckBox"/>
        public static void DrawCheckBox(
            this Graphics canvas,
            AbstractControl control,
            RectD rect,
            CheckState checkState,
            VisualControlState controlState)
        {
            ControlPaint.Handler.DrawCheckBox(
                control,
                canvas,
                rect,
                checkState,
                controlState);
        }

        /// <inheritdoc cref="IControlPainterHandler.DrawPushButton"/>
        public static void DrawPushButton(
            this Graphics canvas,
            AbstractControl control,
            RectD rect,
            VisualControlState controlState)
        {
            ControlPaint.Handler.DrawPushButton(
                control,
                canvas,
                rect,
                controlState);
        }

        /// <summary>
        /// Draw push button using the specified parameters.
        /// </summary>
        /// <param name="sender">The container control where drawing is performed.</param>
        /// <param name="e">The paint arguments including canvas and bounding rectangle.</param>
        /// <param name="controlState">The state of the push button.</param>
        public static void DrawPushButton(
            object? sender,
            PaintEventArgs e,
            VisualControlState controlState)
        {
            if (sender is not AbstractControl control)
            {
                return;
            }

            DrawPushButton(
                        e.Graphics,
                        control,
                        e.ClipRectangle,
                        controlState);
        }

        /// <summary>
        /// Draw push button in the <see cref="VisualControlState.Normal"/> state
        /// using the specified parameters.
        /// </summary>
        /// <param name="sender">The container control where drawing is performed.</param>
        /// <param name="e">The paint arguments including canvas and bounding rectangle.</param>
        public static void DrawPushButtonNormal(object? sender, PaintEventArgs e)
        {
            DrawPushButton(sender, e, VisualControlState.Normal);
        }

        /// <summary>
        /// Draw push button in the <see cref="VisualControlState.Hovered"/> state
        /// using the specified parameters.
        /// </summary>
        /// <param name="sender">The container control where drawing is performed.</param>
        /// <param name="e">The paint arguments including canvas and bounding rectangle.</param>
        public static void DrawPushButtonHovered(object? sender, PaintEventArgs e)
        {
            DrawPushButton(sender, e, VisualControlState.Hovered);
        }

        /// <summary>
        /// Draw push button in the <see cref="VisualControlState.Pressed"/> state
        /// using the specified parameters.
        /// </summary>
        /// <param name="sender">The container control where drawing is performed.</param>
        /// <param name="e">The paint arguments including canvas and bounding rectangle.</param>
        public static void DrawPushButtonPressed(object? sender, PaintEventArgs e)
        {
            DrawPushButton(sender, e, VisualControlState.Pressed);
        }

        /// <summary>
        /// Draw push button in the <see cref="VisualControlState.Disabled"/> state
        /// using the specified parameters.
        /// </summary>
        /// <param name="sender">The container control where drawing is performed.</param>
        /// <param name="e">The paint arguments including canvas and bounding rectangle.</param>
        public static void DrawPushButtonDisabled(object? sender, PaintEventArgs e)
        {
            DrawPushButton(sender, e, VisualControlState.Disabled);
        }

        /// <summary>
        /// Draw push button in the <see cref="VisualControlState.Focused"/> state
        /// using the specified parameters.
        /// </summary>
        /// <param name="sender">The container control where drawing is performed.</param>
        /// <param name="e">The paint arguments including canvas and bounding rectangle.</param>
        public static void DrawPushButtonFocused(object? sender, PaintEventArgs e)
        {
            DrawPushButton(sender, e, VisualControlState.Focused);
        }

        /// <inheritdoc cref="IControlPainterHandler.DrawRadioButton"/>
        public static void DrawRadioButton(
            this Graphics canvas,
            AbstractControl control,
            RectD rect,
            bool isChecked,
            VisualControlState controlState)
        {
            ControlPaint.Handler.DrawRadioButton(
                control,
                canvas,
                rect,
                isChecked,
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
            if (brush is null && (border is null || !hasBorder))
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
                DrawingUtils.DrawBorderWithBrush(canvas, outerColor.AsBrush, result);
                result.Deflate();
            }

            if (hasInnerBorder)
            {
                DrawingUtils.DrawBorderWithBrush(canvas, innerColor.AsBrush, result);
                result.Deflate();
            }

            return result;
        }

        /// <summary>
        /// Draws rectangle border using the specified brush and
        /// <see cref="Graphics.FillRectangle(Brush,RectD)"/>. This method doesn't fill background,
        /// it only draws border.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <param name="brush">Brush to draw border.</param>
        /// <param name="rect">Border rectangle.</param>
        /// <param name="borderWidth">Border width.</param>
        public static void DrawBorderWithBrush(
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
        /// Draws rectangle border using the specified brush and
        /// <see cref="Graphics.FillRectangle(Brush,RectD)"/>. This method doesn't fill background,
        /// it only draws border.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <param name="brush">Brush to draw border.</param>
        /// <param name="rect">Border rectangle.</param>
        /// <param name="borderWidth">Border width.</param>
        public static void DrawBorderWithBrush(
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
        /// <param name="rectangles">Border rectangles.</param>
        /// <param name="borders">Border width.</param>
        public static void DrawBordersWithBrush(
            this Graphics dc,
            Brush brush,
            RectD[] rectangles,
            Thickness[]? borders = null)
        {
            for (int i = 0; i < rectangles.Length; i++)
            {
                DrawBorderWithBrush(dc, brush, rectangles[i], borders?[i] ?? 1);
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
            StringBuilder result = new(text.Length);
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

            foreach (var s in text)
            {
                var size = canvas.GetTextExtent(s.ToString(), font);
                width = Math.Max(width, size.Width);
                height += size.Height + lineDistance;
            }

            return (width, height);
        }

        /// <summary>
        /// Performs word wrapping of the multiple text lines which can contain
        /// new line characters.
        /// </summary>
        /// <param name="text">Text to wrap.</param>
        /// <param name="maxWidth">Max width of the text in device-independent units.</param>
        /// <param name="font">Text font.</param>
        /// <param name="canvas">Graphics context.</param>
        /// <returns></returns>
        public static IEnumerable<string> WrapTextToList(
            string text,
            Coord maxWidth,
            Font font,
            Graphics canvas)
        {
            var splitText = StringUtils.Split(text, false);

            if (maxWidth <= 0)
            {
                foreach (var s in splitText)
                {
                    yield return s;
                }
            }

            foreach (var s in splitText)
            {
                var wrappedLine = WrapTextLineToList(s, maxWidth, font, canvas);
                foreach (var ws in wrappedLine)
                {
                    yield return ws;
                }
            }
        }

        /// <summary>
        /// Performs word wrapping of the single text line which doesn't contain new line characters.
        /// </summary>
        /// <param name="text">Text to wrap.</param>
        /// <param name="maxWidth">Max width of the text in device-independent units.</param>
        /// <param name="font">Text font.</param>
        /// <param name="canvas">Graphics context.</param>
        /// <param name="splitOptions">The string split options.</param>
        /// <returns></returns>
        public static IEnumerable<string> WrapTextLineToList(
            string text,
            Coord maxWidth,
            Font font,
            Graphics canvas,
            StringSplitOptions splitOptions = StringSplitOptions.None)
        {
            if (text is null || text.Length == 0)
            {
                yield break;
            }

            var words = text.Split(SpaceCharsUsedToSplit, splitOptions);
            var line = string.Empty;

            foreach (var word in words)
            {
                var lineAndWord = line + word;
                var measureResult = GetWidth(lineAndWord);

                if (measureResult > maxWidth)
                {
                    yield return line;
                    line = word;
                }
                else
                {
                    line += (line.Length > 0 ? " " : string.Empty) + word;
                }
            }

            yield return line;

            Coord GetWidth(string s)
            {
                return canvas.MeasureText(s, font).Ceiling().Width;
            }
        }
    }
}
