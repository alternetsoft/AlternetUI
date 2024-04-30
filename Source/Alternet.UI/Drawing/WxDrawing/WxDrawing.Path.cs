using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    internal partial class WxDrawing
    {
        public override object CreateGraphicsPath(object nativeGraphics)
        {
            var result = new UI.Native.GraphicsPath();
            ((UI.Native.GraphicsPath)result).Initialize((UI.Native.DrawingContext)nativeGraphics);
            return result;
        }

        public override object CreateGraphicsPath()
        {
            return new UI.Native.GraphicsPath();
        }

        public override FillMode GraphicsPathGetFillMode(object graphicsPath)
        {
            return (FillMode)((UI.Native.GraphicsPath)graphicsPath).FillMode;
        }

        public override void GraphicsPathSetFillMode(object graphicsPath, FillMode value)
        {
            ((UI.Native.GraphicsPath)graphicsPath).FillMode = (UI.Native.FillMode)value;
        }

        public override void GraphicsPathAddLines(object graphicsPath, PointD[] points)
        {
            ((UI.Native.GraphicsPath)graphicsPath).AddLines(points);
        }

        public override void GraphicsPathAddLine(object graphicsPath, PointD pt1, PointD pt2)
        {
            ((UI.Native.GraphicsPath)graphicsPath).AddLine(pt1, pt2);
        }

        public override void GraphicsPathAddLineTo(object graphicsPath, PointD pt)
        {
            ((UI.Native.GraphicsPath)graphicsPath).AddLineTo(pt);
        }

        public override void GraphicsPathAddEllipse(object graphicsPath, RectD rect)
        {
            ((UI.Native.GraphicsPath)graphicsPath).AddEllipse(rect);
        }

        public override void GraphicsPathAddBezier(
            object graphicsPath,
            PointD startPoint,
            PointD controlPoint1,
            PointD controlPoint2,
            PointD endPoint)
        {
            ((UI.Native.GraphicsPath)graphicsPath).AddBezier(
                startPoint,
                controlPoint1,
                controlPoint2,
                endPoint);
        }

        public override void GraphicsPathAddBezierTo(
            object graphicsPath,
            PointD controlPoint1,
            PointD controlPoint2,
            PointD endPoint)
        {
            ((UI.Native.GraphicsPath)graphicsPath).AddBezierTo(controlPoint1, controlPoint2, endPoint);
        }

        public override void GraphicsPathAddArc(
            object graphicsPath,
            PointD center,
            double radius,
            double startAngle,
            double sweepAngle)
        {
            ((UI.Native.GraphicsPath)graphicsPath).AddArc(center, radius, startAngle, sweepAngle);
        }

        public override void GraphicsPathAddRectangle(object graphicsPath, RectD rect)
        {
            ((UI.Native.GraphicsPath)graphicsPath).AddRectangle(rect);
        }

        public override void GraphicsPathAddRoundedRectangle(
            object graphicsPath,
            RectD rect,
            double cornerRadius)
        {
            ((UI.Native.GraphicsPath)graphicsPath).AddRoundedRectangle(rect, cornerRadius);
        }

        public override RectD GraphicsPathGetBounds(object graphicsPath)
        {
            return ((UI.Native.GraphicsPath)graphicsPath).GetBounds();
        }

        public override void GraphicsPathStartFigure(object graphicsPath, PointD point)
        {
            ((UI.Native.GraphicsPath)graphicsPath).StartFigure(point);
        }

        public override void GraphicsPathCloseFigure(object graphicsPath)
        {
            ((UI.Native.GraphicsPath)graphicsPath).CloseFigure();
        }
    }
}
