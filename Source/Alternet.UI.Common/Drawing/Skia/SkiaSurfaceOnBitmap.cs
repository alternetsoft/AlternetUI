using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using SkiaSharp;

namespace Alternet.Drawing
{
    internal class SkiaSurfaceOnBitmap : DisposableObject, ISkiaSurface
    {
        private readonly int width;
        private readonly int height;
        private readonly SKSurface surface;
        private readonly SKCanvas canvas;
        private readonly SKColorType colorType;
        private readonly SKAlphaType alphaType;
        private readonly bool isOk;
        private readonly Image image;
        private readonly ImageLockMode lockMode;
        private readonly SKMatrix initialMatrix;

        public SkiaSurfaceOnBitmap(Image image, ImageLockMode lockMode)
        {
            this.lockMode = lockMode;
            this.image = image;

            width = image.Width;
            height = image.Height;

            var formatKind = image.Handler.BitsFormat;
            var format = GraphicsFactory.GetBitsFormat(formatKind);
            colorType = format.ColorType;
            alphaType = image.Handler.AlphaType;

            var info = new SKImageInfo(
                width,
                height,
                colorType,
                alphaType);

            var ptr = image.Handler.LockBits();
            var stride = image.Handler.GetStride();
            var negative = stride < 0;
            isOk = ptr != default;

            if (isOk)
            {
                if (negative)
                {
                    stride = -stride;
                    ptr -= stride * (height - 1);
                }

                var props = new SKSurfaceProperties(SKPixelGeometry.RgbHorizontal);
                surface = SKSurface.Create(info, ptr, stride, props);
            }
            else
            {
                surface = SkiaUtils.CreateNullSurface(width, height);
            }

            canvas = surface.Canvas;

            if (negative)
            {
                canvas.Translate(0, height);
                canvas.Scale(1, -1);
            }

            initialMatrix = canvas.TotalMatrix;

            /*
            if (negative)
                canvas.Scale(1, -1, 0, height / 2.0f);
            */
        }

        public SKMatrix InitialMatrix => initialMatrix;

        public int Width => width;

        public int Height => height;

        public bool IsOk => isOk;

        public SKColorType ColorType => colorType;

        public SKAlphaType AlphaType => alphaType;

        public SKBitmap? Bitmap => null;

        public SKSurface? Surface => surface;

        public ImageLockMode LockMode => lockMode;

        public SKCanvas Canvas => canvas;

        protected override void DisposeManaged()
        {
            canvas.Flush();
            surface.Dispose();
            image.Handler.UnlockBits();
            base.DisposeManaged();
        }
    }
}
