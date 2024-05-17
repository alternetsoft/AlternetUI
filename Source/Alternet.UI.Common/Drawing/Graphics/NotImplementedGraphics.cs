using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public class NotImplementedGraphics : Graphics
    {
        public override bool IsOk
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override TransformMatrix Transform
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override Region? Clip
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override InterpolationMode InterpolationMode
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override object NativeObject
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool BlitI(PointI destPt, SizeI sz, Graphics source, PointI srcPt, RasterOperationMode rop = RasterOperationMode.Copy, bool useMask = false, PointI? srcPtMask = null)
        {
            throw new NotImplementedException();
        }

        public override void Circle(Pen pen, Brush brush, PointD center, double radius)
        {
            throw new NotImplementedException();
        }

        public override void DestroyClippingRegion()
        {
            throw new NotImplementedException();
        }

        public override void DrawArc(Pen pen, PointD center, double radius, double startAngle, double sweepAngle)
        {
            throw new NotImplementedException();
        }

        public override void DrawBezier(Pen pen, PointD startPoint, PointD controlPoint1, PointD controlPoint2, PointD endPoint)
        {
            throw new NotImplementedException();
        }

        public override void DrawBeziers(Pen pen, PointD[] points)
        {
            throw new NotImplementedException();
        }

        public override void DrawCircle(Pen pen, PointD center, double radius)
        {
            throw new NotImplementedException();
        }

        public override void DrawEllipse(Pen pen, RectD bounds)
        {
            throw new NotImplementedException();
        }

        public override void DrawImage(Image image, PointD origin, bool useMask = false)
        {
            throw new NotImplementedException();
        }

        public override void DrawImage(Image image, RectD destinationRect, bool useMask = false)
        {
            throw new NotImplementedException();
        }

        public override void DrawImage(Image image, RectD destinationRect, RectD sourceRect)
        {
            throw new NotImplementedException();
        }

        public override void DrawImage(Image image, RectD destinationRect, RectD sourceRect, GraphicsUnit unit)
        {
            throw new NotImplementedException();
        }

        public override void DrawImageI(Image image, RectI destinationRect, RectI sourceRect)
        {
            throw new NotImplementedException();
        }

        public override RectD DrawLabel(string text, Font font, Color foreColor, Color backColor, Image? image, RectD rect, GenericAlignment alignment = GenericAlignment.Left, int indexAccel = -1)
        {
            throw new NotImplementedException();
        }

        public override void DrawLine(Pen pen, PointD a, PointD b)
        {
            throw new NotImplementedException();
        }

        public override void DrawLines(Pen pen, PointD[] points)
        {
            throw new NotImplementedException();
        }

        public override void DrawPath(Pen pen, GraphicsPath path)
        {
            throw new NotImplementedException();
        }

        public override void DrawPie(Pen pen, PointD center, double radius, double startAngle, double sweepAngle)
        {
            throw new NotImplementedException();
        }

        public override void DrawPoint(Pen pen, double x, double y)
        {
            throw new NotImplementedException();
        }

        public override void DrawPolygon(Pen pen, PointD[] points)
        {
            throw new NotImplementedException();
        }

        public override void DrawRectangle(Pen pen, RectD rectangle)
        {
            throw new NotImplementedException();
        }

        public override void DrawRectangles(Pen pen, RectD[] rects)
        {
            throw new NotImplementedException();
        }

        public override void DrawRotatedTextI(string text, PointI location, Font font, Color foreColor, Color backColor, double angle)
        {
            throw new NotImplementedException();
        }

        public override void DrawRoundedRectangle(Pen pen, RectD rect, double cornerRadius)
        {
            throw new NotImplementedException();
        }

        public override void DrawText(string text, Font font, Brush brush, PointD origin, TextFormat format)
        {
            throw new NotImplementedException();
        }

        public override void DrawText(string text, Font font, Brush brush, RectD bounds, TextFormat format)
        {
            throw new NotImplementedException();
        }

        public override void DrawText(string text, PointD location, Font font, Color foreColor, Color backColor)
        {
            throw new NotImplementedException();
        }

        public override void Ellipse(Pen pen, Brush brush, RectD rectangle)
        {
            throw new NotImplementedException();
        }

        public override void FillCircle(Brush brush, PointD center, double radius)
        {
            throw new NotImplementedException();
        }

        public override void FillEllipse(Brush brush, RectD bounds)
        {
            throw new NotImplementedException();
        }

        public override void FillPath(Brush brush, GraphicsPath path)
        {
            throw new NotImplementedException();
        }

        public override void FillPie(Brush brush, PointD center, double radius, double startAngle, double sweepAngle)
        {
            throw new NotImplementedException();
        }

        public override void FillPolygon(Brush brush, PointD[] points, FillMode fillMode = FillMode.Alternate)
        {
            throw new NotImplementedException();
        }

        public override void FillRectangle(Brush brush, RectD rectangle)
        {
            throw new NotImplementedException();
        }

        public override void FillRectangleI(Brush brush, RectI rectangle)
        {
            throw new NotImplementedException();
        }

        public override void FillRectangles(Brush brush, RectD[] rects)
        {
            throw new NotImplementedException();
        }

        public override void FillRoundedRectangle(Brush brush, RectD rect, double cornerRadius)
        {
            throw new NotImplementedException();
        }

        public override void FloodFill(Brush brush, PointD point)
        {
            throw new NotImplementedException();
        }

        public override RectD GetClippingBox()
        {
            throw new NotImplementedException();
        }

        public override SizeD GetDPI()
        {
            throw new NotImplementedException();
        }

        public override Color GetPixel(PointD point)
        {
            throw new NotImplementedException();
        }

        public override SizeD GetTextExtent(string text, Font font, out double descent, out double externalLeading, IControl? control = null)
        {
            throw new NotImplementedException();
        }

        public override SizeD GetTextExtent(string text, Font font, IControl? control)
        {
            throw new NotImplementedException();
        }

        public override SizeD GetTextExtent(string text, Font font)
        {
            throw new NotImplementedException();
        }

        public override SizeD MeasureText(string text, Font font)
        {
            throw new NotImplementedException();
        }

        public override SizeD MeasureText(string text, Font font, double maximumWidth)
        {
            throw new NotImplementedException();
        }

        public override SizeD MeasureText(string text, Font font, double maximumWidth, TextFormat format)
        {
            throw new NotImplementedException();
        }

        public override void Path(Pen pen, Brush brush, GraphicsPath path)
        {
            throw new NotImplementedException();
        }

        public override void Pie(Pen pen, Brush brush, PointD center, double radius, double startAngle, double sweepAngle)
        {
            throw new NotImplementedException();
        }

        public override void Polygon(Pen pen, Brush brush, PointD[] points, FillMode fillMode)
        {
            throw new NotImplementedException();
        }

        public override void Pop()
        {
            throw new NotImplementedException();
        }

        public override void Push()
        {
            throw new NotImplementedException();
        }

        public override void PushTransform(TransformMatrix transform)
        {
            throw new NotImplementedException();
        }

        public override void Rectangle(Pen pen, Brush brush, RectD rectangle)
        {
            throw new NotImplementedException();
        }

        public override void RoundedRectangle(Pen pen, Brush brush, RectD rectangle, double cornerRadius)
        {
            throw new NotImplementedException();
        }

        public override void SetClippingRegion(RectD rect)
        {
            throw new NotImplementedException();
        }

        public override void SetPixel(PointD point, Pen pen)
        {
            throw new NotImplementedException();
        }

        public override void SetPixel(double x, double y, Pen pen)
        {
            throw new NotImplementedException();
        }

        public override void SetPixel(double x, double y, Color color)
        {
            throw new NotImplementedException();
        }

        public override bool StretchBlitI(PointI dstPt, SizeI dstSize, Graphics source, PointI srcPt, SizeI srcSize, RasterOperationMode rop = RasterOperationMode.Copy, bool useMask = false, PointI? srcPtMask = null)
        {
            throw new NotImplementedException();
        }
    }
}
