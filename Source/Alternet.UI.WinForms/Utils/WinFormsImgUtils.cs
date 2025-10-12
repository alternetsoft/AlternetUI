using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkiaSharp;

namespace Alternet.UI.WinForms
{
    /// <summary>
    /// Provides utility methods for working with <see cref="Bitmap"/> objects.
    /// </summary>
    public static class WinFormsImgUtils
    {
        /// <summary>
        /// Determines whether the specified <see cref="Bitmap"/> is compatible with SkiaSharp.
        /// </summary>
        /// <param name="bitmap">The <see cref="Bitmap"/> to evaluate.
        /// Must not be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the <paramref name="bitmap"/>
        /// has a non-zero width and height  and uses a pixel
        /// format compatible with SkiaSharp
        /// (either <see cref="System.Drawing.Imaging.PixelFormat.Format32bppPArgb"/> 
        /// or <see cref="System.Drawing.Imaging.PixelFormat.Format32bppArgb"/>); otherwise,
        /// <see langword="false"/>.</returns>
        public static bool IsSkiaCompatibleBitmap(Bitmap bitmap)
        {
            if (bitmap == null || bitmap.Width == 0 || bitmap.Height == 0)
                return false;

            if (bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppPArgb
                || bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                return true;

            return false;
        }

        /// <summary>
        /// Executes the specified action with a locked <see cref="SKImage"/> representation
        /// of the provided <see cref="Bitmap"/>.
        /// </summary>
        /// <remarks>This method locks the provided <see cref="Bitmap"/> in read-only mode,
        /// creates an <see cref="SKImage"/> from its pixel data,
        /// and ensures that the bitmap is unlocked after the action
        /// completes. The <see cref="SKImage"/> is disposed automatically
        /// after the action is executed.</remarks>
        /// <param name="bitmap">The <see cref="Bitmap"/> to lock and convert
        /// into an <see cref="SKImage"/>.</param>
        /// <param name="action">The action to perform on
        /// the <see cref="SKImage"/>. The <see cref="SKImage"/> is valid only within the scope
        /// of this action.</param>
        public static bool WithLockedImage(Bitmap bitmap, Action<SKImage> action)
        {
            if (!IsSkiaCompatibleBitmap(bitmap))
                return false;

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            System.Drawing.Imaging.BitmapData bmpData
                = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            try
            {
                var info = new SKImageInfo(
                    bitmap.Width,
                    bitmap.Height,
                    SKColorType.Bgra8888, SKAlphaType.Premul);
                var pixMap = new SKPixmap(info, bmpData.Scan0);
                using var skImage = SKImage.FromPixels(pixMap);
                action(skImage);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                bitmap.UnlockBits(bmpData);
            }
        }

        /// <summary>
        /// Draws a bitmap onto the specified <see cref="SKCanvas"/> at the given position.
        /// </summary>
        /// <param name="canvas">The <see cref="SKCanvas"/> on which the bitmap will be drawn.
        /// Cannot be <c>null</c>.</param>
        /// <param name="bitmap">The <see cref="Bitmap"/> to draw. Cannot be <c>null</c>.</param>
        /// <param name="point">The <see cref="SKPoint"/> specifying the position on the
        /// canvas where the bitmap will be drawn.</param>
        /// <returns><see langword="true"/> if the bitmap was successfully drawn;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool DrawBitmapOnCanvas(SKCanvas canvas, Bitmap bitmap, float x, float y)
        {
            var result = WithLockedImage(bitmap, (image) => {
                canvas.DrawImage(image, x, y);
            });

            return result;
        }
    }
}
