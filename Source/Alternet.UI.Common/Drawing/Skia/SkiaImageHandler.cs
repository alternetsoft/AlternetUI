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
            : this(imageSet.AsImage(size))
        {
        }

        public SkiaImageHandler(GenericImage genericImage, int depth = 32)
        {
            CoerceDepth(depth);
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
                if (!source.ScalePixels(bitmap, GraphicsFactory.DefaultScaleQuality))
                    App.LogError("Error scaling pixels in SkiaImageHandler.Create(Image, SizeI)");
            }
        }

        public SkiaImageHandler(SizeI size, int depth = 32)
        {
            depth = CoerceDepth(depth);
            bitmap = new(size.Width, size.Height, depth != 32);
        }

        public SKAlphaType AlphaType => bitmap.AlphaType;

        public int Width => bitmap.Width;

        public int Height => bitmap.Height;

        public SKBitmap Bitmap => bitmap;

        public Coord ScaleFactor
        {
            get => 1;

            set
            {
                throw new NotImplementedException();
            }
        }

        public SizeI DipSize
        {
            get => PixelSize;
        }

        public Coord ScaledHeight
        {
            get => PixelSize.Height;
        }

        public SizeI ScaledSize
        {
            get => new((int)ScaledHeight, (int)ScaledWidth);
        }

        public Coord ScaledWidth
        {
            get => PixelSize.Width;
        }

        public SizeI PixelSize
        {
            get
            {
                return new(bitmap.Width, bitmap.Height);
            }
        }

        public bool IsOk
        {
            get => SkiaUtils.BitmapIsOk(bitmap);
        }

        public bool HasAlpha
        {
            get => !bitmap.Info.IsOpaque;

            set
            {
                if (HasAlpha == value)
                    return;

                SKBitmap? newBitmap = new(bitmap.Width, bitmap.Height, !value);
                if (bitmap.CopyTo(newBitmap))
                {
                    DisposeBitmap();
                    bitmap = newBitmap;
                }
                else
                {
                    SafeDispose(ref newBitmap);
                    App.LogError($"SkiaImageHandler.HasAlpha = {value}");
                }
            }
        }

        public bool HasMask
        {
            get
            {
                return false;
            }
        }

        public int Depth
        {
            get
            {
                return bitmap.BytesPerPixel * 8;
            }
        }

        public ImageBitsFormatKind BitsFormat
        {
            get => ImageBitsFormatKind.Unknown;
        }

        public bool Rescale(SizeI sizeNeeded)
        {
            SKBitmap? newBitmap = new(sizeNeeded.Width, sizeNeeded.Height);
            var result = bitmap.ScalePixels(newBitmap, GraphicsFactory.DefaultScaleQuality);
            if (result)
            {
                DisposeBitmap();
                bitmap = newBitmap;
            }
            else
                SafeDispose(ref newBitmap);
            return result;
        }

        public GenericImage ToGenericImage()
        {
            return (GenericImage)bitmap;
        }

        public IImageHandler GetSubBitmap(RectI rect)
        {
            var resultBitmap = new SKBitmap(rect.Width, rect.Height);
            if (!bitmap.ExtractSubset(resultBitmap, rect))
                App.LogError($"SkiaImageHandler.GetSubBitmap({rect})");
            return new SkiaImageHandler(resultBitmap);
        }

        public bool ResetAlpha()
        {
            HasAlpha = false;
            return HasAlpha == false;
        }

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
            if (depth >= 0 && depth != 32 && depth != 24)
            {
                App.LogError("Depth = 32 or 24 is expected");
                return 32;
            }

            if (depth < 0)
                return 32;

            return depth;
        }

        private void DisposeBitmap()
        {
            SafeDispose(ref bitmap!);
        }

        public nint LockBits()
        {
            return default;
        }

        public void UnlockBits()
        {
        }

        public int GetStride()
        {
            return 0;
        }

        public void Assign(GenericImage image)
        {
            bitmap.Pixels = image.Pixels;
        }

        public void Assign(SKBitmap image)
        {
            if (!image.CopyTo(bitmap))
                bitmap = image.Copy();
        }
    }
}
