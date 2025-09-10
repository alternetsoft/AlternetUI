using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements platform independent image.
    /// </summary>
    public class GenericImage : HandledObject<IGenericImageHandler>
    {
        /// <summary>
        /// Constant used to indicate the alpha value conventionally defined as the complete
        /// transparency.
        /// </summary>
        public const byte AlphaChannelTransparent = 0;

        /// <summary>
        /// Constant used for default threshold separating transparent pixels from opaque for a
        /// few functions dealing with alpha and fully opaque.
        /// </summary>
        public const byte AlphaChannelThreshold = 0x80;

        /// <summary>
        /// Constant used to indicate the alpha value conventionally defined as the complete opacity.
        /// </summary>
        public const byte AlphaChannelOpaque = 0xff;

        /// <summary>
        /// Gets an empty generic image.
        /// </summary>
        public static readonly GenericImage Empty = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an empty image without an alpha channel.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image with the given size and clears it if requested.
        /// Does not create an alpha channel.
        /// </summary>
        /// <param name="width">Specifies the width of the image.</param>
        /// <param name="height">Specifies the height of the image.</param>
        /// <param name="clear">If true, initialize the image to black.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage(int width, int height, bool clear = false)
        {
            Handler = GraphicsFactory.Handler.CreateGenericImageHandler(width, height, clear);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image with the given size and clears it if requested.
        /// Does not create an alpha channel.
        /// </summary>
        /// <param name="size">Specifies the size of the image.</param>
        /// <param name="clear">If true, initialize the image to black.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage(SizeI size, bool clear = false)
        {
            Handler = GraphicsFactory.Handler.CreateGenericImageHandler(
                size.Width,
                size.Height,
                clear);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image from a file.
        /// </summary>
        /// <param name="url">Path or url to file with image data.</param>
        /// <param name="bitmapType">Type of the bitmap. Depending on how library
        /// and OS has been configured and
        /// by which handlers have been loaded, not all formats may be available. If value is
        /// <see cref="BitmapType.Any"/>, function will try to autodetect the format.</param>
        /// <param name="index">Index of the image to load in the case that the image
        /// file contains multiple images.
        /// This is only used by GIF, ICO and TIFF handlers.The default value(-1) means
        /// "choose the default image" and is interpreted as the first image(index= 0) by the GIF and
        /// TIFF handler and as the largest and most colorful one by the ICO handler.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage(string? url, BitmapType bitmapType = BitmapType.Any, int index = -1)
        {
            if (string.IsNullOrEmpty(url))
                return;

            using var stream = ResourceLoader.StreamFromUrlOrDefault(url);
            if (stream is null)
            {
                App.LogError($"GenericImage not loaded from: {url}");
                return;
            }

            Handler = GraphicsFactory.Handler.CreateGenericImageHandler(
                    stream,
                    bitmapType,
                    index);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image from a file using MIME-types to specify the type.
        /// </summary>
        /// <param name="url">Path or url to file with image data.</param>
        /// <param name="mimeType">MIME type string (for example 'image/jpeg').</param>
        /// <param name="index">See description in
        /// <see cref="GenericImage(string, BitmapType, int)"/></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage(string? url, string mimeType, int index = -1)
        {
            if (string.IsNullOrEmpty(url))
                return;

            using var stream = ResourceLoader.StreamFromUrlOrDefault(url!);
            if (stream is null)
            {
                App.LogError($"GenericImage not loaded from: {url}");
                return;
            }

            Handler = GraphicsFactory.Handler.CreateGenericImageHandler(
                    stream,
                    mimeType,
                    index);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image from a stream.
        /// </summary>
        /// <param name="stream">Opened input stream from which to load the image.
        /// Currently, the stream must support seeking.</param>
        /// <param name="bitmapType">See description in
        /// <see cref="GenericImage(string, BitmapType, int)"/>.</param>
        /// <param name="index">See description in
        /// <see cref="GenericImage(string, BitmapType, int)"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage(Stream stream, BitmapType bitmapType = BitmapType.Any, int index = -1)
        {
            Handler = GraphicsFactory.Handler.CreateGenericImageHandler(
                      stream,
                      bitmapType,
                      index);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image from a stream.
        /// </summary>
        /// <param name="stream">Opened input stream from which to load the image.
        /// Currently, the stream must support seeking.</param>
        /// <param name="mimeType">MIME type string (for example 'image/jpeg').</param>
        /// <param name="index">See description in
        /// <see cref="GenericImage(string, BitmapType, int)"/></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage(Stream stream, string mimeType, int index = -1)
        {
            Handler = GraphicsFactory.Handler.CreateGenericImageHandler(
                      stream,
                      mimeType,
                      index);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image from data in memory.
        /// </summary>
        /// <param name="width">Specifies the width of the image.</param>
        /// <param name="height">Specifies the height of the image.</param>
        /// <param name="data">Image data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage(int width, int height, SKColor[] data)
        {
            Handler = GraphicsFactory.Handler.CreateGenericImageHandler(width, height, data);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image from data in memory.
        /// </summary>
        /// <param name="width">Specifies the width of the image.</param>
        /// <param name="height">Specifies the height of the image.</param>
        /// <param name="data">RGB data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage(int width, int height, RGBValue[] data)
        {
            Handler = GraphicsFactory.Handler.CreateGenericImageHandler(width, height, data);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image from data in memory.
        /// </summary>
        /// <param name="width">Specifies the width of the image.</param>
        /// <param name="height">Specifies the height of the image.</param>
        /// <param name="data">RGB data.</param>
        /// <param name="alpha">Alpha-channel data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage(int width, int height, RGBValue[] data, byte[] alpha)
        {
            Handler = GraphicsFactory.Handler.CreateGenericImageHandler(
                width,
                height,
                data,
                alpha);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage(IGenericImageHandler handle)
        {
            Handler = handle;
        }

        /// <summary>
        /// Enumerates known pixel strategies used in the generic images.
        /// </summary>
        public enum PixelStrategy
        {
            /// <summary>
            /// Access to <see cref="GenericImage.Pixels"/> is faster.
            /// </summary>
            Pixels,

            /// <summary>
            /// Access to <see cref="GenericImage.RgbData"/> is faster.
            /// </summary>
            RgbData,
        }

        /// <summary>
        /// Returns <c>true</c> if this image has alpha channel, <c>false</c> otherwise.
        /// </summary>
        /// <returns></returns>
        public virtual bool HasAlpha
        {
            get
            {
                return Handler.HasAlpha;
            }

            set
            {
                if (HasAlpha == value)
                    return;
                if (value)
                    InitAlpha();
                else
                    ClearAlpha();
            }
        }

        /// <summary>
        /// Returns <c>true</c> if there is a mask active, <c>false</c> otherwise.
        /// </summary>
        /// <returns></returns>
        [Browsable(false)]
        public virtual bool HasMask
        {
            get
            {
                return Handler.HasMask;
            }
        }

        /// <summary>
        /// Gets the width of the image in pixels.
        /// </summary>
        public virtual int Width
        {
            get => Handler.Width;
        }

        /// <summary>
        /// Gets the height of the image in pixels.
        /// </summary>
        public virtual int Height
        {
            get => Handler.Height;
        }

        /// <summary>
        /// Returns the size of the image in pixels.
        /// </summary>
        public virtual SizeI Size
        {
            get => new(Width, Height);
        }

        /// <summary>
        /// Returns the bounds of the image in pixels. Result is (0, 0, Width, Height).
        /// </summary>
        public virtual RectI Bounds
        {
            get => (0, 0, Width, Height);
        }

        /// <summary>
        /// Gets this <see cref="GenericImage"/> as <see cref="Image"/>.
        /// </summary>
        [Browsable(false)]
        public virtual Image AsImage
        {
            get
            {
                return new Bitmap(this);
            }
        }

        /// <summary>
        /// Gets number of pixels in this image (Width * Height).
        /// </summary>
        public virtual int PixelCount => Size.PixelCount;

        /// <summary>
        /// Gets or sets pixels using array of <see cref="SKColor"/>.
        /// </summary>
        /// <remarks>
        /// Use <see cref="PixelStrategy"/> to get the best strategy for accessing the pixel data.
        /// </remarks>
        public virtual SKColor[] Pixels
        {
            get => Handler.Pixels;
            set => Handler.Pixels = value;
        }

        /// <summary>
        /// Gets or sets pixels using array of <see cref="RGBValue"/>.
        /// </summary>
        /// <remarks>
        /// Use <see cref="AlphaData"/> to get alpha component of the pixels.
        /// Use <see cref="PixelStrategy"/> to get the best strategy for accessing the pixel data.
        /// </remarks>
        public virtual RGBValue[] RgbData
        {
            get => Handler.RgbData;
            set => Handler.RgbData = value;
        }

        /// <summary>
        /// Gets or sets alpha component of the pixels using array of <see cref="byte"/>.
        /// </summary>
        /// <remarks>
        /// Use <see cref="RgbData"/> to get alpha component of the pixels.
        /// Use <see cref="PixelStrategy"/> to get the best strategy for accessing the pixel data.
        /// </remarks>
        public virtual byte[] AlphaData
        {
            get => Handler.AlphaData;
            set => Handler.AlphaData = value;
        }

        /// <summary>
        /// Returns <c>true</c> if image data is present.
        /// </summary>
        public virtual bool IsOk
        {
            get => Handler.IsOk;
        }

        /// <summary>
        /// Gets best strategy to access pixels.
        /// </summary>
        public PixelStrategy BestStrategy => Handler.BestStrategy;

        /// <summary>
        /// Converts the specified <see cref='SKBitmap'/> to a <see cref='GenericImage'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator GenericImage(SKBitmap bitmap)
        {
            return FromSkia(bitmap);
        }

        /// <summary>
        /// Converts the specified <see cref='SKBitmap'/> to a <see cref='GenericImage'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator SKBitmap(GenericImage bitmap)
        {
            return ToSkia(bitmap);
        }

        /// <summary>
        /// Returns <c>true</c> if at least one of the available image handlers can read the file
        /// with the given name.
        /// </summary>
        /// <param name="url">Path or url to file with image data.</param>
        /// <returns></returns>
        public static bool CanRead(string? url)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            using var stream = ResourceLoader.StreamFromUrlOrDefault(url);
            if (stream is null)
            {
                return false;
            }

            return CanRead(stream);
        }

        /// <summary>
        /// Fills array of <see cref="SKColor"/> with the specified color.
        /// </summary>
        /// <param name="pixels">Array of pixels.</param>
        /// <param name="fill">Color to fill.</param>
        public static unsafe void FillPixels(SKColor[] pixels, SKColor fill)
        {
            var length = pixels.Length;

            fixed (SKColor* rgbPtr = pixels)
            {
                var ptr = rgbPtr;

                for (int i = 0; i < length; i++)
                {
                    *ptr = fill;
                    ptr++;
                }
            }
        }

        /// <summary>
        /// Fills array with alpha component data with the specified alpha value.
        /// </summary>
        /// <param name="alpha">Array of alpha components.</param>
        /// <param name="fill">Value to fill.</param>
        public static unsafe void FillAlphaData(byte[] alpha, byte fill)
        {
            fixed (byte* alphaPtr = alpha)
            {
                BaseMemory.Fill((IntPtr)alphaPtr, fill, alpha.Length);
            }
        }

        /// <summary>
        /// Fills array with <see cref="RGBValue"/> with the specified color.
        /// </summary>
        /// <param name="rgb">Array of pixels.</param>
        /// <param name="fill">Color to fill.</param>
        public static unsafe void FillRgbData(RGBValue[] rgb, RGBValue fill)
        {
            var length = rgb.Length;

            fixed (RGBValue* rgbPtr = rgb)
            {
                var ptr = rgbPtr;

                for (int i = 0; i < length; i++)
                {
                    *ptr = fill;
                    ptr++;
                }
            }
        }

        /// <summary>
        /// Creates <see cref="GenericImage"/> with the specified size and
        /// fills it with the color.
        /// </summary>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="color">Color to fill.</param>
        /// <returns></returns>
        public static GenericImage Create(int width, int height, Color color)
        {
            var alphaData = CreateAlphaData(width, height, color.A);
            var rgbData = CreateRgbData(width, height, color);
            GenericImage image = new(width, height, rgbData, alphaData);
            return image;
        }

        /// <summary>
        /// Creates array of <see cref="SKColor"/> with the specified size and optionally
        /// fills it with the color.
        /// </summary>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="fill">Color to fill.</param>
        /// <returns></returns>
        public static SKColor[] CreatePixels(int width, int height, SKColor? fill = null)
        {
            var size = width * height;
            SKColor[] result = new SKColor[size];
            if (fill is null)
                return result;
            FillPixels(result, fill.Value);
            return result;
        }

        /// <summary>
        /// Creates array of <see cref="RGBValue"/> with the specified size and optionally
        /// fills it with the color.
        /// </summary>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="fill">Color to fill.</param>
        /// <returns></returns>
        public static RGBValue[] CreateRgbData(int width, int height, RGBValue? fill = null)
        {
            var size = width * height;
            RGBValue[] result = new RGBValue[size];
            if (fill is null)
                return result;
            FillRgbData(result, fill.Value);
            return result;
        }

        /// <summary>
        /// Creates array of <see cref="RGBValue"/> with the specified size and copies
        /// pixel data from the <paramref name="source"/> pointer.
        /// </summary>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="source">Pointer to the pixel data (array of <see cref="RGBValue"/>).</param>
        /// <returns></returns>
        public static unsafe RGBValue[] CreateRgbDataFromPtr(int width, int height, IntPtr source)
        {
            var size = width * height;
            RGBValue[] result = new RGBValue[size];

            if(source != default)
            {
                fixed (RGBValue* resultPtr = result)
                {
                    BaseMemory.Move((IntPtr)resultPtr, source, size * 3);
                }
            }

            return result;
        }

        /// <summary>
        /// Creates array of alpha components with the specified size and copies
        /// alpha component data from the <paramref name="source"/> pointer.
        /// </summary>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="source">Pointer to the alpha component data (array of bytes).</param>
        /// <returns></returns>
        public static unsafe byte[] CreateAlphaDataFromPtr(int width, int height, IntPtr source)
        {
            var size = width * height;
            byte[] result = new byte[size];

            if (source != default)
            {
                fixed (byte* resultPtr = result)
                {
                    BaseMemory.Move((IntPtr)resultPtr, source, size);
                }
            }

            return result;
        }

        /// <summary>
        /// Creates array of alpha components with the specified size and optionally fills
        /// it with the given value.
        /// </summary>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="fill">Value to fill.</param>
        /// <returns></returns>
        public static byte[] CreateAlphaData(int width, int height, byte? fill = null)
        {
            var size = width * height;
            byte[] result = new byte[size];
            if (fill is null)
                return result;
            FillAlphaData(result, fill.Value);
            return result;
        }

        /// <summary>
        /// Converts array of <see cref="SKColor"/> to array of <see cref="RGBValue"/>.
        /// </summary>
        /// <param name="data">Array with pixel data.</param>
        /// <returns></returns>
        public static unsafe RGBValue[] GetRGBValues(SKColor[] data)
        {
            var length = data.Length;

            var result = new RGBValue[length];

            fixed (SKColor* dataPtr = data)
            {
                var ptr = dataPtr;

                fixed (RGBValue* resultPtr = result)
                {
                    var aPtr = resultPtr;

                    for (int i = 0; i < length; i++)
                    {
                        *aPtr = *ptr;
                        ptr++;
                        aPtr++;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Copies pixel data from <paramref name="source"/> to <paramref name="data"/>.
        /// </summary>
        /// <param name="data">Destination array of <see cref="SKColor"/>.</param>
        /// <param name="source">Pointer to pixel data.</param>
        public static unsafe void SetRgbValuesFromPtr(SKColor[] data, RGBValue* source)
        {
            var length = data.Length;

            fixed (SKColor* dataPtr = data)
            {
                var ptr = dataPtr;

                for (int i = 0; i < length; i++)
                {
                    byte alpha = (*ptr).Alpha;
                    var rgb = *source;
                    *ptr = new SKColor(rgb.R, rgb.G, rgb.B, alpha);
                    ptr++;
                    source++;
                }
            }
        }

        /// <summary>
        /// Fills alpha component of the colors with the specified value.
        /// </summary>
        /// <param name="data">Array of pixels.</param>
        /// <param name="alpha">Value to fill.</param>
        public static unsafe void FillAlphaData(SKColor[] data, byte alpha)
        {
            var length = data.Length;

            fixed (SKColor* dataPtr = data)
            {
                var ptr = dataPtr;

                for (int i = 0; i < length; i++)
                {
                    SKColor color = *ptr;
                    *ptr = new SKColor(color.Red, color.Green, color.Blue, alpha);
                    ptr++;
                }
            }
        }

        /// <summary>
        /// Copies alpha components from <paramref name="source"/> to <paramref name="data"/>.
        /// </summary>
        /// <param name="data">Destination array of <see cref="SKColor"/>.</param>
        /// <param name="source">Pointer to alpha components.</param>
        public static unsafe void SetAlphaValuesFromPtr(SKColor[] data, byte* source)
        {
            var length = data.Length;

            fixed (SKColor* dataPtr = data)
            {
                var ptr = dataPtr;

                for (int i = 0; i < length; i++)
                {
                    SKColor color = (*ptr).WithAlpha(*source);
                    *ptr = color;
                    ptr++;
                    source++;
                }
            }
        }

        /// <summary>
        /// Creates array of alpha components from the array of <see cref="SKColor"/>.
        /// </summary>
        /// <param name="data">Array of pixel data.</param>
        /// <returns></returns>
        public static unsafe byte[] GetAlphaValues(SKColor[] data)
        {
            var length = data.Length;

            var alpha = new byte[length];

            fixed (SKColor* dataPtr = data)
            {
                var ptr = dataPtr;

                fixed (byte* alphaPtr = alpha)
                {
                    var aPtr = alphaPtr;

                    for (int i = 0; i < length; i++)
                    {
                        *aPtr = (*ptr).Alpha;
                        ptr++;
                        aPtr++;
                    }
                }
            }

            return alpha;
        }

        /// <summary>
        /// Converts array of <see cref="SKColor"/> into array of <see cref="RGBValue"/>
        /// and array of alpha components.
        /// </summary>
        /// <param name="data">Source array with pixel data.</param>
        /// <param name="rgb">Destination array of <see cref="RGBValue"/>.</param>
        /// <param name="alpha">Destination array of <see cref="byte"/> with alpha components.</param>
        public static void SeparateAlphaData(
            SKColor[] data,
            out RGBValue[] rgb,
            out byte[] alpha)
        {
            rgb = GetRGBValues(data);
            alpha = GetAlphaValues(data);
        }

        /// <summary>
        /// Converts <see cref="GenericImage"/> to <see cref="SKBitmap"/>. Optionally
        /// copies pixel data.
        /// </summary>
        /// <param name="bitmap">Image.</param>
        /// <param name="assignPixels">Whether to copy pixel data. Optional. Default is <c>true</c>.</param>
        /// <returns></returns>
        public static SKBitmap ToSkia(GenericImage bitmap, bool assignPixels = true)
        {
            return ToSkia(bitmap.Handler, assignPixels);
        }

        /// <summary>
        /// Creates <see cref="SKBitmap"/> with the specifies size.
        /// </summary>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="hasAlpha">Whether image has alpha component.</param>
        /// <returns></returns>
        public static SKBitmap CreateSkiaBitmapForImage(int width, int height, bool hasAlpha)
        {
            var count = width * height;
            if (count == 0)
                return new SKBitmap();

            var result = new SKBitmap(width, height, isOpaque: !hasAlpha);

            return result;
        }

        /// <summary>
        /// Converts image specified with <see cref="IGenericImageHandler"/> to
        /// <see cref="SKBitmap"/>. Optionally copies pixel data.
        /// </summary>
        /// <param name="bitmap">Image.</param>
        /// <param name="assignPixels">Whether to copy pixel data. Optional. Default is <c>true</c>.</param>
        /// <returns></returns>
        public static SKBitmap ToSkia(IGenericImageHandler bitmap, bool assignPixels = true)
        {
            var result = CreateSkiaBitmapForImage(bitmap.Width, bitmap.Height, bitmap.HasAlpha);

            if (assignPixels)
                result.Pixels = bitmap.Pixels;
            return result;
        }

        /// <summary>
        /// Converts <see cref="SKBitmap"/> to <see cref="GenericImage"/>.
        /// </summary>
        /// <param name="bitmap">Image to convert.</param>
        /// <returns></returns>
        public static GenericImage FromSkia(SKBitmap bitmap)
        {
            var result = new GenericImage(bitmap.Width, bitmap.Height, bitmap.Pixels);
            return result;
        }

        /// <summary>
        /// Returns <c>true</c> if at least one of the available image handlers can read the data in
        /// the given stream.
        /// </summary>
        /// <param name="stream">Opened input stream from which to load the image. Currently,
        /// the stream must support seeking.</param>
        /// <returns></returns>
        /// <remarks>
        /// This function doesn't modify the current stream position
        /// (because it restores the original position before returning; this however requires
        /// the stream to be seekable).
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanRead(Stream stream)
        {
            return GraphicsFactory.Handler.CanReadGenericImage(stream);
        }

        /// <summary>
        /// Iterates all registered image handlers, and returns a string containing
        /// file extension masks suitable for passing to file open/save dialog boxes.
        /// </summary>
        /// <returns>
        /// The format of the returned string is "(*.ext1;*.ext2)|*.ext1;*.ext2". It is usually
        /// a good idea to prepend a description before passing the result to the dialog.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetImageExtWildcard()
        {
            return GraphicsFactory.Handler.GetGenericImageExtWildcard();
        }

        /// <summary>
        /// If the image file contains more than one image and the image handler is capable of
        /// retrieving these individually, this function will return the number of available images.
        /// </summary>
        /// <param name="url">Path or url to file with image data.</param>
        /// <param name="bitmapType">Type of the bitmap. Depending on how library and OS has
        /// been configured and
        /// by which handlers have been loaded, not all formats may be available. If value is
        /// <see cref="BitmapType.Any"/>, function will try to autodetect the format.</param>
        /// <returns>Number of available images. For most image handlers, this is 1
        /// (exceptions are TIFF and ICO formats as well as animated GIFs for which this function
        /// returns the number of frames in the animation).</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetImageCount(
            string? url,
            BitmapType bitmapType = BitmapType.Any)
        {
            if (string.IsNullOrEmpty(url))
                return 0;

            using var stream = ResourceLoader.StreamFromUrlOrDefault(url);
            if (stream is null)
            {
                return 0;
            }

            return GetImageCount(stream, bitmapType);
        }

        /// <summary>
        /// If the image stream contains more than one image and the image handler is capable of
        /// retrieving these individually, this function will return the number of available images.
        /// </summary>
        /// <param name="stream">Opened input stream from which to load the image. Currently,
        /// the stream must support seeking.</param>
        /// <param name="bitmapType">Type of the bitmap. Depending on how library and OS has
        /// been configured and
        /// by which handlers have been loaded, not all formats may be available. If value is
        /// <see cref="BitmapType.Any"/>, function will try to autodetect the format.</param>
        /// <returns>Number of available images. For most image handlers, this is 1
        /// (exceptions are TIFF and ICO formats as well as animated GIFs for which this function
        /// returns the number of frames in the animation).</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetImageCount(
            Stream stream,
            BitmapType bitmapType = BitmapType.Any)
        {
            return GraphicsFactory.Handler.GetGenericImageCount(stream, bitmapType);
        }

        /// <summary>
        /// Returns the currently used default file load flags.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenericImageLoadFlags GetDefaultLoadFlags()
        {
            return GraphicsFactory.Handler.GenericImageDefaultLoadFlags;
        }

        /// <summary>
        /// Sets the default value for the flags used for loading image files.
        /// </summary>
        /// <param name="flags"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetDefaultLoadFlags(GenericImageLoadFlags flags)
        {
            GraphicsFactory.Handler.GenericImageDefaultLoadFlags = flags;
        }

        /// <summary>
        /// Sets the alpha value for the given pixel.
        /// </summary>
        /// <param name="x">X coordinate of the pixel.</param>
        /// <param name="y">Y coordinate of the pixel.</param>
        /// <param name="alpha">New alpha channel (transparency) value of the pixels.</param>
        /// <remarks>
        /// This function should only be called if the image has alpha channel data,
        /// use <see cref="HasAlpha"/> to check for this.
        /// </remarks>
        public virtual void SetAlpha(int x, int y, byte alpha)
        {
            Handler.SetAlpha(x, y, alpha);
        }

        /// <summary>
        /// Removes the alpha channel from the image.
        /// </summary>
        /// <remarks>
        /// This function should only be called if the image has alpha channel data,
        /// use <see cref="HasAlpha"/> to check for this.
        /// </remarks>
        public virtual void ClearAlpha()
        {
            if (HasAlpha)
                Handler.ClearAlpha();
        }

        /// <summary>
        /// Specifies whether there is a mask or not.
        /// </summary>
        /// <param name="hasMask"></param>
        public virtual void SetMask(bool hasMask = true)
        {
            Handler.SetMask(hasMask);
        }

        /// <summary>
        /// Sets the mask color for this image(and tells the image to use the mask).
        /// </summary>
        /// <param name="rgb">Color RGB.</param>
        public virtual void SetMaskColor(RGBValue rgb)
        {
            Handler.SetMaskColor(rgb);
        }

        /// <summary>
        /// Converts all pixels using the specified function.
        /// </summary>
        /// <param name="func">Function used to convert color of the pixel.</param>
        /// <returns></returns>
        public virtual bool ConvertColors(Func<ColorStruct, ColorStruct> func)
        {
            void ChangePixel(ref SKColor rgb, int value)
            {
                rgb = func(rgb);
            }

            return ForEachPixel<int>(ChangePixel, 0);
        }

        /// <summary>
        /// Makes pixels of the image lighter
        /// (this method makes 2x lighter than <see cref="LightColors"/>).
        /// </summary>
        /// <returns></returns>
        public virtual bool LightLightColors()
        {
            return ConvertColors(ControlPaint.LightLight);
        }

        /// <summary>
        /// Makes pixels of the image lighter.
        /// </summary>
        /// <returns></returns>
        public virtual bool LightColors()
        {
            return ConvertColors(ControlPaint.Light);
        }

        /// <summary>
        /// Makes this image grayscale.
        /// </summary>
        public virtual bool ChangeToGrayScale()
        {
            static void ChangePixel(ref RGBValue rgb, int value)
            {
                byte color = (byte)((0.299 * rgb.R) + (0.587 * rgb.G) + (0.114 * rgb.B));
                rgb.R = rgb.G = rgb.B = color;
            }

            return ForEachPixel<int>(ChangePixel, 0);
        }

        /// <summary>
        /// Sets the alpha value for the each pixel.
        /// </summary>
        /// <param name="value">New alpha channel (transparency) value of the pixels.</param>
        /// <remarks>
        /// This function should only be called if the image has alpha channel data,
        /// use <see cref="HasAlpha"/> to check for this.
        /// </remarks>
        public virtual unsafe bool SetAlpha(byte value)
        {
            if (!IsOk)
                return false;
            if (!HasAlpha)
            {
                InitAlpha();
                AlphaData = CreateAlphaData(Width, Height, value);
                return true;
            }

            var alpha = AlphaData;
            var count = alpha.Length;

            fixed (byte* ptr = alpha)
            {
                var p = ptr;

                for (int i = 0; i < count; i++)
                {
                    *p++ = value;
                }
            }

            AlphaData = alpha;
            return true;
        }

        /// <summary>
        /// Sets image's mask so that the pixels that have RGB value of mr,mg,mb in
        /// mask will be masked in the image.
        /// </summary>
        /// <param name="image">Mask image to extract mask shape from. It must have the
        /// same dimensions as the image.</param>
        /// <param name="mask">Mask RGB color.</param>
        /// <returns>Returns <c>false</c> if mask does not have same dimensions as the image
        /// or if there is no unused color left. Returns <c>true</c> if the mask was
        /// successfully applied.</returns>
        /// <remarks>
        /// This is done by first finding an unused color in the image, setting this color
        /// as the mask color and then using this color to draw all pixels in the image
        /// who corresponding pixel in mask has given RGB value.
        /// </remarks>
        /// <remarks>
        /// Note that this method involves computing the histogram, which
        /// is a computationally intensive operation.
        /// </remarks>
        public virtual bool SetMaskFromImage(GenericImage image, RGBValue mask)
        {
            return Handler.SetMaskFromImage(image, mask);
        }

        /// <summary>
        /// Sets a user-defined option.
        /// </summary>
        /// <remarks>
        /// See <see cref="GenericImageOptionNames"/> for known option names.
        /// </remarks>
        /// <param name="name">The name of the option, case-insensitive.</param>
        /// <param name="value">New option value.</param>
        public virtual void SetOptionAsString(string name, string value)
        {
            Handler.SetOptionAsString(name, value);
        }

        /// <summary>
        /// Sets a user-defined option.
        /// </summary>
        /// <remarks>
        /// See <see cref="GenericImageOptionNames"/> for known option names.
        /// </remarks>
        /// <param name="name">The name of the option, case-insensitive.</param>
        /// <param name="value">New option value.</param>
        public virtual void SetOptionAsInt(string name, int value)
        {
            Handler.SetOptionAsInt(name, value);
        }

        /// <summary>
        /// Sets the color of the pixel at the given x and y coordinate.
        /// </summary>
        /// <param name="x">X coordinate of the pixel.</param>
        /// <param name="y">Y coordinate of the pixel.</param>
        /// <param name="rgb">RGB Color.</param>
        /// <remarks>
        /// This routine performs bounds-checks for the coordinate so it can be
        /// considered a safe way to manipulate the data.
        /// </remarks>
        public virtual void SetRGB(int x, int y, RGBValue rgb)
        {
            Handler.SetRGB(x, y, rgb);
        }

        /// <summary>
        /// Sets the color of the pixels within the given rectangle.
        /// </summary>
        /// <param name="rect">Rectangle within the image. If rectangle is null,
        /// <see cref="Bounds"/> property is used.</param>
        /// <param name="rgb">RGB Color.</param>
        public virtual void SetRGBRect(RGBValue rgb, RectI? rect = null)
        {
            Handler.SetRGBRect(rgb, rect);
        }

        /// <summary>
        /// Sets the type of image returned by GetType().
        /// </summary>
        /// <param name="type">Type of the bitmap.</param>
        public virtual void SetImageType(BitmapType type)
        {
            Handler.SetImageType(type);
        }

        /// <summary>
        /// Returns an identical copy of this image.
        /// </summary>
        /// <returns></returns>
        public virtual GenericImage Copy()
        {
            return Handler.Copy();
        }

        /// <summary>
        /// Creates a fresh image.
        /// </summary>
        /// <param name="width">New image width.</param>
        /// <param name="height">New image height</param>
        /// <param name="clear">If true, initialize the image to black.</param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        public virtual bool Reset(int width, int height, bool clear = false)
        {
            return Handler.Reset(width, height, clear);
        }

        /// <summary>
        /// Initialize the image data with zeroes (the default) or with the byte value given as value.
        /// </summary>
        /// <param name="value"></param>
        public virtual void Clear(byte value = 0)
        {
            Handler.Clear(value);
        }

        /// <summary>
        /// Destroys the image data.
        /// </summary>
        public virtual void Reset()
        {
            Handler.Reset();
        }

        /// <summary>
        /// Finds the first color that is never used in the image.
        /// </summary>
        /// <param name="startRGB">Defines the initial value of the color.
        /// If its empty, (1, 0, 0) is used.</param>
        /// <returns>Returns <c>false</c> if there is no unused color left, <c>true</c>
        /// on success.</returns>
        /// <remarks>
        /// The search begins at given initial color and continues by increasing R, G and B
        /// components (in this order) by 1 until an unused color is found or the color space
        /// exhausted.
        /// </remarks>
        /// <remarks>
        /// The parameters startR, startG, startB define the initial values of the color.
        /// The returned
        /// color will have RGB values equal to or greater than these.
        /// </remarks>
        /// <remarks>
        /// This method involves computing the histogram, which is a computationally
        /// intensive operation.
        /// </remarks>
        public virtual Color FindFirstUnusedColor(RGBValue? startRGB = null)
        {
            return Handler.FindFirstUnusedColor(startRGB);
        }

        /// <summary>
        /// Initializes the image alpha channel data.
        /// </summary>
        /// <remarks>
        /// It is an error to call it if the image already has alpha data. If it doesn't,
        /// alpha data will be by default initialized to all pixels being fully opaque.
        /// But if the image has a mask color, all mask pixels will be completely transparent.
        /// </remarks>
        public virtual void InitAlpha()
        {
            if (HasAlpha)
                return;
            Handler.InitAlpha();
        }

        /// <summary>
        /// Blurs the image in both horizontal and vertical directions by the specified
        /// <paramref name="blurRadius"/> (in pixels).
        /// </summary>
        /// <param name="blurRadius">Blur radius in pixels.</param>
        /// <returns>Blurred image.</returns>
        /// <remarks>
        /// This should not be used when using a single mask color for transparency.
        /// </remarks>
        public virtual GenericImage Blur(int blurRadius)
        {
            return Handler.Blur(blurRadius);
        }

        /// <summary>
        /// Blurs the image in the horizontal direction only.
        /// </summary>
        /// <param name="blurRadius">Blur radius in pixels.</param>
        /// <returns>Blurred image.</returns>
        /// <remarks>
        /// This should not be used when using a single mask color for transparency.
        /// </remarks>
        public virtual GenericImage BlurHorizontal(int blurRadius)
        {
            return Handler.BlurHorizontal(blurRadius);
        }

        /// <summary>
        /// Blurs the image in the vertical direction only.
        /// </summary>
        /// <param name="blurRadius">Blur radius in pixels.</param>
        /// <returns>Blurred image.</returns>
        /// <remarks>
        /// This should not be used when using a single mask color for transparency.
        /// </remarks>
        public virtual GenericImage BlurVertical(int blurRadius)
        {
            return Handler.BlurVertical(blurRadius);
        }

        /// <summary>
        /// Returns a mirrored copy of the image.
        /// </summary>
        /// <param name="horizontally"></param>
        /// <returns>Mirrored copy of the image</returns>
        public virtual GenericImage Mirror(bool horizontally = true)
        {
            return Handler.Mirror(horizontally);
        }

        /// <summary>
        /// Copy the data of the given image to the specified position in this image.
        /// </summary>
        /// <param name="image">The image containing the data to copy, must be valid.</param>
        /// <param name="x">The horizontal position of the position to copy the data to.</param>
        /// <param name="y">The vertical position of the position to copy the data to.</param>
        /// <param name="alphaBlend">This parameter determines whether the alpha values of
        /// the original
        /// image replace(default) or are composed with the alpha channel of this image.
        /// Notice that alpha blending overrides the mask handling.</param>
        /// <remarks>
        /// Takes care of the mask color and out of bounds problems.
        /// </remarks>
        public virtual void Paste(
            GenericImage image,
            int x,
            int y,
            GenericImageAlphaBlendMode alphaBlend = GenericImageAlphaBlendMode.Overwrite)
        {
            Handler.Paste(image, x, y, alphaBlend);
        }

        /// <summary>
        /// Replaces the color specified by (r1.R, r1.G, r1.B) by the color (r2.R, r2.G, r2.B).
        /// </summary>
        /// <param name="r1">RGB Color 1.</param>
        /// <param name="r2">RGB Color 2.</param>
        public virtual void Replace(RGBValue r1, RGBValue r2)
        {
            Handler.Replace(r1, r2);
        }

        /// <summary>
        /// Changes the size of the image in-place by scaling it: after a call to this
        /// function,the image will have the given width and height.
        /// </summary>
        /// <param name="width">New image width.</param>
        /// <param name="height">New image height.</param>
        /// <param name="quality">Scaling quality.</param>
        public virtual void Rescale(
            int width,
            int height,
            GenericImageResizeQuality quality = GenericImageResizeQuality.Normal)
        {
            Handler.Rescale(width, height, quality);
        }

        /// <summary>
        /// Changes the size of the image in-place without scaling it by adding either a border
        /// with the given color or cropping as necessary.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="pos"></param>
        /// <param name="color">RGB Color to fill background.</param>
        /// <remarks>
        /// The image is pasted into a new image with the given size and background color
        /// at the position pos relative to the upper left of the new image.
        /// If <paramref name="color"/> is null then use either the current mask color if
        /// set or find, use, and set a suitable mask color for any newly exposed areas.
        /// </remarks>
        public virtual void ResizeNoScale(
            SizeI size,
            PointI pos,
            RGBValue? color = null)
        {
            Handler.ResizeNoScale(size, pos, color);
        }

        /// <summary>
        /// Returns a resized version of this image without scaling it by adding either a
        /// border with the given color or cropping as necessary.
        /// </summary>
        /// <param name="size">New size.</param>
        /// <param name="pos">Position in the new image.</param>
        /// <param name="color">Fill color.</param>
        /// <returns>Resized image.</returns>
        /// <remarks>
        /// The image is pasted into a new image with the given <paramref name="size"/> and
        /// background color at the
        /// position <paramref name="pos"/> relative to the upper left of the new image.
        /// </remarks>
        /// <remarks>
        /// If <paramref name="color"/> is <c>null</c> then the areas of the larger image not covered by
        /// this image are made transparent by filling them with the image mask
        /// color(which will be allocated automatically if it isn't currently set).
        /// Otherwise, the areas will be filled with the color with the specified RGB components.
        /// </remarks>
        public virtual GenericImage SizeNoScale(SizeI size, PointI pos = default, RGBValue? color = null)
        {
            return Handler.SizeNoScale(size, pos, color);
        }

        /// <summary>
        /// Returns a copy of the image rotated 90 degrees in the direction indicated by clockwise.
        /// </summary>
        /// <param name="clockwise">Rotate direction.</param>
        /// <returns></returns>
        public virtual GenericImage Rotate90(bool clockwise = true)
        {
            return Handler.Rotate90(clockwise);
        }

        /// <summary>
        /// Returns a copy of the image rotated by 180 degrees.
        /// </summary>
        /// <returns></returns>
        public virtual GenericImage Rotate180()
        {
            return Handler.Rotate180();
        }

        /// <summary>
        /// Rotates the hue of each pixel in the image by angle, which is a double in the
        /// range [-1.0..+1.0], where -1.0 corresponds to -360 degrees and +1.0 corresponds
        /// to +360 degrees.
        /// </summary>
        /// <param name="angle"></param>
        public virtual void RotateHue(double angle)
        {
            Handler.RotateHue(angle);
        }

        /// <summary>
        /// Changes the saturation of each pixel in the image.
        /// </summary>
        /// <param name="factor">A double in the range [-1.0..+1.0], where -1.0 corresponds
        /// to -100 percent and +1.0 corresponds to +100 percent.</param>
        public virtual void ChangeSaturation(double factor)
        {
            Handler.ChangeSaturation(factor);
        }

        /// <summary>
        /// Changes the brightness(value) of each pixel in the image.
        /// </summary>
        /// <param name="factor">A double in the range [-1.0..+1.0], where -1.0 corresponds
        /// to -100 percent and +1.0 corresponds to +100 percent.</param>
        public virtual void ChangeBrightness(double factor)
        {
            Handler.ChangeBrightness(factor);
        }

        /// <summary>
        /// Returns the file load flags used for this object.
        /// </summary>
        /// <returns></returns>
        public virtual GenericImageLoadFlags GetLoadFlags()
        {
            return Handler.LoadFlags;
        }

        /// <summary>
        /// Sets the flags used for loading image files by this object.
        /// </summary>
        /// <remarks>
        /// The flags will affect any future calls to load from file functions for this object.
        /// To change the flags for all image objects, call <see cref="SetDefaultLoadFlags"/>
        /// before creating any of them.
        /// </remarks>
        /// <param name="flags"></param>
        public virtual void SetLoadFlags(GenericImageLoadFlags flags)
        {
            Handler.LoadFlags = flags;
        }

        /// <summary>
        /// Changes the hue, the saturation and the brightness(value) of each pixel in the image.
        /// </summary>
        /// <param name="angleH">A double in the range [-1.0..+1.0], where -1.0 corresponds
        /// to -360 degrees and
        /// +1.0 corresponds to +360 degrees</param>
        /// <param name="factorS">a double in the range [-1.0..+1.0], where -1.0
        /// corresponds to -100 percent and +1.0 corresponds to +100 percent</param>
        /// <param name="factorV">A double in the range[-1.0..+1.0], where -1.0 corresponds
        /// to -100 percent and +1.0 corresponds to +100 percent.</param>
        public virtual void ChangeHSV(double angleH, double factorS, double factorV)
        {
            Handler.ChangeHSV(angleH, factorS, factorV);
        }

        /// <summary>
        /// Returns a scaled version of the image. This is also useful for scaling bitmaps
        /// in general as the only other way to scale bitmaps is to blit a MemoryDC into
        /// another MemoryDC.
        /// </summary>
        /// <param name="width">New width.</param>
        /// <param name="height">New height.</param>
        /// <param name="quality">Determines what method to use for resampling the image.</param>
        /// <returns></returns>
        /// <remarks>
        /// The algorithm used for the default (normal) quality value doesn't work with images larger
        /// than 65536 (2^16) pixels in either dimension for 32-bit programs.For 64-bit programs the
        /// limit is 2^48 and so not relevant in practice.
        /// </remarks>
        /// <remarks>
        /// It should be noted that although using <see cref="GenericImageResizeQuality.High"/>
        /// produces
        /// much nicer looking results it is a slower method. Downsampling will use the box
        /// averaging method which seems to operate very fast. If you are upsampling larger images
        /// using this method you will most likely notice that it is a bit slower and
        /// in extreme cases it will be quite substantially slower as the bicubic
        /// algorithm has to process a lot of data.
        /// </remarks>
        /// <remarks>
        /// It should also be noted that the high quality scaling may not work as expected when
        /// using a single mask color for transparency, as the scaling will blur the image and
        /// will therefore remove the mask partially. Using the alpha channel will work.
        /// </remarks>
        public virtual GenericImage Scale(
            int width,
            int height,
            GenericImageResizeQuality quality = GenericImageResizeQuality.Normal)
        {
            return Handler.Scale(width, height, quality);
        }

        /// <summary>
        /// If the image has alpha channel, this method converts it to mask.
        /// </summary>
        /// <param name="threshold"></param>
        /// <returns>Returns <c>true</c> on success, <c>false</c> on error.</returns>
        /// <remarks>
        /// If the image has an alpha channel, all pixels with alpha value less
        /// than threshold are replaced with the mask color and the alpha channel
        /// is removed. Otherwise nothing is done.
        /// </remarks>
        /// <remarks>
        /// The mask color is chosen automatically using <see cref="FindFirstUnusedColor"/>,
        /// see the overload method if this is not appropriate.
        /// </remarks>
        public virtual bool ConvertAlphaToMask(byte threshold = AlphaChannelThreshold)
        {
            return Handler.ConvertAlphaToMask(threshold);
        }

        /// <summary>
        /// If the image has alpha channel, this method converts it to mask using the
        /// specified color as the mask color.
        /// </summary>
        /// <param name="rgb">Mask color.</param>
        /// <param name="threshold">Pixels with alpha channel values below the
        /// given threshold are considered to be
        /// transparent, i.e.the corresponding mask pixels are set. Pixels with
        /// the alpha values above the
        /// threshold are considered to be opaque.</param>
        /// <returns>Returns <c>true</c> on success, <c>false</c> on error.</returns>
        /// <remarks>
        /// If the image has an alpha channel, all pixels with alpha value less
        /// than threshold are replaced
        /// with the mask color and the alpha channel is removed.Otherwise nothing is done.
        /// </remarks>
        public virtual bool ConvertAlphaToMask(
            RGBValue rgb,
            byte threshold = AlphaChannelThreshold)
        {
            return Handler.ConvertAlphaToMask(rgb, threshold);
        }

        /// <summary>
        /// Returns a greyscale version of the image.
        /// </summary>
        /// <param name="weightR">Weight of the Red component.</param>
        /// <param name="weightG">Weight of the Green component.</param>
        /// <param name="weightB">Weight of the Blue component.</param>
        /// <returns></returns>
        /// <remarks>
        /// The returned image uses the luminance component of the original to calculate
        /// the greyscale. Defaults to using the standard ITU-T BT.601 when converting to
        /// YUV, where every pixel equals(R* weight_r) + (G* weight_g) + (B* weight_b).
        /// </remarks>
        public virtual GenericImage ConvertToGreyscale(double weightR, double weightG, double weightB)
        {
            return Handler.ConvertToGreyscale(weightR, weightG, weightB);
        }

        /// <summary>
        /// Returns a greyscale version of the image.
        /// </summary>
        /// <returns></returns>
        public virtual GenericImage ConvertToGreyscale()
        {
            return Handler.ConvertToGreyscale();
        }

        /// <summary>
        /// Returns monochromatic version of the image.
        /// </summary>
        /// <param name="rgb">RGB color.</param>
        /// <returns> The returned image has white color where the original has (r,g,b)
        /// color and black color everywhere else.</returns>
        public virtual GenericImage ConvertToMono(RGBValue rgb)
        {
            return Handler.ConvertToMono(rgb);
        }

        /// <summary>
        /// Returns disabled(dimmed) version of the image.
        /// </summary>
        /// <param name="brightness"></param>
        /// <returns></returns>
        public virtual GenericImage ConvertToDisabled(byte brightness = 255)
        {
            GenericImage image = this.Copy();
            image.ChangeToDisabled(brightness);
            return image;
        }

        /// <summary>
        /// Changes lightness of the each pixel.
        /// </summary>
        /// <param name="ialpha">New lightness value (0..200).</param>
        public virtual void ChangeLightness(int ialpha)
        {
            ialpha = MathUtils.ApplyMinMax(ialpha, 0, 200);
            ForEachPixel(Color.ChangeLightness, ialpha);
        }

        /// <summary>
        /// Changes each pixel of this image, making its color disabled.
        /// </summary>
        public virtual void ChangeToDisabled(byte brightness = 255)
        {
            ForEachPixel(Color.MakeDisabled, brightness);
        }

        /// <summary>
        /// Executes specified <paramref name="action"/> for the each pixel of the image.
        /// </summary>
        /// <typeparam name="T">Type of the custom value.</typeparam>
        /// <param name="action">Action to call for the each pixel. <see cref="RGBValue"/>
        /// is passed as the first
        /// parameter of the action.</param>
        /// <param name="param">Custom value. It is passed to the <paramref name="action"/>
        /// as the second parameter.</param>
        public virtual unsafe bool ForEachPixel<T>(ActionRef<SKColor, T> action, T param)
        {
            if (!IsOk)
                return false;

            if (HasMask)
            {
                SKColor mask = GetMaskRGB();
                return ForEachPixelInternal(MaskAction, param);

                void MaskAction(ref SKColor color, T param)
                {
                    if (color != mask)
                        action(ref color, param);
                }
            }
            else
            {
                if (HasAlpha)
                {
                    return ForEachPixelInternal(TransparentAction, param);

                    void TransparentAction(ref SKColor color, T param)
                    {
                        if (color.Alpha != 0)
                            action(ref color, param);
                    }
                }
                else
                {
                    return ForEachPixelInternal(action, param);
                }
            }

            unsafe bool ForEachPixelInternal<T2>(ActionRef<SKColor, T2> action, T2 param)
            {
                var rgb = Pixels;
                var count = rgb.Length;

                fixed (SKColor* ptr = rgb)
                {
                    var p = ptr;

                    for (int i = 0; i < count; i++)
                    {
                        action(ref *p, param);
                        p++;
                    }
                }

                Pixels = rgb;
                return true;
            }
        }

        /// <summary>
        /// Executes specified <paramref name="action"/> for the each pixel of the image.
        /// </summary>
        /// <typeparam name="T">Type of the custom value.</typeparam>
        /// <param name="action">Action to call for the each pixel. <see cref="RGBValue"/>
        /// is passed as the first
        /// parameter of the action.</param>
        /// <param name="param">Custom value. It is passed to the <paramref name="action"/>
        /// as the second parameter.</param>
        /// <remarks>
        /// For an example of the action implementation, see source code of the
        /// <see cref="Color.ChangeLightness(ref RGBValue, int)"/> method.
        /// </remarks>
        public virtual unsafe bool ForEachPixel<T>(ActionRef<RGBValue, T> action, T param)
        {
            if (!IsOk)
                return false;

            if (HasMask)
            {
                var mask = GetMaskRGB();
                return ForEachPixelInternal(MaskAction, param);

                void MaskAction(ref RGBValue rgb, T param)
                {
                    if (rgb != mask)
                        action(ref rgb, param);
                }
            }
            else
            {
                if (HasAlpha)
                {
                    return ForEachPixel(TransparentAction, param);

                    void TransparentAction(ref SKColor color, T param)
                    {
                        var alpha = color.Alpha;

                        if (alpha == 0)
                            return;

                        RGBValue rgb = color;
                        action(ref rgb, param);
                        color = rgb.WithAlpha(alpha);
                    }
                }
                else
                {
                    return ForEachPixelInternal(action, param);
                }
            }

            unsafe bool ForEachPixelInternal<T1>(ActionRef<RGBValue, T1> action, T1 param)
            {
                var rgb = RgbData;
                var count = rgb.Length;

                fixed (RGBValue* ptr = rgb)
                {
                    var p = ptr;

                    for (int i = 0; i < count; i++)
                    {
                        action(ref *p, param);
                        p++;
                    }
                }

                RgbData = rgb;
                return true;
            }
        }

        /// <summary>
        /// Returns a changed version of the image based on the given lightness.
        /// </summary>
        /// <param name="ialpha">Lightness (0..200).</param>
        /// <remarks>
        /// This utility function simply darkens or lightens a color, based on
        /// the specified percentage ialpha. ialpha of 0 would make the color completely black,
        /// 200 completely white and 100 would not change the color.
        /// </remarks>
        /// <returns></returns>
        public virtual GenericImage ConvertLightness(int ialpha)
        {
            var result = Copy();
            result.ChangeLightness(ialpha);
            return result;
        }

        /// <summary>
        /// Return alpha value at given pixel location.
        /// </summary>
        /// <param name="x">X coordinate of the pixel.</param>
        /// <param name="y">Y coordinate of the pixel.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetAlpha(int x, int y)
        {
            return Handler.GetAlpha(x, y);
        }

        /// <summary>
        /// Gets <see cref="RGBValue"/> at given pixel location.
        /// </summary>
        /// <param name="x">X coordinate of the pixel.</param>
        /// <param name="y">Y coordinate of the pixel.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RGBValue GetRGB(int x, int y)
        {
            return Handler.GetRGB(x, y);
        }

        /// <summary>
        /// Gets <see cref="Color"/> at given pixel location.
        /// </summary>
        /// <param name="x">X coordinate of the pixel.</param>
        /// <param name="y">Y coordinate of the pixel.</param>
        /// <param name="withAlpha">If true alpha channel is also returned in
        /// result (<see cref="Color.A"/>);
        /// if false it is set to 255.</param>
        /// <returns></returns>
        /// <remarks>
        /// Some images can have mask color, this method doesn't use this info. You need to add
        /// additional code in order to determine transparency if your image is with mask color.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color GetPixel(int x, int y, bool withAlpha = false)
        {
            return Handler.GetPixel(x, y, withAlpha);
        }

        /// <summary>
        /// Sets the color of the pixel at the given x and y coordinate.
        /// </summary>
        /// <param name="x">X coordinate of the pixel.</param>
        /// <param name="y">Y coordinate of the pixel.</param>
        /// <param name="color">New color of the pixel.</param>
        /// <remarks>
        /// This routine performs bounds-checks for the coordinate so it can be
        /// considered a safe way to manipulate the data.
        /// </remarks>
        /// <param name="withAlpha">If true alpha channel is also set from
        /// <paramref name="color"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetPixel(int x, int y, Color color, bool withAlpha = false)
        {
            Handler.SetPixel(x, y, color, withAlpha);
        }

        /// <summary>
        /// Returns the red intensity at the given coordinate.
        /// </summary>
        /// <param name="x">X coordinate of the pixel.</param>
        /// <param name="y">Y coordinate of the pixel.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetRed(int x, int y)
        {
            return Handler.GetRed(x, y);
        }

        /// <summary>
        /// Returns the green intensity at the given coordinate.
        /// </summary>
        /// <param name="x">X coordinate of the pixel.</param>
        /// <param name="y">Y coordinate of the pixel.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetGreen(int x, int y)
        {
            return Handler.GetGreen(x, y);
        }

        /// <summary>
        /// Returns the blue intensity at the given coordinate.
        /// </summary>
        /// <param name="x">X coordinate of the pixel.</param>
        /// <param name="y">Y coordinate of the pixel.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetBlue(int x, int y)
        {
            return Handler.GetBlue(x, y);
        }

        /// <summary>
        /// Gets the <see cref="RGBValue"/> value of the mask color.
        /// </summary>
        public virtual RGBValue GetMaskRGB()
        {
            return Handler.GetMaskRGB();
        }

        /// <summary>
        /// Gets the red value of the mask color.
        /// </summary>
        /// <returns></returns>
        public virtual byte GetMaskRed()
        {
            return Handler.GetMaskRed();
        }

        /// <summary>
        /// Gets the green value of the mask color.
        /// </summary>
        /// <returns></returns>
        public virtual byte GetMaskGreen()
        {
            return Handler.GetMaskGreen();
        }

        /// <summary>
        /// Gets the blue value of the mask color.
        /// </summary>
        /// <returns></returns>
        public virtual byte GetMaskBlue()
        {
            return Handler.GetMaskBlue();
        }

        /// <summary>
        /// Gets a user-defined string-valued option.
        /// </summary>
        /// <remarks>
        /// See <see cref="GenericImageOptionNames"/> for known option names.
        /// </remarks>
        /// <param name="name">The name of the option, case-insensitive.</param>
        /// <returns></returns>
        public virtual string GetOptionAsString(string name)
        {
            return Handler.GetOptionAsString(name);
        }

        /// <summary>
        /// Gets a user-defined integer-valued option.
        /// </summary>
        /// <remarks>
        /// See <see cref="GenericImageOptionNames"/> for known option names.
        /// </remarks>
        /// <param name="name">The name of the option, case-insensitive.</param>
        /// <returns></returns>
        public virtual int GetOptionAsInt(string name)
        {
            return Handler.GetOptionAsInt(name);
        }

        /// <summary>
        /// Returns a sub image of the current one as long as the rect belongs entirely to the image.
        /// </summary>
        /// <param name="rect">Bounds of the sub-image.</param>
        /// <returns></returns>
        public virtual GenericImage GetSubImage(RectI rect)
        {
            return Handler.GetSubImage(rect);
        }

        /// <summary>
        /// Gets the type of image found when image was loaded or specified when image was saved.
        /// </summary>
        /// <returns></returns>
        public virtual BitmapType GetImageType()
        {
            return Handler.GetImageType();
        }

        /// <summary>
        /// Returns <c>true</c> if the given option is present.
        /// </summary>
        /// <remarks>
        /// See <see cref="GenericImageOptionNames"/> for known option names.
        /// </remarks>
        /// <param name="name">The name of the option, case-insensitive.</param>
        /// <returns></returns>
        public virtual bool HasOption(string name)
        {
            return Handler.HasOption(name);
        }

        /// <summary>
        /// Returns <c>true</c> if the given pixel is transparent, i.e. either has the mask
        /// color if this image has a mask or if this image has alpha channel and alpha value of
        /// this pixel is strictly less than threshold.
        /// </summary>
        /// <param name="x">X coordinate of the pixel.</param>
        /// <param name="y">Y coordinate of the pixel.</param>
        /// <param name="threshold">Alpha value treshold.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsTransparent(int x, int y, byte threshold = AlphaChannelThreshold)
        {
            return Handler.IsTransparent(x, y, threshold);
        }

        /// <summary>
        /// Loads an image from an input stream.
        /// </summary>
        /// <param name="stream">Input stream with image data.</param>
        /// <param name="bitmapType">Type of the bitmap. Depending on how library and OS has
        /// been configured and
        /// by which handlers have been loaded, not all formats may be available. If value is
        /// <see cref="BitmapType.Any"/>, function will try to autodetect the format.</param>
        /// <param name="index">See description in
        /// <see cref="GenericImage(string, BitmapType, int)"/></param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        public virtual bool LoadFromStream(
            Stream stream,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1)
        {
            return Handler.LoadFromStream(stream, bitmapType, index);
        }

        /// <summary>
        /// Loads an image from a file.
        /// </summary>
        /// <param name="url">Path or url to file with image data.</param>
        /// <param name="bitmapType">Type of the bitmap. Depending on how library and OS has
        /// been configured and
        /// by which handlers have been loaded, not all formats may be available. If value is
        /// <see cref="BitmapType.Any"/>, function will try to autodetect the format.</param>
        /// <param name="index">See description in
        /// <see cref="GenericImage(string, BitmapType, int)"/></param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        public virtual bool LoadFromFile(
            string? url,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            using var stream = ResourceLoader.StreamFromUrlOrDefault(url);
            if (stream is null)
            {
                return false;
            }

            return Handler.LoadFromStream(
                stream,
                bitmapType,
                index);
        }

        /// <summary>
        /// Loads an image from a file.
        /// </summary>
        /// <param name="url">Path or url to file with image data.</param>
        /// <param name="mimeType">MIME type string (for example 'image/jpeg').</param>
        /// <param name="index">See description in
        /// <see cref="GenericImage(string, BitmapType, int)"/></param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        public virtual bool LoadFromFile(string? url, string mimeType, int index = -1)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            using var stream = ResourceLoader.StreamFromUrlOrDefault(url);
            if (stream is null)
            {
                return false;
            }

            return Handler.LoadFromStream(stream, mimeType, index);
        }

        /// <summary>
        /// Loads an image from an input stream.
        /// </summary>
        /// <param name="stream">Input stream with image data.</param>
        /// <param name="mimeType">MIME type string (for example 'image/jpeg').</param>
        /// <param name="index">See description in
        /// <see cref="GenericImage(string, BitmapType, int)"/></param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        public virtual bool LoadFromStream(Stream stream, string mimeType, int index = -1)
        {
            return Handler.LoadFromStream(stream, mimeType, index);
        }

        /// <summary>
        /// Saves an image in the given stream.
        /// </summary>
        /// <param name="stream">Output stream.</param>
        /// <param name="mimeType">MIME type string (for example 'image/jpeg').</param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        public virtual bool SaveToStream(Stream stream, string mimeType)
        {
            return Handler.SaveToStream(stream, mimeType);
        }

        /// <summary>
        /// Saves an image in the named file.
        /// </summary>
        /// <param name="filename">Path to file.</param>
        /// <param name="bitmapType">Type of the bitmap. Depending on how library and OS
        /// has been configured and
        /// by which handlers have been loaded, not all formats may be available.</param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        public virtual bool SaveToFile(string filename, BitmapType bitmapType)
        {
            return InsideTryCatch(() =>
            {
                using var stream = FileSystem.Default.Create(filename);
                return SaveToStream(stream, bitmapType);
            });
        }

        /// <summary>
        /// Saves an image in the named file.
        /// </summary>
        /// <param name="filename">Name of the file to save the image to.</param>
        /// <param name="mimeType">MIME type string (for example 'image/jpeg').</param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        public virtual bool SaveToFile(string filename, string mimeType)
        {
            return InsideTryCatch(() =>
            {
                using var stream = FileSystem.Default.Create(filename);
                return SaveToStream(stream, mimeType);
            });
        }

        /// <summary>
        /// Saves an image in the named file.
        /// </summary>
        /// <param name="filename">Path to file.</param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        /// <remarks>
        /// File type is determined from the extension of the file name.
        /// Note that this function may fail if the extension is not recognized!
        /// You can use one of the overload methods to save images to files
        /// with non-standard extensions.
        /// </remarks>
        public virtual bool SaveToFile(string filename)
        {
            return InsideTryCatch(() =>
            {
                using var stream = FileSystem.Default.Create(filename);
                var bitmapType = Image.GetBitmapTypeFromFileName(filename);
                return SaveToStream(stream, bitmapType);
            });
        }

        /// <summary>
        /// Locks pixels data and gets <see cref="ISkiaSurface"/> to access it.
        /// </summary>
        /// <param name="lockMode">Lock mode.</param>
        /// <returns></returns>
        public virtual ISkiaSurface LockSurface(ImageLockMode lockMode = ImageLockMode.ReadWrite)
        {
            Debug.Assert(IsOk, "Image.IsOk == true is required.");
            Debug.Assert(!HasMask, "Image.HasMask == false is required.");

            return GraphicsFactory.CreateSkiaSurface(this, lockMode);
        }

        /// <summary>
        /// Saves an image in the given stream.
        /// </summary>
        /// <param name="stream">Output stream</param>
        /// <param name="type"></param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        public virtual bool SaveToStream(Stream stream, BitmapType type)
        {
            return Handler.SaveToStream(stream, type);
        }

        /// <inheritdoc/>
        protected override IGenericImageHandler CreateHandler()
        {
            return GraphicsFactory.Handler.CreateGenericImageHandler();
        }
    }
}