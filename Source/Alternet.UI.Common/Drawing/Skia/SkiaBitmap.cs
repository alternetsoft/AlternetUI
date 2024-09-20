using System;
using System.IO;
using System.Runtime.CompilerServices;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Encapsulates a bitmap which can be used with <see cref="SkiaGraphics"/>.
    /// </summary>
    internal class SkiaBitmap : Image
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaBitmap"/> class from
        /// a <see cref="SKBitmap"/>.
        /// </summary>
        /// <param name="bitmap"><see cref="SKBitmap"/> with image data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SkiaBitmap(SKBitmap bitmap)
            : base(new SkiaImageHandler(bitmap))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaBitmap"/> class from a stream.
        /// </summary>
        /// <param name="stream">Stream with bitmap.</param>
        /// <param name="bitmapType">Type of the bitmap.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SkiaBitmap(Stream stream, BitmapType bitmapType = BitmapType.Any)
            : base(new SkiaImageHandler())
        {
            Load(stream, bitmapType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaBitmap"/> class from
        /// the specified data stream.
        /// </summary>
        /// <param name="stream">The data stream used to load the image.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SkiaBitmap(Stream? stream)
            : base(new SkiaImageHandler())
        {
            if (stream is null)
                return;
            Handler.LoadFromStream(stream);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaBitmap"/> class
        /// with the specified size in device pixels.
        /// </summary>
        /// <param name="width">The width used to create the image</param>
        /// <param name="height">The height used to create the image</param>
        /// <param name="depth">Specifies the depth of the bitmap.
        /// Some platforms only support (1) for monochrome and (-1) for the current color setting.
        /// A depth of 32 including an alpha channel is supported under MSW, Mac and Linux.
        /// If this parameter is -1, the display depth of the screen is used.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SkiaBitmap(int width, int height, int depth = 32)
            : this(new SizeI(width, height), depth)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaBitmap"/> class
        /// with the specified size in device pixels.
        /// </summary>
        /// <param name="width">The width used to create the image</param>
        /// <param name="height">The height used to create the image</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SkiaBitmap(Coord width, Coord height)
            : this((int)width, (int)height)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaBitmap"/> class
        /// with the specified size in device pixels.
        /// </summary>
        /// <param name="size">The size in device pixels used to create the image.</param>
        /// <param name="depth">Specifies the depth of the bitmap.
        /// Some platforms only support (1) for monochrome and (-1) for the current color setting.
        /// A depth of 32 including an alpha channel is supported under MSW, Mac and Linux.
        /// If this parameter is -1, the display depth of the screen is used.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SkiaBitmap(SizeI size, int depth = 32)
            : base(new SkiaImageHandler(size, depth))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaBitmap"/> class.
        /// </summary>
        /// <param name="url">Url to the image.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SkiaBitmap(string url)
            : base(new SkiaImageHandler())
        {
            using var stream = ResourceLoader.StreamFromUrl(url);
            if (stream is null)
            {
                App.LogError($"Image not loaded from: {url}");
                return;
            }

            var result = Handler.LoadFromStream(stream);

            if (!result)
            {
                App.LogError($"Image not loaded from: {url}");
                return;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaBitmap"/> class.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SkiaBitmap()
            : this(SizeI.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaBitmap"/> class with the image from
        /// <see cref="ImageSet"/>.
        /// </summary>
        /// <param name="imageSet">Source of the image.</param>
        /// <param name="size">Size of the image in device pixels.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SkiaBitmap(ImageSet imageSet, SizeI size)
            : base(new SkiaImageHandler(imageSet, size))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaBitmap"/> class with the image from
        /// <see cref="ImageSet"/>.
        /// </summary>
        /// <param name="imageSet">Source of the image.</param>
        /// <param name="control">Control used to get dpi.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SkiaBitmap(ImageSet imageSet, IControl control)
            : base(new SkiaImageHandler(imageSet, control))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaBitmap"/> class from the specified
        /// existing image.
        /// </summary>
        /// <param name="original">The <see cref="Image"/> from which to create the
        /// new <see cref="Bitmap"/>.</param>
        /// <remarks>
        /// Full image data is copied from the original image.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SkiaBitmap(Image original)
            : base(new SkiaImageHandler(original))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaBitmap" /> class from the specified
        /// existing image, scaled to the specified size.
        /// </summary>
        /// <param name="original">The <see cref="Image" /> from which to create the new image.</param>
        /// <param name="newSize">The <see cref="SizeI" /> structure that represent the
        /// size of the new image.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SkiaBitmap(Image original, SizeI newSize)
            : base(new SkiaImageHandler(original, newSize))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaBitmap"/> class from
        /// the specified <see cref="GenericImage"/>.
        /// </summary>
        /// <param name="genericImage">Generic image.</param>
        /// <param name="depth">Specifies the depth of the bitmap.
        /// Some platforms only support (1) for monochrome and (-1) for the current color setting.
        /// A depth of 32 including an alpha channel is supported under MSW, Mac and Linux.
        /// If this parameter is -1, the display depth of the screen is used.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SkiaBitmap(GenericImage genericImage, int depth = 32)
            : base(new SkiaImageHandler(genericImage, depth))
        {
        }

        /// <summary>
        /// Creates a bitmap compatible with the given <see cref="Graphics"/>, inheriting
        /// its magnification factor.
        /// </summary>
        /// <param name="width">The width of the bitmap in pixels, must be strictly positive.</param>
        /// <param name="height">The height of the bitmap in pixels, must be strictly positive.</param>
        /// <param name="dc"><see cref="Graphics"/> from which the scaling factor is inherited.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SkiaBitmap(int width, int height, Graphics dc)
            : base(new SkiaImageHandler(width, height, dc))
        {
        }

        /// <summary>
        /// Creates a bitmap compatible with the given <see cref="Graphics"/> from
        /// the given <see cref="GenericImage"/>.
        /// </summary>
        /// <param name="genericImage">Platform-independent image object.</param>
        /// <param name="dc"><see cref="Graphics"/> from which the scaling
        /// factor is inherited.</param>
        /// <remarks>
        /// This constructor initializes the bitmap with the data of the given image, which
        /// must be valid, but inherits the scaling factor from the given device context
        /// instead of simply using the default factor of 1.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SkiaBitmap(GenericImage genericImage, Graphics dc)
            : base(new SkiaImageHandler(genericImage, dc))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaBitmap"/> class with the specified size
        /// amd scaling factor from the <paramref name="control"/>.
        /// </summary>
        /// <param name="size">The size, in device pixels, of the new <see cref="Bitmap"/>.</param>
        /// <param name="control">The control from which pixel scaling factor is used.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SkiaBitmap(SizeI size, IControl control)
            : this(size)
        {
            ScaleFactor = control.ScaleFactor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaBitmap"/> class.
        /// </summary>
        /// <param name="nativeImage">Native image instance.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SkiaBitmap(IImageHandler nativeImage)
            : base(nativeImage)
        {
        }

        /// <summary>
        /// Converts the specified <see cref='GenericImage'/> to a <see cref='Bitmap'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator SkiaBitmap(GenericImage image) => new(image);

        protected override IImageHandler CreateHandler()
        {
            return new SkiaImageHandler();
        }
    }
}