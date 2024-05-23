using System;
using System.IO;
using System.Runtime.CompilerServices;
using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements platform independent image.
    /// </summary>
    public class GenericImage : HandledObject<object>
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
            Handler = NativeDrawing.Default.CreateGenericImage(width, height, clear);
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
            Handler = NativeDrawing.Default.CreateGenericImage(size.Width, size.Height, clear);
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
        {
            Handler = NativeDrawing.Default.CreateGenericImage(
                    fileName,
                    bitmapType,
                    index);
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
        {
            Handler = NativeDrawing.Default.CreateGenericImage(name, mimetype, index);
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
            Handler = NativeDrawing.Default.CreateGenericImage(
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
            Handler = NativeDrawing.Default.CreateGenericImage(
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
        /// <param name="data">A pointer to RGB data</param>
        /// <param name="staticData">Indicates if the data should be free'd after use.
        /// If static_data is <c>false</c> then the
        /// library will take ownership of the data and free it afterwards.For this,
        /// it has to be allocated with
        /// malloc.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage(int width, int height, IntPtr data, bool staticData = false)
        {
            Handler = NativeDrawing.Default.CreateGenericImage(width, height, data, staticData);
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
        {
            Handler = NativeDrawing.Default.CreateGenericImage(width, height, data, alpha, staticData);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage(object handle)
        {
            Handler = handle;
        }

        /// <summary>
        /// Gets the width of the image in pixels.
        /// </summary>
        public int Width
        {
            get => NativeDrawing.Default.GetGenericImageWidth(this);
        }

        /// <summary>
        /// Gets the height of the image in pixels.
        /// </summary>
        public int Height
        {
            get => NativeDrawing.Default.GetGenericImageHeight(this);
        }

        /// <summary>
        /// Returns the size of the image in pixels.
        /// </summary>
        public SizeI Size
        {
            get => new(Width, Height);
        }

        /// <summary>
        /// Returns the bounds of the image in pixels. Result is (0, 0, Width, Height).
        /// </summary>
        public RectI Bounds
        {
            get => (0, 0, Width, Height);
        }

        /// <summary>
        /// Gets number of pixels in this image (Width * Height).
        /// </summary>
        public int PixelCount => Size.PixelCount;

        /// <summary>
        /// Returns <c>true</c> if image data is present.
        /// </summary>
        public bool IsOk
        {
            get => NativeDrawing.Default.GetGenericImageIsOk(this);
        }

        /// <summary>
        /// Returns <c>true</c> if at least one of the available image handlers can read the file
        /// with the given name.
        /// </summary>
        /// <param name="filename">Name of the file from which to load the image.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanRead(string filename)
        {
            return NativeDrawing.Default.GenericImageCanRead(filename);
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
            return NativeDrawing.Default.GenericImageCanRead(stream);
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
            return NativeDrawing.Default.GetGenericImageExtWildcard();
        }

        /// <summary>
        /// Finds image load/save handler with the given name, and removes it.
        /// </summary>
        /// <param name="name">Name of the handler.</param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool RemoveHandler(string name)
        {
            return NativeDrawing.Default.GenericImageRemoveHandler(name);
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
            return NativeDrawing.Default.GetGenericImageCount(filename, bitmapType);
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
            return NativeDrawing.Default.GetGenericImageCount(stream, bitmapType);
        }

        /// <summary>
        /// Deletes all image handlers.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CleanUpHandlers()
        {
            NativeDrawing.Default.GenericImageCleanUpHandlers();
        }

        /// <summary>
        /// Returns the currently used default file load flags.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenericImageLoadFlags GetDefaultLoadFlags()
        {
            return NativeDrawing.Default.GetGenericImageDefaultLoadFlags();
        }

        /// <summary>
        /// Sets the default value for the flags used for loading image files.
        /// </summary>
        /// <param name="flags"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetDefaultLoadFlags(GenericImageLoadFlags flags)
        {
            NativeDrawing.Default.SetGenericImageDefaultLoadFlags(flags);
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
            NativeDrawing.Default.GenericImageSetAlpha(this, x, y, alpha);
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
            NativeDrawing.Default.GenericImageClearAlpha(this);
        }

        /// <summary>
        /// Specifies whether there is a mask or not.
        /// </summary>
        /// <param name="hasMask"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetMask(bool hasMask = true)
        {
            NativeDrawing.Default.GenericImageSetMask(this, hasMask);
        }

        /// <summary>
        /// Sets the mask color for this image(and tells the image to use the mask).
        /// </summary>
        /// <param name="rgb">Color RGB.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetMaskColor(RGBValue rgb)
        {
            NativeDrawing.Default.GenericImageSetMaskColor(this, rgb);
        }

        /// <summary>
        /// Makes this image grayscaled.
        /// </summary>
        public void ChangeToGrayScale()
        {
            static void ChangePixel(ref RGBValue rgb, int value)
            {
                byte color = (byte)((0.299 * rgb.R) + (0.587 * rgb.G) + (0.114 * rgb.B));
                rgb.R = rgb.G = rgb.B = color;
            }

            ForEachPixel<int>(ChangePixel, 0);
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
        /// Sets the alpha value for the each pixel.
        /// </summary>
        /// <param name="value">New alpha channel (transparency) value of the pixels.</param>
        /// <remarks>
        /// This function should only be called if the image has alpha channel data,
        /// use <see cref="HasAlpha"/> to check for this.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetAlpha(byte value)
        {
            var nalpha = GetNativeAlphaData();

            byte* alpha = (byte*)nalpha;

            var height = Height;
            var width = Width;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    *alpha++ = value;
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
            return NativeDrawing.Default.GenericImageSetMaskFromImage(this, image, mask);
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
            NativeDrawing.Default.GenericImageSetOptionAsString(this, name, value);
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
            NativeDrawing.Default.GenericImageSetOptionAsInt(this, name, value);
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
            NativeDrawing.Default.GenericImageSetRGB(this, x, y, rgb);
        }

        /// <summary>
        /// Sets the color of the pixels within the given rectangle.
        /// </summary>
        /// <param name="rect">Rectangle within the image. If rectangle is null,
        /// <see cref="Bounds"/> property is used.</param>
        /// <param name="rgb">RGB Color.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetRGBRect(RGBValue rgb, RectI? rect = null)
        {
            NativeDrawing.Default.GenericImageSetRGBRect(this, rgb, rect);
        }

        /// <summary>
        /// Sets the type of image returned by GetType().
        /// </summary>
        /// <param name="type">Type of the bitmap.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetImageType(BitmapType type)
        {
            NativeDrawing.Default.GenericImageSetImageType(this, type);
        }

        /// <summary>
        /// Returns an identical copy of this image.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage Copy()
        {
            return NativeDrawing.Default.GenericImageCopy(this);
        }

        /// <summary>
        /// Creates a fresh image.
        /// </summary>
        /// <param name="width">New image width.</param>
        /// <param name="height">New image height</param>
        /// <param name="clear">If true, initialize the image to black.</param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Reset(int width, int height, bool clear = false)
        {
            return NativeDrawing.Default.GenericImageReset(this, width, height, clear);
        }

        /// <summary>
        /// Initialize the image data with zeroes (the default) or with the byte value given as value.
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear(byte value = 0)
        {
            NativeDrawing.Default.GenericImageClear(this, value);
        }

        /// <summary>
        /// Destroys the image data.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            NativeDrawing.Default.GenericImageReset(this);
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
            return NativeDrawing.Default.GenericImageFindFirstUnusedColor(this, startRGB);
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
            if (HasAlpha())
                return;
            NativeDrawing.Default.GenericImageInitAlpha(this);
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
            return NativeDrawing.Default.GenericImageBlur(this, blurRadius);
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
            return NativeDrawing.Default.GenericImageBlurHorizontal(this, blurRadius);
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
            return NativeDrawing.Default.GenericImageBlurVertical(this, blurRadius);
        }

        /// <summary>
        /// Returns a mirrored copy of the image.
        /// </summary>
        /// <param name="horizontally"></param>
        /// <returns>Mirrored copy of the image</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage Mirror(bool horizontally = true)
        {
            return NativeDrawing.Default.GenericImageMirror(this, horizontally);
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
            NativeDrawing.Default.GenericImagePaste(this, image, x, y, alphaBlend);
        }

        /// <summary>
        /// Replaces the color specified by (r1.R, r1.G, r1.B) by the color (r2.R, r2.G, r2.B).
        /// </summary>
        /// <param name="r1">RGB Color 1.</param>
        /// <param name="r2">RGB Color 2.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Replace(RGBValue r1, RGBValue r2)
        {
            NativeDrawing.Default.GenericImageReplace(this, r1, r2);
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
            NativeDrawing.Default.GenericImageRescale(this, width, height, quality);
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
            SizeI size,
            PointI pos,
            RGBValue? color = null)
        {
            NativeDrawing.Default.GenericImageResizeNoScale(this, size, pos, color);
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
        public GenericImage SizeNoScale(SizeI size, PointI pos = default, RGBValue? color = null)
        {
            return NativeDrawing.Default.GenericImageSizeNoScale(this, size, pos, color);
        }

        /// <summary>
        /// Returns a copy of the image rotated 90 degrees in the direction indicated by clockwise.
        /// </summary>
        /// <param name="clockwise">Rotate direction.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage Rotate90(bool clockwise = true)
        {
            return NativeDrawing.Default.GenericImageRotate90(this, clockwise);
        }

        /// <summary>
        /// Returns a copy of the image rotated by 180 degrees.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage Rotate180()
        {
            return NativeDrawing.Default.GenericImageRotate180(this);
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
            NativeDrawing.Default.GenericImageRotateHue(this, angle);
        }

        /// <summary>
        /// Changes the saturation of each pixel in the image.
        /// </summary>
        /// <param name="factor">A double in the range [-1.0..+1.0], where -1.0 corresponds
        /// to -100 percent and +1.0 corresponds to +100 percent.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ChangeSaturation(double factor)
        {
            NativeDrawing.Default.GenericImageChangeSaturation(this, factor);
        }

        /// <summary>
        /// Changes the brightness(value) of each pixel in the image.
        /// </summary>
        /// <param name="factor">A double in the range [-1.0..+1.0], where -1.0 corresponds
        /// to -100 percent and +1.0 corresponds to +100 percent.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ChangeBrightness(double factor)
        {
            NativeDrawing.Default.GenericImageChangeBrightness(this, factor);
        }

        /// <summary>
        /// Returns the file load flags used for this object.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImageLoadFlags GetLoadFlags()
        {
            return NativeDrawing.Default.GenericImageGetLoadFlags(this);
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
            NativeDrawing.Default.GenericImageSetLoadFlags(this, flags);
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
            NativeDrawing.Default.GenericImageChangeHSV(this, angleH, factorS, factorV);
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
            return NativeDrawing.Default.GenericImageScale(this, width, height, quality);
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
            return NativeDrawing.Default.GenericImageConvertAlphaToMask(this, threshold);
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
            return NativeDrawing.Default.GenericImageConvertAlphaToMask(
                this,
                rgb,
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
            return NativeDrawing.Default.GenericImageConvertToGreyscale(this, weightR, weightG, weightB);
        }

        /// <summary>
        /// Returns a greyscale version of the image.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage ConvertToGreyscale()
        {
            return NativeDrawing.Default.GenericImageConvertToGreyscale(this);
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
            return NativeDrawing.Default.GenericImageConvertToMono(this, rgb);
        }

        /// <summary>
        /// Returns disabled(dimmed) version of the image.
        /// </summary>
        /// <param name="brightness"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage ConvertToDisabled(byte brightness = 255)
        {
            return NativeDrawing.Default.GenericImageConvertToDisabled(this, brightness);
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
            return NativeDrawing.Default.GenericImageChangeLightness(this, ialpha);
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
            return NativeDrawing.Default.GenericImageGetAlpha(this, x, y);
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
            return NativeDrawing.Default.GenericImageGetRGB(this, x, y);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color GetPixel(int x, int y, bool withAlpha = false)
        {
            return NativeDrawing.Default.GenericImageGetPixel(this, x, y, withAlpha);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetPixel(int x, int y, Color color, bool withAlpha = false)
        {
            NativeDrawing.Default.GenericImageSetPixel(this, x, y, color, withAlpha);
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
            return NativeDrawing.Default.GenericImageGetRed(this, x, y);
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
            return NativeDrawing.Default.GenericImageGetGreen(this, x, y);
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
            return NativeDrawing.Default.GenericImageGetBlue(this, x, y);
        }

        /// <summary>
        /// Gets the <see cref="RGBValue"/> value of the mask color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RGBValue GetMaskRGB()
        {
            return NativeDrawing.Default.GenericImageGetMaskRGB(this);
        }

        /// <summary>
        /// Gets the red value of the mask color.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaskRed()
        {
            return NativeDrawing.Default.GenericImageGetMaskRed(this);
        }

        /// <summary>
        /// Gets the green value of the mask color.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaskGreen()
        {
            return NativeDrawing.Default.GenericImageGetMaskGreen(this);
        }

        /// <summary>
        /// Gets the blue value of the mask color.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaskBlue()
        {
            return NativeDrawing.Default.GenericImageGetMaskBlue(this);
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
            return NativeDrawing.Default.GenericImageGetOptionAsString(this, name);
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
            return NativeDrawing.Default.GenericImageGetOptionAsInt(this, name);
        }

        /// <summary>
        /// Returns a sub image of the current one as long as the rect belongs entirely to the image.
        /// </summary>
        /// <param name="rect">Bounds of the sub-image.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage GetSubImage(RectI rect)
        {
            return NativeDrawing.Default.GenericImageGetSubImage(this, rect);
        }

        /// <summary>
        /// Gets the type of image found when image was loaded or specified when image was saved.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BitmapType GetImageType()
        {
            return NativeDrawing.Default.GenericImageGetImageType(this);
        }

        /// <summary>
        /// Returns <c>true</c> if this image has alpha channel, <c>false</c> otherwise.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAlpha()
        {
            return NativeDrawing.Default.GenericImageHasAlpha(this);
        }

        /// <summary>
        /// Returns <c>true</c> if there is a mask active, <c>false</c> otherwise.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasMask()
        {
            return NativeDrawing.Default.GenericImageHasMask(this);
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
            return NativeDrawing.Default.GenericImageHasOption(this, name);
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
            return NativeDrawing.Default.GenericImageIsTransparent(this, x, y, threshold);
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
            return NativeDrawing.Default.GenericImageLoadFromStream(
                this,
                stream,
                bitmapType,
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
            return NativeDrawing.Default.GenericImageLoadFromFile(
                this,
                filename,
                bitmapType,
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
            return NativeDrawing.Default.GenericImageLoadFromFile(this, name, mimetype, index);
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
            return NativeDrawing.Default.GenericImageLoadFromStream(this, stream, mimetype, index);
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
            return NativeDrawing.Default.GenericImageSaveToStream(this, stream, mimetype);
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
            return NativeDrawing.Default.GenericImageSaveToFile(this, filename, bitmapType);
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
            return NativeDrawing.Default.GenericImageSaveToFile(this, filename, mimetype);
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
            return NativeDrawing.Default.GenericImageSaveToFile(this, filename);
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
            return NativeDrawing.Default.GenericImageSaveToStream(this, stream, type);
        }

        /// <summary>
        /// Sets the image data without performing checks.
        /// </summary>
        /// <param name="width">Specifies the width of the image.</param>
        /// <param name="height">Specifies the height of the image.</param>
        /// <param name="data">A pointer to RGB data</param>
        /// <param name="staticData">Indicates if the data should be free'd after use.
        /// If <paramref name="staticData"/> is <c>false</c> then the
        /// library will take ownership of the data and free it afterwards.For this,
        /// it has to be allocated with malloc.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetNativeData(
            IntPtr data,
            int width,
            int height,
            bool staticData = false)
        {
            NativeDrawing.Default.GenericImageSetNativeData(this, data, width, height, staticData);
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
            return NativeDrawing.Default.GenericImageGetNativeAlphaData(this);
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
            return NativeDrawing.Default.GenericImageGetNativeData(this);
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
            return NativeDrawing.Default.GenericImageCreateNativeData(
                this,
                width,
                height,
                data,
                staticData);
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
            return NativeDrawing.Default.GenericImageCreateNativeData(
                this,
                width,
                height,
                data,
                alpha,
                staticData);
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
            NativeDrawing.Default.GenericImageSetNativeAlphaData(this, alpha, staticData);
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
            NativeDrawing.Default.GenericImageSetNativeData(this, data, staticData);
        }

        /// <inheritdoc/>
        protected override object CreateHandler()
        {
            return NativeDrawing.Default.CreateGenericImage();
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            NativeDrawing.Default.DisposeGenericImage(this);
        }
    }
}