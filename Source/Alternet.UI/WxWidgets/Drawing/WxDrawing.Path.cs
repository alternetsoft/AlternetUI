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
        public override object CreateGraphicsPath(Graphics graphics)
        {
            var result = new UI.Native.GraphicsPath();
            ((UI.Native.GraphicsPath)result).Initialize((UI.Native.DrawingContext)graphics.NativeObject);
            return result;
        }

        public override object CreateGraphicsPath()
        {
            return new UI.Native.GraphicsPath();
        }

        public override FillMode GraphicsPathGetFillMode(GraphicsPath graphicsPath)
        {
            return (FillMode)((UI.Native.GraphicsPath)graphicsPath.Handler).FillMode;
        }

        public override void GraphicsPathSetFillMode(GraphicsPath graphicsPath, FillMode value)
        {
            ((UI.Native.GraphicsPath)graphicsPath.Handler).FillMode = (UI.Native.FillMode)value;
        }

        public override void GraphicsPathAddLines(GraphicsPath graphicsPath, PointD[] points)
        {
            ((UI.Native.GraphicsPath)graphicsPath.Handler).AddLines(points);
        }

        public override void GraphicsPathAddLine(GraphicsPath graphicsPath, PointD pt1, PointD pt2)
        {
            ((UI.Native.GraphicsPath)graphicsPath.Handler).AddLine(pt1, pt2);
        }

        public override void GraphicsPathAddLineTo(GraphicsPath graphicsPath, PointD pt)
        {
            ((UI.Native.GraphicsPath)graphicsPath.Handler).AddLineTo(pt);
        }

        public override void GraphicsPathAddEllipse(GraphicsPath graphicsPath, RectD rect)
        {
            ((UI.Native.GraphicsPath)graphicsPath.Handler).AddEllipse(rect);
        }

        public override void GraphicsPathAddBezier(
            GraphicsPath graphicsPath,
            PointD startPoint,
            PointD controlPoint1,
            PointD controlPoint2,
            PointD endPoint)
        {
            ((UI.Native.GraphicsPath)graphicsPath.Handler).AddBezier(
                startPoint,
                controlPoint1,
                controlPoint2,
                endPoint);
        }

        public override void GraphicsPathAddBezierTo(
            GraphicsPath graphicsPath,
            PointD controlPoint1,
            PointD controlPoint2,
            PointD endPoint)
        {
            ((UI.Native.GraphicsPath)graphicsPath.Handler).AddBezierTo(controlPoint1, controlPoint2, endPoint);
        }

        public override void GraphicsPathAddArc(
            GraphicsPath graphicsPath,
            PointD center,
            double radius,
            double startAngle,
            double sweepAngle)
        {
            ((UI.Native.GraphicsPath)graphicsPath.Handler).AddArc(center, radius, startAngle, sweepAngle);
        }

        public override void GraphicsPathAddRectangle(GraphicsPath graphicsPath, RectD rect)
        {
            ((UI.Native.GraphicsPath)graphicsPath.Handler).AddRectangle(rect);
        }

        public override void GraphicsPathAddRoundedRectangle(
            GraphicsPath graphicsPath,
            RectD rect,
            double cornerRadius)
        {
            ((UI.Native.GraphicsPath)graphicsPath.Handler).AddRoundedRectangle(rect, cornerRadius);
        }

        public override RectD GraphicsPathGetBounds(GraphicsPath graphicsPath)
        {
            return ((UI.Native.GraphicsPath)graphicsPath.Handler).GetBounds();
        }

        public override void GraphicsPathStartFigure(GraphicsPath graphicsPath, PointD point)
        {
            ((UI.Native.GraphicsPath)graphicsPath.Handler).StartFigure(point);
        }

        public override void GraphicsPathCloseFigure(GraphicsPath graphicsPath)
        {
            ((UI.Native.GraphicsPath)graphicsPath.Handler).CloseFigure();
        }
    }
}
