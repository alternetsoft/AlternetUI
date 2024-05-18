using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public partial class NativeDrawing
    {
        public abstract object CreateGraphicsPath();

        public abstract object CreateGraphicsPath(Graphics graphics);

        public abstract FillMode GraphicsPathGetFillMode(GraphicsPath graphicsPath);

        public abstract void GraphicsPathSetFillMode(GraphicsPath graphicsPath, FillMode value);

        public abstract void GraphicsPathAddLines(GraphicsPath graphicsPath, PointD[] points);

        public abstract void GraphicsPathAddLine(GraphicsPath graphicsPath, PointD pt1, PointD pt2);

        public abstract void GraphicsPathAddLineTo(GraphicsPath graphicsPath, PointD pt);

        public abstract void GraphicsPathAddEllipse(GraphicsPath graphicsPath, RectD rect);

        public abstract void GraphicsPathAddBezier(
            GraphicsPath graphicsPath,
            PointD startPoint,
            PointD controlPoint1,
            PointD controlPoint2,
            PointD endPoint);

        public abstract void GraphicsPathAddBezierTo(
            GraphicsPath graphicsPath,
            PointD controlPoint1,
            PointD controlPoint2,
            PointD endPoint);

        public abstract void GraphicsPathAddArc(
            GraphicsPath graphicsPath,
            PointD center,
            double radius,
            double startAngle,
            double sweepAngle);

        public abstract void GraphicsPathAddRectangle(GraphicsPath graphicsPath, RectD rect);

        public abstract void GraphicsPathAddRoundedRectangle(
            GraphicsPath graphicsPath,
            RectD rect,
            double cornerRadius);

        public abstract RectD GraphicsPathGetBounds(GraphicsPath graphicsPath);

        public abstract void GraphicsPathStartFigure(GraphicsPath graphicsPath, PointD point);

        public abstract void GraphicsPathCloseFigure(GraphicsPath graphicsPath);
    }
}
