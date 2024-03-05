using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Alternet.UI;
using Alternet.UI.Internal.ComponentModel;
using Alternet.UI.Localization;

namespace Alternet.Drawing
{
    /// <summary>
    /// Describes a drawing surface.
    /// </summary>
    public class Graphics : DisposableObject
    {
        private readonly bool dispose;
        private UI.Native.DrawingContext dc;

        internal Graphics(UI.Native.DrawingContext dc, bool dispose = true)
        {
            this.dc = dc;
            this.dispose = dispose;
        }

        /// <summary>
        /// Gets or sets name of the <see cref="Graphics"/> for the debug purposes.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets a copy of the geometric world transformation for this
        /// <see cref="Graphics"/>.
        /// </summary>
        public TransformMatrix Transform
        {
            get
            {
                return new TransformMatrix(dc.Transform);
            }

            set
            {
                dc.Transform = value.NativeMatrix;
            }
        }

        /// <summary>
        /// Gets or sets a <see cref="Region"/> that limits the drawing region of this
        /// <see cref="Graphics"/>.
        /// </summary>
        /// <value>
        /// A <see cref="Region"/> that limits the portion of this <see cref="Graphics"/>
        /// that is currently available for drawing.
        /// </value>
        public Region? Clip
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
                dc.Clip = value?.NativeRegion;
            }
        }

        /// <summary>
        /// Gets or sets the interpolation mode associated with this <see cref="Graphics"/>.
        /// </summary>
        /// <value>One of the <see cref="InterpolationMode"/> values.</value>
        /// <remarks>
        /// The interpolation mode determines how intermediate values between two endpoints
        /// are calculated.
        /// </remarks>
        public InterpolationMode InterpolationMode
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

        internal UI.Native.DrawingContext NativeDrawingContext
        {
            get => dc;
            set => dc = value;
        }

        /// <summary>
        /// Creates a new <see cref="Graphics"/> from the specified
        /// <see cref="Image"/>.
        /// </summary>
        /// <param name="image"><see cref="Image"/> from which to create the
        /// new <see cref="Graphics"/>.</param>
        /// <returns>A new <see cref="Graphics"/> for the specified
        /// <see cref="Image"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="image"/>
        /// is <see langword="null"/>.</exception>
        /// <remarks>
        /// Use this method to draw on the specified image.
        /// You should always call the Dispose() method to release
        /// the <see cref="Graphics"/> and
        /// related resources created by the <see cref="FromImage"/> method.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Graphics FromImage(Image image)
        {
            DebugImageAssert(image);
            return new Graphics(UI.Native.DrawingContext.FromImage(image.NativeImage));
        }

        /// <summary>
        /// Creates <see cref="Graphics"/> that can be used to paint on the screen.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Graphics FromScreen()
        {
            return new Graphics(UI.Native.DrawingContext.FromScreen());
        }

        /// <summary>
        /// Draws the text rotated by angle degrees (positive angles are counterclockwise;
        /// the full angle is 360 degrees) with the specified font, background and
        /// foreground colors.
        /// </summary>
        /// <param name="location">Location used to draw the text. Specified in pixels.</param>
        /// <param name="text">Text to draw.</param>
        /// <param name="angle">Text angle.</param>
        /// <param name="font">Font used to draw the text.</param>
        /// <param name="foreColor">Foreground color of the text.</param>
        /// <param name="backColor">Background color of the text. If parameter is equal
        /// to <see cref="Color.Empty"/>, background will not be painted. </param>
        /// <remarks>
        /// Notice that, as with other draw text methods, the text can contain multiple
        /// lines separated by the new line('\n') characters.
        /// </remarks>
        /// <remarks>
        /// Under Windows only TrueType fonts can be drawn by this function.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawRotatedTextI(
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
                font.NativeFont,
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

        /// <summary>
        /// Copy from a source <see cref="Graphics"/> to this graphics.
        /// With this method you can specify the destination coordinates and the
        /// size of area to copy which will be the same for both the source and target.
        /// If you need to apply scaling while copying, use <see cref="StretchBlitI"/>.
        /// Sizes and positions are specified in pixels.
        /// </summary>
        /// <param name="destPt">Destination device context position.</param>
        /// <param name="sz">Size of source area to be copied.</param>
        /// <param name="source">Source device context.</param>
        /// <param name="srcPt">Source device context position.</param>
        /// <param name="rop">Logical function to use.</param>
        /// <param name="useMask">If true, Blit does a transparent blit using the mask
        /// that is associated with the bitmap selected into the source device context.</param>
        /// <param name="srcPtMask">Source position on the mask. If it equals (-1, -1),
        /// <paramref name="srcPt"/> will be assumed for the mask source position.
        /// Currently only implemented on Windows.</param>
        /// <returns></returns>
        /// <remarks>
        /// Notice that source coordinate <paramref name="srcPt"/> is interpreted
        /// using the current source DC coordinate system, i.e. the scale, origin position
        /// and axis directions are taken into account when transforming them to
        /// physical(pixel) coordinates.
        /// </remarks>
        /// <remarks>
        /// There is partial support for Blit in PostScript DC, under X.
        /// </remarks>
        /// <remarks>
        /// You can influence whether MaskBlt or the explicit mask blitting code
        /// is used, by using <see cref="Application.SetSystemOption(string, int)"/>
        /// and setting the 'no-maskblt' option to 1.
        /// </remarks>
        /// <remarks>
        /// The Windows implementation does the following if MaskBlt cannot be used:
        /// <list type="bullet">
        /// <listheader><term>Operations:</term></listheader>
        ///     <item>The library or executable built from a compilation.</item>
        /// <item>Creates a temporary bitmap and copies the destination area into it.</item>
        /// <item>Copies the source area into the temporary bitmap using the
        /// specified logical function.</item>
        /// <item>Sets the masked area in the temporary bitmap to BLACK by ANDing the mask bitmap
        /// with the temp bitmap with the foreground color set to WHITE and the bg color
        /// set to BLACK.</item>
        /// <item>Sets the unmasked area in the destination area to BLACK by ANDing
        /// the mask bitmap
        /// with the destination area with the foreground color set to BLACK and the background
        /// color set to WHITE.</item>
        /// <item>ORs the temporary bitmap with the destination area.</item>
        /// <item>Deletes the temporary bitmap.</item>
        /// </list>
        /// This sequence of operations ensures that the source's transparent area
        /// need not be black, and logical functions are supported.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool BlitI(
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
                        source.NativeDrawingContext,
                        srcPt,
                        (int)rop,
                        useMask,
                        srcPtMask.Value);
        }

        /// <summary>
        /// Copies from a source <see cref="Graphics"/> to this graphics
        /// possibly changing the scale. Unlike <see cref="BlitI"/>, this method
        /// allows specifying different source and destination region sizes,
        /// meaning that it can stretch or shrink it while copying.
        /// The meaning of its other parameters is the same as with <see cref="BlitI"/>.
        /// Sizes and positions are specified in pixels.
        /// </summary>
        /// <param name="dstPt">Destination device context position.</param>
        /// <param name="srcSize">Size of source area to be copied.</param>
        /// <param name="dstSize">Size of destination area.</param>
        /// <param name="source">Source device context.</param>
        /// <param name="srcPt">Source device context position.</param>
        /// <param name="rop">Logical function to use.</param>
        /// <param name="useMask">If true, Blit does a transparent blit using the mask
        /// that is associated with the bitmap selected into the source device context.</param>
        /// <param name="srcPtMask">Source position on the mask. If it equals (-1, -1),
        /// <paramref name="srcPt"/> will be assumed for the mask source position.
        /// Currently only implemented on Windows.</param>
        /// <returns></returns>
        /// <remarks>
        /// Notice that source coordinate <paramref name="srcPt"/> is interpreted
        /// using the current source DC coordinate system, i.e. the scale, origin position
        /// and axis directions are taken into account when transforming them to
        /// physical(pixel) coordinates.
        /// </remarks>
        /// <remarks>
        /// There is partial support for Blit in PostScript DC, under X.
        /// </remarks>
        /// <remarks>
        /// You can influence whether MaskBlt or the explicit mask blitting code
        /// is used, by using <see cref="Application.SetSystemOption(string, int)"/>
        /// and setting the 'no-maskblt' option to 1.
        /// </remarks>
        /// <remarks>
        /// The Windows implementation does the following if MaskBlt cannot be used:
        /// <list type="bullet">
        /// <listheader><term>Operations:</term></listheader>
        ///     <item>The library or executable built from a compilation.</item>
        /// <item>Creates a temporary bitmap and copies the destination area into it.</item>
        /// <item>Copies the source area into the temporary bitmap using the
        /// specified logical function.</item>
        /// <item>Sets the masked area in the temporary bitmap to BLACK by ANDing the mask bitmap
        /// with the temp bitmap with the foreground color set to WHITE and the bg color
        /// set to BLACK.</item>
        /// <item>Sets the unmasked area in the destination area to BLACK by ANDing
        /// the mask bitmap
        /// with the destination area with the foreground color set to BLACK and the background
        /// color set to WHITE.</item>
        /// <item>ORs the temporary bitmap with the destination area.</item>
        /// <item>Deletes the temporary bitmap.</item>
        /// </list>
        /// This sequence of operations ensures that the source's transparent area
        /// need not be black, and logical functions are supported.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool StretchBlitI(
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
                source.NativeDrawingContext,
                srcPt,
                srcSize,
                (int)rop,
                useMask,
                srcPtMask.Value);
        }

        /// <summary>
        /// Calls <see cref="FillRoundedRectangle"/> and than <see cref="DrawRoundedRectangle"/>.
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="brush"></param>
        /// <param name="rectangle"></param>
        /// <param name="cornerRadius"></param>
        /// <remarks>
        /// This method works faster than fill and then draw.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RoundedRectangle(Pen pen, Brush brush, RectD rectangle, double cornerRadius)
        {
            dc.RoundedRectangle(pen.NativePen, brush.NativeBrush, rectangle, cornerRadius);
        }

        /// <summary>
        /// Gets the dimensions of the string using the specified font.
        /// </summary>
        /// <param name="text">The text string to measure.</param>
        /// <param name="font">The Font used to get text dimensions.</param>
        /// <param name="control">The control used to get scaling factor. Optional.</param>
        /// <param name="descent">Dimension from the baseline of the font to
        /// the bottom of the descender (the size of the tail below the baseline).</param>
        /// <param name="externalLeading">Any extra vertical space added to the
        /// font by the font designer (inter-line interval).</param>
        /// <returns><see cref="SizeD"/> with the total calculated width and height
        /// of the text.</returns>
        /// <remarks>
        /// This function only works with single-line strings.
        /// It works faster than MeasureText methods.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SizeD GetTextExtent(
            string text,
            Font font,
            out double descent,
            out double externalLeading,
            Control? control = null)
        {
            var result = dc.GetTextExtent(
                text,
                font.NativeFont,
                control is null ? default : control.WxWidget);
            descent = result.X;
            externalLeading = result.Y;
            return result.Size;
        }

        /// <summary>
        /// Gets the dimensions of the string using the specified font.
        /// </summary>
        /// <param name="text">The text string to measure.</param>
        /// <param name="font">The Font used to get text dimensions.</param>
        /// <param name="control">The control used to get scaling factor. Can be null.</param>
        /// <returns><see cref="SizeD"/> with the total calculated width and height
        /// of the text.</returns>
        /// <remarks>
        /// This function only works with single-line strings.
        /// It works faster than MeasureText methods.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SizeD GetTextExtent(
            string text,
            Font font,
            Control? control)
        {
            var result = dc.GetTextExtentSimple(
                text,
                font.NativeFont,
                control is null ? default : control.WxWidget);
            return result;
        }

        /// <summary>
        /// Gets the dimensions of the string using the specified font.
        /// </summary>
        /// <param name="text">The text string to measure.</param>
        /// <param name="font">The Font used to get text dimensions.</param>
        /// <returns><see cref="SizeD"/> with the total calculated width and height
        /// of the text.</returns>
        /// <remarks>
        /// This function only works with single-line strings.
        /// It works faster than MeasureText methods.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SizeD GetTextExtent(string text, Font font)
        {
            var result = dc.GetTextExtentSimple(text, font.NativeFont, default);
            return result;
        }

        /// <summary>
        /// Calls <see cref="FillRectangle"/> and than <see cref="DrawRectangle"/>.
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="brush"></param>
        /// <param name="rectangle"></param>
        /// <remarks>
        /// This method works faster than fill and then draw.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Rectangle(Pen pen, Brush brush, RectD rectangle)
        {
            DebugBrushAssert(brush);
            DebugPenAssert(pen);
            dc.Rectangle(pen.NativePen, brush.NativeBrush, rectangle);
        }

        /// <summary>
        /// Calls <see cref="FillEllipse"/> and than <see cref="DrawEllipse"/>.
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="brush"></param>
        /// <param name="rectangle"></param>
        /// <remarks>
        /// This method works faster than fill and then draw.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Ellipse(Pen pen, Brush brush, RectD rectangle)
        {
            DebugBrushAssert(brush);
            DebugPenAssert(pen);
            dc.Ellipse(pen.NativePen, brush.NativeBrush, rectangle);
        }

        /// <summary>
        /// Calls <see cref="FillPath"/> and than <see cref="DrawPath"/>.
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="brush"></param>
        /// <param name="path"></param>
        /// <remarks>
        /// This method works faster than fill and then draw.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Path(Pen pen, Brush brush, GraphicsPath path)
        {
            DebugBrushAssert(brush);
            DebugPenAssert(pen);
            dc.Path(pen.NativePen, brush.NativeBrush, path.NativePath);
        }

        /// <summary>
        /// Calls <see cref="FillPie"/> and than <see cref="DrawPie"/>.
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="brush"></param>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="startAngle"></param>
        /// <param name="sweepAngle"></param>
        /// <remarks>
        /// This method works faster than fill and then draw.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Pie(
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
                pen.NativePen,
                brush.NativeBrush,
                center,
                radius,
                startAngle,
                sweepAngle);
        }

        /// <summary>
        /// Calls <see cref="FillCircle"/> and than <see cref="DrawCircle"/>.
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="brush"></param>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <remarks>
        /// This method works faster than fill and then draw.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Circle(Pen pen, Brush brush, PointD center, double radius)
        {
            DebugBrushAssert(brush);
            DebugPenAssert(pen);
            dc.Circle(pen.NativePen, brush.NativeBrush, center, radius);
        }

        /// <summary>
        /// Calls <see cref="FillPolygon"/> and than <see cref="DrawPolygon"/>.
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="brush"></param>
        /// <param name="points"></param>
        /// <param name="fillMode"></param>
        /// <remarks>
        /// This method works faster than fill and then draw.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Polygon(Pen pen, Brush brush, PointD[] points, FillMode fillMode)
        {
            DebugBrushAssert(brush);
            DebugPenAssert(pen);
            dc.Polygon(pen.NativePen, brush.NativeBrush, points, (UI.Native.FillMode)fillMode);
        }

        /// <summary>
        /// Fills the interior of a rectangle specified by a <see cref="RectD"/> structure.
        /// Rectangle is specified in dips (1/96 inch).
        /// </summary>
        /// <param name="brush"><see cref="Brush"/> that determines the characteristics
        /// of the fill.</param>
        /// <param name="rectangle"><see cref="RectD"/> structure that represents the
        /// rectangle to fill.</param>
        /// <remarks>
        /// This method fills the interior of the rectangle defined by the <c>rect</c> parameter,
        /// including the specified upper-left corner and up to the calculated
        /// lower and bottom edges.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillRectangle(Brush brush, RectD rectangle)
        {
            DebugBrushAssert(brush);
            dc.FillRectangle(brush.NativeBrush, rectangle);
        }

        /// <summary>
        /// Fills the interior of a rectangle specified by a <see cref="RectI"/> structure.
        /// Rectangle is specified in pixels.
        /// </summary>
        /// <param name="brush"><see cref="Brush"/> that determines the characteristics
        /// of the fill.</param>
        /// <param name="rectangle"><see cref="RectI"/> structure that represents the
        /// rectangle to fill.</param>
        /// <remarks>
        /// This method fills the interior of the rectangle defined by the <c>rect</c> parameter,
        /// including the specified upper-left corner and up to the calculated lower and
        /// bottom edges.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillRectangleI(Brush brush, RectI rectangle)
        {
            DebugBrushAssert(brush);
            dc.FillRectangleI(brush.NativeBrush, rectangle);
        }

        /// <summary>
        /// Draws an arc representing a portion of a circle specified by a center
        /// <see cref="PointD"/> and a radius.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the arc.</param>
        /// <param name="center"><see cref="PointD"/> structure that defines the center of the
        /// circle.</param>
        /// <param name="radius">Defines the radius of the circle.</param>
        /// <param name="startAngle">Angle in degrees measured clockwise from the x-axis to
        /// the starting point of the arc.</param>
        /// <param name="sweepAngle">Angle in degrees measured clockwise from the
        /// <paramref name="startAngle"/>
        /// parameter to ending point of the arc.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawArc(Pen pen, PointD center, double radius, double startAngle, double sweepAngle)
        {
            DebugPenAssert(pen);
            dc.DrawArc(pen.NativePen, center, radius, startAngle, sweepAngle);
        }

        /// <summary>
        /// Draws debug points on the corners of the specified rectangle.
        /// </summary>
        /// <param name="pen">Color of the debug points. if <c>null</c>, red color is used.</param>
        /// <param name="rect"></param>
        [Conditional("DEBUG")]
        public void DrawDebugPoints(RectD rect, Pen? pen = null)
        {
            void DrawDebugPoint(PointD p)
            {
                DrawPoint(pen, p.X, p.Y);
            }

            pen ??= Pens.Red;

            DrawDebugPoint(rect.TopLeft);
            DrawDebugPoint(new PointD(rect.Right - 1, rect.Top));
            DrawDebugPoint(new PointD(rect.Right - 1, rect.Bottom - 1));
            DrawDebugPoint(new PointD(rect.Left, rect.Bottom - 1));
        }

        /// <summary>
        /// Draws point with the specified color.
        /// </summary>
        /// <param name="pen">Color of the point.</param>
        /// <param name="x">X-coordinate of the point.</param>
        /// <param name="y">Y-coordinate of the point.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="pen"/> is <c>null</c>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawPoint(Pen pen, double x, double y)
        {
            DebugPenAssert(pen);
            dc.DrawPoint(pen.NativePen, x, y);
        }

        /// <summary>
        /// Fills the interior of a pie section defined by a circle specified by a center
        /// <see cref="PointD"/> and a radius.
        /// </summary>
        /// <param name="brush"><see cref="Brush"/> that determines the characteristics of
        /// the fill.</param>
        /// <param name="center"><see cref="PointD"/> structure that defines the center of
        /// the circle.</param>
        /// <param name="radius">Defines the radius of the circle.</param>
        /// <param name="startAngle">Angle in degrees measured clockwise from the x-axis
        /// to the first side of the pie section.</param>
        /// <param name="sweepAngle">Angle in degrees measured clockwise from the startAngle
        /// parameter to the second side of the pie section.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillPie(
            Brush brush,
            PointD center,
            double radius,
            double startAngle,
            double sweepAngle)
        {
            DebugBrushAssert(brush);
            dc.FillPie(brush.NativeBrush, center, radius, startAngle, sweepAngle);
        }

        /// <summary>
        /// Draws an outline of a pie section defined by a circle specified by a center
        /// <see cref="PointD"/> and a radius.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and
        /// style of the pie section.</param>
        /// <param name="center"><see cref="PointD"/> structure that defines the center
        /// of the circle.</param>
        /// <param name="radius">Defines the radius of the circle.</param>
        /// <param name="startAngle">Angle in degrees measured clockwise from the x-axis
        /// to the first side of the pie section.</param>
        /// <param name="sweepAngle">Angle in degrees measured clockwise from the startAngle
        /// parameter to the second side of the pie section.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawPie(
            Pen pen,
            PointD center,
            double radius,
            double startAngle,
            double sweepAngle)
        {
            DebugPenAssert(pen);
            dc.DrawPie(pen.NativePen, center, radius, startAngle, sweepAngle);
        }

        /// <summary>
        /// Draws a Bézier spline defined by four <see cref="PointD"/> structures.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the curve.</param>
        /// <param name="startPoint"><see cref="PointD"/> structure that represents the starting
        /// point of the curve.</param>
        /// <param name="controlPoint1"><see cref="PointD"/> structure that represents the first
        /// control point for the curve.</param>
        /// <param name="controlPoint2"><see cref="PointD"/> structure that represents the second
        /// control point for the curve.</param>
        /// <param name="endPoint"><see cref="PointD"/> structure that represents the ending point
        /// of the curve.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawBezier(
            Pen pen,
            PointD startPoint,
            PointD controlPoint1,
            PointD controlPoint2,
            PointD endPoint)
        {
            DebugPenAssert(pen);
            dc.DrawBezier(pen.NativePen, startPoint, controlPoint1, controlPoint2, endPoint);
        }

        /// <summary>
        /// Draws a series of Bézier splines from an array of <see cref="PointD"/> structures.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the curve.</param>
        /// <param name="points">
        /// Array of <see cref="PointD"/> structures that represent the points that determine the curve.
        /// The number of points in the array should be a multiple of 3 plus 1, such as 4, 7, or 10.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawBeziers(Pen pen, PointD[] points)
        {
            DebugPenAssert(pen);
#if DEBUG
            if (points.Length == 0)
                return;

            if ((points.Length - 1) % 3 != 0)
            {
                throw new ArgumentException(
                    "The number of points should be a multiple of 3 plus 1, such as 4, 7, or 10.",
                    nameof(points));
            }
#endif
            dc.DrawBeziers(pen.NativePen, points);
        }

        /// <summary>
        /// Draws an circle specified by a center <see cref="PointD"/> and a radius.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the circle.</param>
        /// <param name="center"><see cref="PointD"/> structure that defines the center of
        /// the circle.</param>
        /// <param name="radius">Defines the radius of the circle.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawCircle(Pen pen, PointD center, double radius)
        {
            DebugPenAssert(pen);
            dc.DrawCircle(pen.NativePen, center, radius);
        }

        /// <summary>
        /// Fills the interior of a circle specified by a center <see cref="PointD"/> and a radius.
        /// </summary>
        /// <param name="brush"><see cref="Brush"/> that determines the characteristics of
        /// the fill.</param>
        /// <param name="center"><see cref="PointD"/> structure that defines the center of
        /// the circle.</param>
        /// <param name="radius">Defines the radius of the circle.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillCircle(Brush brush, PointD center, double radius)
        {
            DebugBrushAssert(brush);
            dc.FillCircle(brush.NativeBrush, center, radius);
        }

        /// <summary>
        /// Draws a rounded rectangle specified by a <see cref="RectD"/> and a corner radius.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and
        /// style of the rounded rectangle.</param>
        /// <param name="rect">A <see cref="RectD"/> that represents the rectangle to add.</param>
        /// <param name="cornerRadius">The corner radius of the rectangle.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawRoundedRectangle(Pen pen, RectD rect, double cornerRadius)
        {
            DebugPenAssert(pen);
            dc.DrawRoundedRectangle(pen.NativePen, rect, cornerRadius);
        }

        /// <summary>
        /// Fills the interior of a rounded rectangle specified by a <see cref="RectD"/> and
        /// a corner radius.
        /// </summary>
        /// <param name="brush"><see cref="Brush"/> that determines the characteristics of the
        /// fill.</param>
        /// <param name="rect">A <see cref="RectD"/> that represents the rectangle to add.</param>
        /// <param name="cornerRadius">The corner radius of the rectangle.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillRoundedRectangle(Brush brush, RectD rect, double cornerRadius)
        {
            DebugBrushAssert(brush);
            dc.FillRoundedRectangle(brush.NativeBrush, rect, cornerRadius);
        }

        /// <summary>
        /// Draws a polygon defined by an array of <see cref="PointD"/> structures.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the polygon.</param>
        /// <param name="points">Array of <see cref="PointD"/> structures that represent the
        /// vertices of the polygon.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawPolygon(Pen pen, PointD[] points)
        {
            DebugPenAssert(pen);
            dc.DrawPolygon(pen.NativePen, points);
        }

        /// <summary>
        /// Fills the interior of a polygon defined by an array of <see cref="PointD"/> structures.
        /// </summary>
        /// <param name="brush"><see cref="Brush"/> that determines the characteristics of
        /// the fill.</param>
        /// <param name="points">Array of <see cref="PointD"/> structures that represent the
        /// vertices of the polygon.</param>
        /// <param name="fillMode">Member of the <see cref="FillMode"/> enumeration that
        /// determines the style of the fill.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillPolygon(Brush brush, PointD[] points, FillMode fillMode = FillMode.Alternate)
        {
            DebugBrushAssert(brush);
            dc.FillPolygon(brush.NativeBrush, points, (UI.Native.FillMode)fillMode);
        }

        /// <summary>
        /// Draws a series of rectangles specified by <see cref="RectD"/> structures.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the outlines of the rectangles.</param>
        /// <param name="rects">Array of <see cref="RectD"/> structures that represent the
        /// rectangles to draw.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawRectangles(Pen pen, RectD[] rects)
        {
            DebugPenAssert(pen);
            dc.DrawRectangles(pen.NativePen, rects);
        }

        /// <summary>
        /// Fills a series of rectangles specified by <see cref="RectD"/> structures.
        /// </summary>
        /// <param name="brush"><see cref="Brush"/> that determines the characteristics
        /// of the fill.</param>
        /// <param name="rects">Array of <see cref="RectD"/> structures that represent the
        /// rectangles to fill.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillRectangles(Brush brush, RectD[] rects)
        {
            DebugBrushAssert(brush);
            dc.FillRectangles(brush.NativeBrush, rects);
        }

        /// <summary>
        /// Fills the interior of an ellipse defined by a bounding rectangle specified by
        /// a <see cref="RectD"/> structure.
        /// </summary>
        /// <param name="brush"><see cref="Brush"/> that determines the characteristics
        /// of the fill.</param>
        /// <param name="bounds"><see cref="RectD"/> structure that represents the bounding
        /// rectangle that defines the ellipse.</param>
        /// <remarks>
        /// This method fills the interior of an ellipse with a <see cref="Brush"/>.
        /// The ellipse is defined by the bounding rectangle represented by the <c>bounds</c>
        /// parameter.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillEllipse(Brush brush, RectD bounds)
        {
            DebugBrushAssert(brush);
            dc.FillEllipse(brush.NativeBrush, bounds);
        }

        /// <summary>
        /// Flood fills the drawing surface starting from the given point, using the given brush.
        /// </summary>
        /// <param name="brush">Brush to fill the surface with. Only <see cref="SolidBrush"/>
        /// objects are supported at the moment.</param>
        /// <param name="point">The point to start filling from.</param>
        /// <exception cref="ArgumentNullException"><paramref name="brush"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="brush"/> is not
        /// <see cref="SolidBrush"/></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FloodFill(Brush brush, PointD point)
        {
            DebugBrushAssert(brush);
#if DEBUG
            if (brush is not SolidBrush)
            {
                throw new ArgumentException(
                    ErrorMessages.Default.OnlySolidBrushInstancesSupported,
                    nameof(brush));
            }
#endif
            dc.FloodFill(brush.NativeBrush, point);
        }

        /// <summary>
        /// Draws a rectangle specified by a <see cref="RectD"/> structure.
        /// </summary>
        /// <param name="pen">A <see cref="Pen"/> that determines the color, width, and style
        /// of the rectangle.</param>
        /// <param name="rectangle">A <see cref="RectD"/> structure that represents the
        /// rectangle to draw.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawRectangle(Pen pen, RectD rectangle)
        {
            DebugPenAssert(pen);
            dc.DrawRectangle(pen.NativePen, rectangle);
        }

        /// <summary>
        /// Draws a <see cref="GraphicsPath"/>.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and
        /// style of the path.</param>
        /// <param name="path"><see cref="GraphicsPath"/> to draw.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawPath(Pen pen, GraphicsPath path)
        {
            DebugPenAssert(pen);
            dc.DrawPath(pen.NativePen, path.NativePath);
        }

        /// <summary>
        /// Fills the interior of a <see cref="GraphicsPath"/>.
        /// </summary>
        /// <param name="brush"><see cref="Brush"/> that determines the characteristics
        /// of the fill.</param>
        /// <param name="path"><see cref="GraphicsPath"/> that represents the path to fill.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillPath(Brush brush, GraphicsPath path)
        {
            DebugBrushAssert(brush);
            dc.FillPath(brush.NativeBrush, path.NativePath);
        }

        /// <summary>
        /// Draws a line connecting two points.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the line.</param>
        /// <param name="a"><see cref="PointD"/> structure that represents the first point
        /// to connect.</param>
        /// <param name="b"><see cref="PointD"/> structure that represents the second point
        /// to connect.</param>
        /// <exception cref="ArgumentNullException"><paramref name="pen"/> is
        /// <see langword="null"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawLine(Pen pen, PointD a, PointD b)
        {
            DebugPenAssert(pen);
            dc.DrawLine(pen.NativePen, a, b);
        }

        /// <summary>
        /// Draws a line connecting two points.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the line.</param>
        /// <param name="x1">X coordinate of the first point.</param>
        /// <param name="y1">Y coordinate of the first point.</param>
        /// <param name="x2">X coordinate of the second point.</param>
        /// <param name="y2">Y coordinate of the second point.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawLine(Pen pen, double x1, double y1, double x2, double y2) =>
            DrawLine(pen, new(x1, y1), new(x2, y2));

        /// <summary>
        /// Draws a series of line segments that connect an array of <see cref="PointD"/> structures.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the line segments.</param>
        /// <param name="points">Array of <see cref="PointD"/> structures that represent the
        /// points to connect.</param>
        /// <remarks>
        /// This method draws a series of lines connecting an array of ending points.
        /// The first two points in the array specify the first line.
        /// Each additional point specifies the end of a line segment whose starting point
        /// is the ending point of the previous line segment.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="pen"/> is
        /// <see langword="null"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawLines(Pen pen, PointD[] points)
        {
            DebugPenAssert(pen);
            dc.DrawLines(pen.NativePen, points);
        }

        /// <summary>
        /// Draws an ellipse defined by a bounding <see cref="RectD"/>.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the ellipse.</param>
        /// <param name="bounds"><see cref="RectD"/> structure that defines the boundaries
        /// of the ellipse.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawEllipse(Pen pen, RectD bounds)
        {
            DebugPenAssert(pen);
            dc.DrawEllipse(pen.NativePen, bounds);
        }

        /// <summary>
        /// Draws the specified <see cref="Image"/>, using its original size, at the
        /// specified location.
        /// </summary>
        /// <param name="image"><see cref="Image"/> to draw.</param>
        /// <param name="origin"><see cref="PointD"/> structure that represents the
        /// upper-left corner of the drawn image.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawImageUnscaled(Image image, PointD origin) => DrawImage(image, origin);

        /// <summary>
        /// Draws the specified <see cref="Image"/>, using its original size, at the
        /// specified location.
        /// </summary>
        /// <param name="image"><see cref="Image"/> to draw.</param>
        /// <param name="origin"><see cref="PointD"/> structure that represents the
        /// upper-left corner of the drawn image.</param>
        /// <param name="useMask">If useMask is true and the bitmap has a transparency mask,
        /// the bitmap will be drawn transparently.</param>
        /// <remarks>
        /// When drawing a mono-bitmap, the current text foreground color will
        /// be used to draw the foreground of the bitmap (all bits set to 1), and
        /// the current text background color to draw the background (all bits set to 0).
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawImage(Image image, PointD origin, bool useMask = false)
        {
            DebugImageAssert(image);
            dc.DrawImageAtPoint(image.NativeImage, origin, useMask);
        }

        /// <summary>
        /// Draws an image into the region defined by the specified <see cref="RectD"/>.
        /// </summary>
        /// <param name="image"><see cref="Image"/> to draw.</param>
        /// <param name="destinationRect">The region in which to draw
        /// <paramref name="image"/>.</param>
        /// <param name="useMask">If useMask is true and the bitmap has a transparency mask,
        /// the bitmap will be drawn transparently.</param>
        /// <remarks>
        /// When drawing a mono-bitmap, the current text foreground color will
        /// be used to draw the foreground of the bitmap (all bits set to 1), and
        /// the current text background color to draw the background (all bits set to 0).
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawImage(Image image, RectD destinationRect, bool useMask = false)
        {
            DebugImageAssert(image);
            dc.DrawImageAtRect(image.NativeImage, destinationRect, useMask);
        }

        /// <summary>
        /// Draws the specified portion of the image into the region defined by the specified
        /// <see cref="RectD"/>.
        /// </summary>
        /// <param name="image"><see cref="Image"/> to draw.</param>
        /// <param name="destinationRect">The region in which to draw
        /// <paramref name="image"/>.</param>
        /// <param name="sourceRect">
        /// <see cref="RectD"/> structure that specifies the portion of the
        /// <paramref name="image"/> object to draw.
        /// </param>
        /// <remarks>
        /// Parameters <paramref name="destinationRect"/> and <paramref name="sourceRect"/>
        /// are specified in dips (1/96 inch).
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawImage(Image image, RectD destinationRect, RectD sourceRect)
        {
            DebugImageAssert(image);
            dc.DrawImagePortionAtRect(image.NativeImage, destinationRect, sourceRect);
        }

        /// <summary>
        /// Draws the specified portion of the image into the region defined by the specified
        /// <see cref="RectI"/>.
        /// </summary>
        /// <param name="image"><see cref="Image"/> to draw.</param>
        /// <param name="destinationRect">The region in which to draw
        /// <paramref name="image"/>.</param>
        /// <param name="sourceRect">
        /// <see cref="RectI"/> structure that specifies the portion of the
        /// <paramref name="image"/> object to draw.
        /// </param>
        /// <remarks>
        /// Parameters <paramref name="destinationRect"/> and <paramref name="sourceRect"/>
        /// are specified in pixels.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawImageI(Image image, RectI destinationRect, RectI sourceRect)
        {
            DebugImageAssert(image);
            dc.DrawImagePortionAtPixelRect(image.NativeImage, destinationRect, sourceRect);
        }

        /// <summary>
        /// Sets the color of the specified pixel in this <see cref="Graphics" />.</summary>
        /// <param name="point">The coordinates of the pixel to set.</param>
        /// <param name="pen">A <see cref="Pen"/> structure that represents the color to
        /// assign to the specified pixel.</param>
        /// <remarks>
        /// Not all drawing contexts support this operation.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetPixel(PointD point, Pen pen)
        {
            DebugPenAssert(pen);
            dc.SetPixel(point, pen.NativePen);
        }

        /// <summary>
        /// Sets the color of the specified pixel in this <see cref="Graphics" />.</summary>
        /// <param name="pen">A <see cref="Pen"/> structure that represents the color to
        /// assign to the specified pixel.</param>
        /// <remarks>
        /// Not all drawing contexts support this operation.
        /// </remarks>
        /// <param name="x">The x-coordinate of the pixel to set.</param>
        /// <param name="y">The y-coordinate of the pixel to set.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetPixel(double x, double y, Pen pen)
        {
            DebugPenAssert(pen);
            dc.SetPixel(new PointD(x, y), pen.NativePen);
        }

        /// <summary>
        /// Sets the color of the specified pixel in this <see cref="Graphics" />.</summary>
        /// <param name="color">A <see cref="Color"/> structure that represents the color to
        /// assign to the specified pixel.</param>
        /// <remarks>
        /// Not all drawing contexts support this operation.
        /// </remarks>
        /// <param name="x">The x-coordinate of the pixel to set.</param>
        /// <param name="y">The y-coordinate of the pixel to set.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetPixel(double x, double y, Color color)
        {
            DebugColorAssert(color);
            dc.SetPixel(new PointD(x, y), color.AsPen.NativePen);
        }

        /// <summary>
        /// Gets the color of the specified pixel in this <see cref="Graphics" />.</summary>
        /// <param name="point">The coordinates of the pixel to retrieve.</param>
        /// <returns>A <see cref="Color" /> structure that represents the color
        /// of the specified pixel.</returns>
        /// <remarks>
        /// Not all drawing contexts support this operation.
        /// </remarks>
        public Color GetPixel(PointD point)
        {
            return dc.GetPixel(point);
        }

        /// <summary>
        /// Draws the specified portion of the image into the region defined by the specified
        /// <see cref="RectD"/>.
        /// </summary>
        /// <param name="unit">Units used to draw the image.
        /// Currently only pixel unit is allowed.</param>
        /// <param name="image"><see cref="Image"/> to draw.</param>
        /// <param name="destinationRect">The region in which to draw <paramref name="image"/>.</param>
        /// <param name="sourceRect">
        /// <see cref="RectD"/> structure that specifies the portion of the
        /// <paramref name="image"/> object to draw.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawImage(
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

        /// <summary>
        /// Draws the specified text string at the specified location with the specified
        /// <see cref="Brush"/> and <see cref="Font"/> objects.
        /// </summary>
        /// <param name="text">String to draw.</param>
        /// <param name="font"><see cref="Font"/> that defines the text format of the string.</param>
        /// <param name="brush"><see cref="Brush"/> that determines the color and texture of
        /// the drawn text.</param>
        /// <param name="origin"><see cref="PointD"/> structure that specifies the upper-left
        /// corner of the drawn text.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawText(string text, Font font, Brush brush, PointD origin)
        {
            DrawText(text, font, brush, origin, TextFormat.Default);
        }

        /// <summary>
        /// Draws the specified text string at the specified location with the specified
        /// <see cref="Brush"/> and <see cref="Font"/> objects.
        /// </summary>
        /// <param name="text">String to draw.</param>
        /// <param name="font"><see cref="Font"/> that defines the text format of the string.</param>
        /// <param name="brush"><see cref="Brush"/> that determines the color and texture of
        /// the drawn text.</param>
        /// <param name="origin"><see cref="PointD"/> structure that specifies the upper-left
        /// corner of the drawn text.</param>
        /// <param name="format"><see cref="TextFormat"/> that specifies formatting attributes,
        /// such as
        /// alignment and trimming, that are applied to the drawn text.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawText(
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
                font.NativeFont,
                brush.NativeBrush);
        }

        /// <summary>
        /// Draws the specified text string at the specified location with the specified
        /// <see cref="Brush"/> and <see cref="Font"/> objects.
        /// </summary>
        /// <param name="text">String to draw.</param>
        /// <param name="font"><see cref="Font"/> that defines the text format of the string.</param>
        /// <param name="brush"><see cref="Brush"/> that determines the color and texture
        /// of the drawn text.</param>
        /// <param name="bounds"><see cref="RectD"/> structure that specifies the bounds of
        /// the drawn text.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawText(string text, Font font, Brush brush, RectD bounds)
        {
            DrawText(text, font, brush, bounds, TextFormat.Default);
        }

        /// <summary>
        /// Draws the specified text string at the specified location with the specified
        /// <see cref="Brush"/> and <see
        /// cref="Font"/> objects.
        /// </summary>
        /// <param name="text">String to draw.</param>
        /// <param name="font"><see cref="Font"/> that defines the text format of the string.</param>
        /// <param name="brush"><see cref="Brush"/> that determines the color and texture
        /// of the drawn text.</param>
        /// <param name="bounds"><see cref="RectD"/> structure that specifies the bounds of
        /// the drawn text.</param>
        /// <param name="format"><see cref="TextFormat"/> that specifies formatting attributes,
        /// such as
        /// alignment and trimming, that are applied to the drawn text.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawText(
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
                font.NativeFont,
                brush.NativeBrush,
                (UI.Native.TextHorizontalAlignment)format.HorizontalAlignment,
                (UI.Native.TextVerticalAlignment)format.VerticalAlignment,
                (UI.Native.TextTrimming)format.Trimming,
                (UI.Native.TextWrapping)format.Wrapping);
        }

        /// <summary>
        /// Draws waved line in the specified rectangular area.
        /// </summary>
        /// <param name="rect">Rectangle that bounds the drawing area for the wave.</param>
        /// <param name="color">Color used to draw wave.</param>
        /// <remarks>
        /// This line looks like line drawn by Visual Studio in error position under the text.
        /// Specify rectangle of the text, line is drawn on the bottom. You can pass the rectangle
        /// which is returned by measure text functions.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void DrawWave(RectD rect, Color color)
        {
            Draw(this, rect.ToRect(), color);

            static void Draw(Graphics dc, RectI rect, Color color)
            {
                int minSize = 4;
                int offset = 6;

                int left = rect.Left - (rect.Left % offset);
                int i = rect.Right % offset;
                int right = (i != 0) ? rect.Right + (offset - i) : rect.Right;

                int scale = 2;
                int size = (right - left) / scale;

                offset = 3;

                if (size < minSize)
                    size = minSize;
                else
                {
                    i = (int)((size - minSize) / offset);
                    if ((size - minSize) % offset != 0)
                        i++;
                    size = minSize + (i * offset);
                }

                PointD[] pts = new PointD[size];
                for (int index = 0; index < size; index++)
                {
                    pts[index].X = left + (index * scale);
                    pts[index].Y = rect.Bottom - 1;
                    switch (index % 3)
                    {
                        case 0:
                            {
                                pts[index].Y -= scale;
                                break;
                            }

                        case 2:
                            {
                                pts[index].Y += scale;
                                break;
                            }
                    }
                }

                dc.DrawBeziers(color.GetAsPen(1), pts);
            }
        }

        /// <summary>
        /// Measures the specified string when drawn with the specified <see cref="Font"/>.
        /// </summary>
        /// <param name="text">String to measure.</param>
        /// <param name="font"><see cref="Font"/> that defines the text format of the string.</param>
        /// <returns>
        /// This method returns a <see cref="SizeD"/> structure that represents the size,
        /// in device-independent units (1/96th inch per unit), of the
        /// string specified by the <c>text</c> parameter as drawn with the <c>font</c> parameter.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SizeD MeasureText(string text, Font font)
        {
            DebugTextAssert(text);
            DebugFontAssert(font);
            return dc.MeasureText(text, font.NativeFont, double.NaN, UI.Native.TextWrapping.None);
        }

        /// <summary>
        /// Measures the specified string when drawn with the specified <see cref="Font"/> and
        /// maximum width.
        /// </summary>
        /// <param name="text">String to measure.</param>
        /// <param name="font"><see cref="Font"/> that defines the text format of the string.</param>
        /// <param name="maximumWidth">Maximum width of the string in device-independent
        /// units (1/96th inch per unit).</param>
        /// <returns>
        /// This method returns a <see cref="SizeD"/> structure that represents the size,
        /// in device-independent units (1/96th inch per unit), of the
        /// string specified by the <c>text</c> parameter as drawn with the <c>font</c> parameter.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SizeD MeasureText(string text, Font font, double maximumWidth)
        {
            DebugTextAssert(text);
            DebugFontAssert(font);
            return dc.MeasureText(
                text,
                font.NativeFont,
                maximumWidth,
                UI.Native.TextWrapping.Character);
        }

        /// <summary>
        /// Measures the specified string when drawn with the specified <see cref="Font"/>,
        /// maximum width and <see cref="TextFormat"/>.
        /// </summary>
        /// <param name="text">String to measure.</param>
        /// <param name="font"><see cref="Font"/> that defines the text format of the string.</param>
        /// <param name="maximumWidth">Maximum width of the string in device-independent
        /// units (1/96th inch per unit).</param>
        /// <param name="format"><see cref="TextFormat"/> that specifies formatting attributes,
        /// such as
        /// alignment and trimming, that are applied to the drawn text.</param>
        /// <returns>
        /// This method returns a <see cref="SizeD"/> structure that represents the size,
        /// in device-independent units (1/96th inch per unit), of the
        /// string specified by the <c>text</c> parameter as drawn with the <c>font</c> parameter.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SizeD MeasureText(string text, Font font, double maximumWidth, TextFormat format)
        {
            DebugTextAssert(text);
            DebugFontAssert(font);
            DebugFormatAssert(format);
            return dc.MeasureText(
                text,
                font.NativeFont,
                maximumWidth,
                (UI.Native.TextWrapping)format.Wrapping);
        }

        /// <summary>
        /// Pushes the current state of the <see cref="Graphics"/> transformation
        /// matrix on a stack.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Push()
        {
            dc.Push();
        }

        /// <summary>
        /// Draws text with the specified font, background and foreground colors.
        /// </summary>
        /// <param name="location">Location used to draw the text.</param>
        /// <param name="text">Text to draw.</param>
        /// <param name="font">Font used to draw the text.</param>
        /// <param name="foreColor">Foreground color of the text.</param>
        /// <param name="backColor">Background color of the text. If parameter is equal
        /// to <see cref="Color.Empty"/>, background will not be painted. </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawText(
            string text,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor)
        {
            DebugTextAssert(text);
            DebugFontAssert(font);
            DebugColorAssert(foreColor);
            dc.DrawText(text, location, font.NativeFont, foreColor, backColor);
        }

        /// <summary>
        /// Draws text with the specified font, background and foreground colors,
        /// optional image, alignment and underlined mnemonic character.
        /// </summary>
        /// <param name="text">Text to draw.</param>
        /// <param name="font">Font used to draw the text.</param>
        /// <param name="foreColor">Foreground color of the text.</param>
        /// <param name="backColor">Background color of the text. If parameter is equal
        /// to <see cref="Color.Empty"/>, background will not be painted.</param>
        /// <param name="image">Optional image.</param>
        /// <param name="rect">Rectangle in which drawing is performed.</param>
        /// <param name="alignment">Alignment of the text.</param>
        /// <param name="indexAccel">Index of underlined mnemonic character</param>
        /// <returns>The bounding rectangle.</returns>
        public RectD DrawLabel(
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
                font.NativeFont,
                foreColor,
                backColor,
                image?.NativeImage,
                rect,
                (int)alignment,
                indexAccel);
        }

        /// <summary>
        /// Pushes the current state of the <see cref="Graphics"/> transformation
        /// matrix on a stack
        /// and concatenates the current transform with a new transform.
        /// </summary>
        /// <param name="transform">A transform to concatenate with the current transform.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PushTransform(TransformMatrix transform)
        {
            Push();
            var currentTransform = Transform;
            currentTransform.Multiply(transform);
            Transform = currentTransform;
        }

        /// <summary>
        /// Returns the DPI of the display used by this object.
        /// </summary>
        /// <returns>
        /// A <see cref="SizeD"/> value that represents DPI of the device
        /// used by this control. If the DPI is not available,
        /// returns Size(0,0) object.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual SizeD GetDPI()
        {
            return dc.GetDpi();
        }

        /// <summary>
        /// Pops a stored state from the stack and sets the current transformation matrix
        /// to that state.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Pop()
        {
            dc.Pop();
        }

        /// <summary>
        /// Destroys the current clipping region so that none of the DC is clipped.
        /// </summary>
        public void DestroyClippingRegion()
        {
            dc.DestroyClippingRegion();
        }

        /// <summary>
        /// Sets the clipping region for this device context to the intersection of the
        /// given region described by the parameters of this method and the previously
        /// set clipping region.
        /// </summary>
        /// <param name="rect">Clipping rectangle.</param>
        /// <remarks>
        /// The clipping region is an area to which drawing is restricted. Possible uses
        /// for the clipping region are for clipping text or for speeding up
        /// window redraws when only a known area of the screen is damaged.
        /// </remarks>
        /// <remarks>
        /// Calling this function can only make the clipping region
        /// smaller, never larger.
        /// You need to call <see cref="DestroyClippingRegion"/> first if you want to set the clipping
        /// region exactly to the region specified.
        /// If resulting clipping region is empty, then all drawing on the DC is
        /// clipped out (all changes made by drawing operations are masked out).
        /// </remarks>
        public void SetClippingRegion(RectD rect)
        {
            dc.SetClippingRegion(rect);
        }

        /// <summary>
        /// Gets the rectangle surrounding the current clipping region.
        /// If no clipping region is set this function returns the extent of the device context.
        /// </summary>
        /// <returns>
        /// <see cref="RectD"/> filled in with the logical coordinates of the clipping region
        /// on success, or <see cref="RectD.Empty"/> otherwise.
        /// </returns>
        public RectD GetClippingBox()
        {
            return dc.GetClippingBox();
        }

        /// <inheritdoc/>
        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            if(dispose)
                dc.Dispose();
            dc = null!;
        }

        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DebugBrushAssert(Brush value)
        {
            if (value is null)
                throw new Exception("Brush is null");
            if (value.IsDisposed)
                throw new Exception("Brush was disposed");
        }

        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DebugPenAssert(Pen value)
        {
            if (value is null)
                throw new Exception("Pen is null");
            if (value.IsDisposed)
                throw new Exception("Pen was disposed");
        }

        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DebugColorAssert(Color value, string? paramName = default)
        {
            if (value is null)
                throw new Exception($"{Fn()} is null");
            if (!value.IsOk)
                throw new Exception($"{Fn()} is not ok");

            string Fn()
            {
                if (paramName is null)
                    return "Color";
                else
                    return $"Color '{paramName}'";
            }
        }

        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DebugImageAssert(Image image)
        {
            if (image is null)
                throw new Exception("Image is null");
            if(image.IsDisposed)
                throw new Exception("Image was disposed");
            if (image.Width <= 0 || image.Height <= 0)
                throw new Exception("Image has invalid size");
        }

        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DebugTextAssert(string text)
        {
            if (text is null)
                throw new Exception("Text is null");
        }

        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DebugFontAssert(Font font)
        {
            if (font is null)
                throw new Exception("Font is null");
            if (font.IsDisposed)
                throw new Exception("Font is disposed");
        }

        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DebugFormatAssert(TextFormat format)
        {
            if (format is null)
                throw new Exception("Text format is null");
        }
    }
}