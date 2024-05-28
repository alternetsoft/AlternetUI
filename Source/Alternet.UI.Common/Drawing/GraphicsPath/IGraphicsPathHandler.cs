using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    public interface IGraphicsPathHandler : IDisposable
    {
        FillMode FillMode { get; set; }

        void AddLines(PointD[] points);

        void AddLine(PointD pt1, PointD pt2);

        void AddLineTo(PointD pt);

        void AddEllipse(RectD rect);

        void AddBezier(
            PointD startPoint,
            PointD controlPoint1,
            PointD controlPoint2,
            PointD endPoint);

        void AddBezierTo(
            PointD controlPoint1,
            PointD controlPoint2,
            PointD endPoint);

        void AddArc(
            PointD center,
            double radius,
            double startAngle,
            double sweepAngle);

        void AddRectangle(RectD rect);

        void AddRoundedRectangle(
            RectD rect,
            double cornerRadius);

        RectD GetBounds();

        void StartFigure(PointD point);

        void CloseFigure();
    }
}
