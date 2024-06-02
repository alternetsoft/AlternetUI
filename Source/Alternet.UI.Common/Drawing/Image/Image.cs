using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Describes an image to be drawn on a <see cref="Graphics"/> or
    /// displayed in a UI control.
    /// </summary>
    [TypeConverter(typeof(ImageConverter))]
    public class Image : HandledObject<IImageHandler>
    {
        private static readonly string[] DefaultExtensionsForLoad =
        {
            ".bmp",
            ".png",
            ".jpeg",
            ".jpg",
            ".pcx",
            ".pnm",
            ".tiff",
            ".tga",
            ".xpm",
            ".gif",
            ".ico",
            ".cur",
            ".iff",
            ".ani",
        };

        private static readonly string[] DefaultExtensionsForSave =
        {
            ".bmp",
            ".png",
            ".jpeg",
            ".jpg",
            ".pcx",
            ".pnm",
            ".tiff",
            ".tga",
            ".xpm",
        };

        /// <summary>
        /// Gets or sets default quality used when images are saved and quality parameter is omitted.
        /// </summary>
        public static int DefaultSaveQuality = 70;

        /// <summary>
        /// Occurs when <see cref="ToGrayScale"/> is called. Used to override default
        /// grayscale method.
        /// </summary>
        public static EventHandler<BaseEventArgs<Image>>? GrayScale;

        private static IEnumerable<string>? extensionsForLoad;
        private static IEnumerable<string>? extensionsForSave;

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="nativeImage">Native image instance.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Image(IImageHandler handler)
        {
            Handler = handler;
        }

        /// <summary>
        /// Gets default <see cref="BitmapType"/> value for the current operating system.
        /// </summary>
        /// <returns></returns>
        public static BitmapType DefaultBitmapType => GraphicsFactory.Handler.GetDefaultBitmapType();

        /// <summary>
        /// Gets or sets list of extensions (including ".") which can be used to filter out
        /// supported image formats when using
        /// <see cref="OpenFileDialog"/>.
        /// </summary>
        public static IEnumerable<string> ExtensionsForLoad
        {
            get
            {
                return extensionsForLoad ?? DefaultExtensionsForLoad;
            }

            set
            {
                extensionsForLoad = value;
            }
        }

        /// <summary>
        /// Gets or sets list of extensions (including ".") which can be used to filter out
        /// supported image formats when using
        /// <see cref="SaveFileDialog"/>.
        /// </summary>
        public static IEnumerable<string> ExtensionsForSave
        {
            get
            {
                return extensionsForSave ?? DefaultExtensionsForSave;
            }

            set
            {
                extensionsForSave = value;
            }
        }

        /// <summary>
        /// Converts this object to <see cref="GenericImage"/>.
        /// </summary>
        [Browsable(false)]
        public virtual GenericImage AsGeneric
        {
            get
            {
                return Handler.ToGenericImage();
            }
        }

        /// <summary>
        /// Gets the size of the image in pixels.
        /// </summary>
        public virtual SizeI PixelSize => Handler.PixelSize;

        /// <summary>
        /// Gets whether image is ok (is not disposed and has non-zero width and height).
        /// </summary>
        public virtual bool IsOk => !IsDisposed && Handler.IsOk;

        /// <summary>
        /// Creates texture brush with this image.
        /// </summary>
        [Browsable(false)]
        public virtual TextureBrush AsBrush => new(this);

        /// <summary>
        /// Gets whether image is empty (is disposed or has an empty width or height).
        /// </summary>
        public virtual bool IsEmpty => !IsOk || Size.AnyIsEmpty;

        /// <summary>
        /// Gets or sets whether this image has an alpha channel.
        /// </summary>
        public virtual bool HasAlpha
        {
            get
            {
                return Handler.HasAlpha;
            }

            set
            {
                Handler.HasAlpha = value;
            }
        }

        /// <summary>
        /// Gets image width in pixels.
        /// </summary>
        public virtual int Width => Handler.PixelSize.Width;

        /// <summary>
        /// Gets image height in pixels.
        /// </summary>
        public virtual int Height => Handler.PixelSize.Height;

        /// <summary>
        /// Gets image bounds in pixels. This method returns (0, 0, Width, Height).
        /// </summary>
        public virtual RectI Bounds
        {
            get
            {
                var size = Size;
                return new(0, 0, size.Width, size.Height);
            }
        }

        /// <summary>
        /// Gets image width in pixels.
        /// </summary>
        public int PixelWidth => Width;

        /// <summary>
        /// Gets image height in pixels.
        /// </summary>
        public int PixelHeight => Height;

        /// <summary>
        /// Gets image size in pixels.
        /// </summary>
        public SizeI Size => PixelSize;

        /// <summary>
        /// Gets or sets the scale factor of this image.
        /// </summary>
        /// <remarks>
        /// Scale factor is 1 by default, but can be greater to indicate that the size of
        /// bitmap in logical, DPI-independent pixels is smaller than its actual size in
        /// physical pixels. Bitmaps with scale factor greater than 1 must be used in high DPI
        /// to appear sharp on the screen.
        /// Note that the scale factor is only used in the ports where logical pixels are not the same
        /// as physical ones, such as MacOs or Linux, and this function always returns 1 under
        /// the other platforms.
        /// Setting scale to 2 means that the bitmap will be twice smaller (in each direction) when
        /// drawn on screen in the ports in which logical and physical pixels differ
        /// (i.e. MacOs and Linux, but not Windows). This doesn't change the bitmap actual size
        /// or its contents, but changes its scale factor, so that it appears in a smaller
        /// size when it is drawn on screen.
        /// </remarks>
        public virtual Coord ScaleFactor
        {
            get
            {
                return Handler.ScaleFactor;
            }

            set
            {
                Handler.ScaleFactor = value;
            }
        }

        /// <summary>
        /// Gets the size of bitmap in DPI-independent units.
        /// </summary>
        /// <remarks>
        /// This assumes that the bitmap was created using the value of scale factor corresponding
        /// to the current DPI and returns its physical size divided by this scale factor.
        /// Unlike LogicalSize, this function returns the same value under all platforms
        /// and so its result should not be used as window or device context coordinates.
        /// </remarks>
        public virtual SizeI DipSize
        {
            get
            {
                return Handler.DipSize;
            }
        }

        /// <summary>
        /// Gets the height of the bitmap in logical pixels.
        /// </summary>
        public virtual Coord ScaledHeight
        {
            get
            {
                return Handler.ScaledHeight;
            }
        }

        /// <summary>
        /// Gets the size of the bitmap in logical pixels.
        /// </summary>
        public virtual SizeI ScaledSize
        {
            get
            {
                return Handler.ScaledSize;
            }
        }

        /// <summary>
        /// Gets the width of the bitmap in logical pixels.
        /// </summary>
        public virtual Coord ScaledWidth
        {
            get
            {
                return Handler.ScaledWidth;
            }
        }

        /// <summary>
        /// Gets the color depth of the image. Returned value is 32, 24, or other.
        /// </summary>
        public virtual int Depth
        {
            get
            {
                return Handler.Depth;
            }
        }

        /// <summary>
        /// Gets <see cref="Graphics"/> which allows to draw on the image.
        /// Same as <see cref="GetDrawingContext"/>.
        /// </summary>
        [Browsable(false)]
        public virtual Graphics Canvas => GetDrawingContext();

        /// <summary>
        /// Converts the specified <see cref='SKBitmap'/> to a <see cref='Image'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Image(SKBitmap bitmap)
        {
            return FromSkia(bitmap);
        }

        /// <summary>
        /// Converts the specified <see cref='SKBitmap'/> to a <see cref='Image'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator SKBitmap(Image bitmap)
        {
            return ToSkia(bitmap);
        }

        /// <summary>
        /// Converts the specified <see cref='GenericImage'/> to a <see cref='Image'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Image(GenericImage image) => new Bitmap(image);

        /// <summary>
        /// Converts the specified <see cref='GenericImage'/> to a <see cref='Image'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator GenericImage(Image image) => image.AsGeneric;

        /// <summary>
        /// Indicates whether the specified image is <c>null</c> or has an empty width (or height).
        /// </summary>
        /// <param name="image">The image to test.</param>
        /// <returns><c>true</c> if the <paramref name="image"/> parameter is <c>null</c> or
        /// has an empty width (or height); otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty([NotNullWhen(false)] Image? image)
        {
            return (image is null) || image.IsEmpty;
        }

        /// <summary>
        /// Indicates whether the specified image is not <c>null</c> and has non-empty width and height.
        /// </summary>
        /// <param name="image">The image to test.</param>
        /// <returns><c>true</c> if the <paramref name="image"/> parameter is not <c>null</c> and
        /// has non-empty width and height; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotNullAndOk([NotNullWhen(true)] Image? image)
        {
            return (image is not null) && image.IsOk;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class
        /// from the specified url.
        /// </summary>
        /// <param name="url">The file or embedded resource url used
        /// to load the image.
        /// </param>
        /// <example>
        /// <code>
        /// var ImageSize = 16;
        /// var ResPrefix = $"embres:ControlsTest.Resources.Png._{ImageSize}.";
        /// var url = $"{ResPrefix}arrow-left-{ImageSize}.png";
        /// button1.Image = Bitmap.FromUrl(url);
        /// </code>
        /// </example>
        public static Image FromUrl(string url)
        {
            using var stream = ResourceLoader.StreamFromUrl(url);
            return new Bitmap(stream);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class
        /// from the specified url which points to svg file or resource.
        /// </summary>
        /// <param name="url">The file or embedded resource url with Svg data used
        /// to load the image.
        /// </param>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <example>
        /// <code>
        /// var imageSize = 16;
        /// var resPrefix = "embres:ControlsTest.Resources.Svg.";
        /// var url = $"{resPrefix}plus.svg";
        /// button1.Image = Bitmap.FromSvgUrl(url, imageSize, imageSize);
        /// </code>
        /// </example>
        /// <remarks>
        /// <paramref name="url"/> can include assembly name. Example:
        /// "embres:Alternet.UI.Resources.Svg.ImageName.svg?assembly=Alternet.UI"
        /// </remarks>
        /// <returns>Image instance with dimensions specified in <paramref name="width"/>
        /// and <paramref name="height"/> and data loaded from the specified
        /// <paramref name="url"/>. </returns>
        /// <param name="color">Svg fill color. Optional.
        /// If provided, svg fill color is changed to the specified value.</param>
        public static Image FromSvgUrl(string url, int width, int height, Color? color = null)
        {
            using var stream = ResourceLoader.StreamFromUrl(url);
            var result = FromSvgStream(stream, width, height, color);
            return result;
        }

        /// <summary>
        /// Creates images with screen pixels.
        /// </summary>
        /// <returns></returns>
        public static Image? FromScreen()
        {
            var handler = GraphicsFactory.Handler.CreateImageHandlerFromScreen();
            return new Image(handler);
        }

        /// <summary>
        /// Creates an <see cref="Image" /> from the specified data stream.
        /// </summary>
        /// <param name="stream">
        /// A <see cref="Stream" /> that contains the data for this <see cref="Image" />.</param>
        /// <returns>The <see cref="Image" /> this method creates.</returns>
        public static Image FromStream(Stream stream)
        {
            return new Bitmap(stream);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class
        /// from the specified <see cref="Stream"/> which contains svg data.
        /// </summary>
        /// <param name="stream">Stream with svg data.</param>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <returns>Image instance with dimensions specified in <paramref name="width"/>
        /// and <paramref name="height"/> and data loaded from <paramref name="stream"/>. </returns>
        /// <param name="color">Svg fill color. Optional.
        /// If provided, svg fill color is changed to the specified value.</param>
        public static Image FromSvgStream(Stream stream, int width, int height, Color? color = null)
        {
            var nativeImage = GraphicsFactory.Handler.CreateImageHandlerFromSvg(
                stream,
                width,
                height,
                color);
            var result = new Image(nativeImage);
            return result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class
        /// from the specified string which contains svg data.
        /// </summary>
        /// <param name="s">String with svg data.</param>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <returns>Image instance with dimensions specified in <paramref name="width"/>
        /// and <paramref name="height"/> and data loaded from <paramref name="s"/>. </returns>
        /// <param name="color">Svg fill color. Optional.
        /// If provided, svg fill color is changed to the specified value.</param>
        public static Image FromSvgString(string s, int width, int height, Color? color = null)
        {
            var nativeImage = GraphicsFactory.Handler.CreateImageHandlerFromSvg(s, width, height, color);
            var result = new Image(nativeImage);
            return result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class
        /// from the specified url which points to svg file or resource.
        /// </summary>
        /// <remarks>
        /// This is similar to <see cref="Image.FromSvgUrl"/> but uses
        /// <see cref="Control.GetDPI"/> and <see cref="ToolBarUtils.GetDefaultImageSize(Coord)"/>
        /// to get appropriate image size which is best suitable for toolbars.
        /// </remarks>
        /// <param name="url">The file or embedded resource url with Svg data used
        /// to load the image.</param>
        /// <param name="control">Control which <see cref="Control.GetDPI"/> method
        /// is used to get DPI.</param>
        /// <returns><see cref="Image"/> instance loaded from Svg data for use
        /// on the toolbars.</returns>
        /// <param name="color">Svg fill color. Optional.
        /// If provided, svg fill color is changed to the specified value.</param>
        public static Image FromSvgUrlForToolbar(string url, IControl control, Color? color = null)
        {
            SizeD deviceDpi = control.GetDPI();
            var width = ToolBarUtils.GetDefaultImageSize(deviceDpi.Width);
            var height = ToolBarUtils.GetDefaultImageSize(deviceDpi.Height);
            var result = Image.FromSvgUrl(url, width, height, color);
            return result;
        }

        /// <summary>
        /// Creates <see cref="Image"/> from <see cref="SKBitmap"/>.
        /// </summary>
        /// <param name="bitmap"><see cref="SKBitmap"/> with image data.</param>
        /// <returns></returns>
        public static Image FromSkia(SKBitmap bitmap)
        {
            var genericImage = GenericImage.FromSkia(bitmap);
            return (Image)genericImage;
        }

        /// <summary>
        /// Creates <see cref="SKBitmap"/> from <see cref="Image"/>.
        /// </summary>
        /// <param name="bitmap"><see cref="SKBitmap"/> with image data.</param>
        /// <returns></returns>
        public static SKBitmap ToSkia(Image bitmap)
        {
            if (bitmap.Handler is SkiaImageHandler skiaHandler)
                return skiaHandler.Bitmap;

            var genericImage = (GenericImage)bitmap;
            return (SKBitmap)genericImage;
        }

        /// <summary>
        /// Gets <see cref="BitmapType"/> from the extension of the <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName">Path to file.</param>
        /// <returns></returns>
        public static BitmapType GetBitmapTypeFromFileName(string fileName)
        {
            var ext = PathUtils.GetExtensionLower(fileName);
            if (string.IsNullOrEmpty(ext))
                return BitmapType.Invalid;
            switch (ext)
            {
                case "bmp":
                    return BitmapType.Bmp;
                case "ico":
                    return BitmapType.Ico;
                case "cur":
                    return BitmapType.Cur;
                case "xbm":
                    return BitmapType.Xbm;
                case "xpm":
                    return BitmapType.Xpm;
                case "tiff":
                    return BitmapType.Tiff;
                case "gif":
                    return BitmapType.Gif;
                case "png":
                    return BitmapType.Png;
                case "jpeg":
                case "jpg":
                    return BitmapType.Jpeg;
                case "pnm":
                    return BitmapType.Pnm;
                case "pcx":
                    return BitmapType.Pcx;
                case "pict":
                    return BitmapType.Pict;
                case "ani":
                    return BitmapType.Ani;
                case "iff":
                    return BitmapType.Iff;
                case "tga":
                    return BitmapType.Tga;
                default:
                    return BitmapType.Invalid;
            }
        }

        /// <summary>
        /// Loads an image from a file or resource.
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
        /// <remarks>Use <see cref="GetExtensionsForLoad"/> to get supported formats
        /// for the load operation.</remarks>
        public virtual bool Load(string name, BitmapType type)
        {
            return Handler.Load(name, type);
        }

        /// <summary>
        /// Saves this <see cref="Image"/> to the specified file.
        /// </summary>
        /// <param name="name">A string that contains the name of the file
        /// to which to save this <see cref="Image"/>.</param>
        /// <param name="type">An <see cref="BitmapType"/> that specifies
        /// the format of the saved image.</param>
        /// <remarks>
        /// Note: Not all values of <see cref="BitmapType"/> enumeration
        /// may be supported by the library and operating system for the save operation.
        /// </remarks>
        /// <returns><c>true</c> if the operation succeeded, <c>false</c> otherwise.</returns>
        /// <remarks>Use <see cref="GetExtensionsForSave"/> to get supported formats for
        /// the save operation.</remarks>
        public virtual bool Save(string name, BitmapType type, int? quality = null)
        {
            quality ??= DefaultSaveQuality;
            return Handler.SaveToFile(name, type, quality.Value);
        }

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
        public virtual bool Save(Stream stream, BitmapType type, int? quality = null)
        {
            quality ??= DefaultSaveQuality;
            return Handler.SaveToStream(stream, type, quality.Value);
        }

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
        public virtual bool Load(Stream stream, BitmapType type)
        {
            return Handler.LoadFromStream(stream, type);
        }

        /// <summary>
        /// Returns a sub image of the current one as long as the <paramref name="rect"/> belongs
        /// entirely to the image.
        /// </summary>
        /// <param name="rect">Rectangle in this image.</param>
        /// <returns></returns>
        public virtual Image GetSubBitmap(RectI rect)
        {
            var converted = Handler.GetSubBitmap(rect);
            return new Image(converted);
        }

        /// <summary>
        /// Sets <see cref="ScaleFactor"/> using DPI value.
        /// </summary>
        /// <param name="dpi"></param>
        public virtual void SetDPI(SizeD dpi)
        {
            var factor = dpi.Width / 96;
            this.ScaleFactor = factor;
        }

        /// <summary>
        /// Returns disabled (dimmed) version of the image.
        /// </summary>
        /// <param name="brightness">Brightness. Default is 255.</param>
        /// <returns></returns>
        public virtual Image ConvertToDisabled(byte brightness = 255)
        {
            var converted = Handler.ConvertToDisabled(brightness);
            return new Image(converted);
        }

        /// <summary>
        /// Rescales this image to the requested size.
        /// </summary>
        /// <remarks>
        /// This function is just a convenient wrapper for <see cref="GenericImage.Rescale"/> used to
        /// resize the given image to the requested size. If you need more control over
        /// resizing, e.g.to specify the quality option different from
        /// <see cref="GenericImageResizeQuality.Nearest"/> used by this function, please use
        /// the <see cref="GenericImage"/> function
        /// directly instead. Size must be valid.
        /// </remarks>
        /// <param name="sizeNeeded"></param>
        public virtual void Rescale(SizeI sizeNeeded)
        {
            Handler.Rescale(sizeNeeded);
        }

        /// <summary>
        /// Resets alpha channel.
        /// </summary>
        public virtual void ResetAlpha()
        {
            Handler.ResetAlpha();
        }

        /// <summary>
        /// Creates a clone of this image with fully copied image data.
        /// </summary>
        /// <returns></returns>
        public virtual Image Clone()
        {
            return new Bitmap(this);
        }

        /// <summary>
        /// Creates grayscaled version of the image.
        /// </summary>
        /// <returns>Returns new grayscaled image from this image.</returns>
        public virtual Image ToGrayScale()
        {
            if(GrayScale is null)
            {
                var generic = (GenericImage)this;
                generic.ChangeToGrayScale();
                return (Image)generic;
            }
            else
            {
                BaseEventArgs<Image> args = new(this);
                GrayScale(this, args);
                return args.Value;
            }
        }

        /// <summary>
        /// Saves this image to the specified stream in the specified format.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> where the image will be
        /// saved.</param>
        /// <param name="format">An <see cref="ImageFormat"/> that specifies
        /// the format of the saved image.</param>
        /// <remarks>
        /// There are other save methods in the <see cref="Image"/> that support image formats not
        /// included in <see cref="ImageFormat"/>.
        /// </remarks>
        public virtual bool Save(Stream stream, ImageFormat format, int? quality = null)
        {
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (format is null)
                throw new ArgumentNullException(nameof(format));

            quality ??= DefaultSaveQuality;

            return Handler.SaveToStream(stream, format, quality.Value);
        }

        /// <summary>
        /// Saves this <see cref="Image"/> to the specified file.
        /// </summary>
        /// <param name="fileName">A string that contains the name of the file
        /// to which to save this <see cref="Image"/>.</param>
        /// <remarks>Use <see cref="GetExtensionsForSave"/> to get supported formats
        /// for the save operation.</remarks>
        public virtual bool Save(string fileName, int? quality = null)
        {
            if (fileName is null)
                throw new ArgumentNullException(nameof(fileName));

            quality ??= DefaultSaveQuality;

            return Handler.SaveToFile(fileName, quality.Value);
        }

        /// <summary>
        /// Gets the size of the image in device-independent units (1/96th inch
        /// per unit).
        /// </summary>
        public virtual SizeD SizeDip(IControl control)
            => control.PixelToDip(PixelSize);

        /// <summary>
        /// Gets image rect as (0, 0, SizeDip().Width, SizeDip().Height).
        /// </summary>
        public virtual RectD BoundsDip(IControl control)
        {
            var size = SizeDip(control);
            return (0, 0, size.Width, size.Height);
        }

        /// <summary>
        /// Gets <see cref="Graphics"/> for this image on which you can paint.
        /// </summary>
        /// <returns></returns>
        public virtual Graphics GetDrawingContext()
        {
            var dc = Graphics.FromImage(this);
            return dc;
        }

        /// <inheritdoc/>
        protected override IImageHandler CreateHandler()
        {
            return GraphicsFactory.Handler.CreateImageHandler();
        }
    }
}