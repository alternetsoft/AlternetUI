using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides an implementation of the <see cref="IGraphicsPathHandler"/> interface for SkiaSharp platform.
    /// </summary>
    public class SkiaGraphicsPathHandler : DisposableObject, IGraphicsPathHandler
    {
        private readonly SkiaGraphics graphics;
        private readonly SKPath path = new ();
        private FillMode fillMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaGraphicsPathHandler"/> class.
        /// </summary>
        /// <param name="graphics">The <see cref="SkiaGraphics"/> instance associated with this path handler.</param>
        public SkiaGraphicsPathHandler(SkiaGraphics graphics)
        {
            this.graphics = graphics;
        }

        /// <summary>
        /// Gets the underlying SkiaSharp <see cref="SKPath"/> object.
        /// </summary>
        public SKPath Path => path;

        /// <inheritdoc/>
        public virtual FillMode FillMode
        {
            get => fillMode;
            set
            {
                if (fillMode == value)
                    return;
                fillMode = value;
                path.FillType = fillMode.ToSkia();
            }
        }

        /// <inheritdoc/>
        public virtual void AddRectangle(RectD rect)
        {
            path.AddRect(rect);
        }

        /// <inheritdoc/>
        public virtual void AddArc(PointD center, float radius, float startAngle, float sweepAngle)
        {
            var rect = RectD.GetCircleBoundingBox(center, radius);
            path.AddArc(rect, startAngle, sweepAngle);
        }

        /// <inheritdoc/>
        public virtual void AddLineTo(PointD pt)
        {
            path.LineTo(pt);
        }

        /// <inheritdoc/>
        public virtual void CloseFigure()
        {
            path.Close();
        }

        /// <inheritdoc/>
        public virtual void AddRoundedRectangle(RectD rect, float cornerRadius)
        {
            path.AddRoundRect(rect, cornerRadius, cornerRadius);
        }

        /// <inheritdoc/>
        public virtual RectD GetBounds()
        {
            return path.Bounds;
        }

        /// <inheritdoc/>
        public virtual void AddEllipse(RectD rect)
        {
            path.AddOval(rect);
        }

        /// <inheritdoc/>
        public virtual void AddLines(ReadOnlySpan<PointD> points)
        {
            if (points.Length < 2)
                return;

            path.MoveTo(points[0]);

            for (int i = 1; i < points.Length; i++)
                path.LineTo(points[i]);
        }

        /// <inheritdoc/>
        public virtual void AddLine(PointD pt1, PointD pt2)
        {
            path.MoveTo(pt1);
            path.LineTo(pt2);
        }

        /// <inheritdoc/>
        public virtual void StartFigure(PointD point)
        {
            path.MoveTo(point);
        }

        /// <inheritdoc/>
        public virtual void AddBezier(PointD startPoint, PointD controlPoint1, PointD controlPoint2, PointD endPoint)
        {
            path.MoveTo(startPoint);
            path.CubicTo(controlPoint1, controlPoint2, endPoint);
        }

        /// <inheritdoc/>
        public virtual void AddBezierTo(PointD controlPoint1, PointD controlPoint2, PointD endPoint)
        {
            path.CubicTo(controlPoint1, controlPoint2, endPoint);
        }
    }
}
