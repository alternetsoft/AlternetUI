using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods related to drawing.
    /// </summary>
    public static class DrawingUtils
    {
        /// <summary>
        /// Gets rectangle of the top border edge with the specified width.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        /// <param name="width">Border side width.</param>
        public static RectD GetTopLineRect(RectD rect, double width)
        {
            var point = rect.TopLeft;
            var size = new SizeD(rect.Width, width);
            return new RectD(point, size);
        }

        /// <summary>
        /// Gets rectangle of the horizontal center line of the rectangle.
        /// </summary>
        /// <param name="rect">Rectangle.</param>
        /// <returns></returns>
        public static RectD GetCenterLineHorz(RectD rect)
        {
            var size = new SizeD(rect.Width, 1);
            var point = new PointD(rect.Left, (int)rect.Center.Y);
            return new RectD(point, size);
        }

        /// <summary>
        /// Gets rectangle of the vertical center line of the rectangle.
        /// </summary>
        /// <param name="rect">Rectangle.</param>
        /// <returns></returns>
        public static RectD GetCenterLineVert(RectD rect)
        {
            var size = new SizeD(1, rect.Height);
            var point = new PointD((int)rect.Center.X, rect.Top);
            return new RectD(point, size);
        }

        /// <summary>
        /// Gets rectangle of the bottom border edge with the specified width.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        /// <param name="width">Border side width.</param>
        public static RectD GetBottomLineRect(RectD rect, double width)
        {
            var point = new PointD(rect.Left, rect.Bottom - width);
            var size = new SizeD(rect.Width, width);
            return new RectD(point, size);
        }

        /// <summary>
        /// Draws horizontal line using <see cref="Graphics.FillRectangle"/>.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <param name="brush">Brush to draw line.</param>
        /// <param name="point">Starting point.</param>
        /// <param name="length">Line length.</param>
        /// <param name="width">Line width.</param>
        public static void DrawHorzLine(
            Graphics dc,
            Brush brush,
            PointD point,
            double length,
            double width)
        {
            var rect = new RectD(point, new SizeD(length, width));
            dc.FillRectangle(brush, rect);
        }

        /// <summary>
        /// Draws vertical line using <see cref="Graphics.FillRectangle"/>.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <param name="brush">Brush to draw line.</param>
        /// <param name="point">Starting point.</param>
        /// <param name="length">Line length.</param>
        /// <param name="width">Line width.</param>
        public static void DrawVertLine(
            Graphics dc,
            Brush brush,
            PointD point,
            double length,
            double width)
        {
            var rect = new RectD(point, new SizeD(width, length));
            dc.FillRectangle(brush, rect);
        }

        /// <summary>
        /// Draws inner and outer border with the specified colors.
        /// </summary>
        /// <param name="canvas"><see cref="Graphics"/> where drawing is performed.</param>
        /// <param name="rect"><see cref="RectD"/> where drawing is performed.</param>
        /// <param name="innerColor">Inner border color.</param>
        /// <param name="outerColor">Outer border color.</param>
        /// <returns>
        /// Value of the <paramref name="rect"/> parameter deflated by number of the painted
        /// borders (0, 1 or 2).
        /// </returns>
        /// <remarks>
        /// If border color is <see cref="Color.Empty"/> it is not painted.
        /// </remarks>
        public static RectD DrawDoubleBorder(
            Graphics canvas,
            RectD rect,
            Color innerColor,
            Color outerColor)
        {
            var result = rect;
            var hasOuterBorder = outerColor != Color.Empty;
            var hasInnerBorder = innerColor != Color.Empty;
            if (hasOuterBorder)
            {
                DrawingUtils.FillRectangleBorder(canvas, outerColor, result);
                result.Deflate();
            }

            if (hasInnerBorder)
            {
                DrawingUtils.FillRectangleBorder(canvas, innerColor, result);
                result.Deflate();
            }

            return result;
        }

        /// <summary>
        /// Draws rectangle border using <see cref="Graphics.FillRectangle"/>.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <param name="brush">Brush to draw border.</param>
        /// <param name="rect">Border rectangle.</param>
        /// <param name="borderWidth">Border width.</param>
        public static void FillRectangleBorder(
            Graphics dc,
            Brush brush,
            RectD rect,
            double borderWidth = 1)
        {
            dc.FillRectangle(brush, GetTopLineRect(rect, borderWidth));
            dc.FillRectangle(brush, GetBottomLineRect(rect, borderWidth));
            dc.FillRectangle(brush, GetLeftLineRect(rect, borderWidth));
            dc.FillRectangle(brush, GetRightLineRect(rect, borderWidth));
        }

        /// <summary>
        /// Draws rectangle border using <see cref="Graphics.FillRectangle"/>.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <param name="brush">Brush to draw border.</param>
        /// <param name="rect">Border rectangle.</param>
        /// <param name="borderWidth">Border width.</param>
        public static void FillRectangleBorder(
            Graphics dc,
            Brush brush,
            RectD rect,
            Thickness borderWidth)
        {
            if(borderWidth.Top > 0)
                dc.FillRectangle(brush, GetTopLineRect(rect, borderWidth.Top));
            if (borderWidth.Bottom > 0)
                dc.FillRectangle(brush, GetBottomLineRect(rect, borderWidth.Bottom));
            if (borderWidth.Left > 0)
                dc.FillRectangle(brush, GetLeftLineRect(rect, borderWidth.Left));
            if (borderWidth.Right > 0)
                dc.FillRectangle(brush, GetRightLineRect(rect, borderWidth.Right));
        }

        /// <summary>
        /// Draws sliced image with the specified
        /// <see cref="NinePatchImagePaintParams"/> parameters. This method can be used,
        /// for example, for drawing complex button bakgrounds using predefined templates.
        /// </summary>
        /// <param name="canvas"><see cref="Graphics"/> where to draw.</param>
        /// <param name="e">Draw parameters.</param>
        /// <remarks>
        /// Source image is sliced into 9 pieces. All parts of the image except corners
        /// (top-left, top-right, bottom-right, bottom-left parts) are used
        /// by <see cref="TextureBrush"/> to fill larger destination rectangle.
        /// </remarks>
        /// <remarks>
        /// Issue with details is here:
        /// <see href="https://github.com/alternetsoft/AlternetUI/issues/115"/>.
        /// </remarks>
        public static void DrawSlicedImage(Graphics canvas, NinePatchImagePaintParams e)
        {
            var src = e.SourceRect;
            var dst = e.DestRect;
            var patchSrc = e.PatchRect;

            var offsetX = patchSrc.X - src.X;
            var offsetY = patchSrc.Y - src.Y;

            RectI patchDst = patchSrc;

            NineRects srcNine = new(src, patchSrc);

            patchDst.X = dst.X + offsetX;
            patchDst.Y = dst.Y + offsetY;
            patchDst.Width = dst.Width - (src.Width - patchSrc.Width);
            patchDst.Height = dst.Height - (src.Height - patchSrc.Height);

            NineRects dstNine = new(dst, patchDst);

            CopyRect(srcNine.Center, dstNine.Center);
            CopyRect(srcNine.TopCenter, dstNine.TopCenter);
            CopyRect(srcNine.BottomCenter, dstNine.BottomCenter);
            CopyRect(srcNine.CenterLeft, dstNine.CenterLeft);
            CopyRect(srcNine.CenterRight, dstNine.CenterRight);

            canvas.DrawImageI(e.Image, dstNine.TopLeft, srcNine.TopLeft);
            canvas.DrawImageI(e.Image, dstNine.TopRight, srcNine.TopRight);
            canvas.DrawImageI(e.Image, dstNine.BottomLeft, srcNine.BottomLeft);
            canvas.DrawImageI(e.Image, dstNine.BottomRight, srcNine.BottomRight);

            void CopyRect(RectI srcRect, RectI dstRect)
            {
                if (e.Tile)
                {
                    var subImage = e.Image.GetSubBitmap(srcRect);
                    var brush = subImage.AsBrush;
                    canvas.FillRectangleI(brush, dstRect);
                }
                else
                {
                    canvas.DrawImageI(e.Image, dstRect, srcRect);
                }
            }
        }

        /// <summary>
        /// Draws rectangles border using <see cref="Graphics.FillRectangle"/>.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <param name="brush">Brush to draw border.</param>
        /// <param name="rects">Border rectangles.</param>
        /// <param name="borders">Border width.</param>
        public static void FillRectanglesBorder(
            Graphics dc,
            Brush brush,
            RectD[] rects,
            Thickness[]? borders = null)
        {
            for (int i = 0; i < rects.Length; i++)
            {
                FillRectangleBorder(dc, brush, rects[i], borders?[i] ?? 1);
            }
        }

        /// <summary>
        /// Gets rectangle of the left border edge with the specified width.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        /// <param name="width">Border side width.</param>
        public static RectD GetLeftLineRect(RectD rect, double width)
        {
            var point = rect.TopLeft;
            var size = new SizeD(width, rect.Height);
            return new RectD(point, size);
        }

        /// <summary>
        /// Gets rectangle of the right border edge with the specified width.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        /// <param name="width">Border side width.</param>
        public static RectD GetRightLineRect(RectD rect, double width)
        {
            var point = new PointD(rect.Right - width, rect.Top);
            var size = new SizeD(width, rect.Height);
            return new RectD(point, size);
        }

        /// <summary>
        /// Defines parameters for <see cref="DrawSlicedImage(Graphics, NinePatchImagePaintParams)"/>.
        /// </summary>
        public class NinePatchImagePaintParams
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="NinePatchImagePaintParams"/> class.
            /// </summary>
            /// <param name="image">Image to draw.</param>
            public NinePatchImagePaintParams(Image image)
            {
                Image = image;
            }

            /// <summary>
            /// Gets or sets image that will be painted.
            /// </summary>
            public Image Image { get; set; }

            /// <summary>
            /// Gets or sets rectangle inside the image that will be used for painting.
            /// </summary>
            public RectI SourceRect { get; set; }

            /// <summary>
            /// Gets or sets destination rectangle. It supposed to be larger than
            /// <see cref="SourceRect"/>. In this case all parts except top-left, top-right,
            /// bottom-right, bottom-left parts will be filled with <see cref="TextureBrush"/>
            /// constructed with correspoding parts of the image.
            /// </summary>
            public RectI DestRect { get; set; }

            /// <summary>
            /// Rectangle that slices image into 9 parts.
            /// </summary>
            public RectI PatchRect { get; set; }

            internal bool Tile { get; set; } = true;
        }
    }
}
