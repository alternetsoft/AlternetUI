using Alternet.Drawing;
using System;

namespace NativeApi.Api
{
    public class GraphicsPath
    {
        public void AddLines(Point[] points) => throw new Exception();

        public void AddLine(Point pt1, Point pt2) => throw new Exception();

        public void AddLineTo(Point pt) => throw new Exception();

        public void AddEllipse(Rect rect) => throw new Exception();

        public void AddBezier(Point startPoint, Point controlPoint1, Point controlPoint2, Point endPoint) => throw new Exception();

        public void AddBezierTo(Point controlPoint1, Point controlPoint2, Point endPoint) => throw new Exception();

        public void AddArc(Point center, double radius, double startAngle, double sweepAngle) => throw new Exception();

        public void AddRectangle(Rect rect) => throw new Exception();

        public void AddRoundedRectangle(Rect rect, double cornerRadius) => throw new Exception();

        public Rect GetBounds() => throw new Exception();

        public void StartFigure() => throw new Exception();

        public void CloseFigure() => throw new Exception();
    }
}