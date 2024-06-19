using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public class PlessGraphics : Graphics
    {
        public static readonly Graphics Default = new PlessGraphics();

        public override bool IsOk
        {
            get;
        }

        public override TransformMatrix Transform
        {
            get => TransformMatrix.Default;

            set
            {
            }
        }

        public override Region? Clip
        {
            get;
            set;
        }

        public override InterpolationMode InterpolationMode { get; set; }

        public override object NativeObject
        {
            get => AssemblyUtils.Default;
        }

        public override bool Blit(
            PointD destPt,
            SizeD sz,
            Graphics source,
            PointD srcPt,
            RasterOperationMode rop = RasterOperationMode.Copy,
            bool useMask = false,
            PointD? srcPtMask = null,
            GraphicsUnit unit = GraphicsUnit.Dip)
        {
            return false;
        }

        public override void Circle(Pen pen, Brush brush, PointD center, Coord radius)
        {
        }

        public override void DestroyClippingRegion()
        {
        }

        public override void DrawArc(Pen pen, PointD center, Coord radius, Coord startAngle, Coord sweepAngle)
        {
        }

        public override void DrawBezier(Pen pen, PointD startPoint, PointD controlPoint1, PointD controlPoint2, PointD endPoint)
        {
        }

        public override void DrawBeziers(Pen pen, PointD[] points)
        {
        }

        public override void DrawCircle(Pen pen, PointD center, Coord radius)
        {
        }

        public override void DrawEllipse(Pen pen, RectD bounds)
        {
        }

        public override void DrawImage(Image image, PointD origin)
        {
        }

        public override void DrawImage(Image image, RectD destinationRect)
        {
        }

        public override void DrawImage(Image image, RectD destinationRect, RectD sourceRect)
        {
        }

        public override void DrawImage(Image image, RectD destinationRect, RectD sourceRect, GraphicsUnit unit)
        {
        }

        public override RectD DrawLabel(string text, Font font, Color foreColor, Color backColor, Image? image, RectD rect, GenericAlignment alignment = GenericAlignment.Left, int indexAccel = -1)
        {
            return default;
        }

        public override void DrawLine(Pen pen, PointD a, PointD b)
        {
        }

        public override void DrawLines(Pen pen, PointD[] points)
        {
        }

        public override void DrawPath(Pen pen, GraphicsPath path)
        {
        }

        public override void DrawPie(Pen pen, PointD center, Coord radius, Coord startAngle, Coord sweepAngle)
        {
        }

        public override void DrawPoint(Pen pen, Coord x, Coord y)
        {
        }

        public override void DrawPolygon(Pen pen, PointD[] points)
        {
        }

        public override void DrawRectangle(Pen pen, RectD rectangle)
        {
        }

        public override void DrawRectangles(Pen pen, RectD[] rects)
        {
        }

        public override void DrawRotatedText(
            string text,
            PointD location,
            Font font,
            Color foreColor,
            Color backColor,
            Coord angle,
            GraphicsUnit unit = GraphicsUnit.Dip)
        {
        }

        public override void DrawRoundedRectangle(Pen pen, RectD rect, Coord cornerRadius)
        {
        }

        public override void DrawText(string text, Font font, Brush brush, PointD origin)
        {
        }

        public override void DrawText(string text, Font font, Brush brush, RectD bounds)
        {
        }

        public override void DrawText(string text, PointD location, Font font, Color foreColor, Color backColor)
        {
        }

        public override void Ellipse(Pen pen, Brush brush, RectD rectangle)
        {
        }

        public override void FillCircle(Brush brush, PointD center, Coord radius)
        {
        }

        public override void FillEllipse(Brush brush, RectD bounds)
        {
        }

        public override void FillPath(Brush brush, GraphicsPath path)
        {
        }

        public override void FillPie(Brush brush, PointD center, Coord radius, Coord startAngle, Coord sweepAngle)
        {
        }

        public override void FillPolygon(Brush brush, PointD[] points, FillMode fillMode = FillMode.Alternate)
        {
        }

        public override void FillRectangle(Brush brush, RectD rectangle)
        {
        }

        public override void FillRectangle(Brush brush, RectD rectangle, GraphicsUnit unit)
        {
            throw new NotImplementedException();
        }

        public override void FillRectangles(Brush brush, RectD[] rects)
        {
        }

        public override void FillRoundedRectangle(Brush brush, RectD rect, Coord cornerRadius)
        {
        }

        public override void FloodFill(Brush brush, PointD point)
        {
        }

        public override RectD GetClippingBox()
        {
            return default;
        }

        public override SizeI GetDPI()
        {
            return 96;
        }

        public override Color GetPixel(PointD point)
        {
            return Color.Black;
        }

        public override SizeD GetTextExtent(string text, Font font, IControl? control)
        {
            return default;
        }

        public override SizeD GetTextExtent(string text, Font font)
        {
            return default;
        }

        public override void Path(Pen pen, Brush brush, GraphicsPath path)
        {
        }

        public override void Pie(Pen pen, Brush brush, PointD center, Coord radius, Coord startAngle, Coord sweepAngle)
        {
        }

        public override void Polygon(Pen pen, Brush brush, PointD[] points, FillMode fillMode)
        {
        }

        public override void Rectangle(Pen pen, Brush brush, RectD rectangle)
        {
        }

        public override void RoundedRectangle(Pen pen, Brush brush, RectD rectangle, Coord cornerRadius)
        {
        }

        public override void SetClippingRegion(RectD rect)
        {
        }

        public override void SetPixel(PointD point, Pen pen)
        {
        }

        public override void SetPixel(Coord x, Coord y, Pen pen)
        {
        }

        public override void SetPixel(Coord x, Coord y, Color color)
        {
        }

        public override bool StretchBlit(
            PointD dstPt,
            SizeD dstSize,
            Graphics source,
            PointD srcPt,
            SizeD srcSize,
            RasterOperationMode rop = RasterOperationMode.Copy,
            bool useMask = false,
            PointD? srcPtMask = null,
            GraphicsUnit unit = GraphicsUnit.Dip)
        {
            return default;
        }
    }
}
