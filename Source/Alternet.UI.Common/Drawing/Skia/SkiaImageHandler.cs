using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.Skia;
using Alternet.UI.Extensions;

using SkiaSharp;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="IImageHandler"/> interface provider for the <see cref="SKBitmap"/>.
    /// </summary>
    public class SkiaImageHandler : PlessImageHandler, IImageHandler
    {
        /// <summary>
        /// Gets or sets default <see cref="SKSamplingOptions"/> used hen images are resized.
        /// If this is Null, <see cref="SKSamplingOptions.Default"/> is used when images are resized.
        /// </summary>
        public static SKSamplingOptions? DefaultSamplingOptions;

        private SKBitmap bitmap;
        private Coord scaleFactor = 1;

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
                if (!source.ScalePixels(bitmap, GetDefaultSamplingOptions()))
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
        public virtual SKAlphaType AlphaType => bitmap.AlphaType;

        /// <inheritdoc/>
        public virtual int Width
        {
            get
            {
                return bitmap.Width;
            }
        }

        /// <inheritdoc/>
        public virtual int Height
        {
            get
            {
                return bitmap.Height;
            }
        }

        /// <summary>
        /// Gets internal <see cref="SKBitmap"/>.
        /// </summary>
        public virtual SKBitmap Bitmap => bitmap;

        /// <inheritdoc/>
        public virtual Coord ScaleFactor
        {
            get => scaleFactor;

            set
            {
                scaleFactor = value;
            }
        }

        /// <inheritdoc/>
        public virtual SizeI DipSize
        {
            get
            {
                return ScaledSize;
            }
        }

        /// <inheritdoc/>
        public virtual Coord ScaledHeight
        {
            get
            {
                return bitmap.Height / ScaleFactor;
            }
        }

        /// <inheritdoc/>
        public virtual SizeI ScaledSize
        {
            get
            {
                return new((int)ScaledHeight, (int)ScaledWidth);
            }
        }

        /// <inheritdoc/>
        public virtual Coord ScaledWidth
        {
            get
            {
                return bitmap.Width / ScaleFactor;
            }
        }

        /// <inheritdoc/>
        public virtual SizeI PixelSize
        {
            get
            {
                return new(bitmap.Width, bitmap.Height);
            }
        }

        /// <inheritdoc/>
        public virtual bool IsOk
        {
            get => SkiaHelper.BitmapIsOk(bitmap);
        }

        /// <inheritdoc/>
        public virtual bool HasAlpha
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
        public virtual bool HasMask
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public virtual int Depth
        {
            get
            {
                return bitmap.BytesPerPixel * 8;
            }
        }

        /// <inheritdoc/>
        public virtual ImageBitsFormatKind BitsFormat
        {
            get => ImageBitsFormatKind.Unknown;
        }

        /// <summary>
        /// Gets default <see cref="SKSamplingOptions"/> used when image is scaled.
        /// Uses <see cref="DefaultSamplingOptions"/>.
        /// </summary>
        /// <returns></returns>
        public static SKSamplingOptions GetDefaultSamplingOptions()
        {
            return DefaultSamplingOptions ?? SKSamplingOptions.Default;
        }

        /// <summary>
        /// Creates <see cref="SkiaImageHandler"/> from <see cref="SKPicture"/>
        /// using the specified width and height.
        /// </summary>
        /// <param name="picture">Picture to use as a source of pixel data.</param>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <returns>Image instance with dimensions specified in <paramref name="width"/>
        /// and <paramref name="height"/>.</returns>
        /// <param name="color">Overrides default fill color.</param>
        /// <returns></returns>
        public static SkiaImageHandler CreateFromPicture(
            SKPicture? picture,
            int width,
            int height,
            Color? color)
        {
            var result = new SkiaImageHandler((width, height));
            if (picture is null)
                return result;
            var cullRect = picture.CullRect;
            var scaleX = width / cullRect.Width;
            var scaleY = height / cullRect.Height;
            var matrix = SKMatrix.CreateScale((float)scaleX, (float)scaleY);
            var skiaBitmap = result.bitmap;

            using var canvas = new SKCanvas(skiaBitmap);

            if(color is not null)
            {
                using SKPaint paint = new();
                paint.ColorFilter = SKColorFilter.CreateBlendMode(color, SKBlendMode.SrcIn);
                canvas.DrawPicture(picture, in matrix, paint);
            }
            else
            {
                canvas.DrawPicture(picture, in matrix);
            }

            canvas.Flush();

            return result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaImageHandler"/> class
        /// from the specified <see cref="Stream"/> which contains svg data.
        /// </summary>
        /// <param name="stream">Stream with svg data.</param>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <returns>Image instance with dimensions specified in <paramref name="width"/>
        /// and <paramref name="height"/> and data loaded from <paramref name="stream"/>. </returns>
        /// <param name="color">Svg fill color. Optional.
        /// If provided, svg fill color is changed to the specified value.</param>
        public static SkiaImageHandler CreateFromSvg(
            Stream stream,
            int width,
            int height,
            Color? color)
        {
            var svg = new Svg.Skia.SKSvg();
            svg.Load(stream);
            var picture = svg.Picture;

            var result = CreateFromPicture(picture, width, height, color);
            return result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class
        /// from the specified string which contains svg data.
        /// </summary>
        /// <param name="s">String with svg data.</param>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <returns>Image instance with dimensions specified in <paramref name="width"/>
        /// and <paramref name="height"/> and data loaded from <paramref name="s"/>. </returns>
        /// <param name="color">Svg fill color. Optional.
        /// If provided, svg fill color is changed to the specified value.</param>
        public static SkiaImageHandler CreateFromSvg(
            string s,
            int width,
            int height,
            Color? color)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            MemoryStream stream = new(bytes);
            var result = CreateFromSvg(stream, width, height, color);
            return result;
        }

        /// <inheritdoc/>
        public virtual bool Rescale(SizeI sizeNeeded)
        {
            SKBitmap? newBitmap = new(sizeNeeded.Width, sizeNeeded.Height);
            var result = bitmap.ScalePixels(newBitmap, GetDefaultSamplingOptions());
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
        public virtual GenericImage ToGenericImage()
        {
            return (GenericImage)bitmap;
        }

        /// <inheritdoc/>
        public virtual IImageHandler GetSubBitmap(RectI rect)
        {
            var resultBitmap = new SKBitmap(rect.Width, rect.Height);
            if (!bitmap.ExtractSubset(resultBitmap, rect))
                App.LogError($"SkiaImageHandler.GetSubBitmap({rect})");
            return new SkiaImageHandler(resultBitmap);
        }

        /// <inheritdoc/>
        public void SetImmutable()
        {
            bitmap.SetImmutable();
        }

        /// <inheritdoc/>
        public virtual bool ResetAlpha()
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
        public virtual nint LockBits()
        {
            return default;
        }

        /// <inheritdoc/>
        public virtual void UnlockBits()
        {
        }

        /// <inheritdoc/>
        public virtual int GetStride()
        {
            return 0;
        }

        /// <inheritdoc/>
        public virtual void Assign(GenericImage image)
        {
            bitmap.Pixels = image.Pixels;
        }

        /// <inheritdoc/>
        public virtual void Assign(SKBitmap image)
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
