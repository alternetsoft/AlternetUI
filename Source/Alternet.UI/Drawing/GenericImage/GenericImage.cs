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
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage(int width, int height, bool clear = true)
            : base(UI.Native.GenericImage.CreateImageWithSize(width, height, clear), true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image from a file.
        /// </summary>
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage(string name, string mimetype, int index = -1)
            : base(UI.Native.GenericImage.CreateImageFromFileWithMimeType(name, mimetype, index), true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image from a stream.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage(Stream stream, BitmapType bitmapType = BitmapType.Any, int index = -1)
            : this(new UI.Native.InputStream(stream), bitmapType, index)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image from a stream.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage(Stream stream, string mimeType, int index = -1)
            : this(new UI.Native.InputStream(stream), mimeType, index)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image from data in memory.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CanRead(string filename)
        {
            return UI.Native.GenericImage.CanRead(filename);
        }

        /// <summary>
        /// Returns true if at least one of the available image handlers can read the data in
        /// the given stream.
        /// </summary>
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
        /// Finds the handler with the given name, and removes it.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool RemoveHandler(string name)
        {
            return UI.Native.GenericImage.RemoveHandler(name);
        }

        /// <summary>
        /// If the image file contains more than one image and the image handler is capable of
        /// retrieving these individually, this function will return the number of available images.
        /// </summary>
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenericImageLoadFlags GetDefaultLoadFlags()
        {
            return (GenericImageLoadFlags)UI.Native.GenericImage.GetDefaultLoadFlags();
        }

        /// <summary>
        /// Sets the default value for the flags used for loading image files.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetDefaultLoadFlags(GenericImageLoadFlags flags)
        {
            UI.Native.GenericImage.SetDefaultLoadFlags((int)flags);
        }

        /// <summary>
        /// Sets the alpha value for the given pixel.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetAlpha(int x, int y, byte alpha)
        {
            UI.Native.GenericImage.SetAlpha(Handle, x, y, alpha);
        }

        /// <summary>
        /// Removes the alpha channel from the image.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearAlpha()
        {
            UI.Native.GenericImage.ClearAlpha(Handle);
        }

        /// <summary>
        /// Specifies whether there is a mask or not.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetMask(bool hasMask = true)
        {
            UI.Native.GenericImage.SetMask(Handle, hasMask);
        }

        /// <summary>
        /// Sets the mask color for this image(and tells the image to use the mask).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetMaskColor(byte red, byte green, byte blue)
        {
            UI.Native.GenericImage.SetMaskColor(Handle, red, green, blue);
        }

        /// <summary>
        /// Sets image's mask so that the pixels that have RGB value of mr,mg,mb in
        /// mask will be masked in the image.
        /// </summary>
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetOptionAsInt(string name, int value)
        {
            UI.Native.GenericImage.SetOptionInt(Handle, name, value);
        }

        /// <summary>
        /// Sets the color of the pixel at the given x and y coordinate.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetRGB(int x, int y, byte r, byte g, byte b)
        {
            UI.Native.GenericImage.SetRGB(Handle, x, y, r, g, b);
        }

        /// <summary>
        /// Sets the color of the pixels within the given rectangle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetRGBRect(Int32Rect rect, byte red, byte green, byte blue)
        {
            UI.Native.GenericImage.SetRGBRect(Handle, rect, red, green, blue);
        }

        /// <summary>
        /// Sets the type of image returned by GetType().
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetImageType(BitmapType type)
        {
            UI.Native.GenericImage.SetImageType(Handle, (int)type);
        }

        /// <summary>
        /// Returns an identical copy of this image.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage Copy()
        {
            return new(UI.Native.GenericImage.Copy(Handle));
        }

        /// <summary>
        /// Creates a fresh image.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Reset(int width, int height, bool clear = true)
        {
            return UI.Native.GenericImage.CreateFreshImage(Handle, width, height, clear);
        }

        /// <summary>
        /// Initialize the image data with zeroes (the default) or with the byte value given as value.
        /// </summary>
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
        /// Initializes the image alpha channel data.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InitAlpha()
        {
            UI.Native.GenericImage.InitAlpha(Handle);
        }

        /// <summary>
        /// Blurs the image in both horizontal and vertical directions by the specified
        /// pixel blurRadius.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage Blur(int blurRadius)
        {
            return new(UI.Native.GenericImage.Blur(Handle, blurRadius));
        }

        /// <summary>
        /// Blurs the image in the horizontal direction only.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage BlurHorizontal(int blurRadius)
        {
            return new(UI.Native.GenericImage.BlurHorizontal(Handle, blurRadius));
        }

        /// <summary>
        /// Blurs the image in the vertical direction only.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage BlurVertical(int blurRadius)
        {
            return new(UI.Native.GenericImage.BlurVertical(Handle, blurRadius));
        }

        /// <summary>
        /// Returns a mirrored copy of the image.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage Mirror(bool horizontally = true)
        {
            return new(UI.Native.GenericImage.Mirror(Handle, horizontally));
        }

        /// <summary>
        /// Copy the data of the given image to the specified position in this image.
        /// </summary>
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Replace(byte r1, byte g1, byte b1, byte r2, byte g2, byte b2)
        {
            UI.Native.GenericImage.Replace(Handle, r1, g1, b1, r2, g2, b2);
        }

        /// <summary>
        /// Changes the size of the image in-place by scaling it: after a call to this
        /// function,the image will have the given width and height.
        /// </summary>
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage SizeNoScale(Int32Size size, Int32Point pos, int red, int green, int blue)
        {
            var image = UI.Native.GenericImage.Size(Handle, size, pos, red, green, blue);
            return new GenericImage(image);
        }
        
        /// <summary>
        /// Returns a copy of the image rotated 90 degrees in the direction indicated by clockwise.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage Rotate90(bool clockwise = true)
        {
            return new(UI.Native.GenericImage.Rotate90(Handle, clockwise));
        }

        /// <summary>
        /// Returns a copy of the image rotated by 180 degrees.
        /// </summary>
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RotateHue(double angle)
        {
            UI.Native.GenericImage.RotateHue(Handle, angle);
        }

        /// <summary>
        /// Changes the saturation of each pixel in the image.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ChangeSaturation(double factor)
        {
            UI.Native.GenericImage.ChangeSaturation(Handle, factor);
        }

        /// <summary>
        /// Changes the brightness(value) of each pixel in the image.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ChangeBrightness(double factor)
        {
            UI.Native.GenericImage.ChangeBrightness(Handle, factor);
        }

        /// <summary>
        /// Returns the file load flags used for this object.
        /// </summary>
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetLoadFlags(GenericImageLoadFlags flags)
        {
            UI.Native.GenericImage.SetLoadFlags(Handle, (int)flags);
        }

        /// <summary>
        /// Changes the hue, the saturation and the brightness(value) of each pixel in the image.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ChangeHSV(double angleH, double factorS, double factorV)
        {
            UI.Native.GenericImage.ChangeHSV(Handle, angleH, factorS, factorV);
        }

        /// <summary>
        /// Returns a scaled version of the image.
        /// </summary>
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ConvertAlphaToMask(byte threshold = AlphaChannelThreshold)
        {
            return UI.Native.GenericImage.ConvertAlphaToMask(Handle, threshold);
        }

        /// <summary>
        /// If the image has alpha channel, this method converts it to mask using the
        /// specified color as the mask color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ConvertAlphaToMaskUseColor(
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage ConvertToGreyscale(double weight_r, double weight_g, double weight_b)
        {
            return new(UI.Native.GenericImage.ConvertToGreyscaleEx(Handle, weight_r, weight_g, weight_b));
        }

        /// <summary>
        /// Returns a greyscale version of the image.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage ConvertToGreyscale()
        {
            return new(UI.Native.GenericImage.ConvertToGreyscale(Handle));
        }

        /// <summary>
        /// Returns monochromatic version of the image.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage ConvertToMono(byte r, byte g, byte b)
        {
            return new(UI.Native.GenericImage.ConvertToMono(Handle, r, g, b));
        }

        /// <summary>
        /// Returns disabled(dimmed) version of the image.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage ConvertToDisabled(byte brightness = 255)
        {
            return new(UI.Native.GenericImage.ConvertToDisabled(Handle, brightness));
        }

        /// <summary>
        /// Returns a changed version of the image based on the given lightness.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage ChangeLightness(int alpha)
        {
            return new(UI.Native.GenericImage.ChangeLightness(Handle, alpha));
        }

        /// <summary>
        /// Return alpha value at given pixel location.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetAlpha(int x, int y)
        {
            return UI.Native.GenericImage.GetAlpha(Handle, x, y);
        }

        /// <summary>
        /// Returns the red intensity at the given coordinate.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetRed(int x, int y)
        {
            return UI.Native.GenericImage.GetRed(Handle, x, y);
        }

        /// <summary>
        /// Returns the green intensity at the given coordinate.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetGreen(int x, int y)
        {
            return UI.Native.GenericImage.GetGreen(Handle, x, y);
        }

        /// <summary>
        /// Returns the blue intensity at the given coordinate.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetBlue(int x, int y)
        {
            return UI.Native.GenericImage.GetBlue(Handle, x, y);
        }

        /// <summary>
        /// Gets the red value of the mask color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaskRed()
        {
            return UI.Native.GenericImage.GetMaskRed(Handle);
        }

        /// <summary>
        /// Gets the green value of the mask color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte GetMaskGreen()
        {
            return UI.Native.GenericImage.GetMaskGreen(Handle);
        }

        /// <summary>
        /// Gets the blue value of the mask color.
        /// </summary>
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetOptionAsInt(string name)
        {
            return UI.Native.GenericImage.GetOptionInt(Handle, name);
        }

        /// <summary>
        /// Returns a sub image of the current one as long as the rect belongs entirely to the image.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage GetSubImage(Int32Rect rect)
        {
            return new(UI.Native.GenericImage.GetSubImage(Handle, rect));
        }

        /// <summary>
        /// Gets the type of image found when image was loaded or specified when image was saved.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetImageType()
        {
            return UI.Native.GenericImage.GetImageType(Handle);
        }

        /// <summary>
        /// Returns true if this image has alpha channel, false otherwise.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAlpha()
        {
            return UI.Native.GenericImage.HasAlpha(Handle);
        }

        /// <summary>
        /// Returns true if there is a mask active, false otherwise.
        /// </summary>
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsTransparent(int x, int y, byte threshold = AlphaChannelThreshold)
        {
            return UI.Native.GenericImage.IsTransparent(Handle, x, y, threshold);
        }

        /// <summary>
        /// Loads an image from an input stream.
        /// </summary>
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LoadFromFile(string name, string mimetype, int index = -1)
        {
            return UI.Native.GenericImage.LoadFileWithMimeType(Handle, name, mimetype, index);
        }

        /// <summary>
        /// Loads an image from an input stream.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LoadFromStream(Stream stream, string mimetype, int index = -1)
        {
            var inputStream = new UI.Native.InputStream(stream);
            return UI.Native.GenericImage.LoadStreamWithMimeType(Handle, inputStream, mimetype, index);
        }

        /// <summary>
        /// Saves an image in the given stream.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SaveToStream(Stream stream, string mimetype)
        {
            var outputStream = new UI.Native.OutputStream(stream);
            return UI.Native.GenericImage.SaveStreamWithMimeType(Handle, outputStream, mimetype);
        }

        /// <summary>
        /// Saves an image in the named file.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SaveToFile(string filename, BitmapType bitmapType)
        {
            return UI.Native.GenericImage.SaveFileWithBitmapType(Handle, filename, (int)bitmapType);
        }

        /// <summary>
        /// Saves an image in the named file.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SaveToFile(string filename, string mimetype)
        {
            return UI.Native.GenericImage.SaveFileWithMimeType(Handle, filename, mimetype);
        }

        /// <summary>
        /// Saves an image in the named file.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SaveToFile(string filename)
        {
            return UI.Native.GenericImage.SaveFile(Handle, filename);
        }

        /// <summary>
        /// Saves an image in the given stream.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SaveToStream(Stream stream, BitmapType type)
        {
            var outputStream = new UI.Native.OutputStream(stream);
            return UI.Native.GenericImage.SaveStreamWithBitmapType(Handle, outputStream, (int)type);
        }

        /// <summary>
        /// Sets the image data without performing checks.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void SetData(
            IntPtr data,
            int new_width,
            int new_height,
            bool static_data = false)
        {
            UI.Native.GenericImage.SetDataWithSize(Handle, data, new_width, new_height, static_data);
        }

        /// <summary>
        /// Register an image handler.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void AddHandler(IntPtr handler)
        {
            UI.Native.GenericImage.AddHandler(handler);
        }

        /// <summary>
        /// Finds the handler with the given name.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static IntPtr FindHandlerByName(string name)
        {
            return UI.Native.GenericImage.FindHandlerByName(name);
        }

        /// <summary>
        /// Finds the handler associated with the given extension and type.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static IntPtr FindHandlerByExt(string extension, BitmapType bitmapType)
        {
            return UI.Native.GenericImage.FindHandlerByExt(extension, (int)bitmapType);
        }

        /// <summary>
        /// Finds the handler associated with the given image type.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static IntPtr FindHandlerByBitmapType(BitmapType bitmapType)
        {
            return UI.Native.GenericImage.FindHandlerByBitmapType((int)bitmapType);
        }

        /// <summary>
        /// Finds the handler associated with the given MIME type.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static IntPtr FindHandlerByMime(string mimetype)
        {
            return UI.Native.GenericImage.FindHandlerByMime(mimetype);
        }

        /// <summary>
        /// Adds a handler at the start of the static list of format handlers.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void InsertHandler(IntPtr handler)
        {
            UI.Native.GenericImage.InsertHandler(handler);
        }

        /// <summary>
        /// Returns pointer to the array storing the alpha values for this image.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal IntPtr GetAlphaData()
        {
            return UI.Native.GenericImage.GetAlphaData(Handle);
        }

        /// <summary>
        /// Returns the image data as an array.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal IntPtr GetData()
        {
            return UI.Native.GenericImage.GetData(Handle);
        }

        /// <summary>
        /// Creates a fresh image.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool CreateData(int width, int height, IntPtr data, bool static_data = false)
        {
            return UI.Native.GenericImage.CreateData(Handle,  width, height, data, static_data);
        }

        /// <summary>
        /// Creates a fresh image.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool CreateData(int width, int height, IntPtr data, IntPtr alpha, bool static_data = false)
        {
            return UI.Native.GenericImage.CreateAlphaData(Handle, width, height, data, alpha, static_data);
        }

        /// <summary>
        /// Sets the image data without performing checks.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void SetAlphaData(IntPtr alpha = default, bool static_data = false)
        {
            UI.Native.GenericImage.SetAlphaData(Handle, alpha, static_data);
        }

        /// <summary>
        /// Sets the image data without performing checks.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void SetData(IntPtr data, bool static_data = false)
        {
            UI.Native.GenericImage.SetData(Handle, data, static_data);
        }

        /// <inheritdoc/>
        protected override void DisposeUnmanagedResources()
        {
            UI.Native.GenericImage.DeleteImage(Handle);
        }
    }
}


/*
wxImage() [2/12]
wxImage::wxImage	(	int 	width,
int 	height,
bool 	clear = true 
)		
Creates an image with the given size and clears it if requested.

Does not create an alpha channel.

Parameters
width	Specifies the width of the image.
height	Specifies the height of the image.
clear	If true, initialize the image to black.

==============

wxImage() [3/12]
wxImage::wxImage	(	const wxSize & 	sz,
bool 	clear = true 
)		
This is an overloaded member function, provided for convenience.
It differs from the above function only in what argument(s) it accepts.

==============
wxImage() [4/12]
wxImage::wxImage	(	int 	width,
int 	height,
unsigned char * 	data,
bool 	static_data = false 
)		
Creates an image from data in memory.

If static_data is false then the wxImage will take ownership of the
data and free it afterwards. For this, it has to be allocated with malloc.

Parameters
width	Specifies the width of the image.
height	Specifies the height of the image.
data	A pointer to RGB data
static_data	Indicates if the data should be free'd after use
==============
wxImage() [5/12]
wxImage::wxImage	(	const wxSize & 	sz,
unsigned char * 	data,
bool 	static_data = false 
)		
This is an overloaded member function, provided for convenience.
It differs from the above function only in what argument(s) it accepts.

==============
wxImage() [6/12]
wxImage::wxImage	(	int 	width,
int 	height,
unsigned char * 	data,
unsigned char * 	alpha,
bool 	static_data = false 
)		
Creates an image from data in memory.

If static_data is false then the wxImage will take ownership of the data
and free it afterwards. For this, it has to be allocated with malloc.

Parameters
width	Specifies the width of the image.
height	Specifies the height of the image.
data	A pointer to RGB data
alpha	A pointer to alpha-channel data
static_data	Indicates if the data should be free'd after use
==============
wxImage() [7/12]
wxImage::wxImage	(	const wxSize & 	sz,
unsigned char * 	data,
unsigned char * 	alpha,
bool 	static_data = false 
)		
This is an overloaded member function, provided for convenience.
It differs from the above function only in what argument(s) it accepts.

==============
wxImage() [8/12]
wxImage::wxImage	(	const char *const * 	xpmData	)	
explicit
Creates an image from XPM data.

Parameters
xpmData	A pointer to XPM image data.
wxPerl Note: Not supported by wxPerl.

This constructor has become explicit in wxWidgets 3.1.6.

==============
wxImage() [9/12]
wxImage::wxImage	(	const wxString & 	name,
wxBitmapType 	type = wxBITMAP_TYPE_ANY,
int 	index = -1 
)		
Creates an image from a file.

Parameters
name	Name of the file from which to load the image.
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
This is only used by GIF, ICO and TIFF handlers. The default value (-1) means
"choose the default image" and is interpreted as the first image (index=0) by the GIF and
TIFF handler and as the largest and most colorful one by the ICO handler.
Remarks
Depending on how wxWidgets has been configured and by which handlers have been loaded,
not all formats may be available. Any handler other than BMP must be previously initialized
with wxImage::AddHandler or wxInitAllImageHandlers.
Note
You can use GetOptionInt() to get the hotspot when loading cursor files:
int hotspot_x = image.GetOptionInt(wxIMAGE_OPTION_CUR_HOTSPOT_X);
int hotspot_y = image.GetOptionInt(wxIMAGE_OPTION_CUR_HOTSPOT_Y);
See also
LoadFile()
==============
wxImage() [10/12]
wxImage::wxImage	(	const wxString & 	name,
const wxString & 	mimetype,
int 	index = -1 
)		
Creates an image from a file using MIME-types to specify the type.

Parameters
name	Name of the file from which to load the image.
mimetype	MIME type string (for example 'image/jpeg')
index	See description in wxImage(const wxString&, wxBitmapType, int) overload.
==============
wxImage() [11/12]
wxImage::wxImage	(	wxInputStream & 	stream,
wxBitmapType 	type = wxBITMAP_TYPE_ANY,
int 	index = -1 
)		
Creates an image from a stream.

Parameters
stream	Opened input stream from which to load the image. Currently, the stream must support seeking.
type	See description in wxImage(const wxString&, wxBitmapType, int) overload.
index	See description in wxImage(const wxString&, wxBitmapType, int) overload.
==============
wxImage() [12/12]
wxImage::wxImage	(	wxInputStream & 	stream,
const wxString & 	mimetype,
int 	index = -1 
)		
Creates an image from a stream using MIME-types to specify the type.

Parameters
stream	Opened input stream from which to load the image. Currently, the stream must support seeking.
mimetype	MIME type string (for example 'image/jpeg')
index	See description in wxImage(const wxString&, wxBitmapType, int) overload.
==============
AddHandler()
static void wxImage::AddHandler	(	wxImageHandler * 	handler	)	
static
Register an image handler.

Typical example of use:

wxImage::AddHandler(new wxPNGHandler);
See Available image handlers for a list of the available handlers. You can also use
wxInitAllImageHandlers() to add handlers for all the image formats supported by wxWidgets at once.

Parameters
handler	A heap-allocated handler object which will be deleted by wxImage if it is removed
later by RemoveHandler() or at program shutdown.
==============
Blur()
wxImage wxImage::Blur	(	int 	blurRadius	)	const
Blurs the image in both horizontal and vertical directions by the specified pixel blurRadius.

This should not be used when using a single mask color for transparency.

See also
BlurHorizontal(), BlurVertical()
==============
BlurHorizontal()
wxImage wxImage::BlurHorizontal	(	int 	blurRadius	)	const
Blurs the image in the horizontal direction only.

This should not be used when using a single mask color for transparency.

See also
Blur(), BlurVertical()
==============
BlurVertical()
wxImage wxImage::BlurVertical	(	int 	blurRadius	)	const
Blurs the image in the vertical direction only.

This should not be used when using a single mask color for transparency.

See also
Blur(), BlurHorizontal()
==============
CanRead() [1/2]
static bool wxImage::CanRead	(	const wxString & 	filename	)	
static
Returns true if at least one of the available image handlers can read the file with the given name.

See wxImageHandler::CanRead for more info.
==============
CanRead() [2/2]
static bool wxImage::CanRead	(	wxInputStream & 	stream	)	
static
Returns true if at least one of the available image handlers can read the data in the given stream.

See wxImageHandler::CanRead for more info.
==============
ChangeBrightness()
void wxImage::ChangeBrightness	(	double 	factor	)	
Changes the brightness (value) of each pixel in the image.

factor is a double in the range [-1.0..+1.0], where -1.0 corresponds to -100 percent
and +1.0 corresponds to +100 percent.
==============
ChangeHSV()
void wxImage::ChangeHSV	(	double 	angleH,
double 	factorS,
double 	factorV 
)		
Changes the hue, the saturation and the brightness (value) of each pixel in the image.

angleH is a double in the range [-1.0..+1.0], where -1.0 corresponds to -360 degrees and
+1.0 corresponds to +360 degrees, factorS is a double in the range [-1.0..+1.0], where -1.0
corresponds to -100 percent and +1.0 corresponds to +100 percent and factorV is a double in
the range [-1.0..+1.0], where -1.0 corresponds to -100 percent and +1.0 corresponds to +100 percent.

==============
ChangeLightness()
wxImage wxImage::ChangeLightness	(	int 	alpha	)	const
Returns a changed version of the image based on the given lightness.

This utility function simply darkens or lightens a color, based on the specified percentage
ialpha. ialpha of 0 would make the color completely black, 200 completely white and 100 would
not change the color.

Remarks
This function calls wxcolor::ChangeLightness() for each pixel in the image.
==============
ChangeSaturation()
void wxImage::ChangeSaturation	(	double 	factor	)	
Changes the saturation of each pixel in the image.

factor is a double in the range [-1.0..+1.0], where -1.0 corresponds to -100 percent and +1.0
corresponds to +100 percent.
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
ComputeHistogram()
unsigned long wxImage::ComputeHistogram	(	wxImageHistogram & 	histogram	)	const
Computes the histogram of the image.

histogram is a reference to wxImageHistogram object. wxImageHistogram is a specialization
of wxHashMap "template" and is defined as follows:

class WXDLLEXPORT wxImageHistogramEntry
{
public:
    wxImageHistogramEntry() : index(0), value(0) {}
    unsigned long index;
    unsigned long value;
};
 
WX_DECLARE_EXPORTED_HASH_MAP(unsigned long, wxImageHistogramEntry,
                            wxIntegerHash, wxIntegerEqual,
                            wxImageHistogram);
Returns
Returns number of colors in the histogram.
==============
ConvertAlphaToMask() [1/2]
bool wxImage::ConvertAlphaToMask	(	unsigned char 	mr,
unsigned char 	mg,
unsigned char 	mb,
unsigned char 	threshold = wxIMAGE_ALPHA_THRESHOLD 
)		
If the image has alpha channel, this method converts it to mask using the specified color as
the mask color.

If the image has an alpha channel, all pixels with alpha value less than threshold are replaced
with the mask color and the alpha channel is removed. Otherwise nothing is done.

Since
2.9.0
Parameters
mr	The red component of the mask color.
mg	The green component of the mask color.
mb	The blue component of the mask color.
threshold	Pixels with alpha channel values below the given threshold are considered to be
transparent, i.e. the corresponding mask pixels are set. Pixels with the alpha values above the
threshold are considered to be opaque.
Returns
Returns true on success, false on error.
==============
ConvertAlphaToMask() [2/2]
bool wxImage::ConvertAlphaToMask	(	unsigned char 	threshold = wxIMAGE_ALPHA_THRESHOLD	)	
If the image has alpha channel, this method converts it to mask.

If the image has an alpha channel, all pixels with alpha value less than threshold are replaced
with the mask color and the alpha channel is removed. Otherwise nothing is done.

The mask color is chosen automatically using FindFirstUnusedcolor(), see the overload below if
this is not appropriate.

Returns
Returns true on success, false on error.
==============
ConvertToDisabled()
wxImage wxImage::ConvertToDisabled	(	unsigned char 	brightness = 255	)	const
Returns disabled (dimmed) version of the image.

Remarks
This function calls wxcolor::MakeDisabled() for each pixel in the image.
==============
ConvertToGreyscale() [1/2]
wxImage wxImage::ConvertToGreyscale	(		)	
Returns a greyscale version of the image.

Since
2.9.0
==============
ConvertToGreyscale() [2/2]
wxImage wxImage::ConvertToGreyscale	(	double 	weight_r,
double 	weight_g,
double 	weight_b 
)		const
Returns a greyscale version of the image.

The returned image uses the luminance component of the original to calculate the greyscale.
Defaults to using the standard ITU-T BT.601 when converting to YUV, where every pixel
equals (R * weight_r) + (G * weight_g) + (B * weight_b).

Remarks
This function calls wxcolor::MakeGrey() for each pixel in the image.
==============
ConvertToMono()
wxImage wxImage::ConvertToMono	(	unsigned char 	r,
unsigned char 	g,
unsigned char 	b 
)		const
Returns monochromatic version of the image.

The returned image has white color where the original has (r,g,b) color and black color
everywhere else.

Remarks
This function calls wxcolor::MakeMono() for each pixel in the image.
==============
Copy()
wxImage wxImage::Copy	(		)	const
Returns an identical copy of this image.

==============
Create() [1/6]
bool wxImage::Create	(	const wxSize & 	sz,
bool 	clear = true 
)		
This is an overloaded member function, provided for convenience. It differs from the above
function only in what argument(s) it accepts.

==============
Create() [2/6]
bool wxImage::Create	(	const wxSize & 	sz,
unsigned char * 	data,
bool 	static_data = false 
)		
This is an overloaded member function, provided for convenience. It differs from the above
function only in what argument(s) it accepts.

==============
Create() [3/6]
bool wxImage::Create	(	const wxSize & 	sz,
unsigned char * 	data,
unsigned char * 	alpha,
bool 	static_data = false 
)		
This is an overloaded member function, provided for convenience. It differs from the above
function only in what argument(s) it accepts.

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
Destroy()
void wxImage::Destroy	(		)	
Destroys the image data.

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
FindHandler() [1/3]
static wxImageHandler* wxImage::FindHandler	(	const wxString & 	extension,
wxBitmapType 	imageType 
)		
static
Finds the handler associated with the given extension and type.

Parameters
extension	The file extension, such as "bmp".
imageType	The image type; one of the wxBitmapType values.
Returns
A pointer to the handler if found, NULL otherwise.
See also
wxImageHandler
==============
FindHandler() [2/3]
static wxImageHandler* wxImage::FindHandler	(	const wxString & 	name	)	
static
Finds the handler with the given name.

Parameters
name	The handler name.
Returns
A pointer to the handler if found, NULL otherwise.
See also
wxImageHandler
==============
FindHandler() [3/3]
static wxImageHandler* wxImage::FindHandler	(	wxBitmapType 	imageType	)	
static
Finds the handler associated with the given image type.

Parameters
imageType	The image type; one of the wxBitmapType values.
Returns
A pointer to the handler if found, NULL otherwise.
See also
wxImageHandler
==============
FindHandlerMime()
static wxImageHandler* wxImage::FindHandlerMime	(	const wxString & 	mimetype	)	
static
Finds the handler associated with the given MIME type.

Parameters
mimetype	MIME type.
Returns
A pointer to the handler if found, NULL otherwise.
See also
wxImageHandler
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
GetDefaultLoadFlags()
static int wxImage::GetDefaultLoadFlags	(		)	
static
Returns the currently used default file load flags.

See SetDefaultLoadFlags() for more information about these flags.

==============
GetGreen()
unsigned char wxImage::GetGreen	(	int 	x,
int 	y 
)		const
Returns the green intensity at the given coordinate.

==============
GetHandlers()
static wxList& wxImage::GetHandlers	(		)	
static
Returns the static list of image format handlers.

See also
wxImageHandler
==============
GetHeight()
int wxImage::GetHeight	(		)	const
Gets the height of the image in pixels.

See also
GetWidth(), GetSize()
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
GetImageExtWildcard()
static wxString wxImage::GetImageExtWildcard	(		)	
static
Iterates all registered wxImageHandler objects, and returns a string containing file extension
masks suitable for passing to file open/save dialog boxes.

Returns
The format of the returned string is "(*.ext1;*.ext2)|*.ext1;*.ext2". It is usually a good idea
to prepend a description before passing the result to the dialog. Example:
wxFileDialog FileDlg( this, "Choose Image", ::wxGetCwd(), "",
                      _("Image Files ") + wxImage::GetImageExtWildcard(),
                      wxFD_OPEN );
See also
wxImageHandler
==============
GetLoadFlags()
int wxImage::GetLoadFlags	(		)	const
Returns the file load flags used for this object.

See SetLoadFlags() for more information about these flags.
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

Generic options:

wxIMAGE_OPTION_FILENAME: The name of the file from which the image was loaded.
Options specific to wxGIFHandler:

Parameters
name	The name of the option, case-insensitive.
Returns
The value of the option or an empty string if not found. Use HasOption() if an empty string can
be a valid option value.
See also
SetOption(), GetOptionInt(), HasOption()
==============
GetOptionInt()
int wxImage::GetOptionInt	(	const wxString & 	name	)	const
Gets a user-defined integer-valued option.

The function is case-insensitive to name. If the given option is not present,
the function returns 0. Use HasOption() if 0 is a possibly valid value for the option.

Generic options:

Parameters
name	The name of the option, case-insensitive.
Returns
The value of the option or 0 if not found. Use HasOption() if 0 can be a valid option value.
See also
SetOption(), GetOption()

===========
GetOrFindMaskcolor()
bool wxImage::GetOrFindMaskcolor	(	unsigned char * 	r,
unsigned char * 	g,
unsigned char * 	b 
)		const
Get the current mask color or find a suitable unused color that could be used as a mask color.

Returns true if the image currently has a mask.

===========
GetPalette()
const wxPalette& wxImage::GetPalette	(		)	const
Returns the palette associated with the image.

Currently the palette is only used when converting to wxBitmap under Windows.

Some of the wxImage handlers have been modified to set the palette if one exists in the image
file (usually 256 or less color images in GIF or PNG format).

===========
GetRed()
unsigned char wxImage::GetRed	(	int 	x,
int 	y 
)		const
Returns the red intensity at the given coordinate.

===========
GetSize()
wxSize wxImage::GetSize	(		)	const
Returns the size of the image in pixels.

Since
2.9.0
See also
GetHeight(), GetWidth()

===========
GetSubImage()
wxImage wxImage::GetSubImage	(	const wxRect & 	rect	)	const
Returns a sub image of the current one as long as the rect belongs entirely to the image.

===========
GetType()
wxBitmapType wxImage::GetType	(		)	const
Gets the type of image found by LoadFile() or specified with SaveFile().


===========
GetWidth()
int wxImage::GetWidth	(		)	const
Gets the width of the image in pixels.

See also
GetHeight(), GetSize()

===========
HasAlpha()
bool wxImage::HasAlpha	(		)	const
Returns true if this image has alpha channel, false otherwise.

See also
GetAlpha(), SetAlpha()

===========
HasMask()
bool wxImage::HasMask	(		)	const
Returns true if there is a mask active, false otherwise.

===========
HasOption()
bool wxImage::HasOption	(	const wxString & 	name	)	const
Returns true if the given option is present.

The function is case-insensitive to name.

The lists of the currently supported options are in GetOption() and GetOptionInt() function docs.

See also
SetOption(), GetOption(), GetOptionInt()

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
InsertHandler()
static void wxImage::InsertHandler	(	wxImageHandler * 	handler	)	
static
Adds a handler at the start of the static list of format handlers.

Parameters
handler	A new image format handler object. There is usually only one instance of a given handler
class in an application session.
See also
wxImageHandler

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

Parameters
name	Name of the file from which to load the image.
type	See the description in the LoadFile(wxInputStream&, wxBitmapType, int) overload.
index	See the description in the LoadFile(wxInputStream&, wxBitmapType, int) overload.
LoadFile() [3/4]
virtual bool wxImage::LoadFile	(	wxInputStream & 	stream,
const wxString & 	mimetype,
int 	index = -1 
)		
virtual
Loads an image from an input stream.

Parameters
stream	Opened input stream from which to load the image. Currently, the stream must support seeking.
mimetype	MIME type string (for example 'image/jpeg')
index	See the description in the LoadFile(wxInputStream&, wxBitmapType, int) overload.

===========
LoadFile() [4/4]
virtual bool wxImage::LoadFile	(	wxInputStream & 	stream,
wxBitmapType 	type = wxBITMAP_TYPE_ANY,
int 	index = -1 
)		
virtual
Loads an image from an input stream.

If the file can't be loaded, this function returns false and logs an error using wxLogError().
If the file can be loaded but some problems were detected while doing it, it can also call
wxLogWarning() to notify about these problems. If this is undesirable, use SetLoadFlags()
to reset Load_Verbose flag and suppress these warnings.

Parameters
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
See also
SaveFile()
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

Parameters
image	The image containing the data to copy, must be valid.
x	The horizontal position of the position to copy the data to.
y	The vertical position of the position to copy the data to.
alphaBlend	This parameter (new in wx 3.1.5) determines whether the alpha values of the original
image replace (default) or are composed with the alpha channel of this image. Notice that alpha
blending overrides the mask handling.

===========
RemoveHandler()
static bool wxImage::RemoveHandler	(	const wxString & 	name	)	
static
Finds the handler with the given name, and removes it.

The handler is also deleted.

Parameters
name	The handler name.
Returns
true if the handler was found and removed, false otherwise.
See also
wxImageHandler

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

See also
Scale()

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
See also
Size()
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

Since
2.9.2
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
SaveFile() [3/5]
virtual bool wxImage::SaveFile	(	const wxString & 	name,
wxBitmapType 	type 
)		const
virtual
Saves an image in the named file.

Parameters
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

Parameters
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
virtual
Saves an image in the given stream.

Parameters
stream	Opened output stream to save the image to.
type	MIME type.

===========

Scale()
wxImage wxImage::Scale	(	int 	width,
int 	height,
wxImageResizeQuality 	quality = wxIMAGE_QUALITY_NORMAL 
)		const
Returns a scaled version of the image.

This is also useful for scaling bitmaps in general as the only other way to scale bitmaps is to
blit a wxMemoryDC into another wxMemoryDC.

The parameter quality determines what method to use for resampling the image, see wxImageResizeQuality
documentation.

It should be noted that although using wxIMAGE_QUALITY_HIGH produces much nicer looking results it is
a slower method. Downsampling will use the box averaging method which seems to operate very fast. If you are upsampling larger images using this method you will most likely notice that it is a bit slower and in extreme cases it will be quite substantially slower as the bicubic algorithm has to process a lot of data.

It should also be noted that the high quality scaling may not work as expected when using a single
mask color for transparency, as the scaling will blur the image and will therefore remove the mask
partially. Using the alpha channel will work.

Example:

// get the bitmap from somewhere
wxBitmap bmp = ...;
 
// rescale it to have size of 32*32
if ( bmp.GetWidth() != 32 || bmp.GetHeight() != 32 )
{
    wxImage image = bmp.ConvertToImage();
    bmp = wxBitmap(image.Scale(32, 32));
 
    // another possibility:
    image.Rescale(32, 32);
    bmp = image;
}
Note
The algorithm used for the default (normal) quality value doesn't work with images larger
than 65536 (2^16) pixels in either dimension for 32-bit programs. For 64-bit programs the
limit is 2^48 and so not relevant in practice.
See also
Rescale()
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

See also
GetOption(), GetOptionInt(), HasOption()
===========
SetOption() [2/2]
void wxImage::SetOption	(	const wxString & 	name,
int 	value 
)		
This is an overloaded member function, provided for convenience. It differs from the above
function only in what argument(s) it accepts.
===========
SetPalette()
void wxImage::SetPalette	(	const wxPalette & 	palette	)	
Associates a palette with the image.

The palette may be used when converting wxImage to wxBitmap (MSW only at present) or in file save
operations (none as yet).

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

Parameters
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