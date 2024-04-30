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

        public abstract object CreateGraphicsPath(object nativeGraphics);

        public abstract FillMode GraphicsPathGetFillMode(object graphicsPath);

        public abstract void GraphicsPathSetFillMode(object graphicsPath, FillMode value);

        public abstract void GraphicsPathAddLines(object graphicsPath, PointD[] points);

        public abstract void GraphicsPathAddLine(object graphicsPath, PointD pt1, PointD pt2);

        public abstract void GraphicsPathAddLineTo(object graphicsPath, PointD pt);

        public abstract void GraphicsPathAddEllipse(object graphicsPath, RectD rect);

        public abstract void GraphicsPathAddBezier(
            object graphicsPath,
            PointD startPoint,
            PointD controlPoint1,
            PointD controlPoint2,
            PointD endPoint);

        public abstract void GraphicsPathAddBezierTo(
            object graphicsPath,
            PointD controlPoint1,
            PointD controlPoint2,
            PointD endPoint);

        public abstract void GraphicsPathAddArc(
            object graphicsPath,
            PointD center,
            double radius,
            double startAngle,
            double sweepAngle);

        public abstract void GraphicsPathAddRectangle(object graphicsPath, RectD rect);

        public abstract void GraphicsPathAddRoundedRectangle(
            object graphicsPath,
            RectD rect,
            double cornerRadius);

        public abstract RectD GraphicsPathGetBounds(object graphicsPath);

        public abstract void GraphicsPathStartFigure(object graphicsPath, PointD point);

        public abstract void GraphicsPathCloseFigure(object graphicsPath);
    }
}
