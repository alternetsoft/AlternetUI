using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
        /// <param name="bitmapType">Type of the bitmap. Depending on how library and OS has been configured and
        /// by which handlers have been loaded, not all formats may be available. If value is
        /// <see cref="BitmapType.Any"/>, function will try to autodetect the format.</param>
        /// <param name="index">Index of the image to load in the case that the image file contains multiple images.
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
        /// <param name="index">See description in <see cref="GenericImage(string, BitmapType, int)"/></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage(string name, string mimetype, int index = -1)
            : base(UI.Native.GenericImage.CreateImageFromFileWithMimeType(name, mimetype, index), true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image from a stream.
        /// </summary>
        /// <param name="stream">Opened input stream from which to load the image. Currently, the stream must support seeking.</param>
        /// <param name="bitmapType">See description in <see cref="GenericImage(string, BitmapType, int)"/>.</param>
        /// <param name="index">See description in <see cref="GenericImage(string, BitmapType, int)"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage(Stream stream, BitmapType bitmapType = BitmapType.Any, int index = -1)
            : this(new UI.Native.InputStream(stream), bitmapType, index)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image from a stream.
        /// </summary>
        /// <param name="stream">Opened input stream from which to load the image. Currently, the stream must support seeking.</param>
        /// <param name="mimeType">MIME type string (for example 'image/jpeg').</param>
        /// <param name="index">See description in <see cref="GenericImage(string, BitmapType, int)"/></param>
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
        /// <param name="staticData">Indicates if the data should be free'd after use. If static_data is false then the
        /// library will take ownership of the data and free it afterwards.For this, it has to be allocated with
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
        /// <param name="staticData">Indicates if the data should be free'd after use. If static_data is false then the
        /// library will take ownership of the data and free it afterwards.For this, it has to be allocated with
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
        /// <param name="stream">Opened input stream from which to load the image. Currently, the stream must support seeking.</param>
        /// <param name="bitmapType">Type of the bitmap. Depending on how library and OS has been configured and
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
        /// Returns true if at least one of the available image handlers can read the file
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
        /// Returns true if at least one of the available image handlers can read the data in
        /// the given stream.
        /// </summary>
        /// <param name="stream">Opened input stream from which to load the image. Currently, the stream must support seeking.</param>
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
        /// <returns></returns>
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
        /// <param name="bitmapType">Type of the bitmap. Depending on how library and OS has been configured and
        /// by which handlers have been loaded, not all formats may be available. If value is
        /// <see cref="BitmapType.Any"/>, function will try to autodetect the format.</param>
        /// <returns></returns>
        /*
         
         */
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
        /// <param name="stream">Opened input stream from which to load the image. Currently, the stream must support seeking.</param>
        /// <param name="bitmapType">Type of the bitmap. Depending on how library and OS has been configured and
        /// by which handlers have been loaded, not all formats may be available. If value is
        /// <see cref="BitmapType.Any"/>, function will try to autodetect the format.</param>
        /// <returns></returns>
        /*
         
         */
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
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenericImageLoadFlags GetDefaultLoadFlags()
        {
            return (GenericImageLoadFlags)UI.Native.GenericImage.GetDefaultLoadFlags();
        }

        /// <summary>
        /// Sets the default value for the flags used for loading image files.
        /// </summary>
        /// <param name="flags"></param>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetDefaultLoadFlags(GenericImageLoadFlags flags)
        {
            UI.Native.GenericImage.SetDefaultLoadFlags((int)flags);
        }

        /// <summary>
        /// Sets the alpha value for the given pixel.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="alpha"></param>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetAlpha(int x, int y, byte alpha)
        {
            UI.Native.GenericImage.SetAlpha(Handle, x, y, alpha);
        }

        /// <summary>
        /// Removes the alpha channel from the image.
        /// </summary>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearAlpha()
        {
            UI.Native.GenericImage.ClearAlpha(Handle);
        }

        /// <summary>
        /// Specifies whether there is a mask or not.
        /// </summary>
        /// <param name="hasMask"></param>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetMask(bool hasMask = true)
        {
            UI.Native.GenericImage.SetMask(Handle, hasMask);
        }

        /// <summary>
        /// Sets the mask color for this image(and tells the image to use the mask).
        /// </summary>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetMaskColor(byte red, byte green, byte blue)
        {
            UI.Native.GenericImage.SetMaskColor(Handle, red, green, blue);
        }

        /// <summary>
        /// Sets image's mask so that the pixels that have RGB value of mr,mg,mb in
        /// mask will be masked in the image.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="mr"></param>
        /// <param name="mg"></param>
        /// <param name="mb"></param>
        /// <returns></returns>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SetMaskFromImage(GenericImage image, byte mr, byte mg, byte mb)
        {
            return UI.Native.GenericImage.SetMaskFromImage(Handle, image.Handle, mr, mg, mb);
        }

        /// <summary>
        /// Sets a user-defined option.
        /// </summary>
        /// <remarks>
        /// See <see cref="GenericImageOptionNames"/> for known option names.
        /// </remarks>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /*
         
         */
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
        /// <param name="name"></param>
        /// <param name="value"></param>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetOptionAsInt(string name, int value)
        {
            UI.Native.GenericImage.SetOptionInt(Handle, name, value);
        }

        /// <summary>
        /// Sets the color of the pixel at the given x and y coordinate.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /*

        */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetRGB(int x, int y, byte r, byte g, byte b)
        {
            UI.Native.GenericImage.SetRGB(Handle, x, y, r, g, b);
        }

        /// <summary>
        /// Sets the color of the pixels within the given rectangle.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetRGBRect(Int32Rect rect, byte red, byte green, byte blue)
        {
            UI.Native.GenericImage.SetRGBRect(Handle, rect, red, green, blue);
        }

        /// <summary>
        /// Sets the type of image returned by GetType().
        /// </summary>
        /// <param name="type"></param>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetImageType(BitmapType type)
        {
            UI.Native.GenericImage.SetImageType(Handle, (int)type);
        }

        /// <summary>
        /// Returns an identical copy of this image.
        /// </summary>
        /// <returns></returns>
        /*
         
         */
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
        /// <returns></returns>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Reset(int width, int height, bool clear = true)
        {
            return UI.Native.GenericImage.CreateFreshImage(Handle, width, height, clear);
        }

        /// <summary>
        /// Initialize the image data with zeroes (the default) or with the byte value given as value.
        /// </summary>
        /// <param name="value"></param>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear(byte value = 0)
        {
            UI.Native.GenericImage.Clear(Handle, value);
        }

        /// <summary>
        /// Destroys the image data.
        /// </summary>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            UI.Native.GenericImage.DestroyImageData(Handle);
        }

        /// <summary>
        /// Initializes the image alpha channel data.
        /// </summary>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InitAlpha()
        {
            UI.Native.GenericImage.InitAlpha(Handle);
        }

        /// <summary>
        /// Blurs the image in both horizontal and vertical directions by the specified
        /// pixel blurRadius.
        /// </summary>
        /// <param name="blurRadius"></param>
        /// <returns></returns>
        /*
   This should not be used when using a single mask color for transparency.
      
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage Blur(int blurRadius)
        {
            return new(UI.Native.GenericImage.Blur(Handle, blurRadius));
        }

        /// <summary>
        /// Blurs the image in the horizontal direction only.
        /// </summary>
        /// <param name="blurRadius"></param>
        /// <returns></returns>
        /*
        Blurs the image in the horizontal direction only.

This should not be used when using a single mask color for transparency.
 
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage BlurHorizontal(int blurRadius)
        {
            return new(UI.Native.GenericImage.BlurHorizontal(Handle, blurRadius));
        }

        /// <summary>
        /// Blurs the image in the vertical direction only.
        /// </summary>
        /// <param name="blurRadius"></param>
        /// <returns></returns>
        /*
      This should not be used when using a single mask color for transparency.
   
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage BlurVertical(int blurRadius)
        {
            return new(UI.Native.GenericImage.BlurVertical(Handle, blurRadius));
        }

        /// <summary>
        /// Returns a mirrored copy of the image.
        /// </summary>
        /// <param name="horizontally"></param>
        /// <returns></returns>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage Mirror(bool horizontally = true)
        {
            return new(UI.Native.GenericImage.Mirror(Handle, horizontally));
        }

        /// <summary>
        /// Copy the data of the given image to the specified position in this image.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="alphaBlend"></param>
        /*
         
         */
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
        /// Replaces the color specified by (r1, g1, b1) by the color (r2, g2, b2).
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="g1"></param>
        /// <param name="b1"></param>
        /// <param name="r2"></param>
        /// <param name="g2"></param>
        /// <param name="b2"></param>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Replace(byte r1, byte g1, byte b1, byte r2, byte g2, byte b2)
        {
            UI.Native.GenericImage.Replace(Handle, r1, g1, b1, r2, g2, b2);
        }

        /// <summary>
        /// Changes the size of the image in-place by scaling it: after a call to this
        /// function,the image will have the given width and height.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="quality"></param>
        /*
         
         */
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
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ResizeNoScale(
            Int32Size size,
            Int32Point pos,
            int red = -1,
            int green = -1,
            int blue = -1)
        {
            UI.Native.GenericImage.Resize(Handle, size, pos, red, green, blue);
        }

        /// <summary>
        /// Returns a resized version of this image without scaling it by adding either a
        /// border with the given color or cropping as necessary.
        /// </summary>
        /// <param name="size">New size.</param>
        /// <param name="pos">Position in the new image.</param>
        /// <param name="red">R component of the fill color.</param>
        /// <param name="green">G component of the fill color.</param>
        /// <param name="blue">B component of the fill color.</param>
        /// <returns></returns>
        /// <remarks>
        /// The image is pasted into a new image with the given <paramref name="size"/> and
        /// background color at the
        /// position <paramref name="pos"/> relative to the upper left of the new image.
        /// </remarks>
        /// <remarks>
        /// If red = green = blue = -1 then the areas of the larger image not covered by
        /// this image are made transparent by filling them with the image mask
        /// color(which will be allocated automatically if it isn't currently set).
        /// Otherwise, the areas will be filled with the color with the specified RGB components.
        /// </remarks>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage SizeNoScale(Int32Size size, Int32Point pos, int red, int green, int blue)
        {
            var image = UI.Native.GenericImage.Size(Handle, size, pos, red, green, blue);
            return new GenericImage(image);
        }

        /// <summary>
        /// Returns a copy of the image rotated 90 degrees in the direction indicated by clockwise.
        /// </summary>
        /// <param name="clockwise"></param>
        /// <returns></returns>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage Rotate90(bool clockwise = true)
        {
            return new(UI.Native.GenericImage.Rotate90(Handle, clockwise));
        }

        /// <summary>
        /// Returns a copy of the image rotated by 180 degrees.
        /// </summary>
        /// <returns></returns>
        /*
         
         */
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
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RotateHue(double angle)
        {
            UI.Native.GenericImage.RotateHue(Handle, angle);
        }

        /// <summary>
        /// Changes the saturation of each pixel in the image.
        /// </summary>
        /// <param name="factor"></param>
        /*
    factor is a double in the range [-1.0..+1.0], where -1.0 corresponds to -100 percent and +1.0
corresponds to +100 percent.
     
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ChangeSaturation(double factor)
        {
            UI.Native.GenericImage.ChangeSaturation(Handle, factor);
        }

        /// <summary>
        /// Changes the brightness(value) of each pixel in the image.
        /// </summary>
        /// <param name="factor"></param>
        /*
       factor is a double in the range [-1.0..+1.0], where -1.0 corresponds to -100 percent
and +1.0 corresponds to +100 percent.  
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ChangeBrightness(double factor)
        {
            UI.Native.GenericImage.ChangeBrightness(Handle, factor);
        }

        /// <summary>
        /// Returns the file load flags used for this object.
        /// </summary>
        /// <returns></returns>
        /*
         
         */
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
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetLoadFlags(GenericImageLoadFlags flags)
        {
            UI.Native.GenericImage.SetLoadFlags(Handle, (int)flags);
        }

        /// <summary>
        /// Changes the hue, the saturation and the brightness(value) of each pixel in the image.
        /// </summary>
        /// <param name="angleH"></param>
        /// <param name="factorS"></param>
        /// <param name="factorV"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        /*
    angleH is a double in the range [-1.0..+1.0], where -1.0 corresponds to -360 degrees and
+1.0 corresponds to +360 degrees, factorS is a double in the range [-1.0..+1.0], where -1.0
corresponds to -100 percent and +1.0 corresponds to +100 percent and factorV is a double in
the range [-1.0..+1.0], where -1.0 corresponds to -100 percent and +1.0 corresponds to +100 percent.
     
         */
        public void ChangeHSV(double angleH, double factorS, double factorV)
        {
            UI.Native.GenericImage.ChangeHSV(Handle, angleH, factorS, factorV);
        }

        /// <summary>
        /// Returns a scaled version of the image.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="quality"></param>
        /// <returns></returns>
        /*
   This is also useful for scaling bitmaps in general as the only other way to scale bitmaps is to
blit a wxMemoryDC into another wxMemoryDC.

The parameter quality determines what method to use for resampling the image, see wxImageResizeQuality
documentation.

It should be noted that although using wxIMAGE_QUALITY_HIGH produces much nicer looking results it is
a slower method. Downsampling will use the box averaging method which seems to operate very fast. If you are upsampling larger images using this method you will most likely notice that it is a bit slower and in extreme cases it will be quite substantially slower as the bicubic algorithm has to process a lot of data.

It should also be noted that the high quality scaling may not work as expected when using a single
mask color for transparency, as the scaling will blur the image and will therefore remove the mask
partially. Using the alpha channel will work.

Note
The algorithm used for the default (normal) quality value doesn't work with images larger
than 65536 (2^16) pixels in either dimension for 32-bit programs. For 64-bit programs the
limit is 2^48 and so not relevant in practice.
      
         */
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
        /// <returns></returns>
        /*
    If the image has an alpha channel, all pixels with alpha value less than threshold are replaced
with the mask color and the alpha channel is removed. Otherwise nothing is done.

The mask color is chosen automatically using FindFirstUnusedcolor(), see the overload below if
this is not appropriate.

Returns
Returns true on success, false on error.
     
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ConvertAlphaToMask(byte threshold = AlphaChannelThreshold)
        {
            return UI.Native.GenericImage.ConvertAlphaToMask(Handle, threshold);
        }

        /// <summary>
        /// If the image has alpha channel, this method converts it to mask using the
        /// specified color as the mask color.
        /// </summary>
        /// <param name="mr"></param>
        /// <param name="mg"></param>
        /// <param name="mb"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        /*
If the image has an alpha channel, all pixels with alpha value less than threshold are replaced
with the mask color and the alpha channel is removed. Otherwise nothing is done.

Parameters
mr	The red component of the mask color.
mg	The green component of the mask color.
mb	The blue component of the mask color.
threshold	Pixels with alpha channel values below the given threshold are considered to be
transparent, i.e. the corresponding mask pixels are set. Pixels with the alpha values above the
threshold are considered to be opaque.
Returns
Returns true on success, false on error.
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ConvertAlphaToMask(
            byte mr,
            byte mg,
            byte mb,
            byte threshold = AlphaChannelThreshold)
        {
            return UI.Native.GenericImage.ConvertAlphaToMaskUseColor(Handle, mr, mg, mb,threshold);
        }

        /// <summary>
        /// Returns a greyscale version of the image.
        /// </summary>
        /// <param name="weightR"></param>
        /// <param name="weightG"></param>
        /// <param name="weightB"></param>
        /// <returns></returns>
        /*
    The returned image uses the luminance component of the original to calculate the greyscale.
Defaults to using the standard ITU-T BT.601 when converting to YUV, where every pixel
equals (R * weight_r) + (G * weight_g) + (B * weight_b).
     
         */
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
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /*
      The returned image has white color where the original has (r,g,b) color and black color
everywhere else.
   
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage ConvertToMono(byte r, byte g, byte b)
        {
            return new(UI.Native.GenericImage.ConvertToMono(Handle, r, g, b));
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
        /// <param name="alpha"></param>
        /// <returns></returns>
        /*
    This utility function simply darkens or lightens a color, based on the specified percentage
ialpha. ialpha of 0 would make the color completely black, 200 completely white and 100 would
not change the color.
     
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage ChangeLightness(int alpha)
        {
            return new(UI.Native.GenericImage.ChangeLightness(Handle, alpha));
        }

        /// <summary>
        /// Return alpha value at given pixel location.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetAlpha(int x, int y)
        {
            return UI.Native.GenericImage.GetAlpha(Handle, x, y);
        }

        /// <summary>
        /// Returns the red intensity at the given coordinate.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetRed(int x, int y)
        {
            return UI.Native.GenericImage.GetRed(Handle, x, y);
        }

        /// <summary>
        /// Returns the green intensity at the given coordinate.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetGreen(int x, int y)
        {
            return UI.Native.GenericImage.GetGreen(Handle, x, y);
        }

        /// <summary>
        /// Returns the blue intensity at the given coordinate.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetBlue(int x, int y)
        {
            return UI.Native.GenericImage.GetBlue(Handle, x, y);
        }

        /// <summary>
        /// Gets the red value of the mask color.
        /// </summary>
        /// <returns></returns>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaskRed()
        {
            return UI.Native.GenericImage.GetMaskRed(Handle);
        }

        /// <summary>
        /// Gets the green value of the mask color.
        /// </summary>
        /// <returns></returns>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaskGreen()
        {
            return UI.Native.GenericImage.GetMaskGreen(Handle);
        }

        /// <summary>
        /// Gets the blue value of the mask color.
        /// </summary>
        /// <returns></returns>
        /*
         
         */
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
        /// <param name="name"></param>
        /// <returns></returns>
        /*
         
         */
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
        /// <param name="name"></param>
        /// <returns></returns>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetOptionAsInt(string name)
        {
            return UI.Native.GenericImage.GetOptionInt(Handle, name);
        }

        /// <summary>
        /// Returns a sub image of the current one as long as the rect belongs entirely to the image.
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage GetSubImage(Int32Rect rect)
        {
            return new(UI.Native.GenericImage.GetSubImage(Handle, rect));
        }

        /// <summary>
        /// Gets the type of image found when image was loaded or specified when image was saved.
        /// </summary>
        /// <returns></returns>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetImageType()
        {
            return UI.Native.GenericImage.GetImageType(Handle);
        }

        /// <summary>
        /// Returns true if this image has alpha channel, false otherwise.
        /// </summary>
        /// <returns></returns>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAlpha()
        {
            return UI.Native.GenericImage.HasAlpha(Handle);
        }

        /// <summary>
        /// Returns true if there is a mask active, false otherwise.
        /// </summary>
        /// <returns></returns>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasMask()
        {
            return UI.Native.GenericImage.HasMask(Handle);
        }

        /// <summary>
        /// Returns true if the given option is present.
        /// </summary>
        /// <remarks>
        /// See <see cref="GenericImageOptionNames"/> for known option names.
        /// </remarks>
        /// <param name="name"></param>
        /// <returns></returns>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasOption(string name)
        {
            return UI.Native.GenericImage.HasOption(Handle, name);
        }

        /// <summary>
        /// Returns true if image data is present.
        /// </summary>
        public bool IsOk
        {
            get => UI.Native.GenericImage.IsOk(Handle);
        }

        /// <summary>
        /// Returns true if the given pixel is transparent, i.e. either has the mask
        /// color if this image has a mask or if this image has alpha channel and alpha value of
        /// this pixel is strictly less than threshold.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsTransparent(int x, int y, byte threshold = AlphaChannelThreshold)
        {
            return UI.Native.GenericImage.IsTransparent(Handle, x, y, threshold);
        }

        /// <summary>
        /// Loads an image from an input stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="bitmapType"></param>
        /// <param name="index">See description in <see cref="GenericImage(string, BitmapType, int)"/></param>
        /// <returns></returns>
        /*
         
         */
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
        /// <param name="filename"></param>
        /// <param name="bitmapType"></param>
        /// <param name="index">See description in <see cref="GenericImage(string, BitmapType, int)"/></param>
        /// <returns></returns>
        /*
         
         */
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
        /// <param name="name"></param>
        /// <param name="mimetype">MIME type string (for example 'image/jpeg').</param>
        /// <param name="index">See description in <see cref="GenericImage(string, BitmapType, int)"/></param>
        /// <returns></returns>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LoadFromFile(string name, string mimetype, int index = -1)
        {
            return UI.Native.GenericImage.LoadFileWithMimeType(Handle, name, mimetype, index);
        }

        /// <summary>
        /// Loads an image from an input stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="mimetype">MIME type string (for example 'image/jpeg').</param>
        /// <param name="index">See description in <see cref="GenericImage(string, BitmapType, int)"/></param>
        /// <returns></returns>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LoadFromStream(Stream stream, string mimetype, int index = -1)
        {
            var inputStream = new UI.Native.InputStream(stream);
            return UI.Native.GenericImage.LoadStreamWithMimeType(Handle, inputStream, mimetype, index);
        }

        /// <summary>
        /// Saves an image in the given stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="mimetype">MIME type string (for example 'image/jpeg').</param>
        /// <returns></returns>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SaveToStream(Stream stream, string mimetype)
        {
            var outputStream = new UI.Native.OutputStream(stream);
            return UI.Native.GenericImage.SaveStreamWithMimeType(Handle, outputStream, mimetype);
        }

        /// <summary>
        /// Saves an image in the named file.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="bitmapType"></param>
        /// <returns></returns>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SaveToFile(string filename, BitmapType bitmapType)
        {
            return UI.Native.GenericImage.SaveFileWithBitmapType(Handle, filename, (int)bitmapType);
        }

        /// <summary>
        /// Saves an image in the named file.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="mimetype">MIME type string (for example 'image/jpeg').</param>
        /// <returns></returns>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SaveToFile(string filename, string mimetype)
        {
            return UI.Native.GenericImage.SaveFileWithMimeType(Handle, filename, mimetype);
        }

        /// <summary>
        /// Saves an image in the named file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SaveToFile(string filename)
        {
            return UI.Native.GenericImage.SaveFile(Handle, filename);
        }

        /// <summary>
        /// Saves an image in the given stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /*
         
         */
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
        /*
         
         */
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
        /// <returns></returns>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IntPtr GetNativeAlphaData()
        {
            return UI.Native.GenericImage.GetAlphaData(Handle);
        }

        /// <summary>
        /// Returns the image data as an array.
        /// </summary>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IntPtr GetNativeData()
        {
            return UI.Native.GenericImage.GetData(Handle);
        }

        /// <summary>
        /// Creates a fresh image.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="data"></param>
        /// <param name="static_data"></param>
        /// <returns></returns>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CreateNativeData(int width, int height, IntPtr data, bool static_data = false)
        {
            return UI.Native.GenericImage.CreateData(Handle,  width, height, data, static_data);
        }

        /// <summary>
        /// Creates a fresh image.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="data"></param>
        /// <param name="alpha"></param>
        /// <param name="static_data"></param>
        /// <returns></returns>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CreateNativeData(int width, int height, IntPtr data, IntPtr alpha, bool static_data = false)
        {
            return UI.Native.GenericImage.CreateAlphaData(Handle, width, height, data, alpha, static_data);
        }

        /// <summary>
        /// Sets the image data without performing checks.
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="static_data"></param>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetNativeAlphaData(IntPtr alpha = default, bool static_data = false)
        {
            UI.Native.GenericImage.SetAlphaData(Handle, alpha, static_data);
        }

        /// <summary>
        /// Sets the image data without performing checks.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="static_data"></param>
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetNativeData(IntPtr data, bool static_data = false)
        {
            UI.Native.GenericImage.SetData(Handle, data, static_data);
        }

        /// <summary>
        /// Register an image handler.
        /// </summary>
        /// <param name="handler"></param>
        /*
         
         */
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
        /*
         
         */
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
        /*
         
         */
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
        /*
         
         */
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
        /*
         
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static IntPtr FindHandlerMime(string mimetype)
        {
            return UI.Native.GenericImage.FindHandlerByMime(mimetype);
        }

        /// <summary>
        /// Adds a handler at the start of the static list of format handlers.
        /// </summary>
        /// <param name="handler"></param>
        /*
         
         */
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


/*


==============
ChangeSaturation()
void wxImage::ChangeSaturation	(	double 	factor	)	
Changes the saturation of each pixel in the image.

==============
Clear()
void wxImage::Clear	(	unsigned char 	value = 0	)	
Initialize the image data with zeroes (the default) or with the byte value given as value.
==============
ClearAlpha()
void wxImage::ClearAlpha	(		)	
Removes the alpha channel from the image.

This function should only be called if the image has alpha channel data, use HasAlpha() to
check for this.

==============
Create() [4/6]
bool wxImage::Create	(	int 	width,
int 	height,
bool 	clear = true 
)		
Creates a fresh image.

See wxImage::wxImage(int,int,bool) for more info.

Returns
true if the call succeeded, false otherwise.
==============
Create() [5/6]
bool wxImage::Create	(	int 	width,
int 	height,
unsigned char * 	data,
bool 	static_data = false 
)		
Creates a fresh image.

See wxImage::wxImage(int,int,unsigned char*,bool) for more info.

Returns
true if the call succeeded, false otherwise.
==============
Create() [6/6]
bool wxImage::Create	(	int 	width,
int 	height,
unsigned char * 	data,
unsigned char * 	alpha,
bool 	static_data = false 
)		
Creates a fresh image.

See wxImage::wxImage(int,int,unsigned char*,unsigned char*,bool) for more info.

Returns
true if the call succeeded, false otherwise.
==============
FindFirstUnusedcolor()
bool wxImage::FindFirstUnusedcolor	(	unsigned char * 	r,
unsigned char * 	g,
unsigned char * 	b,
unsigned char 	startR = 1,
unsigned char 	startG = 0,
unsigned char 	startB = 0 
)		const
Finds the first color that is never used in the image.

The search begins at given initial color and continues by increasing R, G and B components
(in this order) by 1 until an unused color is found or the color space exhausted.

The parameters r, g, b are pointers to variables to save the color.

The parameters startR, startG, startB define the initial values of the color. The returned
color will have RGB values equal to or greater than these.

Returns
Returns false if there is no unused color left, true on success.
Note
This method involves computing the histogram, which is a computationally intensive operation.
==============
GetAlpha() [1/2]
unsigned char* wxImage::GetAlpha	(		)	const
Returns pointer to the array storing the alpha values for this image.

This pointer is NULL for the images without the alpha channel. If the image does have it,
this pointer may be used to directly manipulate the alpha values which are stored as the RGB ones.

==============
GetAlpha() [2/2]
unsigned char wxImage::GetAlpha	(	int 	x,
int 	y 
)		const
Return alpha value at given pixel location.

==============
GetBlue()
unsigned char wxImage::GetBlue	(	int 	x,
int 	y 
)		const
Returns the blue intensity at the given coordinate.

==============
GetData()
unsigned char* wxImage::GetData	(		)	const
Returns the image data as an array.

This is most often used when doing direct image manipulation. The return value points to an array
of characters in RGBRGBRGB... format in the top-to-bottom, left-to-right order, that is the first
RGB triplet corresponds to the first pixel of the first row, the second one — to the second pixel
of the first row and so on until the end of the first row, with second row following after it and
so on.

You should not delete the returned pointer nor pass it to SetData().
==============
GetGreen()
unsigned char wxImage::GetGreen	(	int 	x,
int 	y 
)		const
Returns the green intensity at the given coordinate.

==============
GetImageCount() [1/2]
static int wxImage::GetImageCount	(	const wxString & 	filename,
wxBitmapType 	type = wxBITMAP_TYPE_ANY 
)		
static
If the image file contains more than one image and the image handler is capable of retrieving
these individually, this function will return the number of available images.

For the overload taking the parameter filename, that's the name of the file to query. For the
overload taking the parameter stream, that's the opened input stream with image data.

The parameter type may be one of the following values:

wxBITMAP_TYPE_BMP: Load a Windows bitmap file.
wxBITMAP_TYPE_GIF: Load a GIF bitmap file.
wxBITMAP_TYPE_JPEG: Load a JPEG bitmap file.
wxBITMAP_TYPE_PNG: Load a PNG bitmap file.
wxBITMAP_TYPE_PCX: Load a PCX bitmap file.
wxBITMAP_TYPE_PNM: Load a PNM bitmap file.
wxBITMAP_TYPE_TIFF: Load a TIFF bitmap file.
wxBITMAP_TYPE_TGA: Load a TGA bitmap file.
wxBITMAP_TYPE_XPM: Load a XPM bitmap file.
wxBITMAP_TYPE_ICO: Load a Windows icon file (ICO).
wxBITMAP_TYPE_CUR: Load a Windows cursor file (CUR).
wxBITMAP_TYPE_ANI: Load a Windows animated cursor file (ANI).
wxBITMAP_TYPE_ANY: Will try to autodetect the format.
Returns
Number of available images. For most image handlers, this is 1 (exceptions are TIFF and ICO
formats as well as animated GIFs for which this function returns the number of frames in the
animation).
==============
GetImageCount() [2/2]
static int wxImage::GetImageCount	(	wxInputStream & 	stream,
wxBitmapType 	type = wxBITMAP_TYPE_ANY 
)		
static
If the image file contains more than one image and the image handler is capable of retrieving
these individually, this function will return the number of available images.

For the overload taking the parameter filename, that's the name of the file to query. For the
overload taking the parameter stream, that's the opened input stream with image data.

See wxImageHandler::GetImageCount() for more info.

The parameter type may be one of the following values:

wxBITMAP_TYPE_BMP: Load a Windows bitmap file.
wxBITMAP_TYPE_GIF: Load a GIF bitmap file.
wxBITMAP_TYPE_JPEG: Load a JPEG bitmap file.
wxBITMAP_TYPE_PNG: Load a PNG bitmap file.
wxBITMAP_TYPE_PCX: Load a PCX bitmap file.
wxBITMAP_TYPE_PNM: Load a PNM bitmap file.
wxBITMAP_TYPE_TIFF: Load a TIFF bitmap file.
wxBITMAP_TYPE_TGA: Load a TGA bitmap file.
wxBITMAP_TYPE_XPM: Load a XPM bitmap file.
wxBITMAP_TYPE_ICO: Load a Windows icon file (ICO).
wxBITMAP_TYPE_CUR: Load a Windows cursor file (CUR).
wxBITMAP_TYPE_ANI: Load a Windows animated cursor file (ANI).
wxBITMAP_TYPE_ANY: Will try to autodetect the format.
Returns
Number of available images. For most image handlers, this is 1 (exceptions are TIFF and ICO
formats as well as animated GIFs for which this function returns the number of frames in the animation).
==============
GetMaskBlue()
unsigned char wxImage::GetMaskBlue	(		)	const
Gets the blue value of the mask color.
==============
GetMaskGreen()
unsigned char wxImage::GetMaskGreen	(		)	const
Gets the green value of the mask color.
==============
GetMaskRed()
unsigned char wxImage::GetMaskRed	(		)	const
Gets the red value of the mask color.
==============
GetOption()
wxString wxImage::GetOption	(	const wxString & 	name	)	const
Gets a user-defined string-valued option.

Parameters
name	The name of the option, case-insensitive.
Returns
The value of the option or an empty string if not found. Use HasOption() if an empty string can
be a valid option value.
==============
GetOptionInt()
int wxImage::GetOptionInt	(	const wxString & 	name	)	const
Gets a user-defined integer-valued option.

The function is case-insensitive to name. If the given option is not present,
the function returns 0. Use HasOption() if 0 is a possibly valid value for the option.

Parameters
name	The name of the option, case-insensitive.
Returns
The value of the option or 0 if not found. Use HasOption() if 0 can be a valid option value.

===========
GetOrFindMaskcolor()
bool wxImage::GetOrFindMaskcolor	(	unsigned char * 	r,
unsigned char * 	g,
unsigned char * 	b 
)		const
Get the current mask color or find a suitable unused color that could be used as a mask color.

Returns true if the image currently has a mask.
===========
HasOption()
bool wxImage::HasOption	(	const wxString & 	name	)	const
Returns true if the given option is present.

The function is case-insensitive to name.
===========
HSVtoRGB()
static wxImage::RGBValue wxImage::HSVtoRGB	(	const wxImage::HSVValue & 	hsv	)	
static
Converts a color in HSV color space to RGB color space.

InitAlpha()
void wxImage::InitAlpha	(		)	
Initializes the image alpha channel data.

It is an error to call it if the image already has alpha data. If it doesn't, alpha data will be
by default initialized to all pixels being fully opaque. But if the image has a mask color, all mask pixels will be completely transparent.

===========
IsTransparent()
bool wxImage::IsTransparent	(	int 	x,
int 	y,
unsigned char 	threshold = wxIMAGE_ALPHA_THRESHOLD 
)		const
Returns true if the given pixel is transparent, i.e. either has the mask color if this image has
a mask or if this image has alpha channel and alpha value of this pixel is strictly less than threshold.

===========
LoadFile() [1/4]
virtual bool wxImage::LoadFile	(	const wxString & 	name,
const wxString & 	mimetype,
int 	index = -1 
)		
virtual
Loads an image from a file.

If no handler type is provided, the library will try to autodetect the format.

Parameters
name	Name of the file from which to load the image.
mimetype	MIME type string (for example 'image/jpeg')
index	See the description in the LoadFile(wxInputStream&, wxBitmapType, int) overload.
===========
LoadFile() [2/4]
virtual bool wxImage::LoadFile	(	const wxString & 	name,
wxBitmapType 	type = wxBITMAP_TYPE_ANY,
int 	index = -1 
)		
virtual
Loads an image from a file.

If no handler type is provided, the library will try to autodetect the format.

name	Name of the file from which to load the image.
type	See the description in the LoadFile(wxInputStream&, wxBitmapType, int) overload.
index	See the description in the LoadFile(wxInputStream&, wxBitmapType, int) overload.
=======================
LoadFile() [3/4]
virtual bool wxImage::LoadFile	(	wxInputStream & 	stream,
const wxString & 	mimetype,
int 	index = -1 
)		
virtual
Loads an image from an input stream.

stream	Opened input stream from which to load the image. Currently, the stream must support seeking.
mimetype	MIME type string (for example 'image/jpeg')
index	See the description in the LoadFile(wxInputStream&, wxBitmapType, int) overload.

===========
LoadFile() [4/4]
virtual bool wxImage::LoadFile	(	wxInputStream & 	stream,
wxBitmapType 	type = wxBITMAP_TYPE_ANY,
int 	index = -1 
)		
Loads an image from an input stream.

If the file can't be loaded, this function returns false and logs an error using wxLogError().
If the file can be loaded but some problems were detected while doing it, it can also call
wxLogWarning() to notify about these problems. If this is undesirable, use SetLoadFlags()
to reset Load_Verbose flag and suppress these warnings.

stream	Opened input stream from which to load the image. Currently, the stream must support seeking.
type	May be one of the following:
wxBITMAP_TYPE_BMP: Load a Windows bitmap file.
wxBITMAP_TYPE_GIF: Load a GIF bitmap file.
wxBITMAP_TYPE_JPEG: Load a JPEG bitmap file.
wxBITMAP_TYPE_PNG: Load a PNG bitmap file.
wxBITMAP_TYPE_PCX: Load a PCX bitmap file.
wxBITMAP_TYPE_PNM: Load a PNM bitmap file.
wxBITMAP_TYPE_TIFF: Load a TIFF bitmap file.
wxBITMAP_TYPE_TGA: Load a TGA bitmap file.
wxBITMAP_TYPE_XPM: Load a XPM bitmap file.
wxBITMAP_TYPE_ICO: Load a Windows icon file (ICO).
wxBITMAP_TYPE_CUR: Load a Windows cursor file (CUR).
wxBITMAP_TYPE_ANI: Load a Windows animated cursor file (ANI).
wxBITMAP_TYPE_ANY: Will try to autodetect the format.
index	Index of the image to load in the case that the image file contains multiple images.
This is only used by GIF, ICO and TIFF handlers. The default value (-1) means "choose the default image" and is interpreted as the first image (index=0) by the GIF and TIFF handler and as the largest and most colorful one by the ICO handler.
Returns
true if the operation succeeded, false otherwise. If the optional index parameter is out of range,
false is returned and a call to wxLogError() takes place.
Remarks
Depending on how wxWidgets has been configured, not all formats may be available.
Note
You can use GetOptionInt() to get the hotspot when loading cursor files:
int hotspot_x = image.GetOptionInt(wxIMAGE_OPTION_CUR_HOTSPOT_X);
int hotspot_y = image.GetOptionInt(wxIMAGE_OPTION_CUR_HOTSPOT_Y);
===========
Mirror()
wxImage wxImage::Mirror	(	bool 	horizontally = true	)	const
Returns a mirrored copy of the image.

The parameter horizontally indicates the orientation.
===========
Paste()
void wxImage::Paste	(	const wxImage & 	image,
int 	x,
int 	y,
wxImageAlphaBlendMode 	alphaBlend = wxIMAGE_ALPHA_BLEND_OVER 
)		
Copy the data of the given image to the specified position in this image.

Takes care of the mask color and out of bounds problems.

image	The image containing the data to copy, must be valid.
x	The horizontal position of the position to copy the data to.
y	The vertical position of the position to copy the data to.
alphaBlend	This parameter (new in wx 3.1.5) determines whether the alpha values of the original
image replace (default) or are composed with the alpha channel of this image. Notice that alpha
blending overrides the mask handling.

===========
Replace()
void wxImage::Replace	(	unsigned char 	r1,
unsigned char 	g1,
unsigned char 	b1,
unsigned char 	r2,
unsigned char 	g2,
unsigned char 	b2 
)		
Replaces the color specified by r1,g1,b1 by the color r2,g2,b2.

===========
Rescale()
wxImage& wxImage::Rescale	(	int 	width,
int 	height,
wxImageResizeQuality 	quality = wxIMAGE_QUALITY_NORMAL 
)		
Changes the size of the image in-place by scaling it: after a call to this function,the image will
have the given width and height.

For a description of the quality parameter, see the Scale() function. Returns the (modified) image
itself.
===========
Resize()
wxImage& wxImage::Resize	(	const wxSize & 	size,
const wxPoint & 	pos,
int 	red = -1,
int 	green = -1,
int 	blue = -1 
)		
Changes the size of the image in-place without scaling it by adding either a border with the given
color or cropping as necessary.

The image is pasted into a new image with the given size and background color at the position pos
relative to the upper left of the new image.

If red = green = blue = -1 then use either the current mask color if set or find, use, and set a
suitable mask color for any newly exposed areas.

Returns
The (modified) image itself.
===========

RGBtoHSV()
static wxImage::HSVValue wxImage::RGBtoHSV	(	const wxImage::RGBValue & 	rgb	)	
static
Converts a color in RGB color space to HSV color space.

===========
Rotate()
wxImage wxImage::Rotate	(	double 	angle,
const wxPoint & 	rotationCentre,
bool 	interpolating = true,
wxPoint * 	offsetAfterRotation = NULL 
)		const
Rotates the image about the given point, by angle radians.

Passing true to interpolating results in better image quality, but is slower.

If the image has a mask, then the mask color is used for the uncovered pixels in the rotated image
background. Else, black (rgb 0, 0, 0) will be used.

Returns the rotated image, leaving this image intact.

===========
Rotate180()
wxImage wxImage::Rotate180	(		)	const
Returns a copy of the image rotated by 180 degrees.
===========
Rotate90()
wxImage wxImage::Rotate90	(	bool 	clockwise = true	)	const
Returns a copy of the image rotated 90 degrees in the direction indicated by clockwise.
===========

RotateHue()
void wxImage::RotateHue	(	double 	angle	)	
Rotates the hue of each pixel in the image by angle, which is a double in the range [-1.0..+1.0],
where -1.0 corresponds to -360 degrees and +1.0 corresponds to +360 degrees.
===========

SaveFile() [1/5]
virtual bool wxImage::SaveFile	(	const wxString & 	name	)	const
virtual
Saves an image in the named file.

File type is determined from the extension of the file name. Note that this function may fail if
the extension is not recognized! You can use one of the forms above to save images to files with
non-standard extensions.

Parameters
name	Name of the file to save the image to.
===========

SaveFile() [2/5]
virtual bool wxImage::SaveFile	(	const wxString & 	name,
const wxString & 	mimetype 
)		const
virtual
Saves an image in the named file.

Parameters
name	Name of the file to save the image to.
mimetype	MIME type.
========================
SaveFile() [3/5]
virtual bool wxImage::SaveFile	(	const wxString & 	name,
wxBitmapType 	type 
)		const
virtual
Saves an image in the named file.

name	Name of the file to save the image to.
type	Currently these types can be used:
wxBITMAP_TYPE_BMP: Save a BMP image file.
wxBITMAP_TYPE_JPEG: Save a JPEG image file.
wxBITMAP_TYPE_PNG: Save a PNG image file.
wxBITMAP_TYPE_PCX: Save a PCX image file (tries to save as 8-bit if possible, falls back to 24-bit
otherwise).
wxBITMAP_TYPE_PNM: Save a PNM image file (as raw RGB always).
wxBITMAP_TYPE_TIFF: Save a TIFF image file.
wxBITMAP_TYPE_XPM: Save a XPM image file.
wxBITMAP_TYPE_ICO: Save a Windows icon file (ICO). The size may be up to 255 wide by 127 high.
A single image is saved in 8 colors at the size supplied.
wxBITMAP_TYPE_CUR: Save a Windows cursor file (CUR).

===========

SaveFile() [4/5]
virtual bool wxImage::SaveFile	(	wxOutputStream & 	stream,
const wxString & 	mimetype 
)		const
virtual
Saves an image in the given stream.

stream	Opened output stream to save the image to.
mimetype	MIME type.
Returns
true if the operation succeeded, false otherwise.
Remarks
Depending on how wxWidgets has been configured, not all formats may be available.
Note
You can use SetOption() to set the hotspot when saving an image into a cursor file (default hotspot
is in the centre of the image):
image.SetOption(wxIMAGE_OPTION_CUR_HOTSPOT_X, hotspotX);
image.SetOption(wxIMAGE_OPTION_CUR_HOTSPOT_Y, hotspotY);

===========

SaveFile() [5/5]
virtual bool wxImage::SaveFile	(	wxOutputStream & 	stream,
wxBitmapType 	type 
)		const
Saves an image in the given stream.

stream	Opened output stream to save the image to.
type	MIME type.
===========

SetAlpha() [1/2]
void wxImage::SetAlpha	(	int 	x,
int 	y,
unsigned char 	alpha 
)		
Sets the alpha value for the given pixel.

This function should only be called if the image has alpha channel data, use HasAlpha() to check
for this.

===========

SetAlpha() [2/2]
void wxImage::SetAlpha	(	unsigned char * 	alpha = NULL,
bool 	static_data = false 
)		
This function is similar to SetData() and has similar restrictions.

The pointer passed to it may however be NULL in which case the function will allocate the alpha
array internally – this is useful to add alpha channel data to an image which doesn't have any.

If the pointer is not NULL, it must have one byte for each image pixel and be allocated with
malloc(). wxImage takes ownership of the pointer and will free it unless static_data parameter
is set to true – in this case the caller should do it.

===========

SetData() [1/2]
void wxImage::SetData	(	unsigned char * 	data,
bool 	static_data = false 
)		
Sets the image data without performing checks.

The data given must have the size (width*height*3) or results will be unexpected. Don't use
this method if you aren't sure you know what you are doing.

The data must have been allocated with malloc(), NOT with operator new.

If static_data is false, after this call the pointer to the data is owned by the wxImage object,
that will be responsible for deleting it. Do not pass to this function a pointer obtained through
GetData().

===========

SetData() [2/2]
void wxImage::SetData	(	unsigned char * 	data,
int 	new_width,
int 	new_height,
bool 	static_data = false 
)		
This is an overloaded member function, provided for convenience. It differs from the above
function only in what argument(s) it accepts.

===========
SetDefaultLoadFlags()
static void wxImage::SetDefaultLoadFlags	(	int 	flags	)	
static
Sets the default value for the flags used for loading image files.

This method changes the global value of the flags used for all the subsequently created wxImage
objects by default. It doesn't affect the already existing objects.

By default, the global flags include Load_Verbose flag value.

===========
SetMask()
void wxImage::SetMask	(	bool 	hasMask = true	)	
Specifies whether there is a mask or not.

The area of the mask is determined by the current mask color.
===========
SetMaskcolor()
void wxImage::SetMaskcolor	(	unsigned char 	red,
unsigned char 	green,
unsigned char 	blue 
)		
Sets the mask color for this image (and tells the image to use the mask).
===========
SetMaskFromImage()
bool wxImage::SetMaskFromImage	(	const wxImage & 	mask,
unsigned char 	mr,
unsigned char 	mg,
unsigned char 	mb 
)		
Sets image's mask so that the pixels that have RGB value of mr,mg,mb in mask will be masked in
the image.

This is done by first finding an unused color in the image, setting this color as the mask color
and then using this color to draw all pixels in the image who corresponding pixel in mask has given
RGB value.

The parameter mask is the mask image to extract mask shape from. It must have the same dimensions
as the image.

The parameters mr, mg, mb are the RGB values of the pixels in mask that will be used to create the
mask.

Returns
Returns false if mask does not have same dimensions as the image or if there is no unused color
left. Returns true if the mask was successfully applied.
Note
Note that this method involves computing the histogram, which is a computationally intensive operation.
===========
SetOption() [1/2]
void wxImage::SetOption	(	const wxString & 	name,
const wxString & 	value 
)		
Sets a user-defined option.

The function is case-insensitive to name.

For example, when saving as a JPEG file, the option quality is used, which is a number between
0 and 100 (0 is terrible, 100 is very good).

The lists of the currently supported options are in GetOption() and GetOptionInt() function docs.
===========
SetOption() [2/2]
void wxImage::SetOption	(	const wxString & 	name,
int 	value 
)		
This is an overloaded member function, provided for convenience. It differs from the above
function only in what argument(s) it accepts.
===========
SetRGB() [1/2]
void wxImage::SetRGB	(	const wxRect & 	rect,
unsigned char 	red,
unsigned char 	green,
unsigned char 	blue 
)		
Sets the color of the pixels within the given rectangle.

This routine performs bounds-checks for the coordinate so it can be considered a safe way to
manipulate the data.

===========
SetRGB() [2/2]
void wxImage::SetRGB	(	int 	x,
int 	y,
unsigned char 	r,
unsigned char 	g,
unsigned char 	b 
)		
Set the color of the pixel at the given x and y coordinate.

===============
SetType()
void wxImage::SetType	(	wxBitmapType 	type	)	
Set the type of image returned by GetType().

This method is mostly used internally by the library but can also be called from the user code
if the image was created from data in the given bitmap format without using LoadFile() (which would set the type correctly automatically).

Notice that the image must be created before this function is called.

type	One of bitmap type constants, wxBITMAP_TYPE_INVALID is a valid value for it and can be
used to reset the bitmap type to default but wxBITMAP_TYPE_MAX is not allowed here.

==========
Size()
wxImage wxImage::Size	(	const wxSize & 	size,
const wxPoint & 	pos,
int 	red = -1,
int 	green = -1,
int 	blue = -1 
)		const
Returns a resized version of this image without scaling it by adding either a border with the
given color or cropping as necessary.

The image is pasted into a new image with the given size and background color at the position
pos relative to the upper left of the new image.

If red = green = blue = -1 then the areas of the larger image not covered by this image are made
transparent by filling them with the image mask color (which will be allocated automatically if
it isn't currently set).

Otherwise, the areas will be filled with the color with the specified RGB components. 
=========== 
 */