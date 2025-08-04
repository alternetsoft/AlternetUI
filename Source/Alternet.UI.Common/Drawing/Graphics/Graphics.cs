using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Alternet.UI;
using Alternet.UI.Localization;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines a custom drawing surface.
    /// </summary>
    public abstract partial class Graphics : DisposableObject, IGraphics, IDisposable
    {
        /// <summary>
        /// Gets half of the maximum value of <see cref="Coord"/>.
        /// </summary>
        internal const Coord HalfOfMaxValue = int.MaxValue / 2;

        private Stack<TransformMatrix>? stack;
        private Stack<Region?>? clipStack;
        private TransformMatrix transform = new();
        private GraphicsDocument? document;

        /// <summary>
        /// Returns true if the object is ok to use.
        /// </summary>
        public abstract bool IsOk { get; }

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
                return transform;
            }

            set
            {
                if (transform == value)
                    return;

                transform = value;

                SetHandlerTransform(value);
            }
        }

        /// <summary>
        /// Gets the displacement values (DX and DY) from the current transformation.
        /// Returns an empty point if no transformation is applied.
        /// </summary>
        /// <value>
        /// A <see cref="PointD"/> representing the translation offset.
        /// </value>
        public PointD TransformDXDY
        {
            get
            {
                if (HasTransform)
                {
                    return (Transform.DX, Transform.DY);
                }
                else
                    return PointD.Empty;
            }
        }

        /// <summary>
        /// Gets whether <see cref="Transform"/> is assigned with transform matrix
        /// which is not the identity matrix.
        /// </summary>
        public bool HasTransform
        {
            get
            {
                return !transform.IsIdentity;
            }
        }

        /// <summary>
        /// Gets used scale factor.
        /// </summary>
        public Coord ScaleFactor
        {
            get
            {
                var dpi = GetDPI().Width;
                var result = GraphicsFactory.ScaleFactorFromDpi(dpi);
                return result;
            }
        }

        /// <summary>
        /// Gets horizontal scale factor.
        /// </summary>
        public Coord HorizontalScaleFactor
        {
            get
            {
                return GraphicsFactory.ScaleFactorFromDpi(GetDPI().Width);
            }
        }

        /// <summary>
        /// Gets vertical scale factor.
        /// </summary>
        public Coord VerticalScaleFactor
        {
            get
            {
                return GraphicsFactory.ScaleFactorFromDpi(GetDPI().Height);
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
        public abstract InterpolationMode InterpolationMode { get; set; }

        /// <summary>
        /// Gets native drawing context.
        /// </summary>
        public abstract object NativeObject { get; }

        private GraphicsDocument SafeDocument
        {
            get
            {
                if (document is null)
                {
                    document = new();
                    document.ScaleFactorOverride = ScaleFactor;
                }

                return document;
            }
        }

        /// <summary>
        /// Checks whether <see cref="Brush"/> parameter is ok.
        /// </summary>
        /// <param name="value">Parameter value.</param>
        /// <exception cref="Exception">Raised if parameter is not ok.</exception>
        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugBrushAssert(Brush value)
        {
            if (value is null)
                throw new Exception("Brush is null");
            if (value.IsDisposed)
                throw new Exception("Brush was disposed");
        }

        /// <summary>
        /// Updates <paramref name="storage"/> with measurement canvas which can be used
        /// in order to measure text sizes.
        /// </summary>
        /// <param name="scaleFactor">Scaling factor.</param>
        /// <param name="storage">Updated measurement canvas.</param>
        /// <returns>True if <paramref name="storage"/> contains permanent canvas that
        /// can be kept in memory; False if canvas is temporary and should not be saved.</returns>
        public static bool RequireMeasure(Coord scaleFactor, [NotNull] ref Graphics? storage)
        {
            if (GraphicsFactory.MeasureCanvasOverride is null)
            {
                if (storage?.ScaleFactor != scaleFactor)
                    storage = GraphicsFactory.GetOrCreateMemoryCanvas(scaleFactor);
                return true;
            }
            else
            {
                storage = GraphicsFactory.MeasureCanvasOverride;
                return false;
            }
        }

        /// <summary>
        /// Checks whether <see cref="SolidBrush"/> parameter is ok.
        /// </summary>
        /// <param name="brush">Parameter value.</param>
        /// <exception cref="Exception">Raised if parameter is not ok.</exception>
        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugSolidBrushAssert(Brush brush)
        {
            DebugBrushAssert(brush);
            if (brush is not SolidBrush)
            {
                throw new ArgumentException(
                    ErrorMessages.Default.OnlySolidBrushInstancesSupported,
                    nameof(brush));
            }
        }

        /// <summary>
        /// Checks whether <see cref="Pen"/> parameter is ok.
        /// </summary>
        /// <param name="value">Parameter value.</param>
        /// <exception cref="Exception">Raised if parameter is not ok.</exception>
        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugPenAssert(Pen value)
        {
            if (value is null)
                throw new Exception("Pen is null");
            if (value.IsDisposed)
                throw new Exception("Pen was disposed");
        }

        /// <summary>
        /// Checks whether <see cref="Pen"/> and <see cref="Brush"/> parameters are ok.
        /// </summary>
        /// <param name="pen">Pen parameter value.</param>
        /// <param name="brush">Brush parameter value.</param>
        /// <exception cref="Exception">Raised if parameters are not ok.</exception>
        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugAssert(Pen pen, Brush brush)
        {
            DebugPenAssert(pen);
            DebugBrushAssert(brush);
        }

        /// <summary>
        /// Checks whether array of <see cref="PointD"/> parameter is ok.
        /// </summary>
        /// <param name="points">Parameter value.</param>
        /// <exception cref="Exception">Raised if parameter is not ok.</exception>
        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugBezierPointsAssert(PointD[] points)
        {
            var length = points.Length;

            if (length == 0)
                return;

            if ((length - 1) % 3 != 0)
            {
                throw new ArgumentException(
                    "The number of points should be a multiple of 3 plus 1, such as 4, 7, or 10.",
                    nameof(points));
            }
        }

        /// <summary>
        /// Checks whether <see cref="Color"/> parameter is ok.
        /// </summary>
        /// <param name="value">Parameter value.</param>
        /// <param name="paramName">Parameter name.</param>
        /// <exception cref="Exception">Raised if parameter is not ok.</exception>
        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugColorAssert(Color value, string? paramName = default)
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

        /// <summary>
        /// Checks whether <see cref="Image"/> parameter is ok.
        /// </summary>
        /// <param name="image">Parameter value.</param>
        /// <exception cref="Exception">Raised if parameter is not ok.</exception>
        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugImageAssert(Image image)
        {
            if (image is null)
                throw new Exception("Image is null");
            if (image.IsDisposed)
                throw new Exception("Image was disposed");
            if (image.Width <= 0 || image.Height <= 0)
                throw new Exception("Image has invalid size");
        }

        /// <summary>
        /// Checks whether <see cref="string"/> parameter is ok.
        /// </summary>
        /// <param name="text">Parameter value.</param>
        /// <exception cref="Exception">Raised if parameter is not ok.</exception>
        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugTextAssert(string text)
        {
            if (text is null)
                throw new Exception("Text is null");
        }

        /// <summary>
        /// Checks whether <see cref="Font"/> parameter is ok.
        /// </summary>
        /// <param name="font">Parameter value.</param>
        /// <exception cref="Exception">Raised if parameter is not ok.</exception>
        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugFontAssert(Font font)
        {
            if (font is null)
                throw new Exception("Font is null");
            if (font.IsDisposed)
                throw new Exception("Font is disposed");
        }

        /// <summary>
        /// Checks whether <see cref="TextFormat"/> parameter is ok.
        /// </summary>
        /// <param name="format">Parameter value.</param>
        /// <exception cref="Exception">Raised if parameter is not ok.</exception>
        [Conditional("DEBUG")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugFormatAssert(TextFormat format)
        {
            if (format is null)
                throw new Exception("Text format is null");
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
            return GraphicsFactory.Handler.CreateGraphicsFromImage(image);
        }

        /// <summary>
        /// Creates <see cref="Graphics"/> that can be used to paint on the screen.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Graphics FromScreen()
        {
            return GraphicsFactory.Handler.CreateGraphicsFromScreen();
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
        public abstract void RoundedRectangle(
            Pen pen,
            Brush brush,
            RectD rectangle,
            Coord cornerRadius);

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
        public abstract SizeD GetTextExtent(string text, Font font);

        /// <summary>
        /// Draws point at the center of the specified rectangle.
        /// </summary>
        /// <param name="brush">Brush used to fill the rectangle.</param>
        /// <param name="container">Rectangle to use as a container for the point.</param>
        /// <param name="size">Size of the rectangle which is painted at the center.</param>
        public void FillRectangleAtCenter(Brush brush, RectD container, SizeD size)
        {
            RectD rect = ((0, 0), size);
            var alignedRect = AlignUtils.AlignRectInRect(
                rect,
                container,
                HorizontalAlignment.Center,
                VerticalAlignment.Center);
            FillRectangle(brush, alignedRect);
        }

        /// <summary>
        /// Draws point at the center of the specified rectangle.
        /// </summary>
        /// <param name="color">Color of the point.</param>
        /// <param name="container">Rectangle to use as a container for the point.</param>
        public void DrawPointAtCenter(Color color, RectD container)
        {
            FillRectangle(color.AsBrush, container.Center.AsRect(1));
        }

        /// <summary>
        /// Resets transformation matrix in the <see cref="Transform"/> property.
        /// </summary>
        public void ResetTransform()
        {
            Transform = new TransformMatrix();
        }

        /// <summary>
        /// Calls <see cref="FillRectangle(Brush, RectD)"/> and than <see cref="DrawRectangle"/>.
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="brush"></param>
        /// <param name="rectangle"></param>
        /// <remarks>
        /// This method works faster than fill and then draw.
        /// </remarks>
        public abstract void Rectangle(Pen pen, Brush brush, RectD rectangle);

        /// <summary>
        /// Calls <see cref="FillEllipse"/> and than <see cref="DrawEllipse"/>.
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="brush"></param>
        /// <param name="rectangle"></param>
        /// <remarks>
        /// This method works faster than fill and then draw.
        /// </remarks>
        public abstract void Ellipse(Pen pen, Brush brush, RectD rectangle);

        /// <summary>
        /// Calls <see cref="FillPath"/> and than <see cref="DrawPath"/>.
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="brush"></param>
        /// <param name="path"></param>
        /// <remarks>
        /// This method works faster than fill and then draw.
        /// </remarks>
        public abstract void Path(Pen pen, Brush brush, GraphicsPath path);

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
        public abstract void Pie(
            Pen pen,
            Brush brush,
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle);

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
        public abstract void Circle(Pen pen, Brush brush, PointD center, Coord radius);

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
        public abstract void Polygon(
            Pen pen,
            Brush brush,
            PointD[] points,
            FillMode fillMode = FillMode.Alternate);

        /// <summary>
        /// Fills the interior of a rectangle specified by a <see cref="RectD"/> structure.
        /// Rectangle is specified in device-independent units.
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
        public abstract void FillRectangle(Brush brush, RectD rectangle);

        /// <summary>
        /// Fills the interior of a rectangle specified by a pair of coordinates,
        /// a width, and a height.
        /// </summary>
        /// <param name="brush">
        /// <see cref="Brush" /> that determines the characteristics of the fill.
        /// </param>
        /// <param name="x">The x-coordinate of the upper-left corner of the rectangle to fill.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the rectangle to fill.</param>
        /// <param name="width">Width of the rectangle to fill.</param>
        /// <param name="height">Height of the rectangle to fill.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillRectangle(Brush brush, Coord x, Coord y, Coord width, Coord height)
        {
            FillRectangle(brush, (x, y, width, height));
        }

        /// <summary>
        /// Fills the interior of a rectangle specified by a <see cref="RectD"/> structure.
        /// Rectangle is specified in device-independent units.
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
        /// <param name="unit"><see cref="GraphicsUnit"/> that determines
        /// the unit of measure for the rectangle parameter.</param>
        public abstract void FillRectangle(Brush brush, RectD rectangle, GraphicsUnit unit);

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
        public abstract void DrawArc(
            Pen pen,
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle);

        /// <summary>
        /// Draws debug points on the corners of the specified rectangle.
        /// </summary>
        /// <param name="pen">Color of the debug points. if <c>null</c>, red color is used.</param>
        /// <param name="rect"></param>
        [Conditional("DEBUG")]
        public virtual void DrawDebugPoints(RectD rect, Pen? pen = null)
        {
            pen ??= Pens.Red;

            void DrawDebugPoint(PointD p)
            {
                DrawingUtils.DrawHorzLine(this, pen.AsBrush, p, 1, 1);
            }

            DrawDebugPoint(rect.TopLeft);
            DrawDebugPoint(new PointD(rect.Right - 1, rect.Top));
            DrawDebugPoint(new PointD(rect.Right - 1, rect.Bottom - 1));
            DrawDebugPoint(new PointD(rect.Left, rect.Bottom - 1));
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
        public abstract void FillPie(
            Brush brush,
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle);

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
        public abstract void DrawPie(
            Pen pen,
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle);

        /// <summary>
        /// Draws a Bezier spline defined by four <see cref="PointD"/> structures.
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
        public abstract void DrawBezier(
            Pen pen,
            PointD startPoint,
            PointD controlPoint1,
            PointD controlPoint2,
            PointD endPoint);

        /// <summary>
        /// Draws a series of Bezier splines from an array of <see cref="PointD"/> structures.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the curve.</param>
        /// <param name="points">
        /// Array of <see cref="PointD"/> structures that represent the points that
        /// determine the curve.
        /// The number of points in the array should be a multiple of 3 plus 1, such as 4, 7, or 10.
        /// </param>
        public abstract void DrawBeziers(Pen pen, PointD[] points);

        /// <summary>
        /// Draws an circle specified by a center <see cref="PointD"/> and a radius.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the circle.</param>
        /// <param name="center"><see cref="PointD"/> structure that defines the center of
        /// the circle.</param>
        /// <param name="radius">Defines the radius of the circle.</param>
        public abstract void DrawCircle(Pen pen, PointD center, Coord radius);

        /// <summary>
        /// Fills the interior of a circle specified by a center <see cref="PointD"/> and a radius.
        /// </summary>
        /// <param name="brush"><see cref="Brush"/> that determines the characteristics of
        /// the fill.</param>
        /// <param name="center"><see cref="PointD"/> structure that defines the center of
        /// the circle.</param>
        /// <param name="radius">Defines the radius of the circle.</param>
        public abstract void FillCircle(Brush brush, PointD center, Coord radius);

        /// <summary>
        /// Draws a rounded rectangle specified by a <see cref="RectD"/> and a corner radius.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and
        /// style of the rounded rectangle.</param>
        /// <param name="rect">A <see cref="RectD"/> that represents the rectangle to add.</param>
        /// <param name="cornerRadius">The corner radius of the rectangle.</param>
        public abstract void DrawRoundedRectangle(Pen pen, RectD rect, Coord cornerRadius);

        /// <summary>
        /// Fills the interior of a rounded rectangle specified by a <see cref="RectD"/> and
        /// a corner radius.
        /// </summary>
        /// <param name="brush"><see cref="Brush"/> that determines the characteristics of the
        /// fill.</param>
        /// <param name="rect">A <see cref="RectD"/> that represents the rectangle to add.</param>
        /// <param name="cornerRadius">The corner radius of the rectangle.</param>
        public abstract void FillRoundedRectangle(Brush brush, RectD rect, Coord cornerRadius);

        /// <summary>
        /// Draws a polygon defined by an array of <see cref="PointD"/> structures.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the polygon.</param>
        /// <param name="points">Array of <see cref="PointD"/> structures that represent the
        /// vertices of the polygon.</param>
        public abstract void DrawPolygon(Pen pen, PointD[] points);

        /// <summary>
        /// Fills the interior of a polygon defined by an array of <see cref="PointD"/> structures.
        /// </summary>
        /// <param name="brush"><see cref="Brush"/> that determines the characteristics of
        /// the fill.</param>
        /// <param name="points">Array of <see cref="PointD"/> structures that represent the
        /// vertices of the polygon.</param>
        /// <param name="fillMode">Member of the <see cref="FillMode"/> enumeration that
        /// determines the style of the fill.</param>
        public abstract void FillPolygon(
            Brush brush,
            PointD[] points,
            FillMode fillMode = FillMode.Alternate);

        /// <summary>
        /// Draws a series of rectangles specified by <see cref="RectD"/> structures.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the outlines of the rectangles.</param>
        /// <param name="rects">Array of <see cref="RectD"/> structures that represent the
        /// rectangles to draw.</param>
        public void DrawRectangles(Pen pen, RectD[] rects)
        {
            for (int i = 0; i < rects.Length; i++)
                DrawRectangle(pen, rects[i]);
        }

        /// <summary>
        /// Fills a series of rectangles specified by <see cref="RectD"/> structures.
        /// </summary>
        /// <param name="brush"><see cref="Brush"/> that determines the characteristics
        /// of the fill.</param>
        /// <param name="rects">Array of <see cref="RectD"/> structures that represent the
        /// rectangles to fill.</param>
        public void FillRectangles(Brush brush, RectD[] rects)
        {
            for (int i = 0; i < rects.Length; i++)
                FillRectangle(brush, rects[i]);
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
        public abstract void FillEllipse(Brush brush, RectD bounds);

        /// <summary>
        /// Draws a rectangle specified by a <see cref="RectD"/> structure.
        /// </summary>
        /// <param name="pen">A <see cref="Pen"/> that determines the color, width, and style
        /// of the rectangle.</param>
        /// <param name="rectangle">A <see cref="RectD"/> structure that represents the
        /// rectangle to draw.</param>
        public abstract void DrawRectangle(Pen pen, RectD rectangle);

        /// <summary>
        /// Draws a <see cref="GraphicsPath"/>.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and
        /// style of the path.</param>
        /// <param name="path"><see cref="GraphicsPath"/> to draw.</param>
        public abstract void DrawPath(Pen pen, GraphicsPath path);

        /// <summary>
        /// Fills the interior of a <see cref="GraphicsPath"/>.
        /// </summary>
        /// <param name="brush"><see cref="Brush"/> that determines the characteristics
        /// of the fill.</param>
        /// <param name="path"><see cref="GraphicsPath"/> that represents the path to fill.</param>
        public abstract void FillPath(Brush brush, GraphicsPath path);

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
        public abstract void DrawLine(Pen pen, PointD a, PointD b);

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
        public void DrawLine(Pen pen, Coord x1, Coord y1, Coord x2, Coord y2) =>
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
        public abstract void DrawLines(Pen pen, PointD[] points);

        /// <summary>
        /// Draws an ellipse defined by a bounding <see cref="RectD"/>.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the ellipse.</param>
        /// <param name="bounds"><see cref="RectD"/> structure that defines the boundaries
        /// of the ellipse.</param>
        public abstract void DrawEllipse(Pen pen, RectD bounds);

        /// <summary>
        /// Draws the specified <see cref="Image"/>, using its original size, at the
        /// specified location.
        /// </summary>
        /// <param name="image"><see cref="Image"/> to draw.</param>
        /// <param name="origin"><see cref="PointD"/> structure that represents the
        /// upper-left corner of the drawn image.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawImageUnscaled(Image image, PointD origin)
            => DrawImage(image, origin);

        /// <summary>
        /// Draws the specified <see cref="Image"/>, using its original size, at the
        /// specified location.
        /// </summary>
        /// <param name="image"><see cref="Image"/> to draw.</param>
        /// <param name="origin"><see cref="PointD"/> structure that represents the
        /// upper-left corner of the drawn image.</param>
        /// <remarks>
        /// When drawing a mono-bitmap, the current text foreground color will
        /// be used to draw the foreground of the bitmap (all bits set to 1), and
        /// the current text background color to draw the background (all bits set to 0).
        /// </remarks>
        public abstract void DrawImage(Image image, PointD origin);

        /// <summary>
        /// Draws the specified <see cref="Image"/>, using its original size, at the
        /// specified location.
        /// </summary>
        /// <param name="image"><see cref="Image"/> to draw.</param>
        /// <param name="x">Horizontal position of the image.</param>
        /// <param name="y">Vertical position of the image.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawImage(Image image, Coord x, Coord y) => DrawImage(image, (x, y));

        /// <summary>
        /// Draws an image into the region defined by the specified <see cref="RectD"/>.
        /// </summary>
        /// <param name="image"><see cref="Image"/> to draw.</param>
        /// <param name="destinationRect">The region in which to draw
        /// <paramref name="image"/>.</param>
        /// <remarks>
        /// When drawing a mono-bitmap, the current text foreground color will
        /// be used to draw the foreground of the bitmap (all bits set to 1), and
        /// the current text background color to draw the background (all bits set to 0).
        /// </remarks>
        public abstract void DrawImage(Image image, RectD destinationRect);

        /// <summary>
        /// Creates translation matrix and calls <see cref="PushTransform(TransformMatrix)"/> with it.
        /// </summary>
        /// <param name="offsetX">The X value of the translation matrix.</param>
        /// <param name="offsetY">The Y value of the translation matrix.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PushAndTranslate(Coord offsetX, Coord offsetY)
        {
            if(offsetX == 0 && offsetY == 0)
            {
                PushTransform();
            }
            else
            {
                var transform = TransformMatrix.CreateTranslation(offsetX, offsetY);
                PushTransform(transform);
            }
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
        /// Calls internally <see cref="GetTextExtent"/>.
        /// </summary>
        /// <param name="text">String to measure.</param>
        /// <param name="font"><see cref="Font"/> that defines the text format of the string.</param>
        /// <returns>
        /// This method returns a <see cref="SizeD"/> structure that represents the size,
        /// in device-independent units, of the
        /// string specified by the <c>text</c> parameter as drawn with the <c>font</c> parameter.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SizeD MeasureText(string text, Font font)
            => GetTextExtent(text, font);

        /// <summary>
        /// Pops a stored transformation matrix from the stack and sets
        /// it to the <see cref="Transform"/> property.
        /// </summary>
        public void PopTransform()
        {
            stack ??= new();
            Transform = stack.Pop();
        }

        /// <summary>
        /// Pushes the current state of the <see cref="Graphics"/> transformation
        /// matrix on a stack
        /// and concatenates the current transform with a new transform.
        /// </summary>
        /// <param name="transform">A transform to concatenate with the current transform.</param>
        public virtual void PushTransform(TransformMatrix transform)
        {
            PushTransform();
            var currentTransform = Transform;
            currentTransform.Multiply(transform);
            Transform = currentTransform;
        }

        /// <summary>
        /// Pushes the current state of the transformation matrix on a stack.
        /// </summary>
        public void PushTransform()
        {
            stack ??= new();
            stack.Push(Transform);
        }

        /// <summary>
        /// Returns the DPI of the display used by this object.
        /// </summary>
        /// <returns>
        /// A <see cref="SizeD"/> value that represents DPI of the device
        /// used by this control. If the DPI is not available,
        /// returns Size(0,0) object.
        /// </returns>
        public abstract SizeI GetDPI();

        /// <summary>
        /// Converts point to device-independent units.
        /// </summary>
        /// <param name="point">Point.</param>
        /// <param name="unit">The unit of measure for the point.</param>
        public virtual void ToDip(ref PointD point, GraphicsUnit unit)
        {
            if (unit != GraphicsUnit.Dip)
            {
                var dpi = GetDPI();
                var graphicsType = GraphicsUnitConverter.GraphicsType.Undefined;
                point = GraphicsUnitConverter.ConvertPoint(
                    unit,
                    GraphicsUnit.Dip,
                    dpi,
                    point,
                    graphicsType);
            }
        }

        /// <summary>
        /// Converts point to pixels.
        /// </summary>
        /// <param name="point">Point.</param>
        /// <param name="unit">The unit of measure for the point.</param>
        public virtual void ToPixels(ref PointD point, GraphicsUnit unit)
        {
            if (unit != GraphicsUnit.Pixel)
            {
                var dpi = GetDPI();
                var graphicsType = GraphicsUnitConverter.GraphicsType.Undefined;
                point = GraphicsUnitConverter.ConvertPoint(
                    unit,
                    GraphicsUnit.Pixel,
                    dpi,
                    point,
                    graphicsType);
            }
        }

        /// <summary>
        /// Converts <see cref="SizeD"/> to device-independent units.
        /// </summary>
        /// <param name="size">Size.</param>
        /// <param name="unit">The unit of measure for the size.</param>
        public virtual void ToDip(ref SizeD size, GraphicsUnit unit)
        {
            if (unit != GraphicsUnit.Dip)
            {
                var dpi = GetDPI();
                var graphicsType = GraphicsUnitConverter.GraphicsType.Undefined;
                size = GraphicsUnitConverter.ConvertSize(
                    unit,
                    GraphicsUnit.Dip,
                    dpi,
                    size,
                    graphicsType);
            }
        }

        /// <summary>
        /// Converts rectangle to device-independent units.
        /// </summary>
        /// <param name="rect">Rectangle.</param>
        /// <param name="unit">The unit of measure for the rectangle.</param>
        public virtual void ToDip(ref RectD rect, GraphicsUnit unit)
        {
            if (unit != GraphicsUnit.Dip)
            {
                var dpi = GetDPI();
                var graphicsType = GraphicsUnitConverter.GraphicsType.Undefined;
                rect = GraphicsUnitConverter.ConvertRect(
                    unit,
                    GraphicsUnit.Dip,
                    dpi,
                    rect,
                    graphicsType);
            }
        }

        /// <summary>
        /// Sets transform matrix of the handler.
        /// </summary>
        /// <param name="matrix">New transform value.</param>
        protected abstract void SetHandlerTransform(TransformMatrix matrix);
    }
}