using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public partial class NativeDrawing
    {
        /// <summary>
        /// Creates native generic image.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateGenericImage();

        /// <summary>
        /// Creates native generic image.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateGenericImage(int width, int height, bool clear = false);

        /// <summary>
        /// Creates native generic image.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateGenericImage(SizeI size, bool clear = false);

        /// <summary>
        /// Creates native generic image.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateGenericImage(
            string fileName,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1);

        /// <summary>
        /// Creates native generic image.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateGenericImage(string name, string mimetype, int index = -1);

        /// <summary>
        /// Creates native generic image.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateGenericImage(
            Stream stream,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1);

        /// <summary>
        /// Creates native generic image.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateGenericImage(
            Stream stream,
            string mimeType,
            int index = -1);

        /// <summary>
        /// Creates native generic image.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateGenericImage(
            int width,
            int height,
            IntPtr data,
            bool staticData = false);

        /// <summary>
        /// Creates native generic image.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateGenericImage(
            int width,
            int height,
            IntPtr data,
            IntPtr alpha,
            bool staticData = false);

        /// <summary>
        /// Gets the width of the native generic image in pixels.
        /// </summary>
        /// <param name="genericImage">Native generic image instance.</param>
        public abstract int GetGenericImageWidth(GenericImage img);

        /// <summary>
        /// Gets the height of the native generic image in pixels.
        /// </summary>
        /// <param name="genericImage">Native generic image instance.</param>
        public abstract int GetGenericImageHeight(GenericImage img);

        /// <summary>
        /// Disposes native generic image.
        /// </summary>
        /// <param name="genericImage">Native generic image instance.</param>
        public abstract void DisposeGenericImage(GenericImage img);

        /// <summary>
        /// Returns <c>true</c> if image data is present in the native generic image.
        /// </summary>
        /// <param name="genericImage">Native generic image instance.</param>
        public abstract bool GetGenericImageIsOk(GenericImage img);

        /// <summary>
        /// Returns <c>true</c> if at least one of the available image handlers can read the file
        /// with the given name.
        /// </summary>
        /// <param name="filename">Name of the file from which to load the image.</param>
        /// <returns></returns>
        public abstract bool GenericImageCanRead(string filename);

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
        public abstract bool GenericImageCanRead(Stream stream);

        /// <summary>
        /// Iterates all registered image handlers, and returns a string containing
        /// file extension masks suitable for passing to file open/save dialog boxes.
        /// </summary>
        /// <returns>
        /// The format of the returned string is "(*.ext1;*.ext2)|*.ext1;*.ext2". It is usually
        /// a good idea to prepend a description before passing the result to the dialog.
        /// </returns>
        public abstract string GetGenericImageExtWildcard();

        /// <summary>
        /// Finds image load/save handler with the given name, and removes it.
        /// </summary>
        /// <param name="name">Name of the handler.</param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        public abstract bool GenericImageRemoveHandler(string name);

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
        public abstract int GetGenericImageCount(
            string filename,
            BitmapType bitmapType = BitmapType.Any);

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
        public abstract int GetGenericImageCount(
            Stream stream,
            BitmapType bitmapType = BitmapType.Any);

        /// <summary>
        /// Deletes all image handlers.
        /// </summary>
        public abstract void GenericImageCleanUpHandlers();

        /// <summary>
        /// Returns the currently used default file load flags.
        /// </summary>
        /// <returns></returns>
        public abstract GenericImageLoadFlags GetGenericImageDefaultLoadFlags();

        /// <summary>
        /// Sets the default value for the flags used for loading image files.
        /// </summary>
        /// <param name="flags"></param>
        public abstract void SetGenericImageDefaultLoadFlags(GenericImageLoadFlags flags);

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
        public abstract void GenericImageSetAlpha(GenericImage img, int x, int y, byte alpha);

        /// <summary>
        /// Removes the alpha channel from the image.
        /// </summary>
        /// <remarks>
        /// This function should only be called if the image has alpha channel data,
        /// use <see cref="HasAlpha"/> to check for this.
        /// </remarks>
        /// <param name="genericImage">Native generic image instance.</param>
        public abstract void GenericImageClearAlpha(GenericImage img);

        /// <summary>
        /// Specifies whether there is a mask or not.
        /// </summary>
        /// <param name="hasMask"></param>
        public abstract void GenericImageSetMask(GenericImage img, bool hasMask = true);

        /// <summary>
        /// Sets the mask color for this image(and tells the image to use the mask).
        /// </summary>
        /// <param name="rgb">Color RGB.</param>
        public abstract void GenericImageSetMaskColor(GenericImage img, RGBValue rgb);

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
        public abstract bool GenericImageSetMaskFromImage(GenericImage img1, GenericImage img2, RGBValue mask);

        /// <summary>
        /// Sets a user-defined option.
        /// </summary>
        /// <remarks>
        /// See <see cref="GenericImageOptionNames"/> for known option names.
        /// </remarks>
        /// <param name="name">The name of the option, case-insensitive.</param>
        /// <param name="value">New option value.</param>
        public abstract void GenericImageSetOptionAsString(GenericImage img, string name, string value);

        /// <summary>
        /// Sets a user-defined option.
        /// </summary>
        /// <remarks>
        /// See <see cref="GenericImageOptionNames"/> for known option names.
        /// </remarks>
        /// <param name="name">The name of the option, case-insensitive.</param>
        /// <param name="value">New option value.</param>
        public abstract void GenericImageSetOptionAsInt(GenericImage img, string name, int value);

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
        public abstract void GenericImageSetRGB(GenericImage img, int x, int y, RGBValue rgb);

        /// <summary>
        /// Sets the color of the pixels within the given rectangle.
        /// </summary>
        /// <param name="rect">Rectangle within the image. If rectangle is null,
        /// <see cref="Bounds"/> property is used.</param>
        /// <param name="rgb">RGB Color.</param>
        public abstract void GenericImageSetRGBRect(GenericImage img, RGBValue rgb, RectI? rect = null);

        /// <summary>
        /// Sets the type of image returned by GetType().
        /// </summary>
        /// <param name="type">Type of the bitmap.</param>
        public abstract void GenericImageSetImageType(GenericImage img, BitmapType type);

        /// <summary>
        /// Returns an identical copy of this image.
        /// </summary>
        /// <returns></returns>
        /// <param name="genericImage">Native generic image instance.</param>
        public abstract GenericImage GenericImageCopy(GenericImage img);

        /// <summary>
        /// Creates a fresh image.
        /// </summary>
        /// <param name="width">New image width.</param>
        /// <param name="height">New image height</param>
        /// <param name="clear">If true, initialize the image to black.</param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        public abstract bool GenericImageReset(GenericImage img, int width, int height, bool clear = false);

        /// <summary>
        /// Initialize the image data with zeroes (the default) or with the byte value given as value.
        /// </summary>
        /// <param name="value"></param>
        public abstract void GenericImageClear(GenericImage img, byte value = 0);

        /// <summary>
        /// Destroys the image data.
        /// </summary>
        /// <param name="genericImage">Native generic image instance.</param>
        public abstract void GenericImageReset(GenericImage img);

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
        public abstract Color GenericImageFindFirstUnusedColor(
            GenericImage img,
            RGBValue? startRGB = null);

        /// <summary>
        /// Initializes the image alpha channel data.
        /// </summary>
        /// <remarks>
        /// It is an error to call it if the image already has alpha data. If it doesn't,
        /// alpha data will be by default initialized to all pixels being fully opaque.
        /// But if the image has a mask color, all mask pixels will be completely transparent.
        /// </remarks>
        /// <param name="genericImage">Native generic image instance.</param>
        public abstract void GenericImageInitAlpha(GenericImage img);

        /// <summary>
        /// Blurs the image in both horizontal and vertical directions by the specified
        /// <paramref name="blurRadius"/> (in pixels).
        /// </summary>
        /// <param name="blurRadius">Blur radius in pixels.</param>
        /// <returns>Blurred image.</returns>
        /// <remarks>
        /// This should not be used when using a single mask color for transparency.
        /// </remarks>
        public abstract GenericImage GenericImageBlur(GenericImage img, int blurRadius);

        /// <summary>
        /// Blurs the image in the horizontal direction only.
        /// </summary>
        /// <param name="blurRadius">Blur radius in pixels.</param>
        /// <returns>Blurred image.</returns>
        /// <remarks>
        /// This should not be used when using a single mask color for transparency.
        /// </remarks>
        public abstract GenericImage GenericImageBlurHorizontal(GenericImage img, int blurRadius);

        /// <summary>
        /// Blurs the image in the vertical direction only.
        /// </summary>
        /// <param name="blurRadius">Blur radius in pixels.</param>
        /// <returns>Blurred image.</returns>
        /// <remarks>
        /// This should not be used when using a single mask color for transparency.
        /// </remarks>
        public abstract GenericImage GenericImageBlurVertical(GenericImage img, int blurRadius);

        /// <summary>
        /// Returns a mirrored copy of the image.
        /// </summary>
        /// <param name="horizontally"></param>
        /// <returns>Mirrored copy of the image</returns>
        public abstract GenericImage GenericImageMirror(GenericImage img, bool horizontally = true);

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
        public abstract void GenericImagePaste(
            GenericImage img1,
            GenericImage img2,
            int x,
            int y,
            GenericImageAlphaBlendMode alphaBlend = GenericImageAlphaBlendMode.Overwrite);

        /// <summary>
        /// Replaces the color specified by (r1.R, r1.G, r1.B) by the color (r2.R, r2.G, r2.B).
        /// </summary>
        /// <param name="r1">RGB Color 1.</param>
        /// <param name="r2">RGB Color 2.</param>
        public abstract void GenericImageReplace(GenericImage img, RGBValue r1, RGBValue r2);

        /// <summary>
        /// Changes the size of the image in-place by scaling it: after a call to this
        /// function,the image will have the given width and height.
        /// </summary>
        /// <param name="width">New image width.</param>
        /// <param name="height">New image height.</param>
        /// <param name="quality">Scaling quality.</param>
        public abstract void GenericImageRescale(
            GenericImage img,
            int width,
            int height,
            GenericImageResizeQuality quality = GenericImageResizeQuality.Normal);

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
        public abstract void GenericImageResizeNoScale(
            GenericImage img,
            SizeI size,
            PointI pos,
            RGBValue? color = null);

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
        public abstract GenericImage GenericImageSizeNoScale(
            GenericImage img,
            SizeI size,
            PointI pos = default,
            RGBValue? color = null);

        /// <summary>
        /// Returns a copy of the image rotated 90 degrees in the direction indicated by clockwise.
        /// </summary>
        /// <param name="clockwise">Rotate direction.</param>
        /// <returns></returns>
        public abstract GenericImage GenericImageRotate90(GenericImage img, bool clockwise = true);

        /// <summary>
        /// Returns a copy of the image rotated by 180 degrees.
        /// </summary>
        /// <returns></returns>
        /// <param name="genericImage">Native generic image instance.</param>
        public abstract GenericImage GenericImageRotate180(GenericImage img);

        /// <summary>
        /// Rotates the hue of each pixel in the image by angle, which is a double in the
        /// range [-1.0..+1.0], where -1.0 corresponds to -360 degrees and +1.0 corresponds
        /// to +360 degrees.
        /// </summary>
        /// <param name="angle"></param>
        public abstract void GenericImageRotateHue(GenericImage img, double angle);

        /// <summary>
        /// Changes the saturation of each pixel in the image.
        /// </summary>
        /// <param name="factor">A double in the range [-1.0..+1.0], where -1.0 corresponds
        /// to -100 percent and +1.0 corresponds to +100 percent.</param>
        public abstract void GenericImageChangeSaturation(GenericImage img, double factor);

        /// <summary>
        /// Changes the brightness(value) of each pixel in the image.
        /// </summary>
        /// <param name="factor">A double in the range [-1.0..+1.0], where -1.0 corresponds
        /// to -100 percent and +1.0 corresponds to +100 percent.</param>
        public abstract void GenericImageChangeBrightness(GenericImage img, double factor);

        /// <summary>
        /// Returns the file load flags used for this object.
        /// </summary>
        /// <returns></returns>
        public abstract GenericImageLoadFlags GenericImageGetLoadFlags(GenericImage img);

        /// <summary>
        /// Sets the flags used for loading image files by this object.
        /// </summary>
        /// <remarks>
        /// The flags will affect any future calls to load from file functions for this object.
        /// To change the flags for all image objects, call <see cref="SetDefaultLoadFlags"/>
        /// before creating any of them.
        /// </remarks>
        /// <param name="flags"></param>
        public abstract void GenericImageSetLoadFlags(GenericImage img, GenericImageLoadFlags flags);

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
        public abstract void GenericImageChangeHSV(
            GenericImage img,
            double angleH,
            double factorS,
            double factorV);

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
        public abstract GenericImage GenericImageScale(
            GenericImage img,
            int width,
            int height,
            GenericImageResizeQuality quality = GenericImageResizeQuality.Normal);

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
        public abstract bool GenericImageConvertAlphaToMask(GenericImage img, byte threshold);

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
        public abstract bool GenericImageConvertAlphaToMask(
            GenericImage img,
            RGBValue rgb,
            byte threshold);

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
        public abstract GenericImage GenericImageConvertToGreyscale(
            GenericImage img,
            double weightR,
            double weightG,
            double weightB);

        /// <summary>
        /// Returns a greyscale version of the image.
        /// </summary>
        /// <returns></returns>
        /// <param name="genericImage">Native generic image instance.</param>
        public abstract GenericImage GenericImageConvertToGreyscale(GenericImage img);

        /// <summary>
        /// Returns monochromatic version of the image.
        /// </summary>
        /// <param name="rgb">RGB color.</param>
        /// <returns> The returned image has white color where the original has (r,g,b)
        /// color and black color everywhere else.</returns>
        public abstract GenericImage GenericImageConvertToMono(GenericImage img, RGBValue rgb);

        /// <summary>
        /// Returns disabled(dimmed) version of the image.
        /// </summary>
        /// <param name="brightness"></param>
        /// <returns></returns>
        public abstract GenericImage GenericImageConvertToDisabled(GenericImage img, byte brightness = 255);

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
        public abstract GenericImage GenericImageChangeLightness(GenericImage img, int ialpha);

        /// <summary>
        /// Return alpha value at given pixel location.
        /// </summary>
        /// <param name="x">X coordinate of the pixel.</param>
        /// <param name="y">Y coordinate of the pixel.</param>
        /// <returns></returns>
        public abstract byte GenericImageGetAlpha(GenericImage img, int x, int y);

        /// <summary>
        /// Gets <see cref="RGBValue"/> at given pixel location.
        /// </summary>
        /// <param name="x">X coordinate of the pixel.</param>
        /// <param name="y">Y coordinate of the pixel.</param>
        /// <returns></returns>
        public abstract RGBValue GenericImageGetRGB(GenericImage img, int x, int y);

        /// <summary>
        /// Gets <see cref="Color"/> at given pixel location.
        /// </summary>
        /// <param name="x">X coordinate of the pixel.</param>
        /// <param name="y">Y coordinate of the pixel.</param>
        /// <param name="withAlpha">If true alpha channel is also returned in result
        /// (<see cref="Color.A"/>);
        /// if false it is set to 255.</param>
        /// <returns></returns>
        /// <remarks>
        /// Some images can have mask color, this method doesn't use this info. You need to add
        /// additional code in order to determine transparency if your image is with mask color.
        /// </remarks>
        public abstract Color GenericImageGetPixel(GenericImage img, int x, int y, bool withAlpha = false);

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
        /// <param name="withAlpha">If true alpha channel
        /// is also set from <paramref name="color"/>.</param>
        public abstract void GenericImageSetPixel(
            GenericImage img,
            int x,
            int y,
            Color color,
            bool withAlpha = false);

        /// <summary>
        /// Returns the red intensity at the given coordinate.
        /// </summary>
        /// <param name="x">X coordinate of the pixel.</param>
        /// <param name="y">Y coordinate of the pixel.</param>
        /// <returns></returns>
        public abstract byte GenericImageGetRed(GenericImage img, int x, int y);

        /// <summary>
        /// Returns the green intensity at the given coordinate.
        /// </summary>
        /// <param name="x">X coordinate of the pixel.</param>
        /// <param name="y">Y coordinate of the pixel.</param>
        /// <returns></returns>
        public abstract byte GenericImageGetGreen(GenericImage img, int x, int y);

        /// <summary>
        /// Returns the blue intensity at the given coordinate.
        /// </summary>
        /// <param name="x">X coordinate of the pixel.</param>
        /// <param name="y">Y coordinate of the pixel.</param>
        /// <returns></returns>
        public abstract byte GenericImageGetBlue(GenericImage img, int x, int y);

        /// <summary>
        /// Gets the <see cref="RGBValue"/> value of the mask color.
        /// </summary>
        /// <param name="genericImage">Native generic image instance.</param>
        public abstract RGBValue GenericImageGetMaskRGB(GenericImage img);

        /// <summary>
        /// Gets the red value of the mask color.
        /// </summary>
        /// <returns></returns>
        /// <param name="genericImage">Native generic image instance.</param>
        public abstract byte GenericImageGetMaskRed(GenericImage img);

        /// <summary>
        /// Gets the green value of the mask color.
        /// </summary>
        /// <returns></returns>
        /// <param name="genericImage">Native generic image instance.</param>
        public abstract byte GenericImageGetMaskGreen(GenericImage img);

        /// <summary>
        /// Gets the blue value of the mask color.
        /// </summary>
        /// <returns></returns>
        /// <param name="genericImage">Native generic image instance.</param>
        public abstract byte GenericImageGetMaskBlue(GenericImage img);

        /// <summary>
        /// Gets a user-defined string-valued option.
        /// </summary>
        /// <remarks>
        /// See <see cref="GenericImageOptionNames"/> for known option names.
        /// </remarks>
        /// <param name="name">The name of the option, case-insensitive.</param>
        /// <returns></returns>
        public abstract string GenericImageGetOptionAsString(GenericImage img, string name);

        /// <summary>
        /// Gets a user-defined integer-valued option.
        /// </summary>
        /// <remarks>
        /// See <see cref="GenericImageOptionNames"/> for known option names.
        /// </remarks>
        /// <param name="name">The name of the option, case-insensitive.</param>
        /// <returns></returns>
        public abstract int GenericImageGetOptionAsInt(GenericImage img, string name);

        /// <summary>
        /// Returns a sub image of the current one as long as the rect belongs entirely to the image.
        /// </summary>
        /// <param name="rect">Bounds of the sub-image.</param>
        /// <returns></returns>
        public abstract GenericImage GenericImageGetSubImage(GenericImage img, RectI rect);

        /// <summary>
        /// Gets the type of image found when image was loaded or specified when image was saved.
        /// </summary>
        /// <returns></returns>
        public abstract BitmapType GenericImageGetImageType(GenericImage img);

        /// <summary>
        /// Returns <c>true</c> if this image has alpha channel, <c>false</c> otherwise.
        /// </summary>
        /// <returns></returns>
        /// <param name="genericImage">Native generic image instance.</param>
        public abstract bool GenericImageHasAlpha(GenericImage img);

        /// <summary>
        /// Returns <c>true</c> if there is a mask active, <c>false</c> otherwise.
        /// </summary>
        /// <returns></returns>
        /// <param name="genericImage">Native generic image instance.</param>
        public abstract bool GenericImageHasMask(GenericImage img);

        /// <summary>
        /// Returns <c>true</c> if the given option is present.
        /// </summary>
        /// <remarks>
        /// See <see cref="GenericImageOptionNames"/> for known option names.
        /// </remarks>
        /// <param name="name">The name of the option, case-insensitive.</param>
        /// <returns></returns>
        public abstract bool GenericImageHasOption(GenericImage img, string name);

        /// <summary>
        /// Returns <c>true</c> if the given pixel is transparent, i.e. either has the mask
        /// color if this image has a mask or if this image has alpha channel and alpha value of
        /// this pixel is strictly less than threshold.
        /// </summary>
        /// <param name="x">X coordinate of the pixel.</param>
        /// <param name="y">Y coordinate of the pixel.</param>
        /// <param name="threshold">Alpha value treshold.</param>
        /// <returns></returns>
        public abstract bool GenericImageIsTransparent(GenericImage img, int x, int y, byte threshold);

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
        public abstract bool GenericImageLoadFromStream(
            GenericImage img,
            Stream stream,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1);

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
        public abstract bool GenericImageLoadFromFile(
            GenericImage img,
            string filename,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1);

        /// <summary>
        /// Loads an image from a file.
        /// </summary>
        /// <param name="name">Path to file.</param>
        /// <param name="mimetype">MIME type string (for example 'image/jpeg').</param>
        /// <param name="index">See description in
        /// <see cref="GenericImage(string, BitmapType, int)"/></param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        public abstract bool GenericImageLoadFromFile(
            GenericImage img,
            string name,
            string mimetype,
            int index = -1);

        /// <summary>
        /// Loads an image from an input stream.
        /// </summary>
        /// <param name="stream">Input stream with image data.</param>
        /// <param name="mimetype">MIME type string (for example 'image/jpeg').</param>
        /// <param name="index">See description in
        /// <see cref="GenericImage(string, BitmapType, int)"/></param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        public abstract bool GenericImageLoadFromStream(
            GenericImage img,
            Stream stream,
            string mimetype,
            int index = -1);

        /// <summary>
        /// Saves an image in the given stream.
        /// </summary>
        /// <param name="stream">Output stream.</param>
        /// <param name="mimetype">MIME type string (for example 'image/jpeg').</param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        public abstract bool GenericImageSaveToStream(GenericImage img, Stream stream, string mimetype);

        /// <summary>
        /// Saves an image in the named file.
        /// </summary>
        /// <param name="filename">Path to file.</param>
        /// <param name="bitmapType">Type of the bitmap. Depending on how library and OS
        /// has been configured and
        /// by which handlers have been loaded, not all formats may be available.</param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        public abstract bool GenericImageSaveToFile(GenericImage img, string filename, BitmapType bitmapType);

        /// <summary>
        /// Saves an image in the named file.
        /// </summary>
        /// <param name="filename">Name of the file to save the image to.</param>
        /// <param name="mimetype">MIME type string (for example 'image/jpeg').</param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        public abstract bool GenericImageSaveToFile(GenericImage img, string filename, string mimetype);

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
        public abstract bool GenericImageSaveToFile(GenericImage img, string filename);

        /// <summary>
        /// Saves an image in the given stream.
        /// </summary>
        /// <param name="stream">Output stream</param>
        /// <param name="type"></param>
        /// <returns><c>true</c> if the call succeeded, <c>false</c> otherwise.</returns>
        public abstract bool GenericImageSaveToStream(GenericImage img, Stream stream, BitmapType type);

        /// <summary>
        /// Sets the image data without performing checks.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="new_width"></param>
        /// <param name="new_height"></param>
        /// <param name="static_data"></param>
        public abstract void GenericImageSetNativeData(
            GenericImage img,
            IntPtr data,
            int new_width,
            int new_height,
            bool static_data = false);

        /// <summary>
        /// Returns pointer to the array storing the alpha values for this image.
        /// </summary>
        /// <returns>This pointer is NULL for the images without the alpha channel.
        /// If the image does have it, this pointer may be used to directly manipulate the
        /// alpha values which are stored as the RGB ones.</returns>
        public abstract IntPtr GenericImageGetNativeAlphaData(GenericImage img);

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
        /// <param name="genericImage">Native generic image instance.</param>
        public abstract IntPtr GenericImageGetNativeData(GenericImage img);

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
        public abstract bool GenericImageCreateNativeData(
            GenericImage img,
            int width,
            int height,
            IntPtr data,
            bool staticData = false);

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
        public abstract bool GenericImageCreateNativeData(
            GenericImage img,
            int width,
            int height,
            IntPtr data,
            IntPtr alpha,
            bool staticData = false);

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
        public abstract void GenericImageSetNativeAlphaData(
            GenericImage img,
            IntPtr alpha = default,
            bool staticData = false);

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
        public abstract void GenericImageSetNativeData(
            GenericImage img,
            IntPtr data,
            bool staticData = false);
    }
}
