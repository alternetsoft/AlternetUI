using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// Constant used to indicate the alpha value conventionally defined as the complete transparency.
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
        public GenericImage()
            : base(UI.Native.GenericImage.CreateImage(), true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image with the given size and clears it if requested.
        /// </summary>
        public GenericImage(int width, int height, bool clear = true)
            : base(UI.Native.GenericImage.CreateImageWithSize(width, height, clear), true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image from a file.
        /// </summary>
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
        public GenericImage(string name, string mimetype, int index = -1)
            : base(UI.Native.GenericImage.CreateImageFromFileWithMimeType(name, mimetype, index), true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image from a stream.
        /// </summary>
        public GenericImage(Stream stream, BitmapType bitmapType = BitmapType.Any, int index = -1)
            : this(new UI.Native.InputStream(stream), bitmapType, index)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image from a stream.
        /// </summary>
        public GenericImage(Stream stream, string mimeType, int index = -1)
            : this(new UI.Native.InputStream(stream), mimeType, index)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image from data in memory.
        /// </summary>
        public GenericImage(int width, int height, IntPtr data, bool static_data = false)
            : base(
                  UI.Native.GenericImage.CreateImageWithSizeAndData(width, height, data, static_data),
                  true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image from data in memory.
        /// </summary>
        public GenericImage(int width, int height, IntPtr data, IntPtr alpha, bool staticData = false)
            : base(
                  UI.Native.GenericImage.CreateImageWithAlpha(width, height, data, alpha, staticData),
                  true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// </summary>
        internal GenericImage(IntPtr handle, bool disposeHandle)
            : base(handle, disposeHandle)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image from a stream.
        /// </summary>
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
        public static bool CanRead(string filename)
        {
            return false;
        }

        /// <summary>
        /// Returns true if at least one of the available image handlers can read the data in
        /// the given stream.
        /// </summary>
        public static bool CanReadStream(Stream stream)
        {
            return false;
        }

        /// <summary>
        /// Iterates all registered wxImageHandler objects, and returns a string containing
        /// file extension masks suitable for passing to file open/save dialog boxes.
        /// </summary>
        public static string GetImageExtWildcard()
        {
            return string.Empty;
        }

        /// <summary>
        /// Finds the handler with the given name, and removes it.
        /// </summary>
        public static bool RemoveHandler(string name)
        {
            return false;
        }

        /// <summary>
        /// If the image file contains more than one image and the image handler is capable of
        /// retrieving these individually, this function will return the number of available images.
        /// </summary>
        public static int GetImageCountInFile(
            string filename,
            BitmapType bitmapType = BitmapType.Any)
        {
            return 0;
        }

        /// <summary>
        /// If the image stream contains more than one image and the image handler is capable of
        /// retrieving these individually, this function will return the number of available images.
        /// </summary>
        public static int GetImageCountInStream(
            Stream stream,
            BitmapType bitmapType = BitmapType.Any)
        {
            return 0;
        }

        /// <summary>
        /// Sets the alpha value for the given pixel.
        /// </summary>
        public void SetAlpha(int x, int y, byte alpha)
        {
        }

        /// <summary>
        /// Removes the alpha channel from the image.
        /// </summary>
        public void ClearAlpha()
        {
        }

        /// <summary>
        /// Specifies whether there is a mask or not.
        /// </summary>
        public void SetMask(bool hasMask = true)
        {
        }

        /// <summary>
        /// Sets the mask color for this image(and tells the image to use the mask).
        /// </summary>
        public void SetMaskColor(byte red, byte green, byte blue)
        {
        }

        /// <summary>
        /// Sets image's mask so that the pixels that have RGB value of mr,mg,mb in
        /// mask will be masked in the image.
        /// </summary>
        public bool SetMaskFromImage(GenericImage image, byte mr, byte mg, byte mb)
        {
            return false;
        }

        /// <summary>
        /// Sets a user-defined option.
        /// </summary>
        /// <remarks>
        /// See <see cref="GenericImageOptionNames"/> for known option names.
        /// </remarks>
        public void SetOptionString(string name, string value)
        {
        }

        /// <summary>
        /// Sets a user-defined option.
        /// </summary>
        /// <remarks>
        /// See <see cref="GenericImageOptionNames"/> for known option names.
        /// </remarks>
        public void SetOptionInt(string name, int value)
        {
        }

        /// <summary>
        /// Sets the color of the pixel at the given x and y coordinate.
        /// </summary>
        public void SetRGB(int x, int y, byte r, byte g, byte b)
        {
        }

        /// <summary>
        /// Sets the color of the pixels within the given rectangle.
        /// </summary>
        public void SetRGBRect(Int32Rect rect, byte red, byte green, byte blue)
        {
        }

        /// <summary>
        /// Sets the type of image returned by GetType().
        /// </summary>
        public void SetImageType(BitmapType type)
        {
        }

        /// <summary>
        /// Returns an identical copy of this image.
        /// </summary>
        public GenericImage Copy()
        {
            return new();
        }

        /// <summary>
        /// Creates a fresh image.
        /// </summary>
        public bool CreateFreshImage(int width, int height, bool clear = true)
        {
            return false;
        }

        /// <summary>
        /// Initialize the image data with zeroes (the default) or with the byte value given as value.
        /// </summary>
        public void Clear(byte value = 0)
        {
        }

        /// <summary>
        /// Destroys the image data.
        /// </summary>
        public void DestroyImageData()
        {
        }

        /// <summary>
        /// Initializes the image alpha channel data.
        /// </summary>
        public void InitAlpha()
        {
        }

        /// <summary>
        /// Blurs the image in both horizontal and vertical directions by the specified
        /// pixel blurRadius.
        /// </summary>
        public GenericImage Blur(int blurRadius)
        {
            return new();
        }

        /// <summary>
        /// Blurs the image in the horizontal direction only.
        /// </summary>
        public GenericImage BlurHorizontal(int blurRadius)
        {
            return new();
        }

        /// <summary>
        /// Blurs the image in the vertical direction only.
        /// </summary>
        public GenericImage BlurVertical(int blurRadius)
        {
            return new();
        }

        /// <summary>
        /// Returns a mirrored copy of the image.
        /// </summary>
        public GenericImage Mirror(bool horizontally = true)
        {
            return new();
        }

        /// <summary>
        /// Copy the data of the given image to the specified position in this image.
        /// </summary>
        public void Paste(
            GenericImage image,
            int x,
            int y,
            GenericImageAlphaBlendMode alphaBlend = GenericImageAlphaBlendMode.Overwrite)
        {
        }

        /// <summary>
        /// Replaces the color specified by (r1, g1, b1) by the color (r2, g2, b2).
        /// </summary>
        public void Replace(byte r1, byte g1, byte b1, byte r2, byte g2, byte b2)
        {
        }

        /// <summary>
        /// Changes the size of the image in-place by scaling it: after a call to this
        /// function,the image will have the given width and height.
        /// </summary>
        public GenericImage Rescale(
            int width,
            int height,
            GenericImageResizeQuality quality = GenericImageResizeQuality.Normal)
        {
            return new();
        }

        /// <summary>
        /// Changes the size of the image in-place without scaling it by adding either a border
        /// with the given color or cropping as necessary.
        /// </summary>
        public GenericImage ResizeWithoutScale(
            Int32Size size,
            Int32Point pos,
            int red = -1,
            int green = -1,
            int blue = -1)
        {
            return new();
        }

        /// <summary>
        /// Returns a copy of the image rotated 90 degrees in the direction indicated by clockwise.
        /// </summary>
        public GenericImage Rotate90(bool clockwise = true)
        {
            return new();
        }

        /// <summary>
        /// Returns a copy of the image rotated by 180 degrees.
        /// </summary>
        public GenericImage Rotate180()
        {
            return new();
        }

        /// <summary>
        /// Rotates the hue of each pixel in the image by angle, which is a double in the
        /// range [-1.0..+1.0], where -1.0 corresponds to -360 degrees and +1.0 corresponds
        /// to +360 degrees.
        /// </summary>
        public void RotateHue(double angle)
        {
        }

        /// <summary>
        /// Changes the saturation of each pixel in the image.
        /// </summary>
        public void ChangeSaturation(double factor)
        {
        }

        /// <summary>
        /// Changes the brightness(value) of each pixel in the image.
        /// </summary>
        public void ChangeBrightness(double factor)
        {
        }

        /// <summary>
        /// Changes the hue, the saturation and the brightness(value) of each pixel in the image.
        /// </summary>
        public void ChangeHSV(double angleH, double factorS, double factorV)
        {
        }

        /// <summary>
        /// Returns a scaled version of the image.
        /// </summary>
        public GenericImage Scale(
            int width,
            int height,
            GenericImageResizeQuality quality = GenericImageResizeQuality.Normal)
        {
            return new();
        }

        /// <summary>
        /// If the image has alpha channel, this method converts it to mask.
        /// </summary>
        public bool ConvertAlphaToMask(byte threshold = AlphaChannelThreshold)
        {
            return false;
        }

        /// <summary>
        /// If the image has alpha channel, this method converts it to mask using the
        /// specified color as the mask color.
        /// </summary>
        public bool ConvertAlphaToMaskUseColor(
            byte mr,
            byte mg,
            byte mb,
            byte threshold = AlphaChannelThreshold)
        {
            return false;
        }

        /// <summary>
        /// Returns a greyscale version of the image.
        /// </summary>
        public GenericImage ConvertToGreyscale(double weight_r, double weight_g, double weight_b)
        {
            return new();
        }

        /// <summary>
        /// Returns a greyscale version of the image.
        /// </summary>
        public GenericImage ConvertToGreyscale()
        {
            return new();
        }

        /// <summary>
        /// Returns monochromatic version of the image.
        /// </summary>
        public GenericImage ConvertToMono(byte r, byte g, byte b)
        {
            return new();
        }

        /// <summary>
        /// Returns disabled(dimmed) version of the image.
        /// </summary>
        public GenericImage ConvertToDisabled(byte brightness = 255)
        {
            return new();
        }

        /// <summary>
        /// Returns a changed version of the image based on the given lightness.
        /// </summary>
        public GenericImage ChangeLightness(int alpha)
        {
            return new();
        }

        /// <summary>
        /// Return alpha value at given pixel location.
        /// </summary>
        public byte GetAlpha(int x, int y)
        {
            return 0;
        }

        /// <summary>
        /// Returns the red intensity at the given coordinate.
        /// </summary>
        public byte GetRed(int x, int y)
        {
            return 0;
        }

        /// <summary>
        /// Returns the green intensity at the given coordinate.
        /// </summary>
        public byte GetGreen(int x, int y)
        {
            return 0;
        }

        /// <summary>
        /// Returns the blue intensity at the given coordinate.
        /// </summary>
        public byte GetBlue(int x, int y)
        {
            return 0;
        }

        /// <summary>
        /// Gets the red value of the mask color.
        /// </summary>
        public byte GetMaskRed()
        {
            return 0;
        }

        /// <summary>
        /// Gets the green value of the mask color.
        /// </summary>
        public byte GetMaskGreen()
        {
            return 0;
        }

        /// <summary>
        /// Gets the blue value of the mask color.
        /// </summary>
        public byte GetMaskBlue()
        {
            return 0;
        }

        /// <summary>
        /// Gets a user-defined string-valued option.
        /// </summary>
        /// <remarks>
        /// See <see cref="GenericImageOptionNames"/> for known option names.
        /// </remarks>
        public string GetOptionAsString(string name)
        {
            return string.Empty;
        }

        /// <summary>
        /// Gets a user-defined integer-valued option.
        /// </summary>
        /// <remarks>
        /// See <see cref="GenericImageOptionNames"/> for known option names.
        /// </remarks>
        public int GetOptionAsInt(string name)
        {
            return 0;
        }

        /// <summary>
        /// Returns a sub image of the current one as long as the rect belongs entirely to the image.
        /// </summary>
        public GenericImage GetSubImage(Int32Rect rect)
        {
            return new();
        }

        /// <summary>
        /// Gets the type of image found when image was loaded or specified when image was saved.
        /// </summary>
        public int GetImageType()
        {
            return 0;
        }

        /// <summary>
        /// Returns true if this image has alpha channel, false otherwise.
        /// </summary>
        public bool HasAlpha()
        {
            return false;
        }

        /// <summary>
        /// Returns true if there is a mask active, false otherwise.
        /// </summary>
        public bool HasMask()
        {
            return false;
        }

        /// <summary>
        /// Returns true if the given option is present.
        /// </summary>
        /// <remarks>
        /// See <see cref="GenericImageOptionNames"/> for known option names.
        /// </remarks>
        public bool HasOption(string name)
        {
            return false;
        }

        /// <summary>
        /// Returns true if image data is present.
        /// </summary>
        public bool IsOk()
        {
            return false;
        }

        /// <summary>
        /// Returns true if the given pixel is transparent, i.e. either has the mask
        /// color if this image has a mask or if this image has alpha channel and alpha value of
        /// this pixel is strictly less than threshold.
        /// </summary>
        public bool IsTransparent(int x, int y, byte threshold = AlphaChannelThreshold)
        {
            return false;
        }

        /// <summary>
        /// Loads an image from an input stream.
        /// </summary>
        public bool LoadFromStream(
            Stream stream,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1)
        {
            return false;
        }

        /// <summary>
        /// Loads an image from a file.
        /// </summary>
        public bool LoadFromFile(
            string filename,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1)
        {
            return false;
        }

        /// <summary>
        /// Loads an image from a file.
        /// </summary>
        public bool LoadFromFile(string name, string mimetype, int index = -1)
        {
            return false;
        }

        /// <summary>
        /// Loads an image from an input stream.
        /// </summary>
        public bool LoadFromStream(Stream stream, string mimetype, int index = -1)
        {
            return false;
        }

        /// <summary>
        /// Saves an image in the given stream.
        /// </summary>
        public bool SaveToStream(Stream stream, string mimetype)
        {
            return false;
        }

        /// <summary>
        /// Saves an image in the named file.
        /// </summary>
        public bool SaveToFile(string filename, BitmapType bitmapType)
        {
            return false;
        }

        /// <summary>
        /// Saves an image in the named file.
        /// </summary>
        public bool SaveToFile(string filename, string mimetype)
        {
            return false;
        }

        /// <summary>
        /// Saves an image in the named file.
        /// </summary>
        public bool SaveToFile(string filename)
        {
            return false;
        }

        /// <summary>
        /// Saves an image in the given stream.
        /// </summary>
        public bool SaveToStream(Stream stream, BitmapType type)
        {
            return false;
        }

        /// <summary>
        /// Sets the image data without performing checks.
        /// </summary>
        internal static void SetData(
            IntPtr data,
            int new_width,
            int new_height,
            bool static_data = false)
        {
        }

        /// <summary>
        /// Returns the currently used default file load flags.
        /// </summary>
        internal static int GetDefaultLoadFlags()
        {
            return 0;
        }

        /// <summary>
        /// Sets the default value for the flags used for loading image files.
        /// </summary>
        internal static void SetDefaultLoadFlags(int flags)
        {
        }

        /// <summary>
        /// Register an image handler.
        /// </summary>
        internal static void AddHandler(IntPtr handler)
        {
        }

        /// <summary>
        /// Deletes all image handlers.
        /// </summary>
        internal static void CleanUpHandlers()
        {
        }

        /// <summary>
        /// Finds the handler with the given name.
        /// </summary>
        internal static IntPtr FindHandlerByName(string name)
        {
            return default;
        }

        /// <summary>
        /// Finds the handler associated with the given extension and type.
        /// </summary>
        internal static IntPtr FindHandlerByExt(string extension, BitmapType bitmapType)
        {
            return default;
        }

        /// <summary>
        /// Finds the handler associated with the given image type.
        /// </summary>
        internal static IntPtr FindHandlerByBitmapType(BitmapType bitmapType)
        {
            return default;
        }

        /// <summary>
        /// Finds the handler associated with the given MIME type.
        /// </summary>
        internal static IntPtr FindHandlerByMime(string mimetype)
        {
            return default;
        }

        /// <summary>
        /// Adds a handler at the start of the static list of format handlers.
        /// </summary>
        internal static void InsertHandler(IntPtr handler)
        {
        }

        /// <summary>
        /// Sets the flags used for loading image files by this object.
        /// </summary>
        internal void SetLoadFlags(int flags)
        {
        }

        /// <summary>
        /// Returns pointer to the array storing the alpha values for this image.
        /// </summary>
        internal IntPtr GetAlphaData()
        {
            return default;
        }

        /// <summary>
        /// Returns the image data as an array.
        /// </summary>
        internal IntPtr GetData()
        {
            return default;
        }

        /// <summary>
        /// Creates a fresh image.
        /// </summary>
        internal bool CreateData(int width, int height, IntPtr data, bool static_data = false)
        {
            return false;
        }

        /// <summary>
        /// Creates a fresh image.
        /// </summary>
        internal bool CreateData(int width, int height, IntPtr data, IntPtr alpha, bool static_data = false)
        {
            return false;
        }

        /// <summary>
        /// Sets the image data without performing checks.
        /// </summary>
        internal void SetAlphaData(IntPtr alpha = default, bool static_data = false)
        {
        }

        /// <summary>
        /// Sets the image data without performing checks.
        /// </summary>
        internal void SetData(IntPtr data, bool static_data = false)
        {
        }

        /// <summary>
        /// Returns the file load flags used for this object.
        /// </summary>
        internal int GetLoadFlags(IntPtr handle)
        {
            return 0;
        }

        /// <inheritdoc/>
        protected override void DisposeUnmanagedResources()
        {
            UI.Native.GenericImage.DeleteImage(Handle);
        }
    }
}