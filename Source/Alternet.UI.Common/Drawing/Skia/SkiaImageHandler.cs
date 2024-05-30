using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI.Extensions;

using SkiaSharp;

namespace Alternet.UI
{
    public class SkiaImageHandler : PlessImageHandler, IImageHandler
    {
        private SKBitmap bitmap;

        public SkiaImageHandler(SKBitmap bitmap)
        {
            this.bitmap = bitmap;
        }

        public SkiaImageHandler()
        {
            bitmap = new();
        }

        public SkiaImageHandler(ImageSet imageSet, SizeI size)
        {
            throw new NotImplementedException();
        }

        public SkiaImageHandler(GenericImage genericImage, int depth = -1)
        {
            throw new NotImplementedException();
        }

        public SkiaImageHandler(int width, int height, Graphics dc)
        {
            throw new NotImplementedException();
        }

        public SkiaImageHandler(ImageSet imageSet, IControl control)
        {
            throw new NotImplementedException();
        }

        public SkiaImageHandler(GenericImage genericImage, Graphics dc)
        {
            throw new NotImplementedException();
        }

        public SkiaImageHandler(Image image)
        {
            throw new NotImplementedException();
        }

        public SkiaImageHandler(Image original, SizeI newSize)
        {
            throw new NotImplementedException();
        }

        public SkiaImageHandler(SizeI size, int depth = 32)
        {
            throw new NotImplementedException();
        }

        public override double ScaleFactor
        {
            get;
            set;
        }

        public override SizeI DipSize
        {
            get;
        }

        public override double ScaledHeight
        {
            get;
        }

        public override SizeI ScaledSize
        {
            get => new((int)ScaledHeight, (int)ScaledWidth);
        }

        public override double ScaledWidth
        {
            get;
        }

        public override SizeI PixelSize
        {
            get
            {
                return new(bitmap.Width, bitmap.Height);
            }
        }

        public override bool IsOk
        {
            get;
        }

        public override bool HasAlpha
        {
            get;
            set;
        }

        public override int Depth
        {
            get;
        }

        public override void Rescale(SizeI sizeNeeded)
        {
            throw new NotImplementedException();
        }

        public override void ResetAlpha()
        {
            throw new NotImplementedException();
        }

        public override GenericImage ToGenericImage()
        {
            throw new NotImplementedException();
        }

        public override IImageHandler ConvertToDisabled(byte brightness = 255)
        {
            throw new NotImplementedException();
        }

        public override IImageHandler GetSubBitmap(RectI rect)
        {
            var resultBitmap = new SKBitmap();
            bitmap.ExtractSubset(resultBitmap, rect);
            return new SkiaImageHandler(resultBitmap);
        }

        public override bool GrayScale()
        {
            throw new NotImplementedException();
        }

        public override bool Load(string name, BitmapType type)
        {
            throw new NotImplementedException();
        }

        public virtual SKBitmap ToSkia() => bitmap;

        public override bool LoadFromStream(Stream stream)
        {
            return InsideTryCatch(() =>
            {
                DisposeBitmap();
                bitmap = SKBitmap.Decode(stream);
                if (bitmap is null)
                {
                    bitmap = new();
                    return false;
                }

                return true;
            });
        }

        public override bool SaveToStream(Stream stream, BitmapType type, int quality)
        {
            return InsideTryCatch(() =>
            {
                var format = type.ToSKEncodedImageFormat();
                if (format is null)
                    return false;
                return bitmap.Encode(stream, format.Value, quality);
            });
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            DisposeBitmap();
        }

        private void DisposeBitmap()
        {
            bitmap.Dispose();
            bitmap = null!;
        }
    }
}
