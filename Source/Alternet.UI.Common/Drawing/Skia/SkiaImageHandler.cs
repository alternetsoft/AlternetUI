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
    /// <summary>
    /// Implements <see cref="IImageHandler"/> interface provider for the <see cref="SKBitmap"/>.
    /// </summary>
    public class SkiaImageHandler : PlessImageHandler, IImageHandler
    {
        private SKBitmap bitmap;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaImageHandler"/> class using the
        /// specified parameters.
        /// </summary>
        /// <param name="bitmap"></param>
        public SkiaImageHandler(SKBitmap bitmap)
        {
            this.bitmap = bitmap;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaImageHandler"/> class.
        /// </summary>
        public SkiaImageHandler()
        {
            bitmap = new();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaImageHandler"/> class using the
        /// specified parameters.
        /// </summary>
        /// <param name="imageSet"></param>
        /// <param name="size"></param>
        public SkiaImageHandler(ImageSet imageSet, SizeI size)
            : this(imageSet.AsImage(size))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaImageHandler"/> class using the
        /// specified parameters.
        /// </summary>
        /// <param name="genericImage"></param>
        /// <param name="depth"></param>
        public SkiaImageHandler(GenericImage genericImage, int depth = 32)
        {
            CoerceDepth(depth);
            bitmap = (SKBitmap)genericImage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaImageHandler"/> class using the
        /// specified parameters.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="dc"></param>
        public SkiaImageHandler(int width, int height, Graphics dc)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaImageHandler"/> class using the
        /// specified parameters.
        /// </summary>
        /// <param name="imageSet"></param>
        /// <param name="control"></param>
        public SkiaImageHandler(ImageSet imageSet, IControl control)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaImageHandler"/> class using the
        /// specified parameters.
        /// </summary>
        /// <param name="genericImage"></param>
        /// <param name="dc"></param>
        public SkiaImageHandler(GenericImage genericImage, Graphics dc)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaImageHandler"/> class using the
        /// specified parameters.
        /// </summary>
        /// <param name="image"></param>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaImageHandler"/> class using the
        /// specified parameters.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="newSize"></param>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaImageHandler"/> class using the
        /// specified parameters.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="depth"></param>
        public SkiaImageHandler(SizeI size, int depth = 32)
        {
            depth = CoerceDepth(depth);
            bitmap = new(size.Width, size.Height, depth != 32);
        }

        /// <inheritdoc/>
        public SKAlphaType AlphaType => bitmap.AlphaType;

        /// <inheritdoc/>
        public int Width => bitmap.Width;

        /// <inheritdoc/>
        public int Height => bitmap.Height;

        /// <summary>
        /// Gets internal <see cref="SKBitmap"/>.
        /// </summary>
        public SKBitmap Bitmap => bitmap;

        /// <inheritdoc/>
        public Coord ScaleFactor
        {
            get => 1;

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc/>
        public SizeI DipSize
        {
            get => PixelSize;
        }

        /// <inheritdoc/>
        public Coord ScaledHeight
        {
            get => PixelSize.Height;
        }

        /// <inheritdoc/>
        public SizeI ScaledSize
        {
            get => new((int)ScaledHeight, (int)ScaledWidth);
        }

        /// <inheritdoc/>
        public Coord ScaledWidth
        {
            get => PixelSize.Width;
        }

        /// <inheritdoc/>
        public SizeI PixelSize
        {
            get
            {
                return new(bitmap.Width, bitmap.Height);
            }
        }

        /// <inheritdoc/>
        public bool IsOk
        {
            get => SkiaUtils.BitmapIsOk(bitmap);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public bool HasMask
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public int Depth
        {
            get
            {
                return bitmap.BytesPerPixel * 8;
            }
        }

        /// <inheritdoc/>
        public ImageBitsFormatKind BitsFormat
        {
            get => ImageBitsFormatKind.Unknown;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public GenericImage ToGenericImage()
        {
            return (GenericImage)bitmap;
        }

        /// <inheritdoc/>
        public IImageHandler GetSubBitmap(RectI rect)
        {
            var resultBitmap = new SKBitmap(rect.Width, rect.Height);
            if (!bitmap.ExtractSubset(resultBitmap, rect))
                App.LogError($"SkiaImageHandler.GetSubBitmap({rect})");
            return new SkiaImageHandler(resultBitmap);
        }

        /// <inheritdoc/>
        public bool ResetAlpha()
        {
            HasAlpha = false;
            return HasAlpha == false;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public nint LockBits()
        {
            return default;
        }

        /// <inheritdoc/>
        public void UnlockBits()
        {
        }

        /// <inheritdoc/>
        public int GetStride()
        {
            return 0;
        }

        /// <inheritdoc/>
        public void Assign(GenericImage image)
        {
            bitmap.Pixels = image.Pixels;
        }

        /// <inheritdoc/>
        public void Assign(SKBitmap image)
        {
            if (!image.CopyTo(bitmap))
                bitmap = image.Copy();
        }

        /// <inheritdoc/>
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
    }
}
