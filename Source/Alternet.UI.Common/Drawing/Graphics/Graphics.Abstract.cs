using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.Drawing
{
    public partial class Graphics
    {
        /// <summary>
        /// Returns true if the object is ok to use.
        /// </summary>
        public abstract bool IsOk { get; }

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
        /// Gets the type of graphics backend used by the implementation.
        /// </summary>
        public abstract GraphicsBackendType BackendType { get; }

        /// <summary>
        /// Gets native drawing context.
        /// </summary>
        public abstract object NativeObject { get; }

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
            ReadOnlySpan<PointD> points,
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
        /// Gets the handle to the device context associated with this <see cref="Graphics" />.
        /// The following values are returned by wxWidgets backend: on Windows the return value is an HDC,
        /// on macOS it is a CGContextRef and on wxGTK it will be a GdkDrawable.
        /// </summary>
        /// <returns>
        /// Handle to the device context associated with this <see cref="Graphics" />.
        /// A value of <see cref="IntPtr.Zero"/> is returned if the DC does
        /// not have anything that fits the handle concept.
        /// </returns>
        /// <remarks>
        /// Call the <see cref="ReleaseHdc" /> method to release the device context handle when it is no longer needed.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public virtual IntPtr GetHdc() => IntPtr.Zero;

        /// <summary>
        /// Releases a device context handle obtained by a previous call to the <see cref="GetHdc" />
        /// method of this <see cref="Graphics" />.</summary>
        /// <param name="hdc">Handle to a device context obtained by a previous call to the
        /// <see cref="GetHdc" /> method of this <see cref="Graphics" />.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public virtual void ReleaseHdc(IntPtr hdc)
        {
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
        public abstract void DrawBeziers(Pen pen, ReadOnlySpan<PointD> points);

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
        public abstract void DrawPolygon(Pen pen, ReadOnlySpan<PointD> points);

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
            ReadOnlySpan<PointD> points,
            FillMode fillMode = FillMode.Alternate);

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
        /// Returns the DPI of the display used by this object.
        /// </summary>
        /// <returns>
        /// A <see cref="SizeD"/> value that represents DPI of the device
        /// used by this control. If the DPI is not available,
        /// returns Size(0,0) object.
        /// </returns>
        public abstract SizeI GetDPI();

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
        public abstract void DrawLines(Pen pen, ReadOnlySpan<PointD> points);

        /// <summary>
        /// Draws an ellipse defined by a bounding <see cref="RectD"/>.
        /// </summary>
        /// <param name="pen"><see cref="Pen"/> that determines the color, width, and style
        /// of the ellipse.</param>
        /// <param name="bounds"><see cref="RectD"/> structure that defines the boundaries
        /// of the ellipse.</param>
        public abstract void DrawEllipse(Pen pen, RectD bounds);

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
        /// Sets transform matrix of the handler.
        /// </summary>
        /// <param name="matrix">New transform value.</param>
        protected abstract void SetHandlerTransform(TransformMatrix matrix);
    }
}
