using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements dummy drawing context which does no drawing.
    /// </summary>
    public class PlessGraphics : Graphics
    {
        /// <summary>
        /// Gets default dummy drawing context.
        /// </summary>
        public static readonly Graphics Default = new PlessGraphics();

        /// <inheritdoc/>
        public override bool IsOk
        {
            get;
        }

        /// <inheritdoc/>
        public override bool HasClip => Clip is not null;

        /// <inheritdoc/>
        public override Region? Clip
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public override InterpolationMode InterpolationMode { get; set; }

        /// <inheritdoc/>
        public override object NativeObject
        {
            get => AssemblyUtils.Default;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public override void Circle(Pen pen, Brush brush, PointD center, Coord radius)
        {
        }

        /// <inheritdoc/>
        public override void DestroyClippingRegion()
        {
        }

        /// <inheritdoc/>
        public override void DrawArc(Pen pen, PointD center, Coord radius, Coord startAngle, Coord sweepAngle)
        {
        }

        /// <inheritdoc/>
        public override void DrawBezier(Pen pen, PointD startPoint, PointD controlPoint1, PointD controlPoint2, PointD endPoint)
        {
        }

        /// <inheritdoc/>
        public override void DrawBeziers(Pen pen, PointD[] points)
        {
        }

        /// <inheritdoc/>
        public override void DrawCircle(Pen pen, PointD center, Coord radius)
        {
        }

        /// <inheritdoc/>
        public override void DrawEllipse(Pen pen, RectD bounds)
        {
        }

        /// <inheritdoc/>
        public override void DrawImage(Image image, PointD origin)
        {
        }

        /// <inheritdoc/>
        public override void DrawImage(Image image, RectD destinationRect)
        {
        }

        /// <inheritdoc/>
        public override void DrawImage(Image image, RectD destinationRect, RectD sourceRect)
        {
        }

        /// <inheritdoc/>
        public override void DrawImage(Image image, RectD destinationRect, RectD sourceRect, GraphicsUnit unit)
        {
        }

        /// <inheritdoc/>
        public override void DrawLine(Pen pen, PointD a, PointD b)
        {
        }

        /// <inheritdoc/>
        public override void DrawLines(Pen pen, PointD[] points)
        {
        }

        /// <inheritdoc/>
        public override void DrawPath(Pen pen, GraphicsPath path)
        {
        }

        /// <inheritdoc/>
        public override void DrawPie(Pen pen, PointD center, Coord radius, Coord startAngle, Coord sweepAngle)
        {
        }

        /// <inheritdoc/>
        public override void DrawPoint(Pen pen, Coord x, Coord y)
        {
        }

        /// <inheritdoc/>
        public override void DrawPolygon(Pen pen, PointD[] points)
        {
        }

        /// <inheritdoc/>
        public override void DrawRectangle(Pen pen, RectD rectangle)
        {
        }

        /// <inheritdoc/>
        public override void DrawRectangles(Pen pen, RectD[] rects)
        {
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public override void DrawRoundedRectangle(Pen pen, RectD rect, Coord cornerRadius)
        {
        }

        /// <inheritdoc/>
        public override void DrawText(string text, Font font, Brush brush, PointD origin)
        {
        }

        /// <inheritdoc/>
        public override void DrawText(string text, Font font, Brush brush, RectD bounds)
        {
        }

        /// <inheritdoc/>
        public override void DrawText(string text, PointD location, Font font, Color foreColor, Color backColor)
        {
        }

        /// <inheritdoc/>
        public override void Ellipse(Pen pen, Brush brush, RectD rectangle)
        {
        }

        /// <inheritdoc/>
        public override void FillCircle(Brush brush, PointD center, Coord radius)
        {
        }

        /// <inheritdoc/>
        public override void FillEllipse(Brush brush, RectD bounds)
        {
        }

        /// <inheritdoc/>
        public override void FillPath(Brush brush, GraphicsPath path)
        {
        }

        /// <inheritdoc/>
        public override void FillPie(Brush brush, PointD center, Coord radius, Coord startAngle, Coord sweepAngle)
        {
        }

        /// <inheritdoc/>
        public override void FillPolygon(Brush brush, PointD[] points, FillMode fillMode = FillMode.Alternate)
        {
        }

        /// <inheritdoc/>
        public override void FillRectangle(Brush brush, RectD rectangle)
        {
        }

        /// <inheritdoc/>
        public override void FillRectangle(Brush brush, RectD rectangle, GraphicsUnit unit)
        {
        }

        /// <inheritdoc/>
        public override void FillRectangles(Brush brush, RectD[] rects)
        {
        }

        /// <inheritdoc/>
        public override void FillRoundedRectangle(Brush brush, RectD rect, Coord cornerRadius)
        {
        }

        /// <inheritdoc/>
        public override void FloodFill(Brush brush, PointD point)
        {
        }

        /// <inheritdoc/>
        public override RectD GetClippingBox()
        {
            return default;
        }

        /// <inheritdoc/>
        public override SizeI GetDPI()
        {
            return 96;
        }

        /// <inheritdoc/>
        public override SizeD GetTextExtent(string text, Font font)
        {
            return default;
        }

        /// <inheritdoc/>
        public override void Path(Pen pen, Brush brush, GraphicsPath path)
        {
        }

        /// <inheritdoc/>
        public override void Pie(Pen pen, Brush brush, PointD center, Coord radius, Coord startAngle, Coord sweepAngle)
        {
        }

        /// <inheritdoc/>
        public override void Polygon(Pen pen, Brush brush, PointD[] points, FillMode fillMode)
        {
        }

        /// <inheritdoc/>
        public override void Rectangle(Pen pen, Brush brush, RectD rectangle)
        {
        }

        /// <inheritdoc/>
        public override void RoundedRectangle(Pen pen, Brush brush, RectD rectangle, Coord cornerRadius)
        {
        }

        /// <inheritdoc/>
        public override void SetClippingRegion(RectD rect)
        {
        }

        /// <inheritdoc/>
        public override void SetPixel(PointD point, Pen pen)
        {
        }

        /// <inheritdoc/>
        public override void SetPixel(Coord x, Coord y, Pen pen)
        {
        }

        /// <inheritdoc/>
        public override void SetPixel(Coord x, Coord y, Color color)
        {
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        protected override void SetHandlerTransform(TransformMatrix matrix)
        {
        }
    }
}
