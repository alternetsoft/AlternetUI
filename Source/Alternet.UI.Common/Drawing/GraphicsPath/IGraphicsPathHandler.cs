using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains methods and properties which allow to work with graphics path.
    /// </summary>
    public interface IGraphicsPathHandler : IDisposable
    {
        /// <inheritdoc cref="GraphicsPath.FillMode"/>
        FillMode FillMode { get; set; }

        /// <inheritdoc cref="GraphicsPath.AddLines"/>
        void AddLines(ReadOnlySpan<PointD> points);

        /// <inheritdoc cref="GraphicsPath.AddLine(PointD, PointD)"/>
        void AddLine(PointD pt1, PointD pt2);

        /// <inheritdoc cref="GraphicsPath.AddLineTo"/>
        void AddLineTo(PointD pt);

        /// <inheritdoc cref="GraphicsPath.AddEllipse"/>
        void AddEllipse(RectD rect);

        /// <inheritdoc cref="GraphicsPath.AddBezier"/>
        void AddBezier(
            PointD startPoint,
            PointD controlPoint1,
            PointD controlPoint2,
            PointD endPoint);

        /// <inheritdoc cref="GraphicsPath.AddBezierTo"/>
        void AddBezierTo(
            PointD controlPoint1,
            PointD controlPoint2,
            PointD endPoint);

        /// <inheritdoc cref="GraphicsPath.AddArc(PointD, Coord, Coord, Coord)"/>
        void AddArc(
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle);

        /// <summary>
        /// Appends an elliptical arc to the current figure.
        /// </summary>
        /// <param name="rect">A <see cref="RectD"/> that represents the bounding rectangle of the arc.</param>
        /// <param name="startAngle">The starting angle of the arc, measured in degrees clockwise from the x-axis.</param>
        /// <param name="sweepAngle">The angle between <paramref name="startAngle"/> and the end of the arc.</param>
        void AddArc(RectD rect, float startAngle, float sweepAngle);

        /// <inheritdoc cref="GraphicsPath.AddRectangle"/>
        void AddRectangle(RectD rect);

        /// <inheritdoc cref="GraphicsPath.AddRoundedRectangle"/>
        void AddRoundedRectangle(
            RectD rect,
            Coord cornerRadius);

        /// <inheritdoc cref="GraphicsPath.GetBounds"/>
        RectD GetBounds();

        /// <inheritdoc cref="GraphicsPath.StartFigure"/>
        void StartFigure(PointD point);

        /// <inheritdoc cref="GraphicsPath.CloseFigure"/>
        void CloseFigure();
    }
}
