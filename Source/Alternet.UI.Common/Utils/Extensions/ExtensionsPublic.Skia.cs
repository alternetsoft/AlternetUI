using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

using Alternet.Drawing;

using SkiaSharp;

namespace Alternet.UI.Extensions
{
    public static partial class ExtensionsPublic
    {
        /// <summary>
        /// Converts array of <see cref="PointD"/> to array of <see cref="SKPoint"/>.
        /// </summary>
        /// <param name="points">Array of points.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SKPoint[] ToSkia(this PointD[] points)
        {
            return PointD.ToSkiaArray(points);
        }

        /// <summary>
        /// Converts array of <see cref="PointD"/> to array of <see cref="SKPointI"/>
        /// using the specified <paramref name="scaleFactor"/>.
        /// </summary>
        /// <param name="points">Array of points.</param>
        /// <param name="scaleFactor">Scale factor. Optional. If not specified,
        /// the default scale factor is used.</param>
        /// <returns></returns>
        public static SKPointI[] PixelFromDipI(this PointD[] points, Coord? scaleFactor = null)
        {
            var length = points.Length;
            SKPointI[] result = new SKPointI[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = GraphicsFactory.PixelFromDip(points[i], scaleFactor);
            }

            return result;
        }

        /// <summary>
        /// Converts array of <see cref="PointD"/> to array of <see cref="SKPoint"/>
        /// using the specified <paramref name="scaleFactor"/>.
        /// </summary>
        /// <param name="points">Array of points.</param>
        /// <param name="scaleFactor">Scale factor. Optional. If not specified,
        /// the default scale factor is used.</param>
        /// <returns></returns>
        public static SKPoint[] PixelFromDipD(this PointD[] points, Coord? scaleFactor = null)
        {
            scaleFactor = GraphicsFactory.ScaleFactorOrDefault(scaleFactor);
            if (scaleFactor == CoordD.One)
                return PointD.ToSkiaArray(points);

            var length = points.Length;
            SKPoint[] result = new SKPoint[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = GraphicsFactory.PixelFromDip(points[i], scaleFactor).SkiaPoint;
            }

            return result;
        }

        /// <summary>
        /// Converts <see cref="LineJoin"/> to <see cref="SKStrokeJoin"/>.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns></returns>
        public static SKStrokeJoin ToSkia(this LineJoin value)
        {
            switch (value)
            {
                case LineJoin.Miter:
                default:
                    return SKStrokeJoin.Miter;
                case LineJoin.Bevel:
                    return SKStrokeJoin.Bevel;
                case LineJoin.Round:
                    return SKStrokeJoin.Round;
            }
        }

        /// <summary>
        /// Converts <see cref="LineCap"/> to <see cref="SKStrokeCap"/>.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns></returns>
        public static SKStrokeCap ToSkia(this LineCap value)
        {
            switch (value)
            {
                case LineCap.Flat:
                default:
                    return SKStrokeCap.Butt;
                case LineCap.Square:
                    return SKStrokeCap.Square;
                case LineCap.Round:
                    return SKStrokeCap.Round;
            }
        }

        /// <summary>
        /// Converts <see cref="BitmapType"/> to <see cref="SKEncodedImageFormat"/>.
        /// </summary>
        /// <param name="type">Bitmap type</param>
        /// <returns>
        /// <see cref="SKEncodedImageFormat"/> if <see cref="BitmapType"/> can be converted,
        /// <c>null</c> otherwise.
        /// </returns>
        public static SKEncodedImageFormat? ToSKEncodedImageFormat(this BitmapType type)
        {
            switch (type)
            {
                default:
                    return null;
                case BitmapType.Bmp:
                    return SKEncodedImageFormat.Bmp;
                case BitmapType.Ico:
                    return SKEncodedImageFormat.Ico;
                case BitmapType.Gif:
                    return SKEncodedImageFormat.Gif;
                case BitmapType.Png:
                    return SKEncodedImageFormat.Png;
                case BitmapType.Jpeg:
                    return SKEncodedImageFormat.Jpeg;
                case BitmapType.Icon:
                    return SKEncodedImageFormat.Ico;
            }
        }
    }
}
