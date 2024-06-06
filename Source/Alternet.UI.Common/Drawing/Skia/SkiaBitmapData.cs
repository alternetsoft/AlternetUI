using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkiaSharp;

using Alternet.UI;

namespace Alternet.Drawing
{
    internal class SkiaBitmapData: BitmapData, ISkiaBitmapData
    {
        public static SKColorType DefaultColorType = SKImageInfo.PlatformColorType;

        public static SKAlphaType DefaultAlphaType = SKAlphaType.Premul;

        private readonly Image image;
        private readonly SKSurface surface;
        private readonly SKCanvas canvas;
        private readonly SKColorType colorType = DefaultColorType;
        private readonly SKAlphaType alphaType = DefaultAlphaType;
        private readonly bool isOk;

        public SkiaBitmapData(Image image)
        {
            this.image = image;
            Width = image.Width;
            Height = image.Height;

            var info = new SKImageInfo(
                Width,
                Height,
                colorType,
                alphaType);

            var handler = image.Handler;

            var ptr = handler.LockBits();
            var stride = handler.GetStride();
            var negative = stride < 0;
            isOk = ptr != default;

            if (isOk)
            {
                if (negative)
                {
                    stride = -stride;
                    ptr -= stride * (Height - 1);
                }

                surface = SKSurface.Create(info, ptr, stride);
            }
            else
            {
                surface = SKSurface.CreateNull(Width, Height);
            }

            canvas = surface.Canvas;
            if(negative)
                canvas.Scale(1, -1, 0, Height / 2.0f);
        }

        public bool IsOk => isOk;

        public SKColorType ColorType => colorType;

        public SKAlphaType AlphaType => alphaType;

        public Image Image => image;

        public SKSurface Surface => surface;

        public SKCanvas Canvas => canvas;

        protected override void DisposeManaged()
        {
            canvas.Flush();
            surface.Dispose();
            base.DisposeManaged();
        }
    }
}
