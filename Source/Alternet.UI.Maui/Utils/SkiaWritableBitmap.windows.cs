using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

using SkiaSharp;
using SkiaSharp.Views.Windows;

using Windows.Storage.Streams;

using WinRT;

namespace Alternet.UI
{
    public class SkiaWritableBitmap
    {
        public double ActualWidth;

        public double ActualHeight;

        public float Dpi;

        public SKSizeI UnscaledSize;

        public SKSizeI ScaledSize;

        public IntPtr Pixels;

        public WriteableBitmap? Bitmap;

        public void UpdateSize()
        {
            UnscaledSize = SKSizeI.Empty;
            if (!IsPositive(ActualWidth) || !IsPositive(ActualHeight))
            {
                ScaledSize = SKSizeI.Empty;
                return;
            }

            UnscaledSize = new SKSizeI((int)ActualWidth, (int)ActualHeight);

            ScaledSize = new SKSizeI((int)(ActualWidth * (double)Dpi), (int)(ActualHeight * (double)Dpi));

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
            ImageBrush background = new ImageBrush
            {
                ImageSource = Bitmap,
                AlignmentX = AlignmentX.Left,
                AlignmentY = AlignmentY.Top,
                Stretch = Stretch.Fill,
            };

            return background;
        }

        public IntPtr GetByteBuffer(IBuffer buffer)
        {
            IBufferByteAccess bufferByteAccess = buffer.As<IBufferByteAccess>();

            if (bufferByteAccess == null)
            {
                throw new InvalidCastException("Unable to convert WriteableBitmap.PixelBuffer to IBufferByteAccess.");
            }

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
    }
}
