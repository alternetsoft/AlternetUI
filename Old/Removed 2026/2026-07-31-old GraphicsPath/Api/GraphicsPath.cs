using Alternet.Drawing;
using System;

namespace NativeApi.Api
{
    public class GraphicsPath
    {
        public void Initialize(DrawingContext dc) => throw new Exception();

        public unsafe void AddLines(PointD* points, int pointCount) => throw new Exception();

        public void AddLine(PointD pt1, PointD pt2) => throw new Exception();

        public void AddLineTo(PointD pt) => throw new Exception();

        public void AddEllipse(RectD rect) => throw new Exception();

        public void AddBezier(PointD startPoint, PointD controlPoint1, PointD controlPoint2, PointD endPoint) => throw new Exception();

        public void AddBezierTo(PointD controlPoint1, PointD controlPoint2, PointD endPoint) => throw new Exception();

        public void AddArc(PointD center, float radius, float startAngle, float sweepAngle) => throw new Exception();

        public void AddRectangle(RectD rect) => throw new Exception();

        public void AddRoundedRectangle(RectD rect, float cornerRadius) => throw new Exception();

        public RectD GetBounds() => throw new Exception();

        public void StartFigure(PointD point) => throw new Exception();

        public void CloseFigure() => throw new Exception();

        public FillMode FillMode { get; set; }
    }
}