using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkiaSharp;

namespace Alternet.UI.Native
{
    internal partial class Image : Alternet.Drawing.IImageHandler
    {
        public int Width => PixelWidth;

        public int Height => PixelHeight;

        public Alternet.Drawing.SizeI DipSize => (DipSizeX, DipSizeY);

        public Alternet.Drawing.SizeI ScaledSize => (ScaledSizeX, ScaledSizeY);

        public Alternet.Drawing.SizeI PixelSize => (PixelSizeX, PixelSizeY);

        public SKAlphaType AlphaType
        {
            get
            {
                if(!HasAlpha)
                    return SKAlphaType.Opaque;

                if (App.IsWindowsOS)
                {
                    return SKAlphaType.Premul;
                }
                else
                if (App.IsLinuxOS)
                {
                    return SKAlphaType.Unpremul;
                }
                else
                if (App.IsMacOS)
                {
                    return SKAlphaType.Premul;
                }
                else
                {
                    return SKAlphaType.Premul;
                }
            }
        }

        public Alternet.Drawing.ImageBitsFormatKind BitsFormat
        {
            get
            {
                if(Depth != 24 && Depth != 32)
                    return Alternet.Drawing.ImageBitsFormatKind.Unknown;
                if (HasMask)
                    return Alternet.Drawing.ImageBitsFormatKind.Unknown;
                if (HasAlpha)
                    return Alternet.Drawing.ImageBitsFormatKind.Alpha;
                else
                    return Alternet.Drawing.ImageBitsFormatKind.Native;
            }
        }

        public void Assign(SKBitmap bitmap)
        {
            var image = Alternet.Drawing.GenericImage.FromSkia(bitmap);
            Assign(image);
        }

        public void Assign(Alternet.Drawing.GenericImage image)
        {
            var depth = image.HasAlpha ? 32 : 24;
            LoadFromGenericImage(Alternet.Drawing.WxGenericImageHandler.GetPtr(image), depth);
        }

        /// <inheritdoc/>
        public SKBitmap ToSkia(bool assignPixels = true)
        {
            SKBitmap result = DrawingUtils.CreateSkiaBitmapForImage(Width, Height, HasAlpha);

            if (assignPixels)
            {
                var genericImage = ToGenericImage();
                result.Pixels = genericImage.Pixels;
            }

            if (Immutable)
                result.SetImmutable();
            return result;
        }

        public bool LoadFromStream(Stream stream)
        {
            using var inputStream = new UI.Native.InputStream(stream);
            if (inputStream is null)
                return false;
            return LoadFromStream(inputStream);
        }

        public void SetImmutable()
        {
        }

        public bool LoadFromStream(Stream stream, BitmapType type)
        {
            using var inputStream = new UI.Native.InputStream(stream);
            return LoadStream(inputStream, (int)type);
        }

        public bool SaveToStream(Stream stream, BitmapType type, int quality)
        {
            using var outputStream = new UI.Native.OutputStream(stream);
            return SaveStream(outputStream, (int)type);
        }

        public Alternet.Drawing.GenericImage ToGenericImage()
        {
            return Alternet.Drawing.WxGenericImageHandler.Create(ConvertToGenericImage());
        }

        Alternet.Drawing.IImageHandler Alternet.Drawing.IImageHandler.GetSubBitmap(Alternet.Drawing.RectI rect)
        {
            return GetSubBitmap(rect);
        }

        public static UI.Native.Image CreateImage(Drawing.SizeI size, int depth = 32)
        {
            var result = new UI.Native.Image();
            ((UI.Native.Image)result).Initialize(size, depth);
            return result;
        }

        public class DynamicBitmap : Drawing.DynamicBitmap<Drawing.Image>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DynamicBitmap"/> class.
            /// </summary>
            public DynamicBitmap()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="DynamicBitmap"/> class.
            /// </summary>
            /// <param name="size"></param>
            /// <param name="scaleFactor"></param>
            /// <param name="isTransparent"></param>
            public DynamicBitmap(Drawing.SizeD size, Coord scaleFactor, bool isTransparent)
                : base(size, scaleFactor, isTransparent)
            {
            }

            /// <summary>
            /// Updates the properties of an existing <see cref="DynamicBitmap"/> instance or creates
            /// a new instance if the reference is null.
            /// </summary>
            /// <remarks>If the <paramref name="bitmap"/> parameter is null, a new <see cref="DynamicBitmap"/>
            /// instance is created with the specified properties.
            /// If the <paramref name="bitmap"/> parameter is not null,
            /// its properties are updated to match the specified values.</remarks>
            /// <param name="bitmap">A reference to the <see cref="DynamicBitmap"/> instance to update.
            /// If null, a new instance is created and assigned to this reference.</param>
            /// <param name="size">The dimensions of the bitmap, specified as
            /// a <see cref="Drawing.SizeD"/> structure.</param>
            /// <param name="scaleFactor">The scaling factor to apply to the bitmap, specified
            /// as a <see cref="Coord"/>.</param>
            /// <param name="isTransparent">A value indicating whether the bitmap should support transparency.
            /// <see langword="true"/> if transparency is
            /// enabled; otherwise, <see langword="false"/>.</param>
            public static void CreateOrUpdate(
                [NotNull] ref DynamicBitmap? bitmap,
                Drawing.SizeD size,
                Coord scaleFactor,
                bool isTransparent)
            {
                if (bitmap == null)
                    bitmap = new DynamicBitmap(size, scaleFactor, isTransparent);
                else
                    bitmap.SetDynamicProperties(size, scaleFactor, isTransparent);
            }

            /// <inheritdoc/>
            public override Drawing.Image CreateBitmap()
            {
                var sizeI = SizeInPixels;
                var bmp = CreateImage(sizeI);
                bmp.HasAlpha = IsTransparent;
                bmp.ScaleFactor = ScaleFactor;

                var result = new Drawing.Image(bmp);
                return result;
            }
        }
    }
}