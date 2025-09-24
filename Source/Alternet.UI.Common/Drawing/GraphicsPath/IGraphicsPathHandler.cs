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

        /// <inheritdoc cref="GraphicsPath.AddLine"/>
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

        /// <inheritdoc cref="GraphicsPath.AddArc"/>
        void AddArc(
            PointD center,
            Coord radius,
            Coord startAngle,
            Coord sweepAngle);

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
