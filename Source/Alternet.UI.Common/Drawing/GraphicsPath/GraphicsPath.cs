using System;
using System.Runtime.CompilerServices;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents a series of connected lines and curves. This class cannot be inherited.
    /// </summary>
    /// <remarks>
    /// Applications use paths to draw outlines of shapes, fill the interiors of shapes,
    /// and create clipping regions.
    /// The graphics engine maintains the coordinates of geometric shapes in a path in
    /// world coordinate space.
    /// A path may be composed of any number of figures (sub-paths).
    /// Each figure is either composed of a sequence of connected lines and curves or a
    /// geometric shape primitive.
    /// The starting point of a figure is the first point in the sequence of connected
    /// lines and curves.
    /// The ending point is the last point in the sequence.
    /// The starting and ending points of a geometric shape primitive are defined by
    /// the primitive specification.
    /// </remarks>
    public partial class GraphicsPath : HandledObject<IGraphicsPathHandler>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsPath"/> class.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GraphicsPath(Graphics drawingContext)
        {
            if (drawingContext is null)
                throw new ArgumentNullException(nameof(drawingContext));
            Handler = GraphicsFactory.Handler.CreateGraphicsPathHandler(drawingContext);
        }

        /// <summary>
        /// Gets or sets a <see cref="FillMode"/> enumeration that determines how the
        /// interiors of shapes in this <see cref="GraphicsPath"/> are filled.
        /// </summary>
        /// <value>
        /// A <see cref="Drawing.FillMode"/> enumeration that specifies how the interiors
        /// of shapes in this <see cref="GraphicsPath"/> are filled.
        /// The default mode is <see cref="FillMode.Alternate"/>.
        /// </value>
        public FillMode FillMode
        {
            get
            {
                CheckDisposed();
                return Handler.FillMode;
            }

            set
            {
                CheckDisposed();
                Handler.FillMode = value;
            }
        }

        /// <summary>
        /// Appends a series of connected line segments to the end of this <see cref="GraphicsPath"/>.
        /// </summary>
        /// <param name="points">
        /// An array of <see cref="PointD"/> structures that represents the points that define
        /// the line segments to add.
        /// </param>
        /// <remarks>
        /// If there are previous lines or curves in the figure, a line is added to connect
        /// the endpoint
        /// of the previous segment the starting point of the line. The points parameter
        /// specifies an array of endpoints.
        /// The first two specify the first line. Each additional point specifies
        /// the endpoint of a line segment
        /// whose starting point is the endpoint of the previous line.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddLines(ReadOnlySpan<PointD> points)
        {
            CheckDisposed();
            Handler.AddLines(points);
        }

        /// <summary>
        /// Appends a line segment to this <see cref="GraphicsPath"/>.
        /// </summary>
        /// <param name="pt1">A <see cref="PointD"/> that represents the starting
        /// point of the line.</param>
        /// <param name="pt2">A <see cref="PointD"/> that represents the endpoint
        /// of the line.</param>
        /// <remarks>
        /// This method adds the line segment defined by the specified points to the
        /// end of this <see cref="GraphicsPath"/>. If there
        /// are previous lines or curves in the <see cref="GraphicsPath"/>, a line
        /// segment is drawn to connect the last point in the
        /// path to the first point in the new line segment.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddLine(PointD pt1, PointD pt2)
        {
            CheckDisposed();
            Handler.AddLine(pt1, pt2);
        }

        /// <summary>
        /// Appends a line segment to this <see cref="GraphicsPath"/>.
        /// </summary>
        /// <param name="pt">A <see cref="PointD"/> that represents the endpoint of the line.</param>
        /// <remarks>
        /// This method adds the line segment defined by the specified point to
        /// the end of this <see cref="GraphicsPath"/>. If there
        /// are no previous lines or curves in the <see cref="GraphicsPath"/>,
        /// a line segment is drawn for the (0, 0) point.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddLineTo(PointD pt)
        {
            CheckDisposed();
            Handler.AddLineTo(pt);
        }

        /// <summary>
        /// Adds an ellipse to the current path.
        /// </summary>
        /// <param name="rect">A <see cref="RectD"/> that represents the bounding
        /// rectangle that defines the ellipse.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddEllipse(RectD rect)
        {
            CheckDisposed();
            Handler.AddEllipse(rect);
        }

        /// <summary>
        /// Adds a cubic Bezier curve to the current figure.
        /// </summary>
        /// <param name="startPoint">A <see cref="PointD"/> that represents the
        /// starting point of the curve.</param>
        /// <param name="controlPoint1">A <see cref="PointD"/> that represents the
        /// first control point for the curve.</param>
        /// <param name="controlPoint2">A <see cref="PointD"/> that represents the
        /// second control point for the curve.</param>
        /// <param name="endPoint">A <see cref="PointD"/> that represents the
        /// endpoint of the curve.</param>
        /// <remarks>
        /// The cubic curve is constructed from the first point to the fourth point
        /// by using the second and third points
        /// as control points. If there is a previous line or curve segment in the figure,
        /// a line is added to connect
        /// the endpoint of the previous segment to the starting point of the cubic curve.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddBezier(
            PointD startPoint,
            PointD controlPoint1,
            PointD controlPoint2,
            PointD endPoint)
        {
            CheckDisposed();
            Handler.AddBezier(
                startPoint,
                controlPoint1,
                controlPoint2,
                endPoint);
        }

        /// <summary>
        /// Adds a cubic Bezier curve to the current figure.
        /// </summary>
        /// <param name="controlPoint1">A <see cref="PointD"/> that represents the first
        /// control point for the curve.</param>
        /// <param name="controlPoint2">A <see cref="PointD"/> that represents the second
        /// control point for the curve.</param>
        /// <param name="endPoint">A <see cref="PointD"/> that represents the endpoint
        /// of the curve.</param>
        /// <remarks>
        /// The cubic curve is constructed from the last point in the figure to the fourth
        /// point by using the second and third points
        /// as control points. If there is a previous line or curve segment in the figure,
        /// a line is added to connect
        /// the endpoint of the previous segment to the starting point of the cubic curve.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddBezierTo(PointD controlPoint1, PointD controlPoint2, PointD endPoint)
        {
            CheckDisposed();
            Handler.AddBezierTo(
                controlPoint1,
                controlPoint2,
                endPoint);
        }

        /// <summary>
        /// Appends a circular arc to the current figure.
        /// </summary>
        /// <param name="center">The center <see cref="PointD"/> of the arc.</param>
        /// <param name="radius">The radius of the arc.</param>
        /// <param name="startAngle">The starting angle of the arc, measured
        /// in degrees clockwise from the
        /// x-axis.</param>
        /// <param name="sweepAngle">The angle between <paramref name="startAngle"/>
        /// and the end of the arc.</param>
        /// <remarks>
        /// If there are previous lines or curves in the figure, a line is added to connect
        /// the endpoint of the previous
        /// segment to the beginning of the arc. The starting point of the arc is determined
        /// by measuring clockwise from
        /// the x-axis of the ellipse (at the 0-degree angle) by the number of degrees in
        /// the start angle. The endpoint
        /// is similarly located by measuring clockwise from the starting point by the
        /// number of degrees in the sweep
        /// angle.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddArc(PointD center, Coord radius, Coord startAngle, Coord sweepAngle)
        {
            CheckDisposed();
            Handler.AddArc(
                center,
                radius,
                startAngle,
                sweepAngle);
        }

        /// <summary>
        /// Adds a rectangle to this path.
        /// </summary>
        /// <param name="rect">A <see cref="RectD"/> that represents the rectangle to add.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddRectangle(RectD rect)
        {
            CheckDisposed();
            Handler.AddRectangle(rect);
        }

        /// <summary>
        /// Adds a rounded rectangle to this path.
        /// </summary>
        /// <param name="rect">A <see cref="RectD"/> that represents the rectangle to add.</param>
        /// <param name="cornerRadius">The corner radius of the rectangle.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddRoundedRectangle(RectD rect, Coord cornerRadius)
        {
            CheckDisposed();
            Handler.AddRoundedRectangle(rect, cornerRadius);
        }

        /// <summary>
        /// Returns a rectangle that bounds this <see cref="GraphicsPath"/>.
        /// </summary>
        /// <returns>A <see cref="RectD"/> that represents a rectangle that bounds this
        /// <see cref="GraphicsPath"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RectD GetBounds()
        {
            CheckDisposed();
            return Handler.GetBounds();
        }

        /// <summary>
        /// Starts a new figure without closing the current figure. All subsequent points
        /// added to the path are added to this new figure.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StartFigure(PointD point)
        {
            CheckDisposed();
            Handler.StartFigure(point);
        }

        /// <summary>
        /// Closes the current figure and starts a new figure. If the current figure
        /// contains a sequence of connected
        /// lines and curves, the method closes the loop by connecting a line from the
        /// endpoint to the starting point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CloseFigure()
        {
            CheckDisposed();
            Handler.CloseFigure();
        }

        /// <inheritdoc/>
        protected override IGraphicsPathHandler CreateHandler()
        {
            return GraphicsFactory.Handler.CreateGraphicsPathHandler();
        }
    }
}