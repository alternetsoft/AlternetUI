using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides a implementation of the <see cref="IGraphicsPathHandler"/> interface
    /// which does not perform any operations.
    /// </summary>
    public class PlessGraphicsPathHandler: DisposableObject, IGraphicsPathHandler
    {
        /// <inheritdoc/>
        public FillMode FillMode { get; set; }

        /// <inheritdoc/>
        public void AddArc(PointD center, float radius, float startAngle, float sweepAngle)
        {
        }

        /// <inheritdoc/>
        public void AddBezier(PointD startPoint, PointD controlPoint1, PointD controlPoint2, PointD endPoint)
        {
        }

        /// <inheritdoc/>
        public void AddBezierTo(PointD controlPoint1, PointD controlPoint2, PointD endPoint)
        {
        }

        /// <inheritdoc/>
        public void AddEllipse(RectD rect)
        {
        }

        /// <inheritdoc/>
        public void AddLine(PointD pt1, PointD pt2)
        {
        }

        /// <inheritdoc/>
        public void AddLines(ReadOnlySpan<PointD> points)
        {
        }

        /// <inheritdoc/>
        public void AddLineTo(PointD pt)
        {
        }

        /// <inheritdoc/>
        public void AddRectangle(RectD rect)
        {
        }

        /// <inheritdoc/>
        public void AddRoundedRectangle(RectD rect, float cornerRadius)
        {
        }

        /// <inheritdoc/>
        public void CloseFigure()
        {
        }

        /// <inheritdoc/>
        public RectD GetBounds()
        {
            return RectD.Empty;
        }

        /// <inheritdoc/>
        public void StartFigure(PointD point)
        {
        }
    }
}
