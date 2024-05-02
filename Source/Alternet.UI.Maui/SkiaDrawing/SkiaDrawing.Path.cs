using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    public partial class SkiaDrawing
    {
        /// <inheritdoc/>
        public override object CreateGraphicsPath(object nativeGraphics)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateGraphicsPath()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override FillMode GraphicsPathGetFillMode(object graphicsPath)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void GraphicsPathSetFillMode(object graphicsPath, FillMode value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void GraphicsPathAddLines(object graphicsPath, PointD[] points)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void GraphicsPathAddLine(object graphicsPath, PointD pt1, PointD pt2)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void GraphicsPathAddLineTo(object graphicsPath, PointD pt)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void GraphicsPathAddEllipse(object graphicsPath, RectD rect)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void GraphicsPathAddBezier(
            object graphicsPath,
            PointD startPoint,
            PointD controlPoint1,
            PointD controlPoint2,
            PointD endPoint)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void GraphicsPathAddBezierTo(
            object graphicsPath,
            PointD controlPoint1,
            PointD controlPoint2,
            PointD endPoint)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void GraphicsPathAddArc(
            object graphicsPath,
            PointD center,
            double radius,
            double startAngle,
            double sweepAngle)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void GraphicsPathAddRectangle(object graphicsPath, RectD rect)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void GraphicsPathAddRoundedRectangle(
            object graphicsPath,
            RectD rect,
            double cornerRadius)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override RectD GraphicsPathGetBounds(object graphicsPath)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void GraphicsPathStartFigure(object graphicsPath, PointD point)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void GraphicsPathCloseFigure(object graphicsPath)
        {
            throw new NotImplementedException();
        }
    }
}
