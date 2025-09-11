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
        private readonly ImageLockMode lockMode;
        private SKBitmap? bitmap;
        private SKSurface? surface;
        private SKCanvas? canvas;

        public SkiaSurfaceOnSkia(SKBitmap? bitmap, ImageLockMode lockMode)
        {
            this.lockMode = lockMode;
            CreateCanvas(bitmap, lockMode);
        }

        public ISkiaSurface.SurfaceKind Kind { get; internal set; }

        public ImageLockMode LockMode => lockMode;

        public int Width => bitmap?.Width ?? 0;

        public int Height => bitmap?.Height ?? 0;

        public SKColorType ColorType => bitmap?.Info.ColorType ?? SKColorType.Unknown;

        public SKAlphaType AlphaType => bitmap?.Info.AlphaType ?? SKAlphaType.Unknown;

        public SKSurface? Surface => surface;

        public SKCanvas Canvas => canvas!;

        public SKBitmap? Bitmap => bitmap;

        public virtual bool IsOk => SkiaUtils.BitmapIsOk(bitmap);

        protected override void DisposeManaged()
        {
            canvas?.Flush();
            base.DisposeManaged();
        }

        protected virtual void CreateCanvas(SKBitmap? bitmap, ImageLockMode lockMode)
        {
            if (bitmap is null)
            {
                surface = SkiaUtils.CreateNullSurface(0, 0);
                canvas = surface.Canvas;
            }
            else
            {
                this.bitmap = bitmap;
                canvas = new SKCanvas(bitmap);
            }
        }
    }
}
