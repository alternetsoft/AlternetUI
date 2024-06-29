using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

using SkiaSharp;
using SkiaSharp.Views.Windows;

using Windows.Storage.Streams;

using WinRT;

using Alternet.UI.Extensions;

namespace Alternet.UI
{
    public class SkiaWritableBitmap
    {
        public double ActualWidth;

        public double ActualHeight;

        public float Dpi = 1.0f;

        public SKSize CanvasSize;

        public SKSizeI UnscaledSize;

        public SKSizeI ScaledSize;

        public IntPtr Pixels;

        public WriteableBitmap? Bitmap;

        internal const float DpiBase = 96f;

        public void UpdateSize()
        {
            UnscaledSize = SKSizeI.Empty;
            if (!IsPositive(ActualWidth) || !IsPositive(ActualHeight))
            {
                ScaledSize = SKSizeI.Empty;
                return;
            }

            UnscaledSize = new SKSizeI((int)ActualWidth, (int)ActualHeight);

            ScaledSize = new SKSizeI(
                (int)(ActualWidth * (double)Dpi),
                (int)(ActualHeight * (double)Dpi));

            static bool IsPositive(double value)
            {
                if (!double.IsNaN(value) && !double.IsInfinity(value))
                {
                    return value > 0.0;
                }

                return false;
            }
        }

        public void FreeBitmap()
        {
            Bitmap = null;
            Pixels = IntPtr.Zero;
        }

        public ImageBrush CreateBrush()
        {
            ImageBrush background = new()
            {
                ImageSource = Bitmap,
                AlignmentX = AlignmentX.Left,
                AlignmentY = AlignmentY.Top,
                Stretch = Microsoft.UI.Xaml.Media.Stretch.Fill,
            };

            return background;
        }

        public IntPtr GetByteBuffer(IBuffer buffer)
        {
            IBufferByteAccess bufferByteAccess = buffer.As<IBufferByteAccess>()
                ?? throw new InvalidCastException("Unable to convert WriteableBitmap.PixelBuffer to IBufferByteAccess.");
            return bufferByteAccess.Buffer;
        }

        public IntPtr GetPixels(WriteableBitmap bitmap)
        {
            return GetByteBuffer(bitmap.PixelBuffer);
        }

        public SKImageInfo UpdateBitmap()
        {
            UpdateSize();
            SKImageInfo result = new(
                ScaledSize.Width,
                ScaledSize.Height,
                SKImageInfo.PlatformColorType,
                SKAlphaType.Premul);

            if (Bitmap?.PixelWidth != result.Width || Bitmap?.PixelHeight != result.Height)
            {
                FreeBitmap();
            }

            if (Bitmap == null && result.Width > 0 && result.Height > 0)
            {
                Bitmap = new WriteableBitmap(result.Width, result.Height);
                Pixels = GetPixels(Bitmap);
            }

            return result;
        }

        public void DoInvalidate(
            GraphicsView graphicsView,
            Action<SkiaSharp.Views.Maui.SKPaintSurfaceEventArgs> onPaintSurface)
        {
            bool ignorePixelScaling = false;

            SKImageInfo sKImageInfo = UpdateBitmap();
            if (sKImageInfo.Width <= 0 || sKImageInfo.Height <= 0)
            {
                CanvasSize = SKSize.Empty;
                return;
            }

            SKSizeI sKSizeI = ignorePixelScaling ? UnscaledSize : sKImageInfo.Size;

            CanvasSize = sKSizeI;

            using (SKSurface sKSurface = SKSurface.Create(sKImageInfo, Pixels, sKImageInfo.RowBytes))
            {
                SKCanvas canvas = sKSurface.Canvas;

                if (ignorePixelScaling)
                {
                    canvas.Scale(Dpi);
                    canvas.Save();
                }

                SkiaSharp.Views.Maui.SKPaintSurfaceEventArgs e =
                    new(sKSurface, sKImageInfo.WithSize(sKSizeI), sKImageInfo);

                onPaintSurface(e);

                canvas.Flush();
            }

            Bitmap?.Invalidate();
        }
    }
}
