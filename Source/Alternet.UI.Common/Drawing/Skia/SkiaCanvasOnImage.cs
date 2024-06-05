using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkiaSharp;

using Alternet.UI;

namespace Alternet.Drawing
{
    internal class SkiaCanvasOnImage: DisposableObject, ISkiaCanvasLock
    {
        private readonly Image image;
        private readonly int width;
        private readonly int height;
        private readonly SKSurface surface;
        private readonly SKCanvas canvas;

        public SkiaCanvasOnImage(Image image, Coord scaleFactor)
        {
            this.image = image;
            this.width = image.Width;
            this.height = image.Height;

            var info = new SKImageInfo(
                width,
                height,
                SKImageInfo.PlatformColorType,
                SKAlphaType.Unpremul);

            var ptr = image.LockPixels();
            var stride = image.GetPixelsStride();

            if (stride < 0)
            {
                stride = -stride;
                ptr -= stride * (height - 1);
            }

            surface = SKSurface.Create(info, ptr, stride);
            canvas = surface.Canvas;
            canvas.Scale(1, -1, 0, height / 2.0f);
            canvas.Scale((float)scaleFactor);
        }

        public int Width => width;
        
        public int Height => height;
        
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
