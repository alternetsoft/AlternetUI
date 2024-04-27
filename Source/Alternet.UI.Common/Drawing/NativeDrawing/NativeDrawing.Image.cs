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
        /// Creates native image.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateImage();

        /// <summary>
        /// Saves native image to file.
        /// </summary>
        /// <param name="fileName">A string that contains the name of the file
        /// to which to save the image.</param>
        /// <param name="image">Native image instance.</param>
        /// <returns></returns>
        public abstract bool ImageSave(object image, string fileName);

        /// <summary>
        /// Saves native image to stream.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> where the image will be
        /// saved.</param>
        /// <param name="format">Image format.</param>
        /// <param name="image">Native image instance.</param>
        /// <returns></returns>
        public abstract bool ImageSave(object image, Stream stream, ImageFormat format);

        /// <summary>
        /// Gets native image size in pixels.
        /// </summary>
        /// <param name="image">Native image instance.</param>
        /// <returns></returns>
        public abstract SizeI GetImagePixelSize(object image);

        /// <summary>
        /// Loads native image from the stream.
        /// </summary>
        /// <param name="image">Native image instance.</param>
        /// <param name="stream">Stream with image data.</param>
        public abstract bool ImageLoadFromStream(object image, Stream stream);

        /// <summary>
        /// Initializes a new instance of the native image from the specified
        /// existing native image, scaled to the specified size.
        /// </summary>
        /// <param name="original">The native image from which to create the new image.</param>
        /// <param name="newSize">The <see cref="SizeI" /> structure that represent the
        /// size of the new image.</param>
        public abstract object CreateImageFromImage(object original, SizeI newSize);

        /// <summary>
        /// Initializes a new instance of the native image from
        /// the specified generic native image.
        /// </summary>
        /// <param name="genericImage">Generic image.</param>
        /// <param name="depth">Specifies the depth of the bitmap.
        /// Some platforms only support (1) for monochrome and (-1) for the current color setting.
        /// A depth of 32 including an alpha channel is supported under MSW, Mac and Linux.
        /// If this parameter is omitted
        /// (= -1), the display depth of the screen is used.</param>
        public abstract object CreateImageFromGenericImage(object genericImage, int depth = -1);

        /// <summary>
        /// Creates a bitmap compatible with the given native graphics, inheriting
        /// its magnification factor.
        /// </summary>
        /// <param name="width">The width of the bitmap in pixels, must be strictly positive.</param>
        /// <param name="height">The height of the bitmap in pixels, must be strictly positive.</param>
        /// <param name="dc">Native graphics from which the scaling factor is inherited.</param>
        public abstract object CreateImageFromGraphics(int width, int height, object dc);

        /// <summary>
        /// Creates a bitmap compatible with the given native grahics and
        /// native generic image.
        /// </summary>
        /// <param name="genericImage">Platform-independent image object.</param>
        /// <param name="dc">Native graphics from which the scaling
        /// factor is inherited.</param>
        /// <remarks>
        /// This constructor initializes the bitmap with the data of the given image, which
        /// must be valid, but inherits the scaling factor from the given device context
        /// instead of simply using the default factor of 1.
        /// </remarks>
        public abstract object CreateImageFromGraphicsAndGenericImage(object genericImage, object dc);

        /// <summary>
        /// Initializes a new instance of the native image
        /// with the specified size in device pixels.
        /// </summary>
        /// <param name="size">The size in device pixels used to create the image.</param>
        /// <param name="depth">Specifies the depth of the bitmap.
        /// Some platforms only support (1) for monochrome and (-1) for the current color setting.
        /// A depth of 32 including an alpha channel is supported under MSW, Mac and Linux.
        /// If this parameter is omitted
        /// (= -1), the display depth of the screen is used.</param>
        public abstract object CreateImageWithSizeAndDepth(SizeI size, int depth = 32);

        /// <summary>
        /// Gets default <see cref="BitmapType"/> value for the current operating system.
        /// </summary>
        /// <returns></returns>
        public abstract BitmapType GetDefaultBitmapType();

        /// <summary>
        /// Gets whether native image has an alpha channel.
        /// </summary>
        public abstract bool GetImageHasAlpha(object image);

        /// <summary>
        /// Gets whether native image is ok.
        /// </summary>
        public abstract bool GetImageIsOk(object image);

        /// <summary>
        /// Sets whether this image has an alpha channel.
        /// </summary>
        public abstract void SetImageHasAlpha(object image, bool hasAlpha);

        /// <summary>
        /// Gets the scale factor of the native image.
        /// </summary>
        public abstract double GetImageScaleFactor(object image);

        /// <summary>
        /// Sets the scale factor of the native image.
        /// </summary>
        public abstract void SetImageScaleFactor(object image, double value);

        /// <summary>
        /// Gets the size of bitmap in DPI-independent units.
        /// </summary>
        /// <remarks>
        /// This assumes that the bitmap was created using the value of scale factor corresponding
        /// to the current DPI and returns its physical size divided by this scale factor.
        /// Unlike LogicalSize, this function returns the same value under all platforms
        /// and so its result should not be used as window or device context coordinates.
        /// </remarks>
        public abstract SizeI GetImageDipSize(object image);

        /// <summary>
        /// Gets the height of the native image in logical pixels.
        /// </summary>
        public abstract double GetImageScaledHeight(object image);

        /// <summary>
        /// Gets the size of the native image in logical pixels.
        /// </summary>
        public abstract SizeI GetImageScaledSize(object image);

        /// <summary>
        /// Gets the width of the native image in logical pixels.
        /// </summary>
        public abstract double GetImageScaledWidth(object image);

        /// <summary>
        /// Gets the color depth of the image. Returned value is 32, 24, or other.
        /// </summary>
        public abstract int GetImageDepth(object image);

        /// <summary>
        /// Creates images with screen pixels.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateImageFromScreen();

        /// <summary>
        /// Initializes a new instance of the native image
        /// from the specified <see cref="Stream"/> which contains svg data.
        /// </summary>
        /// <param name="stream">Stream with svg data.</param>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <returns>Image instance with dimensions specified in <paramref name="width"/>
        /// and <paramref name="height"/> and data loaded from <paramref name="stream"/>. </returns>
        /// <param name="color">Svg fill color. Optional.
        /// If provided, svg fill color is changed to the specified value.</param>
        public abstract object CreateImageFromSvgStream(
            Stream stream,
            int width,
            int height,
            Color? color = null);

        /// <summary>
        /// Initializes a new instance of the native image
        /// from the specified string which contains svg data.
        /// </summary>
        /// <param name="s">String with svg data.</param>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <returns>Image instance with dimensions specified in <paramref name="width"/>
        /// and <paramref name="height"/> and data loaded from <paramref name="s"/>. </returns>
        /// <param name="color">Svg fill color. Optional.
        /// If provided, svg fill color is changed to the specified value.</param>
        public abstract object CreateImageFromSvgString(
            string s,
            int width,
            int height,
            Color? color = null);

        /// <summary>
        /// Loads native image from a file or resource.
        /// </summary>
        /// <param name="name">Either a filename or a resource name. The meaning of name
        /// is determined by the type parameter.</param>
        /// <param name="type">One of the <see cref="BitmapType"/> values</param>
        /// <returns><c>true</c> if the operation succeeded, <c>false</c> otherwise.</returns>
        /// <remarks>
        /// Note: Not all values of <see cref="BitmapType"/> enumeration
        /// may be supported by the library and operating system for the load operation.
        /// </remarks>
        /// <remarks>
        /// You can specify <see cref="BitmapType.Any"/> to guess image type using file extension.
        /// </remarks>
        public abstract bool ImageLoad(object image, string name, BitmapType type);

        /// <summary>
        /// Saves native image to the specified file.
        /// </summary>
        /// <param name="name">A string that contains the name of the file
        /// to which to save the image.</param>
        /// <param name="type">An <see cref="BitmapType"/> that specifies
        /// the format of the saved image.</param>
        /// <remarks>
        /// Note: Not all values of <see cref="BitmapType"/> enumeration
        /// may be supported by the library and operating system for the save operation.
        /// </remarks>
        /// <returns><c>true</c> if the operation succeeded, <c>false</c> otherwise.</returns>
        public abstract bool ImageSaveToFile(object image, string name, BitmapType type);

        /// <summary>
        /// Saves this image to the specified stream in the specified format defined
        /// in <paramref name="type"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> where the image will be
        /// saved.</param>
        /// <param name="type">An <see cref="BitmapType"/> that specifies
        /// the format of the saved image.</param>
        /// <remarks>
        /// Note: Not all values of <see cref="BitmapType"/> enumeration
        /// may be supported by the library and operating system for the save operation.
        /// </remarks>
        /// <returns><c>true</c> if the operation succeeded, <c>false</c> otherwise.</returns>
        /// <remarks>Use <see cref="GetExtensionsForSave"/> to get supported formats for
        /// the save operation.</remarks>
        public abstract bool ImageSaveToStream(object image, Stream stream, BitmapType type);

        /// <summary>
        /// Loads an image from an input stream.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> from where the image will be
        /// loaded.</param>
        /// <param name="type">One of the <see cref="BitmapType"/> values</param>
        /// <remarks>
        /// Note: Not all values of <see cref="BitmapType"/> enumeration
        /// may be supported by the library and operating system for the load operation.
        /// </remarks>
        /// <returns><c>true</c> if the operation succeeded, <c>false</c> otherwise.</returns>
        /// <remarks>Use <see cref="GetExtensionsForLoad"/> to get supported formats
        /// for the load operation.</remarks>
        public abstract bool ImageLoadFromStream(object image, Stream stream, BitmapType type);

        /// <summary>
        /// Returns a sub image of the current one as long as the <paramref name="rect"/> belongs
        /// entirely to the image.
        /// </summary>
        /// <param name="rect">Rectangle in this image.</param>
        /// <returns></returns>
        public abstract object ImageGetSubBitmap(object image, RectI rect);

        /// <summary>
        /// Returns disabled (dimmed) version of the native image.
        /// </summary>
        /// <param name="brightness">Brightness. Default is 255.</param>
        /// <returns></returns>
        public abstract object ImageConvertToDisabled(object image, byte brightness = 255);

        /// <summary>
        /// Rescales native image to the requested size.
        /// </summary>
        /// <param name="sizeNeeded">New image size.</param>
        public abstract void ImageRescale(object image, SizeI sizeNeeded);

        /// <summary>
        /// Resets alpha channel.
        /// </summary>
        public abstract void ImageResetAlpha(object image);
    }
}