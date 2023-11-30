using System;
using System.IO;
using System.Runtime.CompilerServices;
using Alternet.UI;
using Alternet.UI.Native;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements platform independant image.
    /// </summary>
    public class GenericImage : DisposableObject
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
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an empty image without an alpha channel.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage()
            : base(UI.Native.GenericImage.CreateImage(), true)
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
        public GenericImage(int width, int height, bool clear = true)
            : base(UI.Native.GenericImage.CreateImageWithSize(width, height, clear), true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image from a file.
        /// </summary>
        /// <param name="fileName">Path to file.</param>
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
        public GenericImage(string fileName, BitmapType bitmapType = BitmapType.Any, int index = -1)
            : base(
                  UI.Native.GenericImage.CreateImageFromFileWithBitmapType(
                    fileName,
                    (int)bitmapType,
                    index), true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image from a file using MIME-types to specify the type.
        /// </summary>
        /// <param name="name">Name of the file from which to load the image.</param>
        /// <param name="mimetype">MIME type string (for example 'image/jpeg').</param>
        /// <param name="index">See description in
        /// <see cref="GenericImage(string, BitmapType, int)"/></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage(string name, string mimetype, int index = -1)
            : base(UI.Native.GenericImage.CreateImageFromFileWithMimeType(name, mimetype, index), true)
        {
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
            : this(new UI.Native.InputStream(stream), bitmapType, index)
        {
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
            : this(new UI.Native.InputStream(stream), mimeType, index)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image from data in memory.
        /// </summary>
        /// <param name="width">Specifies the width of the image.</param>
        /// <param name="height">Specifies the height of the image.</param>
        /// <param name="data">A pointer to RGB data</param>
        /// <param name="staticData">Indicates if the data should be free'd after use.
        /// If static_data is <c>false</c> then the
        /// library will take ownership of the data and free it afterwards.For this,
        /// it has to be allocated with
        /// malloc.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage(int width, int height, IntPtr data, bool staticData = false)
            : base(
                  UI.Native.GenericImage.CreateImageWithSizeAndData(width, height, data, staticData),
                  true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image from data in memory.
        /// </summary>
        /// <param name="width">Specifies the width of the image.</param>
        /// <param name="height">Specifies the height of the image.</param>
        /// <param name="data">A pointer to RGB data</param>
        /// <param name="alpha">A pointer to alpha-channel data</param>
        /// <param name="staticData">Indicates if the data should be free'd after use.
        /// If <paramref name="staticData"/> is <c>false</c> then the
        /// library will take ownership of the data and free it afterwards.For this,
        /// it has to be allocated with
        /// malloc.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage(int width, int height, IntPtr data, IntPtr alpha, bool staticData = false)
            : base(
                  UI.Native.GenericImage.CreateImageWithAlpha(width, height, data, alpha, staticData),
                  true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal GenericImage(IntPtr handle, bool disposeHandle)
            : base(handle, disposeHandle)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal GenericImage(IntPtr handle)
            : base(handle, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image from a stream.
        /// </summary>
        /// <param name="stream">Opened input stream from which to load the image.
        /// Currently, the stream must support seeking.</param>
        /// <param name="bitmapType">Type of the bitmap. Depending on how library and OS has
        /// been configured and
        /// by which handlers have been loaded, not all formats may be available. If value is
        /// <see cref="BitmapType.Any"/>, function will try to autodetect the format.</param>
        /// <param name="index"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal GenericImage(InputStream stream, BitmapType bitmapType = BitmapType.Any, int index = -1)
            : base(
                  UI.Native.GenericImage.CreateImageFromStreamWithBitmapData(
                      stream,
                      (int)bitmapType,
                      index), true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image from a stream using MIME-types to specify the type.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal GenericImage(InputStream stream, string mimeType, int index = -1)
            : base(
                  UI.Native.GenericImage.CreateImageFromStreamWithMimeType(
                      stream,
                      mimeType,
                      index), true)
        {
        }

        /// <summary>
        /// Gets the width of the image in pixels.
        /// </summary>
        public int Width
        {
            get => UI.Native.GenericImage.GetWidth(Handle);
        }

        /// <summary>
        /// Gets the height of the image in pixels.
        /// </summary>
        public int Height
        {
            get => UI.Native.GenericImage.GetHeight(Handle);
        }

        /// <summary>
        /// Returns the size of the image in pixels.
        /// </summary>
        public Int32Size Size
        {
            get => UI.Native.GenericImage.GetSize(Handle);
        }

        /// <summary>
        /// Returns the bounds of the image in pixels. Result is (0, 0, Width, Height).
        /// </summary>
        public Int32Rect Bounds
        {
            get => (0, 0, Width, Height);
        }

        /// <summary>
        /// Returns <c>true</c> if image data is present.
        /// </summary>
        public bool IsOk
        {
            get => UI.Native.GenericImage.IsOk(Handle);
        }

        /// <summary>
        /// Converts the specified <see cref='GenericImage'/> to a <see cref='Image'/>.
        /// </summary>
        public static explicit operator Image(GenericImage image) => new Bitmap(image);

        /// <summary>
        /// Converts the specified <see cref='GenericImage'/> to a <see cref='Bitmap'/>.
        /// </summary>
        public static explicit operator Bitmap(GenericImage image) => new(image);

        /// <summary>
        /// Converts the specified <see cref='GenericImage'/> to a <see cref='Image'/>.
        /// </summary>
        public static explicit operator GenericImage(Image image) => image.AsGeneric;

        /// <summary>
        /// Returns <c>true</c> if at least one of the available image handlers can read the file
        /// with the given name.
        /// </summary>
        /// <param name="filename">Name of the file from which to load the image.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanRead(string filename)
        {
            return UI.Native.GenericImage.CanRead(filename);
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
            var inputStream = new UI.Native.InputStream(stream);
            return UI.Native.GenericImage.CanReadStream(inputStream);
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
            return UI.Native.GenericImage.GetImageExtWildcard();
        }

        /// <summary>
        /// Finds image load/save handler with the given name, and removes it.
        /// </summary>
        /// <param name="name">Name of the handler.</param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool RemoveHandler(string name)
        {
            return UI.Native.GenericImage.RemoveHandler(name);
        }

        /// <summary>
        /// If the image file contains more than one image and the image handler is capable of
        /// retrieving these individually, this function will return the number of available images.
        /// </summary>
        /// <param name="filename">Name of the file from which to load the image.</param>
        /// <param name="bitmapType">Type of the bitmap. Depending on how library and OS has
        /// been configured and
        /// by which handlers have been loaded, not all formats may be available. If value is
        /// <see cref="BitmapType.Any"/>, function will try to autodetect the format.</param>
        /// <returns>Number of available images. For most image handlers, this is 1
        /// (exceptions are TIFF and ICO formats as well as animated GIFs for which this function
        /// returns the number of frames in the animation).</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetImageCount(
            string filename,
            BitmapType bitmapType = BitmapType.Any)
        {
            return UI.Native.GenericImage.GetImageCountInFile(filename, (int)bitmapType);
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
            var inputStream = new UI.Native.InputStream(stream);
            return UI.Native.GenericImage.GetImageCountInStream(inputStream, (int)bitmapType);
        }

        /// <summary>
        /// Deletes all image handlers.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CleanUpHandlers()
        {
            UI.Native.GenericImage.CleanUpHandlers();
        }

        /// <summary>
        /// Returns the currently used default file load flags.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenericImageLoadFlags GetDefaultLoadFlags()
        {
            return (GenericImageLoadFlags)UI.Native.GenericImage.GetDefaultLoadFlags();
        }

        /// <summary>
        /// Sets the default value for the flags used for loading image files.
        /// </summary>
        /// <param name="flags"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetDefaultLoadFlags(GenericImageLoadFlags flags)
        {
            UI.Native.GenericImage.SetDefaultLoadFlags((int)flags);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetAlpha(int x, int y, byte alpha)
        {
            UI.Native.GenericImage.SetAlpha(Handle, x, y, alpha);
        }

        /// <summary>
        /// Removes the alpha channel from the image.
        /// </summary>
        /// <remarks>
        /// This function should only be called if the image has alpha channel data,
        /// use <see cref="HasAlpha"/> to check for this.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearAlpha()
        {
            UI.Native.GenericImage.ClearAlpha(Handle);
        }

        /// <summary>
        /// Specifies whether there is a mask or not.
        /// </summary>
        /// <param name="hasMask"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetMask(bool hasMask = true)
        {
            UI.Native.GenericImage.SetMask(Handle, hasMask);
        }

        /// <summary>
        /// Sets the mask color for this image(and tells the image to use the mask).
        /// </summary>
        /// <param name="rgb">Color RGB.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetMaskColor(RGBValue rgb)
        {
            UI.Native.GenericImage.SetMaskColor(Handle, rgb.R, rgb.G, rgb.B);
        }

        /// <summary>
        /// Executes specified <paramref name="action"/> for the each pixel of the image.
        /// </summary>
        /// <typeparam name="T">Type of the custom value.</typeparam>
        /// <param name="action">Action to call for the each pixel. <see cref="RGBValue"/> is passed as the first
        /// parameter of the action.</param>
        /// <param name="param">Custom value. It is passed to the <paramref name="action"/> as the second parameter.</param>
        /// <remarks>
        /// For an example of the action implementation, see source code of the
        /// <see cref="Color.ChangeLightness(ref RGBValue, int)"/> method.
        /// </remarks>
        public unsafe void ForEachPixel<T>(ActionRef<RGBValue, T> action, T param)
        {
            var ndata = GetNativeData();
            RGBValue* data = (RGBValue*)ndata;
            var height = Height;
            var width = Width;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    action(ref *data, param);
                    data++;
                }
            }
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SetMaskFromImage(GenericImage image, RGBValue mask)
        {
            return UI.Native.GenericImage.SetMaskFromImage(Handle, image.Handle, mask.R, mask.G, mask.B);
        }

        /// <summary>
        /// Sets a user-defined option.
        /// </summary>
        /// <remarks>
        /// See <see cref="GenericImageOptionNames"/> for known option names.
        /// </remarks>
        /// <param name="name">The name of the option, case-insensitive.</param>
        /// <param name="value">New option value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetOptionAsString(string name, string value)
        {
            UI.Native.GenericImage.SetOptionString(Handle, name, value);
        }

        /// <summary>
        /// Sets a user-defined option.
        /// </summary>
        /// <remarks>
        /// See <see cref="GenericImageOptionNames"/> for known option names.
        /// </remarks>
        /// <param name="name">The name of the option, case-insensitive.</param>
        /// <param name="value">New option value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetOptionAsInt(string name, int value)
        {
            UI.Native.GenericImage.SetOptionInt(Handle, name, value);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetRGB(int x, int y, RGBValue rgb)
        {
            UI.Native.GenericImage.SetRGB(Handle, x, y, rgb.R, rgb.G, rgb.B);
        }

        /// <summary>
        /// Sets the color of the pixels within the given rectangle.
        /// </summary>
        /// <param name="rect">Rectangle within the image. If rectangle is null,
        /// <see cref="Bounds"/> property is used.</param>
        /// <param name="rgb">RGB Color.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetRGBRect(RGBValue rgb, Int32Rect? rect = null)
        {
            rect ??= Bounds;
            UI.Native.GenericImage.SetRGBRect(Handle, rect.Value, rgb.R, rgb.G, rgb.B);
        }

        /// <summary>
        /// Sets the type of image returned by GetType().
        /// </summary>
        /// <param name="type">Type of the bitmap.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetImageType(BitmapType type)
        {
            UI.Native.GenericImage.SetImageType(Handle, (int)type);
        }

        /// <summary>
        /// Returns an identical copy of this image.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage Copy()
        {
            return new(UI.Native.GenericImage.Copy(Handle));
        }

        /// <summary>
        /// Creates a fresh image.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="clear"></param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Reset(int width, int height, bool clear = true)
        {
            return UI.Native.GenericImage.CreateFreshImage(Handle, width, height, clear);
        }

        /// <summary>
        /// Initialize the image data with zeroes (the default) or with the byte value given as value.
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear(byte value = 0)
        {
            UI.Native.GenericImage.Clear(Handle, value);
        }

        /// <summary>
        /// Destroys the image data.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            UI.Native.GenericImage.DestroyImageData(Handle);
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
        public Color FindFirstUnusedColor(RGBValue? startRGB = null)
        {
            var value = startRGB ?? new(1, 0, 0);

            return UI.Native.GenericImage.FindFirstUnusedColor(
                Handle, value.R, value.G, value.B);
        }

        /// <summary>
        /// Initializes the image alpha channel data.
        /// </summary>
        /// <remarks>
        /// It is an error to call it if the image already has alpha data. If it doesn't,
        /// alpha data will be by default initialized to all pixels being fully opaque.
        /// But if the image has a mask color, all mask pixels will be completely transparent.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InitAlpha()
        {
            UI.Native.GenericImage.InitAlpha(Handle);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage Blur(int blurRadius)
        {
            return new(UI.Native.GenericImage.Blur(Handle, blurRadius));
        }

        /// <summary>
        /// Blurs the image in the horizontal direction only.
        /// </summary>
        /// <param name="blurRadius">Blur radius in pixels.</param>
        /// <returns>Blurred image.</returns>
        /// <remarks>
        /// This should not be used when using a single mask color for transparency.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage BlurHorizontal(int blurRadius)
        {
            return new(UI.Native.GenericImage.BlurHorizontal(Handle, blurRadius));
        }

        /// <summary>
        /// Blurs the image in the vertical direction only.
        /// </summary>
        /// <param name="blurRadius">Blur radius in pixels.</param>
        /// <returns>Blurred image.</returns>
        /// <remarks>
        /// This should not be used when using a single mask color for transparency.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage BlurVertical(int blurRadius)
        {
            return new(UI.Native.GenericImage.BlurVertical(Handle, blurRadius));
        }

        /// <summary>
        /// Returns a mirrored copy of the image.
        /// </summary>
        /// <param name="horizontally"></param>
        /// <returns>Mirrored copy of the image</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage Mirror(bool horizontally = true)
        {
            return new(UI.Native.GenericImage.Mirror(Handle, horizontally));
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Paste(
            GenericImage image,
            int x,
            int y,
            GenericImageAlphaBlendMode alphaBlend = GenericImageAlphaBlendMode.Overwrite)
        {
            UI.Native.GenericImage.Paste(Handle, image.Handle, x, y, (int)alphaBlend);
        }

        /// <summary>
        /// Replaces the color specified by (r1.R, r1.G, r1.B) by the color (r2.R, r2.G, r2.B).
        /// </summary>
        /// <param name="r1">RGB Color 1.</param>
        /// <param name="r2">RGB Color 2.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Replace(RGBValue r1, RGBValue r2)
        {
            UI.Native.GenericImage.Replace(Handle, r1.R, r1.G, r1.B, r2.R, r2.G, r2.B);
        }

        /// <summary>
        /// Changes the size of the image in-place by scaling it: after a call to this
        /// function,the image will have the given width and height.
        /// </summary>
        /// <param name="width">New image width.</param>
        /// <param name="height">New image height.</param>
        /// <param name="quality">Scaling quality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Rescale(
            int width,
            int height,
            GenericImageResizeQuality quality = GenericImageResizeQuality.Normal)
        {
            UI.Native.GenericImage.Rescale(Handle, width, height, (int)quality);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ResizeNoScale(
            Int32Size size,
            Int32Point pos,
            RGBValue? color = null)
        {
            if (color is null)
            {
                UI.Native.GenericImage.Resize(Handle, size, pos, -1, -1, -1);
                return;
            }

            var red = color.Value.R;
            var green = color.Value.G;
            var blue = color.Value.G;

            UI.Native.GenericImage.Resize(Handle, size, pos, red, green, blue);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage SizeNoScale(Int32Size size, Int32Point pos = default, RGBValue? color = null)
        {
            var red = color?.R ?? -1;
            var green = color?.G ?? -1;
            var blue = color?.G ?? -1;

            var image = UI.Native.GenericImage.Size(Handle, size, pos, red, green, blue);
            return new GenericImage(image);
        }

        /// <summary>
        /// Returns a copy of the image rotated 90 degrees in the direction indicated by clockwise.
        /// </summary>
        /// <param name="clockwise">Rotate direction.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage Rotate90(bool clockwise = true)
        {
            return new(UI.Native.GenericImage.Rotate90(Handle, clockwise));
        }

        /// <summary>
        /// Returns a copy of the image rotated by 180 degrees.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage Rotate180()
        {
            return new(UI.Native.GenericImage.Rotate180(Handle));
        }

        /// <summary>
        /// Rotates the hue of each pixel in the image by angle, which is a double in the
        /// range [-1.0..+1.0], where -1.0 corresponds to -360 degrees and +1.0 corresponds
        /// to +360 degrees.
        /// </summary>
        /// <param name="angle"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RotateHue(double angle)
        {
            UI.Native.GenericImage.RotateHue(Handle, angle);
        }

        /// <summary>
        /// Changes the saturation of each pixel in the image.
        /// </summary>
        /// <param name="factor">A double in the range [-1.0..+1.0], where -1.0 corresponds
        /// to -100 percent and +1.0 corresponds to +100 percent.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ChangeSaturation(double factor)
        {
            UI.Native.GenericImage.ChangeSaturation(Handle, factor);
        }

        /// <summary>
        /// Changes the brightness(value) of each pixel in the image.
        /// </summary>
        /// <param name="factor">A double in the range [-1.0..+1.0], where -1.0 corresponds
        /// to -100 percent and +1.0 corresponds to +100 percent.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ChangeBrightness(double factor)
        {
            UI.Native.GenericImage.ChangeBrightness(Handle, factor);
        }

        /// <summary>
        /// Returns the file load flags used for this object.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImageLoadFlags GetLoadFlags()
        {
            return (GenericImageLoadFlags)UI.Native.GenericImage.GetLoadFlags(Handle);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetLoadFlags(GenericImageLoadFlags flags)
        {
            UI.Native.GenericImage.SetLoadFlags(Handle, (int)flags);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ChangeHSV(double angleH, double factorS, double factorV)
        {
            UI.Native.GenericImage.ChangeHSV(Handle, angleH, factorS, factorV);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage Scale(
            int width,
            int height,
            GenericImageResizeQuality quality = GenericImageResizeQuality.Normal)
        {
            return new(UI.Native.GenericImage.Scale(Handle, width, height, (int)quality));
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ConvertAlphaToMask(byte threshold = AlphaChannelThreshold)
        {
            return UI.Native.GenericImage.ConvertAlphaToMask(Handle, threshold);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ConvertAlphaToMask(
            RGBValue rgb,
            byte threshold = AlphaChannelThreshold)
        {
            return UI.Native.GenericImage.ConvertAlphaToMaskUseColor(
                Handle,
                rgb.R,
                rgb.G,
                rgb.B,
                threshold);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage ConvertToGreyscale(double weightR, double weightG, double weightB)
        {
            return new(UI.Native.GenericImage.ConvertToGreyscaleEx(Handle, weightR, weightG, weightB));
        }

        /// <summary>
        /// Returns a greyscale version of the image.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage ConvertToGreyscale()
        {
            return new(UI.Native.GenericImage.ConvertToGreyscale(Handle));
        }

        /// <summary>
        /// Returns monochromatic version of the image.
        /// </summary>
        /// <param name="rgb">RGB color.</param>
        /// <returns> The returned image has white color where the original has (r,g,b)
        /// color and black color everywhere else.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage ConvertToMono(RGBValue rgb)
        {
            return new(UI.Native.GenericImage.ConvertToMono(Handle, rgb.R, rgb.G, rgb.B));
        }

        /// <summary>
        /// Returns disabled(dimmed) version of the image.
        /// </summary>
        /// <param name="brightness"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage ConvertToDisabled(byte brightness = 255)
        {
            return new(UI.Native.GenericImage.ConvertToDisabled(Handle, brightness));
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage ChangeLightness(int ialpha)
        {
            return new(UI.Native.GenericImage.ChangeLightness(Handle, ialpha));
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
            return UI.Native.GenericImage.GetAlpha(Handle, x, y);
        }

        /// <summary>
        /// Gets <see cref="RGBValue"/> at given pixel location.
        /// </summary>
        /// <param name="x">X coordinate of the pixel.</param>
        /// <param name="y">Y coordinate of the pixel.</param>
        /// <returns></returns>
        public RGBValue GetRGB(int x, int y)
        {
            var r = GetRed(x, y);
            var g = GetGreen(x, y);
            var b = GetBlue(x, y);
            return new(r, g, b);
        }

        /// <summary>
        /// Gets <see cref="Color"/> at given pixel location.
        /// </summary>
        /// <param name="x">X coordinate of the pixel.</param>
        /// <param name="y">Y coordinate of the pixel.</param>
        /// <param name="withAlpha">If true alpha channel is also returned in result (<see cref="Color.A"/>);
        /// if false it is set to 255.</param>
        /// <returns></returns>
        /// <remarks>
        /// Some images can have mask color, this method doesn't use this info. You need to add
        /// additional code in order to determine transparency if your image is with mask color.
        /// </remarks>
        public Color GetPixel(int x, int y, bool withAlpha = false)
        {
            var r = GetRed(x, y);
            var g = GetGreen(x, y);
            var b = GetBlue(x, y);
            var a = (withAlpha && HasAlpha()) ? GetAlpha(x, y) : 255;
            return Color.FromArgb(a, r, g, b);
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
        /// <param name="withAlpha">If true alpha channel is also set from <paramref name="color"/>.</param>
        public void SetPixel(int x, int y, Color color, bool withAlpha = false)
        {
            color.GetArgbValues(out var a, out var r, out var g, out var b);
            SetRGB(x, y, new RGBValue(r, g, b));
            if (withAlpha && HasAlpha())
                SetAlpha(x, y, a);
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
            return UI.Native.GenericImage.GetRed(Handle, x, y);
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
            return UI.Native.GenericImage.GetGreen(Handle, x, y);
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
            return UI.Native.GenericImage.GetBlue(Handle, x, y);
        }

        /// <summary>
        /// Gets the <see cref="RGBValue"/> value of the mask color.
        /// </summary>
        public RGBValue GetMaskRGB()
        {
            var r = GetMaskRed();
            var g = GetMaskGreen();
            var b = GetMaskBlue();
            return new(r, g, b);
        }

        /// <summary>
        /// Gets the red value of the mask color.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaskRed()
        {
            return UI.Native.GenericImage.GetMaskRed(Handle);
        }

        /// <summary>
        /// Gets the green value of the mask color.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaskGreen()
        {
            return UI.Native.GenericImage.GetMaskGreen(Handle);
        }

        /// <summary>
        /// Gets the blue value of the mask color.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaskBlue()
        {
            return UI.Native.GenericImage.GetMaskBlue(Handle);
        }

        /// <summary>
        /// Gets a user-defined string-valued option.
        /// </summary>
        /// <remarks>
        /// See <see cref="GenericImageOptionNames"/> for known option names.
        /// </remarks>
        /// <param name="name">The name of the option, case-insensitive.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetOptionAsString(string name)
        {
            return UI.Native.GenericImage.GetOptionString(Handle, name);
        }

        /// <summary>
        /// Gets a user-defined integer-valued option.
        /// </summary>
        /// <remarks>
        /// See <see cref="GenericImageOptionNames"/> for known option names.
        /// </remarks>
        /// <param name="name">The name of the option, case-insensitive.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetOptionAsInt(string name)
        {
            return UI.Native.GenericImage.GetOptionInt(Handle, name);
        }

        /// <summary>
        /// Returns a sub image of the current one as long as the rect belongs entirely to the image.
        /// </summary>
        /// <param name="rect">Bounds of the sub-image.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage GetSubImage(Int32Rect rect)
        {
            return new(UI.Native.GenericImage.GetSubImage(Handle, rect));
        }

        /// <summary>
        /// Gets the type of image found when image was loaded or specified when image was saved.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetImageType()
        {
            return UI.Native.GenericImage.GetImageType(Handle);
        }

        /// <summary>
        /// Returns <c>true</c> if this image has alpha channel, <c>false</c> otherwise.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAlpha()
        {
            return UI.Native.GenericImage.HasAlpha(Handle);
        }

        /// <summary>
        /// Returns <c>true</c> if there is a mask active, <c>false</c> otherwise.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasMask()
        {
            return UI.Native.GenericImage.HasMask(Handle);
        }

        /// <summary>
        /// Returns <c>true</c> if the given option is present.
        /// </summary>
        /// <remarks>
        /// See <see cref="GenericImageOptionNames"/> for known option names.
        /// </remarks>
        /// <param name="name">The name of the option, case-insensitive.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasOption(string name)
        {
            return UI.Native.GenericImage.HasOption(Handle, name);
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
            return UI.Native.GenericImage.IsTransparent(Handle, x, y, threshold);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LoadFromStream(
            Stream stream,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1)
        {
            var inputStream = new UI.Native.InputStream(stream);
            return UI.Native.GenericImage.LoadStreamWithBitmapType(
                Handle,
                inputStream,
                (int)bitmapType,
                index);
        }

        /// <summary>
        /// Loads an image from a file.
        /// </summary>
        /// <param name="filename">Path to file.</param>
        /// <param name="bitmapType">Type of the bitmap. Depending on how library and OS has
        /// been configured and
        /// by which handlers have been loaded, not all formats may be available. If value is
        /// <see cref="BitmapType.Any"/>, function will try to autodetect the format.</param>
        /// <param name="index">See description in
        /// <see cref="GenericImage(string, BitmapType, int)"/></param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LoadFromFile(
            string filename,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1)
        {
            return UI.Native.GenericImage.LoadFileWithBitmapType(
                Handle,
                filename,
                (int)bitmapType,
                index);
        }

        /// <summary>
        /// Loads an image from a file.
        /// </summary>
        /// <param name="name">Path to file.</param>
        /// <param name="mimetype">MIME type string (for example 'image/jpeg').</param>
        /// <param name="index">See description in
        /// <see cref="GenericImage(string, BitmapType, int)"/></param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LoadFromFile(string name, string mimetype, int index = -1)
        {
            return UI.Native.GenericImage.LoadFileWithMimeType(Handle, name, mimetype, index);
        }

        /// <summary>
        /// Loads an image from an input stream.
        /// </summary>
        /// <param name="stream">Input stream with image data.</param>
        /// <param name="mimetype">MIME type string (for example 'image/jpeg').</param>
        /// <param name="index">See description in
        /// <see cref="GenericImage(string, BitmapType, int)"/></param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LoadFromStream(Stream stream, string mimetype, int index = -1)
        {
            var inputStream = new UI.Native.InputStream(stream);
            return UI.Native.GenericImage.LoadStreamWithMimeType(Handle, inputStream, mimetype, index);
        }

        /// <summary>
        /// Saves an image in the given stream.
        /// </summary>
        /// <param name="stream">Output stream.</param>
        /// <param name="mimetype">MIME type string (for example 'image/jpeg').</param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SaveToStream(Stream stream, string mimetype)
        {
            var outputStream = new UI.Native.OutputStream(stream);
            return UI.Native.GenericImage.SaveStreamWithMimeType(Handle, outputStream, mimetype);
        }

        /// <summary>
        /// Saves an image in the named file.
        /// </summary>
        /// <param name="filename">Path to file.</param>
        /// <param name="bitmapType">Type of the bitmap. Depending on how library and OS
        /// has been configured and
        /// by which handlers have been loaded, not all formats may be available.</param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SaveToFile(string filename, BitmapType bitmapType)
        {
            return UI.Native.GenericImage.SaveFileWithBitmapType(Handle, filename, (int)bitmapType);
        }

        /// <summary>
        /// Saves an image in the named file.
        /// </summary>
        /// <param name="filename">Name of the file to save the image to.</param>
        /// <param name="mimetype">MIME type string (for example 'image/jpeg').</param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SaveToFile(string filename, string mimetype)
        {
            return UI.Native.GenericImage.SaveFileWithMimeType(Handle, filename, mimetype);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SaveToFile(string filename)
        {
            return UI.Native.GenericImage.SaveFile(Handle, filename);
        }

        /// <summary>
        /// Saves an image in the given stream.
        /// </summary>
        /// <param name="stream">Output stream</param>
        /// <param name="type"></param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SaveToStream(Stream stream, BitmapType type)
        {
            var outputStream = new UI.Native.OutputStream(stream);
            return UI.Native.GenericImage.SaveStreamWithBitmapType(Handle, outputStream, (int)type);
        }

        /// <summary>
        /// Sets the image data without performing checks.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="new_width"></param>
        /// <param name="new_height"></param>
        /// <param name="static_data"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetNativeData(
            IntPtr data,
            int new_width,
            int new_height,
            bool static_data = false)
        {
            UI.Native.GenericImage.SetDataWithSize(Handle, data, new_width, new_height, static_data);
        }

        /// <summary>
        /// Returns pointer to the array storing the alpha values for this image.
        /// </summary>
        /// <returns>This pointer is NULL for the images without the alpha channel.
        /// If the image does have it, this pointer may be used to directly manipulate the
        /// alpha values which are stored as the RGB ones.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IntPtr GetNativeAlphaData()
        {
            return UI.Native.GenericImage.GetAlphaData(Handle);
        }

        /// <summary>
        /// Returns the image data as an array.
        /// </summary>
        /// <remarks>
        /// This is most often used when doing direct image manipulation. The return value
        /// points to an array of bytes in RGBRGBRGB...format in the top-to-bottom,
        /// left-to-right order, that is the first RGB triplet corresponds to the first pixel
        /// of the first row, the second one — to the second pixel of the first row and so on
        /// until the end of the first row, with second row following after it and so on.
        /// You should not delete the returned pointer nor pass it to
        /// <see cref="SetNativeData(IntPtr, bool)"/> and similar methods.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IntPtr GetNativeData()
        {
            return UI.Native.GenericImage.GetData(Handle);
        }

        /// <summary>
        /// Creates a fresh image.
        /// </summary>
        /// <param name="width">Specifies the width of the image.</param>
        /// <param name="height">Specifies the height of the image.</param>
        /// <param name="data">A pointer to RGB data</param>
        /// <param name="staticData">Indicates if the data should be free'd after use.
        /// If <paramref name="staticData"/> is <c>false</c> then the
        /// library will take ownership of the data and free it afterwards.For this,
        /// it has to be allocated with malloc.</param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CreateNativeData(int width, int height, IntPtr data, bool staticData = false)
        {
            return UI.Native.GenericImage.CreateData(Handle, width, height, data, staticData);
        }

        /// <summary>
        /// Creates a fresh image.
        /// </summary>
        /// <param name="width">Specifies the width of the image.</param>
        /// <param name="height">Specifies the height of the image.</param>
        /// <param name="data">A pointer to RGB data</param>
        /// <param name="staticData">Indicates if the data should be free'd after use.
        /// If <paramref name="staticData"/> is <c>false</c> then the
        /// library will take ownership of the data and free it afterwards.For this,
        /// it has to be allocated with malloc.</param>
        /// <param name="alpha">A pointer to alpha-channel data</param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CreateNativeData(
            int width,
            int height,
            IntPtr data,
            IntPtr alpha,
            bool staticData = false)
        {
            return UI.Native.GenericImage.CreateAlphaData(Handle, width, height, data, alpha, staticData);
        }

        /// <summary>
        /// Sets the image data without performing checks.
        /// </summary>
        /// <param name="alpha">Pointer to alpha channel data.</param>
        /// <param name="staticData">Specifies whether data is static.</param>
        /// <remarks>
        /// This function is similar to <see cref="SetNativeData(IntPtr, bool)"/>
        /// and has similar restrictions.
        /// </remarks>
        /// <remarks>
        /// The pointer passed to it may however be <c>null</c> in which case the function will
        /// allocate the alpha array internally – this is useful to add alpha channel
        /// data to an image which doesn't have any.
        /// </remarks>
        /// <remarks>
        /// If the pointer is not <c>null</c>, it must have one byte for each image
        /// pixel and be allocated with malloc(). Library takes ownership of the pointer
        /// and will free it unless <paramref name="staticData"/> parameter is set
        /// to <c>true</c> – in this case the caller should do it.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetNativeAlphaData(IntPtr alpha = default, bool staticData = false)
        {
            UI.Native.GenericImage.SetAlphaData(Handle, alpha, staticData);
        }

        /// <summary>
        /// Sets the image data without performing checks.
        /// </summary>
        /// <param name="data">Pointer to RGB data.</param>
        /// <param name="staticData">Specifies whether data is static.</param>
        /// <remarks>
        /// The data given must have the size (width*height*3) or results will be unexpected.
        /// Don't use this method if you aren't sure you know what you are doing.
        /// </remarks>
        /// <remarks>
        /// The data must have been allocated with malloc(), NOT with operator new.
        /// </remarks>
        /// <remarks>
        /// If <paramref name="staticData"/> is false, after this call the pointer to the data is owned
        /// by the Library, that will be responsible for deleting it. Do not pass to
        /// this function a pointer obtained through GetData().
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetNativeData(IntPtr data, bool staticData = false)
        {
            UI.Native.GenericImage.SetData(Handle, data, staticData);
        }

        /// <summary>
        /// Register an image handler.
        /// </summary>
        /// <param name="handler"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void AddHandler(IntPtr handler)
        {
            UI.Native.GenericImage.AddHandler(handler);
        }

        /// <summary>
        /// Finds the handler with the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static IntPtr FindHandler(string name)
        {
            return UI.Native.GenericImage.FindHandlerByName(name);
        }

        /// <summary>
        /// Finds the handler associated with the given extension and type.
        /// </summary>
        /// <param name="extension"></param>
        /// <param name="bitmapType"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static IntPtr FindHandler(string extension, BitmapType bitmapType)
        {
            return UI.Native.GenericImage.FindHandlerByExt(extension, (int)bitmapType);
        }

        /// <summary>
        /// Finds the handler associated with the given image type.
        /// </summary>
        /// <param name="bitmapType"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static IntPtr FindHandler(BitmapType bitmapType)
        {
            return UI.Native.GenericImage.FindHandlerByBitmapType((int)bitmapType);
        }

        /// <summary>
        /// Finds the handler associated with the given MIME type.
        /// </summary>
        /// <param name="mimetype">MIME type string (for example 'image/jpeg').</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static IntPtr FindHandlerMime(string mimetype)
        {
            return UI.Native.GenericImage.FindHandlerByMime(mimetype);
        }

        /// <summary>
        /// Adds a handler at the start of the static list of format handlers.
        /// </summary>
        /// <param name="handler"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void InsertHandler(IntPtr handler)
        {
            UI.Native.GenericImage.InsertHandler(handler);
        }

        /// <inheritdoc/>
        protected override void DisposeUnmanagedResources()
        {
            UI.Native.GenericImage.DeleteImage(Handle);
        }
    }
}