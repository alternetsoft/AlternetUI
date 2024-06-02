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
        public static SKFilterQuality DefaultScaleQuality = SKFilterQuality.High;

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
            : this(imageSet.AsImage(size))
        {
        }

        public SkiaImageHandler(GenericImage genericImage, int depth = 32)
        {
            bitmap = (SKBitmap)genericImage;
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
            if (image?.Handler is SkiaImageHandler skiaHandler)
            {
                bitmap = skiaHandler.bitmap.Copy();
            }
            else
            if (image is null)
                bitmap = new();
            else
                bitmap = (SKBitmap)image;
        }

        public SkiaImageHandler(Image image, SizeI newSize)
        {
            bitmap = new(newSize.Width, newSize.Height);

            if (image?.Handler is SkiaImageHandler skiaHandler)
                Fn(skiaHandler.bitmap);
            else
            if (image is not null)
                Fn((SKBitmap)image);

            void Fn(SKBitmap source)
            {
                if (!source.ScalePixels(bitmap, DefaultScaleQuality))
                    BaseApplication.LogError("Error scaling pixels in SkiaImageHandler.Create(Image, SizeI)");
            }
        }

        public SkiaImageHandler(SizeI size, int depth = 32)
        {
            CoerceDepth(depth);
            bitmap = new(size.Width, size.Height);
        }

        public SKBitmap Bitmap => bitmap;

        public override Coord ScaleFactor
        {
            get => 1;

            set
            {
            }
        }

        public override SizeI DipSize
        {
            get => PixelSize;
        }

        public override Coord ScaledHeight
        {
            get => PixelSize.Height;
        }

        public override SizeI ScaledSize
        {
            get => new((int)ScaledHeight, (int)ScaledWidth);
        }

        public override Coord ScaledWidth
        {
            get => PixelSize.Width;
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
            get => bitmap.ReadyToDraw && bitmap.Height > 0 && bitmap.Width > 0;
        }

        public override bool HasAlpha
        {
            get => !bitmap.Info.IsOpaque;
            
            set
            {
                if (HasAlpha == value)
                    return;

                SKBitmap newBitmap = new(bitmap.Width, bitmap.Height, !value);
                if (bitmap.CopyTo(newBitmap))
                    bitmap = newBitmap;
                else
                    BaseApplication.LogError($"SkiaImageHandler.HasAlpha = {value}");
            }
        }

        public override int Depth
        {
            get => 32;
        }

        public override bool Rescale(SizeI sizeNeeded)
        {
            SKBitmap newBitmap = new(sizeNeeded.Width, sizeNeeded.Height);
            var result = bitmap.ScalePixels(newBitmap, DefaultScaleQuality);
            if(result)
                bitmap = newBitmap;
            return result;
        }

        public override GenericImage ToGenericImage()
        {
            return (GenericImage)bitmap;
        }

        public override IImageHandler GetSubBitmap(RectI rect)
        {
            var resultBitmap = new SKBitmap();
            if(bitmap.ExtractSubset(resultBitmap, rect))
                return new SkiaImageHandler(resultBitmap);
            else
            {
                BaseApplication.LogError($"SkiaImageHandler.GetSubBitmap({rect})");
                return new SkiaImageHandler(new SKBitmap(rect.Width, rect.Height));
            }
        }

        public override IImageHandler ConvertToDisabled(byte brightness = 255)
        {
            throw new NotImplementedException();
        }

        public override bool ResetAlpha()
        {
            HasAlpha = false;
            return HasAlpha == false;
        }

        public override bool GrayScale()
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

        private int CoerceDepth(int depth)
        {
            if (depth >= 0 && depth != 32)
                BaseApplication.LogError("Depth = 32 is expected");
            return 32;
        }

        private void DisposeBitmap()
        {
            bitmap.Dispose();
            bitmap = null!;
        }
    }
}
