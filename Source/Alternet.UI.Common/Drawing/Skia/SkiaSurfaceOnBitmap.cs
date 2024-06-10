using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkiaSharp;

using Alternet.UI;
using System.Diagnostics;

namespace Alternet.Drawing
{
    internal class SkiaSurfaceOnBitmap: DisposableObject, ISkiaSurface
    {
        private readonly int width;
        private readonly int height;
        private readonly SKSurface surface;
        private readonly SKCanvas canvas;
        private readonly SKColorType colorType;
        private readonly SKAlphaType alphaType;
        private readonly bool isOk;
        private readonly Image image;

        public SkiaSurfaceOnBitmap(Image image)
        {
            this.image = image;

            if (App.IsWindowsOS)
            {
                alphaType = SKAlphaType.Premul;
            }
            else
            if (App.IsLinuxOS)
            {
                alphaType = SKAlphaType.Unpremul;
            }
            else
            if (App.IsMacOS)
            {
                alphaType = SKAlphaType.Premul;
            }
            else
            {
                alphaType = SKAlphaType.Premul;
            }

            width = image.Width;
            height = image.Height;

            var formatKind = image.Handler.BitsFormat;
            var format = GraphicsFactory.GetBitsFormat(formatKind);
            colorType = format.ColorType;

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

                surface = SKSurface.Create(info, ptr, stride);
            }
            else
            {
                surface = SKSurface.CreateNull(width, height);
            }

            canvas = surface.Canvas;

            if(negative)
                canvas.Scale(1, -1, 0, height / 2.0f);
        }

        public int Width => width;

        public int Height => height;

        public bool IsOk => isOk;

        public SKColorType ColorType => colorType;

        public SKAlphaType AlphaType => alphaType;

        public SKBitmap? Bitmap => null;

        public SKSurface? Surface => surface;

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
