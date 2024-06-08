using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    internal class SkiaSurfaceOnSkia : DisposableObject, ISkiaSurface
    {
        private SKBitmap bitmap;
        private SKCanvas canvas;

        public SkiaSurfaceOnSkia(SKBitmap bitmap)
        {
            this.bitmap = bitmap;
            canvas = new SKCanvas(bitmap);
        }

        public int Width => bitmap.Width;

        public int Height => bitmap.Height;

        public SKColorType ColorType => bitmap.Info.ColorType;

        public SKAlphaType AlphaType => bitmap.Info.AlphaType;

        public SKSurface? Surface => null;

        public SKCanvas Canvas => canvas;

        public SKBitmap? Bitmap => bitmap;

        public bool IsOk => SkiaUtils.BitmapIsOk(bitmap);
    }
}
