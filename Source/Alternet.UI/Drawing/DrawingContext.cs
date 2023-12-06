using System;
using System.Diagnostics;
using Alternet.UI;
using Alternet.UI.Internal.ComponentModel;
using Alternet.UI.Localization;
using Alternet.UI.Native;

namespace Alternet.Drawing
{
    /// <summary>
    /// Describes a drawing surface.
    /// </summary>
    public class DrawingContext : DisposableObject
    {
        private UI.Native.DrawingContext dc;

        internal DrawingContext(UI.Native.DrawingContext dc)
        {
            this.dc = dc;
        }

        /// <summary>
        /// Gets or sets a copy of the geometric world transformation for this
        /// <see cref="DrawingContext"/>.
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
        /// <see cref="DrawingContext"/>.
        /// </summary>
        /// <value>
        /// A <see cref="Region"/> that limits the portion of this <see cref="DrawingContext"/>
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
        /// Gets or sets the interpolation mode associated with this <see cref="DrawingContext"/>.
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

        internal UI.Native.DrawingContext NativeDrawingContext => dc;

        /// <summary>
        /// Creates a new <see cref="DrawingContext"/> from the specified
        /// <see cref="Image"/>.
        /// </summary>
        /// <param name="image"><see cref="Image"/> from which to create the
        /// new <see cref="DrawingContext"/>.</param>
        /// <returns>A new <see cref="DrawingContext"/> for the specified
        /// <see cref="Image"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="image"/>
        /// is <see langword="null"/>.</exception>
        /// <remarks>
        /// Use this method to draw on the specified image.
        /// You should always call the Dispose() method to release
        /// the <see cref="DrawingContext"/> and
        /// related resources created by the <see cref="FromImage"/> method.
        /// </remarks>
        public static DrawingContext FromImage(Image image)
        {
#if DEBUG
            if (image is null)
                throw new ArgumentNullException(nameof(image));
#endif
            return new DrawingContext(UI.Native.DrawingContext.FromImage(image.NativeImage));
        }

        /// <summary>
        /// Fills the interior of a rectangle specified by a <see cref="Rect"/> structure.
        /// </summary>
        /// <param name="brush"><see cref="Brush"/> that determines the characteristics
        /// of the fill.</param>
        /// <param name="rectangle"><see cref="Rect"/> structure that represents the
        /// rectangle to fill.</param>
        /// <remarks>
        /// This method fills the interior of the rectangle defined by the <c>rect</c> parameter,
        /// including the specified upper-left corner and up to the calculated lower and bottom edges.
        /// </remarks>
        public void FillRectangle(Brush brush, Rect rectangle)
        {
#if DEBUG
            if (brush is null)
                throw new ArgumentNullException(nameof(brush));
#endif
            dc.FillRectangle(brush.NativeBrush, rectangle);
        }

        /// <summary>
        /// Draws an arc representing a portion of a circle specified by a center
        /// <see cref="Point"/> and a radius.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the arc.</param>
        /// <param name="center"><see cref="Point"/> structure that defines the center of the
        /// circle.</param>
        /// <param name="radius">Defines the radius of the circle.</param>
        /// <param name="startAngle">Angle in degrees measured clockwise from the x-axis to
        /// the starting point of the arc.</param>
        /// <param name="sweepAngle">Angle in degrees measured clockwise from the
        /// <paramref name="startAngle"/>
        /// parameter to ending point of the arc.</param>
        public void DrawArc(Pen pen, Point center, double radius, double startAngle, double sweepAngle)
        {
#if DEBUG
            if (pen is null)
                throw new ArgumentNullException(nameof(pen));
#endif
            dc.DrawArc(pen.NativePen, center, radius, startAngle, sweepAngle);
        }

        /// <summary>
        /// Draws debug points on the corners of the specified rectangle.
        /// </summary>
        /// <param name="pen">Color of the debug points. if <c>null</c>, red color is used.</param>
        /// <param name="rect"></param>
        [Conditional("DEBUG")]
        public void DrawDebugPoints(Rect rect, Pen? pen = null)
        {
            void DrawDebugPoint(Point p)
            {
                DrawPoint(pen, p.X, p.Y);
            }

            pen ??= Pens.Red;

            DrawDebugPoint(rect.TopLeft);
            DrawDebugPoint(new Point(rect.Right - 1, rect.Top));
            DrawDebugPoint(new Point(rect.Right - 1, rect.Bottom - 1));
            DrawDebugPoint(new Point(rect.Left, rect.Bottom - 1));
        }

        /// <summary>
        /// Draws point with the specified color.
        /// </summary>
        /// <param name="pen">Color of the point.</param>
        /// <param name="x">X-coordinate of the point.</param>
        /// <param name="y">Y-coordinate of the point.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="pen"/> is <c>null</c>.</exception>
        public void DrawPoint(Pen pen, double x, double y)
        {
#if DEBUG
            if (pen is null)
                throw new ArgumentNullException(nameof(pen));
#endif
            dc.DrawPoint(pen.NativePen, x, y);
        }

        /// <summary>
        /// Fills the interior of a pie section defined by a circle specified by a center
        /// <see cref="Point"/> and a radius.
        /// </summary>
        /// <param name="brush"><see cref="Brush"/> that determines the characteristics of
        /// the fill.</param>
        /// <param name="center"><see cref="Point"/> structure that defines the center of
        /// the circle.</param>
        /// <param name="radius">Defines the radius of the circle.</param>
        /// <param name="startAngle">Angle in degrees measured clockwise from the x-axis
        /// to the first side of the pie section.</param>
        /// <param name="sweepAngle">Angle in degrees measured clockwise from the startAngle
        /// parameter to the second side of the pie section.</param>
        public void FillPie(
            Brush brush,
            Point center,
            double radius,
            double startAngle,
            double sweepAngle)
        {
#if DEBUG
            if (brush is null)
                throw new ArgumentNullException(nameof(brush));
#endif
            dc.FillPie(brush.NativeBrush, center, radius, startAngle, sweepAngle);
        }

        /// <summary>
        /// Draws an outline of a pie section defined by a circle specified by a center
        /// <see cref="Point"/> and a radius.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and
        /// style of the pie section.</param>
        /// <param name="center"><see cref="Point"/> structure that defines the center
        /// of the circle.</param>
        /// <param name="radius">Defines the radius of the circle.</param>
        /// <param name="startAngle">Angle in degrees measured clockwise from the x-axis
        /// to the first side of the pie section.</param>
        /// <param name="sweepAngle">Angle in degrees measured clockwise from the startAngle
        /// parameter to the second side of the pie section.</param>
        public void DrawPie(
            Pen pen,
            Point center,
            double radius,
            double startAngle,
            double sweepAngle)
        {
#if DEBUG
            if (pen is null)
                throw new ArgumentNullException(nameof(pen));
#endif
            dc.DrawPie(pen.NativePen, center, radius, startAngle, sweepAngle);
        }

        /// <summary>
        /// Draws a Bézier spline defined by four <see cref="Point"/> structures.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the curve.</param>
        /// <param name="startPoint"><see cref="Point"/> structure that represents the starting
        /// point of the curve.</param>
        /// <param name="controlPoint1"><see cref="Point"/> structure that represents the first
        /// control point for the curve.</param>
        /// <param name="controlPoint2"><see cref="Point"/> structure that represents the second
        /// control point for the curve.</param>
        /// <param name="endPoint"><see cref="Point"/> structure that represents the ending point
        /// of the curve.</param>
        public void DrawBezier(
            Pen pen,
            Point startPoint,
            Point controlPoint1,
            Point controlPoint2,
            Point endPoint)
        {
#if DEBUG
            if (pen is null)
                throw new ArgumentNullException(nameof(pen));
#endif
            dc.DrawBezier(pen.NativePen, startPoint, controlPoint1, controlPoint2, endPoint);
        }

        /// <summary>
        /// Draws a series of Bézier splines from an array of <see cref="Point"/> structures.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the curve.</param>
        /// <param name="points">
        /// Array of <see cref="Point"/> structures that represent the points that determine the curve.
        /// The number of points in the array should be a multiple of 3 plus 1, such as 4, 7, or 10.
        /// </param>
        public void DrawBeziers(Pen pen, Point[] points)
        {
#if DEBUG
            if (pen is null)
                throw new ArgumentNullException(nameof(pen));

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
        /// Draws an circle specified by a center <see cref="Point"/> and a radius.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the circle.</param>
        /// <param name="center"><see cref="Point"/> structure that defines the center of
        /// the circle.</param>
        /// <param name="radius">Defines the radius of the circle.</param>
        public void DrawCircle(Pen pen, Point center, double radius)
        {
#if DEBUG
            if (pen is null)
                throw new ArgumentNullException(nameof(pen));
#endif
            dc.DrawCircle(pen.NativePen, center, radius);
        }

        /// <summary>
        /// Fills the interior of a circle specified by a center <see cref="Point"/> and a radius.
        /// </summary>
        /// <param name="brush"><see cref="Brush"/> that determines the characteristics of
        /// the fill.</param>
        /// <param name="center"><see cref="Point"/> structure that defines the center of
        /// the circle.</param>
        /// <param name="radius">Defines the radius of the circle.</param>
        public void FillCircle(Brush brush, Point center, double radius)
        {
#if DEBUG
            if (brush is null)
                throw new ArgumentNullException(nameof(brush));
#endif
            dc.FillCircle(brush.NativeBrush, center, radius);
        }

        /// <summary>
        /// Draws a rounded rectangle specified by a <see cref="Rect"/> and a corner radius.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and
        /// style of the rounded rectangle.</param>
        /// <param name="rect">A <see cref="Rect"/> that represents the rectangle to add.</param>
        /// <param name="cornerRadius">The corner radius of the rectangle.</param>
        public void DrawRoundedRectangle(Pen pen, Rect rect, double cornerRadius)
        {
#if DEBUG
            if (pen is null)
                throw new ArgumentNullException(nameof(pen));
#endif

            dc.DrawRoundedRectangle(pen.NativePen, rect, cornerRadius);
        }

        /// <summary>
        /// Fills the interior of a rounded rectangle specified by a <see cref="Rect"/> and
        /// a corner radius.
        /// </summary>
        /// <param name="brush"><see cref="Brush"/> that determines the characteristics of the
        /// fill.</param>
        /// <param name="rect">A <see cref="Rect"/> that represents the rectangle to add.</param>
        /// <param name="cornerRadius">The corner radius of the rectangle.</param>
        public void FillRoundedRectangle(Brush brush, Rect rect, double cornerRadius)
        {
#if DEBUG
            if (brush is null)
                throw new ArgumentNullException(nameof(brush));
#endif

            dc.FillRoundedRectangle(brush.NativeBrush, rect, cornerRadius);
        }

        /// <summary>
        /// Draws a polygon defined by an array of <see cref="Point"/> structures.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the polygon.</param>
        /// <param name="points">Array of <see cref="Point"/> structures that represent the
        /// vertices of the polygon.</param>
        public void DrawPolygon(Pen pen, Point[] points)
        {
#if DEBUG
            if (pen is null)
                throw new ArgumentNullException(nameof(pen));
#endif
            dc.DrawPolygon(pen.NativePen, points);
        }

        /// <summary>
        /// Fills the interior of a polygon defined by an array of <see cref="Point"/> structures.
        /// </summary>
        /// <param name="brush"><see cref="Brush"/> that determines the characteristics of
        /// the fill.</param>
        /// <param name="points">Array of <see cref="Point"/> structures that represent the
        /// vertices of the polygon.</param>
        /// <param name="fillMode">Member of the <see cref="FillMode"/> enumeration that
        /// determines the style of the fill.</param>
        public void FillPolygon(Brush brush, Point[] points, FillMode fillMode = FillMode.Alternate)
        {
#if DEBUG
            if (brush is null)
                throw new ArgumentNullException(nameof(brush));
#endif
            dc.FillPolygon(brush.NativeBrush, points, (UI.Native.FillMode)fillMode);
        }

        /// <summary>
        /// Draws a series of rectangles specified by <see cref="Rect"/> structures.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the outlines of the rectangles.</param>
        /// <param name="rects">Array of <see cref="Rect"/> structures that represent the
        /// rectangles to draw.</param>
        public void DrawRectangles(Pen pen, Rect[] rects)
        {
#if DEBUG
            if (pen is null)
                throw new ArgumentNullException(nameof(pen));
#endif
            dc.DrawRectangles(pen.NativePen, rects);
        }

        /// <summary>
        /// Fills a series of rectangles specified by <see cref="Rect"/> structures.
        /// </summary>
        /// <param name="brush"><see cref="Brush"/> that determines the characteristics
        /// of the fill.</param>
        /// <param name="rects">Array of <see cref="Rect"/> structures that represent the
        /// rectangles to fill.</param>
        public void FillRectangles(Brush brush, Rect[] rects)
        {
#if DEBUG
            if (brush is null)
                throw new ArgumentNullException(nameof(brush));
#endif
            dc.FillRectangles(brush.NativeBrush, rects);
        }

        /// <summary>
        /// Fills the interior of an ellipse defined by a bounding rectangle specified by
        /// a <see cref="Rect"/> structure.
        /// </summary>
        /// <param name="brush"><see cref="Brush"/> that determines the characteristics
        /// of the fill.</param>
        /// <param name="bounds"><see cref="Rect"/> structure that represents the bounding
        /// rectangle that defines the ellipse.</param>
        /// <remarks>
        /// This method fills the interior of an ellipse with a <see cref="Brush"/>.
        /// The ellipse is defined by the bounding rectangle represented by the <c>bounds</c>
        /// parameter.
        /// </remarks>
        public void FillEllipse(Brush brush, Rect bounds)
        {
#if DEBUG
            if (brush is null)
                throw new ArgumentNullException(nameof(brush));
#endif
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
        public void FloodFill(Brush brush, Point point)
        {
#if DEBUG
            if (brush is null)
                throw new ArgumentNullException(nameof(brush));

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
        /// Draws a rectangle specified by a <see cref="Rect"/> structure.
        /// </summary>
        /// <param name="pen">A <see cref="Pen"/> that determines the color, width, and style
        /// of the rectangle.</param>
        /// <param name="rectangle">A <see cref="Rect"/> structure that represents the
        /// rectangle to draw.</param>
        public void DrawRectangle(Pen pen, Rect rectangle)
        {
#if DEBUG
            if (pen is null)
                throw new ArgumentNullException(nameof(pen));
#endif
            dc.DrawRectangle(pen.NativePen, rectangle);
        }

        /// <summary>
        /// Draws a <see cref="GraphicsPath"/>.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and
        /// style of the path.</param>
        /// <param name="path"><see cref="GraphicsPath"/> to draw.</param>
        public void DrawPath(Pen pen, GraphicsPath path)
        {
#if DEBUG
            if (pen is null)
                throw new ArgumentNullException(nameof(pen));
#endif
            dc.DrawPath(pen.NativePen, path.NativePath);
        }

        /// <summary>
        /// Fills the interior of a <see cref="GraphicsPath"/>.
        /// </summary>
        /// <param name="brush"><see cref="Brush"/> that determines the characteristics
        /// of the fill.</param>
        /// <param name="path"><see cref="GraphicsPath"/> that represents the path to fill.</param>
        public void FillPath(Brush brush, GraphicsPath path)
        {
#if DEBUG
            if (brush is null)
                throw new ArgumentNullException(nameof(brush));
#endif
            dc.FillPath(brush.NativeBrush, path.NativePath);
        }

        /// <summary>
        /// Draws a line connecting two <see cref="Point"/> structures.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the line.</param>
        /// <param name="a"><see cref="Point"/> structure that represents the first point
        /// to connect.</param>
        /// <param name="b"><see cref="Point"/> structure that represents the second point
        /// to connect.</param>
        /// <exception cref="ArgumentNullException"><paramref name="pen"/> is
        /// <see langword="null"/>.</exception>
        public void DrawLine(Pen pen, Point a, Point b)
        {
#if DEBUG
            if (pen is null)
                throw new ArgumentNullException(nameof(pen));
#endif
            dc.DrawLine(pen.NativePen, a, b);
        }

        /// <summary>
        /// Draws a series of line segments that connect an array of <see cref="Point"/> structures.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the line segments.</param>
        /// <param name="points">Array of <see cref="Point"/> structures that represent the
        /// points to connect.</param>
        /// <remarks>
        /// This method draws a series of lines connecting an array of ending points.
        /// The first two points in the array specify the first line.
        /// Each additional point specifies the end of a line segment whose starting point
        /// is the ending point of the previous line segment.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="pen"/> is
        /// <see langword="null"/>.</exception>
        public void DrawLines(Pen pen, Point[] points)
        {
#if DEBUG
            if (pen is null)
                throw new ArgumentNullException(nameof(pen));
#endif
            dc.DrawLines(pen.NativePen, points);
        }

        /// <summary>
        /// Draws an ellipse defined by a bounding <see cref="Rect"/>.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the ellipse.</param>
        /// <param name="bounds"><see cref="Rect"/> structure that defines the boundaries
        /// of the ellipse.</param>
        public void DrawEllipse(Pen pen, Rect bounds)
        {
#if DEBUG
            if (pen is null)
                throw new ArgumentNullException(nameof(pen));
#endif
            dc.DrawEllipse(pen.NativePen, bounds);
        }

        /// <summary>
        /// Draws the specified <see cref="Image"/>, using its original size, at the
        /// specified location.
        /// </summary>
        /// <param name="image"><see cref="Image"/> to draw.</param>
        /// <param name="origin"><see cref="Point"/> structure that represents the
        /// upper-left corner of the drawn image.</param>
        public void DrawImageUnscaled(Image image, Point origin) => DrawImage(image, origin);

        /// <summary>
        /// Draws the specified <see cref="Image"/>, using its original size, at the
        /// specified location.
        /// </summary>
        /// <param name="image"><see cref="Image"/> to draw.</param>
        /// <param name="origin"><see cref="Point"/> structure that represents the
        /// upper-left corner of the drawn image.</param>
        public void DrawImage(Image image, Point origin)
        {
#if DEBUG
            if (image is null)
                throw new ArgumentNullException(nameof(image));
#endif
            dc.DrawImageAtPoint(image.NativeImage, origin);
        }

        /// <summary>
        /// Draws an image into the region defined by the specified <see cref="Rect"/>.
        /// </summary>
        /// <param name="image"><see cref="Image"/> to draw.</param>
        /// <param name="destinationRect">The region in which to draw
        /// <paramref name="image"/>.</param>
        public void DrawImage(Image image, Rect destinationRect)
        {
#if DEBUG
            if (image is null)
                throw new ArgumentNullException(nameof(image));
#endif
            dc.DrawImageAtRect(image.NativeImage, destinationRect);
        }

        /// <summary>
        /// Draws the specified portion of the image into the region defined by the specified
        /// <see cref="Rect"/>.
        /// </summary>
        /// <param name="image"><see cref="Image"/> to draw.</param>
        /// <param name="destinationRect">The region in which to draw
        /// <paramref name="image"/>.</param>
        /// <param name="sourceRect">
        /// <see cref="Rect"/> structure that specifies the portion of the
        /// <paramref name="image"/> object to draw.
        /// </param>
        public void DrawImage(Image image, Rect destinationRect, Rect sourceRect)
        {
#if DEBUG
            if (image is null)
                throw new ArgumentNullException(nameof(image));
#endif
            dc.DrawImagePortionAtRect(image.NativeImage, destinationRect, sourceRect);
        }

        /// <summary>
        /// Draws the specified text string at the specified location with the specified
        /// <see cref="Brush"/> and <see cref="Font"/> objects.
        /// </summary>
        /// <param name="text">String to draw.</param>
        /// <param name="font"><see cref="Font"/> that defines the text format of the string.</param>
        /// <param name="brush"><see cref="Brush"/> that determines the color and texture of
        /// the drawn text.</param>
        /// <param name="origin"><see cref="Point"/> structure that specifies the upper-left
        /// corner of the drawn text.</param>
        public void DrawText(string text, Font font, Brush brush, Point origin)
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
        /// <param name="origin"><see cref="Point"/> structure that specifies the upper-left
        /// corner of the drawn text.</param>
        /// <param name="format"><see cref="TextFormat"/> that specifies formatting attributes,
        /// such as
        /// alignment and trimming, that are applied to the drawn text.</param>
        public void DrawText(string text, Font font, Brush brush, Point origin, TextFormat format)
        {
#if DEBUG
            if (text is null)
                throw new ArgumentNullException(nameof(text));

            if (font is null)
                throw new ArgumentNullException(nameof(font));

            if (format is null)
                throw new ArgumentNullException(nameof(format));
#endif
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
        /// <param name="bounds"><see cref="Rect"/> structure that specifies the bounds of
        /// the drawn text.</param>
        public void DrawText(string text, Font font, Brush brush, Rect bounds)
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
        /// <param name="bounds"><see cref="Rect"/> structure that specifies the bounds of
        /// the drawn text.</param>
        /// <param name="format"><see cref="TextFormat"/> that specifies formatting attributes,
        /// such as
        /// alignment and trimming, that are applied to the drawn text.</param>
        public void DrawText(
            string text,
            Font font,
            Brush brush,
            Rect bounds,
            TextFormat format)
        {
#if DEBUG
            if (text is null)
                throw new ArgumentNullException(nameof(text));

            if (font is null)
                throw new ArgumentNullException(nameof(font));
#endif
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
        /// Measures the specified string when drawn with the specified <see cref="Font"/>.
        /// </summary>
        /// <param name="text">String to measure.</param>
        /// <param name="font"><see cref="Font"/> that defines the text format of the string.</param>
        /// <returns>
        /// This method returns a <see cref="Size"/> structure that represents the size,
        /// in device-independent units (1/96th inch per unit), of the
        /// string specified by the <c>text</c> parameter as drawn with the <c>font</c> parameter.
        /// </returns>
        public Size MeasureText(string text, Font font)
        {
#if DEBUG
            if (text is null)
                throw new ArgumentNullException(nameof(text));

            if (font is null)
                throw new ArgumentNullException(nameof(font));
#endif
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
        /// This method returns a <see cref="Size"/> structure that represents the size,
        /// in device-independent units (1/96th inch per unit), of the
        /// string specified by the <c>text</c> parameter as drawn with the <c>font</c> parameter.
        /// </returns>
        public Size MeasureText(string text, Font font, double maximumWidth)
        {
#if DEBUG
            if (text is null)
                throw new ArgumentNullException(nameof(text));

            if (font is null)
                throw new ArgumentNullException(nameof(font));
#endif
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
        /// This method returns a <see cref="Size"/> structure that represents the size,
        /// in device-independent units (1/96th inch per unit), of the
        /// string specified by the <c>text</c> parameter as drawn with the <c>font</c> parameter.
        /// </returns>
        public Size MeasureText(string text, Font font, double maximumWidth, TextFormat format)
        {
#if DEBUG
            if (text is null)
                throw new ArgumentNullException(nameof(text));

            if (font is null)
                throw new ArgumentNullException(nameof(font));

            if (format is null)
                throw new ArgumentNullException(nameof(format));
#endif
            return dc.MeasureText(
                text,
                font.NativeFont,
                maximumWidth,
                (UI.Native.TextWrapping)format.Wrapping);
        }

        /// <summary>
        /// Pushes the current state of the <see cref="DrawingContext"/> transformation
        /// matrix on a stack.
        /// </summary>
        public void Push()
        {
            dc.Push();
        }

        /// <summary>
        /// Pushes the current state of the <see cref="DrawingContext"/> transformation
        /// matrix on a stack
        /// and concatenates the current transform with a new transform.
        /// </summary>
        /// <param name="transform">A transform to concatenate with the current transform.</param>
        public void PushTransform(TransformMatrix transform)
        {
            Push();
            var currentTransform = Transform;
            currentTransform.Multiply(transform);
            Transform = currentTransform;
        }

        /// <summary>
        /// Pops a stored state from the stack and sets the current transformation matrix
        /// to that state.
        /// </summary>
        public void Pop()
        {
            dc.Pop();
        }

        /// <inheritdoc/>
        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            dc.Dispose();
            dc = null!;
        }
    }
}