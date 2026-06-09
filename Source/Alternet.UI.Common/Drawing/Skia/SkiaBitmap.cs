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
            InternalLoadFromStream(stream);
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

            var result = InternalLoadFromStream(stream);

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
        /// Initializes a new instance of the <see cref="SkiaBitmap"/> class.
        /// </summary>
        /// <param name="nativeImage">Native image instance.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SkiaBitmap(IImageHandler nativeImage)
            : base(nativeImage)
        {
        }

        protected override IImageHandler CreateHandler()
        {
            return new SkiaImageHandler();
        }
    }
}