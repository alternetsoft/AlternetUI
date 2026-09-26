using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

using SkiaSharp;

namespace Alternet.Winforms
{
    /// <summary>
    /// Provides a set of methods for working with SkiaSharp in WinForms applications.
    /// </summary>
    public static class SkiaWinforms
    {
        /// <summary>
        /// Converts a System.Drawing.PointF to an SKPoint.
        /// </summary>
        /// <param name="point">The System.Drawing.PointF to convert.</param>
        /// <returns>The converted SKPoint.</returns>
        public static SKPoint ToSKPoint(this System.Drawing.PointF point)
        {
            return new SKPoint(point.X, point.Y);
        }

        /// <summary>
        /// Converts a System.Drawing.Point to an SKPointI.
        /// </summary>
        /// <param name="point">The System.Drawing.Point to convert.</param>
        /// <returns>The converted SKPointI.</returns>
        public static SKPointI ToSKPoint(this System.Drawing.Point point)
        {
            return new SKPointI(point.X, point.Y);
        }
        
        /// <summary>
        /// Converts an SKPoint to a System.Drawing.PointF.
        /// </summary>
        /// <param name="point">The SKPoint to convert.</param>
        /// <returns>The converted System.Drawing.PointF.</returns>
        public static System.Drawing.PointF ToDrawingPoint(this SKPoint point)
        {
            return new System.Drawing.PointF(point.X, point.Y);
        }
        
        /// <summary>
        /// Converts an SKPointI to a System.Drawing.Point.
        /// </summary>
        /// <param name="point">The SKPointI to convert.</param>
        /// <returns>The converted System.Drawing.Point.</returns>
        public static System.Drawing.Point ToDrawingPoint(this SKPointI point)
        {
            return new System.Drawing.Point(point.X, point.Y);
        }

        /// <summary>
        /// Converts a System.Drawing.RectangleF to an SKRect.
        /// </summary>
        /// <param name="rect">The System.Drawing.RectangleF to convert.</param>
        /// <returns>The converted SKRect.</returns>
        public static SKRect ToSKRect(this System.Drawing.RectangleF rect)
        {
            return new SKRect(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        /// <summary>
        /// Converts a System.Drawing.Rectangle to an SKRectI.
        /// </summary>
        /// <param name="rect">The System.Drawing.Rectangle to convert.</param>
        /// <returns>The converted SKRectI.</returns>
        public static SKRectI ToSKRect(this System.Drawing.Rectangle rect)
        {
            return new SKRectI(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        /// <summary>
        /// Converts an SKRect to a System.Drawing.RectangleF.
        /// </summary>
        /// <param name="rect">The SKRect to convert.</param>
        /// <returns>The converted System.Drawing.RectangleF.</returns>
        public static System.Drawing.RectangleF ToDrawingRect(this SKRect rect)
        {
            return System.Drawing.RectangleF.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        /// <summary>
        /// Converts an SKRectI to a System.Drawing.Rectangle.
        /// </summary>
        /// <param name="rect">The SKRectI to convert.</param>
        /// <returns>The converted System.Drawing.Rectangle.</returns>
        public static System.Drawing.Rectangle ToDrawingRect(this SKRectI rect)
        {
            return System.Drawing.Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        /// <summary>
        /// Converts a System.Drawing.SizeF to an SKSize.
        /// </summary>
        /// <param name="size">The System.Drawing.SizeF to convert.</param>
        /// <returns>The converted SKSize.</returns>
        public static SKSize ToSKSize(this System.Drawing.SizeF size)
        {
            return new SKSize(size.Width, size.Height);
        }

        /// <summary>
        /// Converts a System.Drawing.Size to an SKSizeI.
        /// </summary>
        /// <param name="size">The System.Drawing.Size to convert.</param>
        /// <returns>The converted SKSizeI.</returns>
        public static SKSizeI ToSKSize(this System.Drawing.Size size)
        {
            return new SKSizeI(size.Width, size.Height);
        }

        /// <summary>
        /// Converts an SKSize to a System.Drawing.SizeF.
        /// </summary>
        /// <param name="size">The SKSize to convert.</param>
        /// <returns>The converted System.Drawing.SizeF.</returns>
        public static System.Drawing.SizeF ToDrawingSize(this SKSize size)
        {
            return new System.Drawing.SizeF(size.Width, size.Height);
        }

        /// <summary>
        /// Converts an SKSizeI to a System.Drawing.Size.
        /// </summary>
        /// <param name="size">The SKSizeI to convert.</param>
        /// <returns>The converted System.Drawing.Size.</returns>
        public static System.Drawing.Size ToDrawingSize(this SKSizeI size)
        {
            return new System.Drawing.Size(size.Width, size.Height);
        }

        /// <summary>
        /// Converts an SKPicture to a System.Drawing.Bitmap with the specified dimensions.
        /// </summary>
        /// <param name="picture">The SKPicture to convert.</param>
        /// <param name="dimensions">The dimensions of the resulting bitmap.</param>
        /// <returns>The converted System.Drawing.Bitmap.</returns>
        public static System.Drawing.Bitmap ToBitmap(this SKPicture picture, SKSizeI dimensions)
        {
            using var image = SKImage.FromPicture(picture, dimensions);
            return image.ToBitmap();
        }
        
        /// <summary>
        /// Converts an SKImage to a System.Drawing.Bitmap.
        /// </summary>
        /// <param name="skiaImage">The SKImage to convert.</param>
        /// <returns>The converted System.Drawing.Bitmap.</returns>
        public static System.Drawing.Bitmap ToBitmap(this SKImage skiaImage)
        {
            var bitmap = new System.Drawing.Bitmap(
                skiaImage.Width,
                skiaImage.Height,
                System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            var data = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.WriteOnly,
                bitmap.PixelFormat);

            using var pixmap = new SKPixmap(new SKImageInfo(data.Width, data.Height), data.Scan0, data.Stride);
            skiaImage.ReadPixels(pixmap, 0, 0);

            bitmap.UnlockBits(data);
            return bitmap;
        }

        /// <summary>
        /// Converts an SKBitmap to a System.Drawing.Bitmap.
        /// </summary>
        /// <param name="skiaBitmap">The SKBitmap to convert.</param>
        /// <returns>The converted System.Drawing.Bitmap.</returns>
        public static System.Drawing.Bitmap ToBitmap(this SKBitmap skiaBitmap)
        {
            using var pixmap = skiaBitmap.PeekPixels();
            using var image = SKImage.FromPixels(pixmap);
            var bmp = image.ToBitmap();
            GC.KeepAlive(skiaBitmap);
            return bmp;
        }

        /// <summary>
        /// Converts an SKPixmap to a System.Drawing.Bitmap.
        /// </summary>
        /// <param name="pixmap">The SKPixmap to convert.</param>
        /// <returns>The converted System.Drawing.Bitmap.</returns>
        public static System.Drawing.Bitmap ToBitmap(this SKPixmap pixmap)
        {
            using var image = SKImage.FromPixels(pixmap);
            return image.ToBitmap();
        }

        /// <summary>
        /// Converts an Alternet.Drawing.SvgImage to a System.Drawing.Bitmap
        /// with the specified dimensions and optional color.
        /// </summary>
        /// <param name="svg">The SVG image to convert.</param>
        /// <param name="width">The width of the resulting bitmap.</param>
        /// <param name="height">The height of the resulting bitmap.</param>
        /// <param name="color">The optional color to apply to the SVG image.</param>
        /// <returns>The converted System.Drawing.Bitmap.</returns>
        public static System.Drawing.Bitmap ToBitmap(
            this Alternet.Drawing.SvgImage svg,
            int width,
            int height,
            Color? color = null)
        {
            var skiaBitmap = svg.CreateSkiaBitmap(new(width, height), color);
            var bitmap = skiaBitmap.ToBitmap();
            return bitmap;
        }

        /// <summary>
        /// Converts an Alternet.Drawing.SvgImage to a System.Drawing.Bitmap 
        /// with the specified dimensions and dark mode option.
        /// When svg is loaded and it is mono, it is filled with the color corresponding to the disabled state.
        /// </summary>
        /// <param name="svg">The SVG image to convert.</param>
        /// <param name="width">The width of the resulting bitmap.</param>
        /// <param name="height">The height of the resulting bitmap.</param>
        /// <param name="isDark">Whether the dark mode is enabled.</param>
        /// <returns>The converted System.Drawing.Bitmap.</returns>
        public static System.Drawing.Bitmap ToDisabledBitmap(
            this Alternet.Drawing.SvgImage svg,
            int width,
            int height,
            bool isDark)
        {
            var skiaBitmap = svg.CreateSkiaDisabledBitmap(new(width, height), isDark);
            var bitmap = skiaBitmap.ToBitmap();
            return bitmap;
        }

        /// <summary>
        /// Converts an Alternet.Drawing.SvgImage to a System.Drawing.Bitmap 
        /// with the specified dimensions and dark mode option.
        /// When svg is loaded and it is mono, it is filled with the color corresponding to the normal state.
        /// </summary>
        /// <param name="svg">The SVG image to convert.</param>
        /// <param name="width">The width of the resulting bitmap.</param>
        /// <param name="height">The height of the resulting bitmap.</param>
        /// <param name="isDark">Whether the dark mode is enabled.</param>
        /// <returns>The converted System.Drawing.Bitmap.</returns>
        public static System.Drawing.Bitmap ToNormalBitmap(
            this Alternet.Drawing.SvgImage svg,
            int width,
            int height,
            bool isDark)
        {
            var skiaBitmap = svg.CreateSkiaNormalBitmap(new(width, height), isDark);
            var bitmap = skiaBitmap.ToBitmap();
            return bitmap;
        }

        /// <summary>
        /// Converts a System.Drawing.Bitmap to an SKBitmap.
        /// </summary>
        /// <param name="bitmap">The System.Drawing.Bitmap to convert.</param>
        /// <returns>The converted SKBitmap.</returns>
        public static SKBitmap ToSKBitmap(this System.Drawing.Bitmap bitmap)
        {
            var info = new SKImageInfo(bitmap.Width, bitmap.Height);
            var skiaBitmap = new SKBitmap(info);
            using var pixmap = skiaBitmap.PeekPixels();
            bitmap.ToSKPixmap(pixmap);
            return skiaBitmap;
        }

        /// <summary>
        /// Converts a System.Drawing.Bitmap to an SKImage.
        /// </summary>
        /// <param name="bitmap">The System.Drawing.Bitmap to convert.</param>
        /// <returns>The converted SKImage.</returns>
        public static SKImage ToSKImage(this System.Drawing.Bitmap bitmap)
        {
            var info = new SKImageInfo(bitmap.Width, bitmap.Height);
            var image = SKImage.Create(info);
            using var pixmap = image.PeekPixels();
            bitmap.ToSKPixmap(pixmap);
            return image;
        }

        /// <summary>
        /// Copies the pixel data from a System.Drawing.Bitmap to an SKPixmap.
        /// </summary>
        /// <param name="bitmap">The System.Drawing.Bitmap to copy pixels from.</param>
        /// <param name="pixmap">The SKPixmap to copy pixels to.</param>
        public static void ToSKPixmap(this System.Drawing.Bitmap bitmap, SKPixmap pixmap)
        {
            if (pixmap.ColorType == SKImageInfo.PlatformColorType)
            {
                var info = pixmap.Info;
                using var tempBitmap = new System.Drawing.Bitmap(
                    info.Width,
                    info.Height,
                    info.RowBytes,
                    System.Drawing.Imaging.PixelFormat.Format32bppPArgb,
                    pixmap.GetPixels());

                using var gr = System.Drawing.Graphics.FromImage(tempBitmap);
                gr.Clear(System.Drawing.Color.Transparent);
                gr.DrawImageUnscaled(bitmap, 0, 0);
            }
            else
            {
                using var tempImage = bitmap.ToSKImage();
                tempImage.ReadPixels(pixmap, 0, 0);
            }
        }
        
        /// <summary>
        /// Converts a System.Drawing.Color to an SKColor.
        /// </summary>
        /// <param name="color">The System.Drawing.Color to convert.</param>
        /// <returns>The converted SKColor.</returns>
        public static SKColor ToSKColor(this System.Drawing.Color color)
        {
            return (SKColor)(uint)color.ToArgb();
        }

        /// <summary>
        /// Converts an SKColor to a System.Drawing.Color.
        /// </summary>
        /// <param name="color">The SKColor to convert.</param>
        /// <returns>The converted System.Drawing.Color.</returns>
        public static System.Drawing.Color ToDrawingColor(this SKColor color)
        {
            return System.Drawing.Color.FromArgb((int)(uint)color);
        }
    }
}