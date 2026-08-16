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
        private SKPathBuilder pathBuilder;
        private SKPath? path;
        private FillMode fillMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaGraphicsPathHandler"/> class
        /// with the specified <see cref="SKPathBuilder"/>.
        /// </summary>
        public SkiaGraphicsPathHandler(SKPathBuilder path)
        {
            this.pathBuilder = path;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaGraphicsPathHandler"/> class.
        /// </summary>
        public SkiaGraphicsPathHandler()
        {
            pathBuilder = new();
        }

        /// <summary>
        /// Gets the underlying SkiaSharp <see cref="SKPathBuilder"/> object.
        /// </summary>
        public SKPathBuilder PathBuilder
        {
            get
            {
                return pathBuilder;
            }

            set
            {
                pathBuilder = value;
                ResetPath();
            }
        }

        /// <inheritdoc/>
        public virtual FillMode FillMode
        {
            get => fillMode;
            set
            {
                if (fillMode == value)
                    return;
                ResetPath();
                fillMode = value;
                pathBuilder.FillType = fillMode.ToSkia();
            }
        }

        /// <inheritdoc/>
        public virtual void AddRectangle(RectD rect)
        {
            ResetPath();
            pathBuilder.AddRect(rect);
        }

        /// <inheritdoc/>
        public virtual void AddLineTo(PointD pt)
        {
            ResetPath();
            pathBuilder.LineTo(pt);
        }

        /// <inheritdoc/>
        public virtual void CloseFigure()
        {
            ResetPath();
            pathBuilder.Close();
        }

        /// <inheritdoc/>
        public virtual void AddRoundedRectangle(RectD rect, float cornerRadius)
        {
            ResetPath();
            pathBuilder.AddRoundRect(rect, cornerRadius, cornerRadius);
        }

        /// <inheritdoc/>
        public virtual RectD GetBounds()
        {
            return GetSnapshot().Bounds;
        }

        /// <inheritdoc/>
        public virtual void AddEllipse(RectD rect)
        {
            ResetPath();
            pathBuilder.AddOval(rect);
        }

        /// <inheritdoc/>
        public virtual void AddLines(ReadOnlySpan<PointD> points)
        {
            if (points.Length < 2)
                return;
            ResetPath();

            pathBuilder.MoveTo(points[0]);

            for (int i = 1; i < points.Length; i++)
                pathBuilder.LineTo(points[i]);
        }

        /// <inheritdoc/>
        public virtual void AddLine(PointD pt1, PointD pt2)
        {
            ResetPath();
            pathBuilder.MoveTo(pt1);
            pathBuilder.LineTo(pt2);
        }

        /// <inheritdoc/>
        public virtual void StartFigure(PointD point)
        {
            ResetPath();
            pathBuilder.MoveTo(point);
        }

        /// <inheritdoc/>
        public virtual void AddBezier(PointD startPoint, PointD controlPoint1, PointD controlPoint2, PointD endPoint)
        {
            ResetPath();
            pathBuilder.MoveTo(startPoint);
            pathBuilder.CubicTo(controlPoint1, controlPoint2, endPoint);
        }

        /// <inheritdoc/>
        public virtual void AddBezierTo(PointD controlPoint1, PointD controlPoint2, PointD endPoint)
        {
            ResetPath();
            pathBuilder.CubicTo(controlPoint1, controlPoint2, endPoint);
        }

        /// <inheritdoc/>
        public virtual void AddArc(PointD center, float radius, float startAngle, float sweepAngle)
        {
            ResetPath();
            var rect = RectD.GetCircleBoundingBox(center, radius);
            pathBuilder.AddArc(rect, startAngle, sweepAngle);
        }

        /// <inheritdoc/>
        public virtual void AddArc(RectD rect, float startAngle, float sweepAngle)
        {
            ResetPath();
            pathBuilder.AddArc(rect, startAngle, sweepAngle);
        }

        /// <summary>
        /// Gets a snapshot of the current path as an <see cref="SKPath"/> object.
        /// </summary>
        /// <returns></returns>
        public virtual SKPath GetSnapshot()
        {
            if (path == null)
            {
                path = pathBuilder.Snapshot();
            }

            return path;
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            ResetPath();
        }

        private void ResetPath()
        {
            SafeDispose(ref path);
        }
    }
}
