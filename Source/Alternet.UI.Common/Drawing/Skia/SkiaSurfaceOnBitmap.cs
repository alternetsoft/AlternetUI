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
        private readonly ILockImageBits image;

        static SkiaSurfaceOnBitmap()
        {
        }

        public SkiaSurfaceOnBitmap(ILockImageBits image)
        {
            if (App.IsWindowsOS)
            {
                colorType = SKColorType.Bgra8888;
                alphaType = SKAlphaType.Premul;
            }
            else
            if (App.IsLinuxOS)
            {
                colorType = SKColorType.Rgba8888;
                alphaType = SKAlphaType.Unpremul;
            }
            else
            if (App.IsMacOS)
            {
                colorType = SKColorType.Rgba8888;
                alphaType = SKAlphaType.Premul;
            }
            else
            {
                colorType = SKColorType.Unknown;
                alphaType = SKAlphaType.Premul;
            }

            width = image.Width;
            height = image.Height;
            this.image = image;

            var info = new SKImageInfo(
                width,
                height,
                colorType,
                alphaType);

            var ptr = image.LockBits();
            var stride = image.GetStride();
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
            image.UnlockBits();
            base.DisposeManaged();
        }
    }
}
