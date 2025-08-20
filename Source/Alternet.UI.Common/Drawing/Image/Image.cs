using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
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
    public partial class Image : HandledObject<IImageHandler>, IImageSource
    {
        /// <summary>
        /// Gets or sets a value indicating whether to use SkiaSharp for loading images.
        /// Default is True. If this value is False, platform specific image loading will be used.
        /// </summary>
        public static bool UseSkiaSharpForLoading = true;

        /// <summary>
        /// Gets or sets default quality used when images are saved and quality parameter is omitted.
        /// </summary>
        public static int DefaultSaveQuality = 70;

        /// <summary>
        /// Called from <see cref="ToGrayScale"/>. Used to override default
        /// grayscale method.
        /// </summary>
        public static EventHandler<BaseEventArgs<Image>>? GrayScale;

        internal string? url;

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

        private static BaseDictionary<string, Image>? cachedBitmaps;
        private static IEnumerable<string>? extensionsForLoad;
        private static IEnumerable<string>? extensionsForSave;

        private Image? grayScaleCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="handler">Image handler.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Image(IImageHandler handler)
        {
            Handler = handler;
        }

        /// <summary>
        /// Gets default <see cref="BitmapType"/> value for the current operating system.
        /// </summary>
        /// <returns></returns>
        public static BitmapType DefaultBitmapType
        {
            get
            {
                return GraphicsFactory.Handler.GetDefaultBitmapType();
            }
        }

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
        /// Gets or sets source url of this image. This is informational property and
        /// doesn't reload the image.
        /// </summary>
        public virtual string? Url
        {
            get => url;

            set => url = value;
        }

        /// <summary>
        /// Gets whether this image has mask.
        /// </summary>
        public virtual bool HasMask => Handler.HasMask;

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
        [Browsable(false)]
        public virtual bool IsEmpty => !IsOk || Size.AnyIsEmpty;

        /// <summary>
        /// Gets <see cref="ImageBitsFormat"/> for this image.
        /// </summary>
        [Browsable(false)]
        public virtual ImageBitsFormat BitsFormat
        {
            get
            {
                var formatKind = Handler.BitsFormat;
                var format = GraphicsFactory.GetBitsFormat(formatKind);
                return format;
            }
        }

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
                if (Immutable)
                    return;
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
                if (Immutable)
                    return;
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

        Image? IImageSource.Image => this;

        ImageList? IImageSource.ImageList => null;

        int IImageSource.ImageIndex => 0;

        ImageSet? IImageSource.ImageSet => null;

        SvgImage? IImageSource.SvgImage => null;

        int? IImageSource.SvgSize => null;

        ImageSourceKind IImageSource.Kind => ImageSourceKind.Image;

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
        public static explicit operator Image(GenericImage image) => image.AsImage;

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
        /// Creates image with the specified size, filled with <paramref name="color"/>.
        /// </summary>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="color">Color to fill.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Image Create(int width, int height, Color color)
        {
            var image = GenericImage.Create(width, height, color);
            return (Image)image;
        }

        /// <summary>
        /// Creates image with the specified size and pixel data.
        /// </summary>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="pixels">Pixel data.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Image Create(int width, int height, SKColor[] pixels)
        {
            var image = new GenericImage(width, height, pixels);
            return (Image)image;
        }

        /// <summary>
        /// Indicates whether the specified image is not <c>null</c> and has non-empty
        /// width and height.
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
        /// from the specified url. Raises exceptions on errors.
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
        /// <param name="bitmapType">Type of the bitmap. Optional.</param>
        /// <remarks>
        /// Use <see cref="FromUrlOrNull"/> to load without exceptions.
        /// </remarks>
        public static Image FromUrl(string url, BitmapType bitmapType = BitmapType.Any)
        {
            using var stream = ResourceLoader.StreamFromUrl(url);
            var result = new Bitmap(stream, bitmapType);
            result.url = url;
            return result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class
        /// from the specified url. Do not raise exceptions on errors, just returns Null image.
        /// Uses image cache, so the bitmap with the same url is loaded faster for second time.
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
        /// <returns></returns>
        public static Image? FromUrlCached(string url)
        {
            cachedBitmaps ??= new();
            if (cachedBitmaps.TryGetValue(url, out var result))
                return result;
            result = FromUrlOrNull(url);
            if (result is null)
                return null;
            else
                result.SetImmutable();
            cachedBitmaps.Add(url, result);
            return result;
        }

        /// <summary>
        /// Clears images cache used in <see cref="FromUrlCached"/>.
        /// </summary>
        public static void ClearCachedImages()
        {
            cachedBitmaps?.Clear();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class
        /// from the specified relative url and assembly.
        /// </summary>
        /// <param name="asm">Assembly from which image will be loaded.</param>
        /// <param name="relativeName">Image name of relative path.
        /// Slash characters must be changed to '.'.
        /// Example: "ToolBarPng.Large.Calendar32.png".</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Image FromAssemblyUrl(Assembly asm, string relativeName)
        {
            return Image.FromUrl(AssemblyUtils.GetImageUrlInAssembly(asm, relativeName));
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
            result.url = url;
            return result;
        }

        /// <summary>
        /// Creates images with screen pixels.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Image FromStream(Stream stream)
        {
            return new Bitmap(stream);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class
        /// from the specified url. Returns null if error occurs during image load.
        /// No exceptions are raised.
        /// </summary>
        /// <param name="url">The file or embedded resource url used
        /// to load the image.
        /// </param>
        /// <example>
        /// <code>
        /// var ImageSize = 16;
        /// var ResPrefix = $"embres:ControlsTest.Resources.Png._{ImageSize}.";
        /// var url = $"{ResPrefix}arrow-left-{ImageSize}.png";
        /// button1.Image = Bitmap.FromUrlOrNull(url);
        /// </code>
        /// </example>
        /// <remarks>
        /// If DEBUG is defined, exception info is logged.
        /// </remarks>
        public static Image? FromUrlOrNull(string url)
        {
            try
            {
                var result = Image.FromUrl(url);
                return result;
            }
            catch (Exception e)
            {
                LogUtils.LogExceptionIfDebug(e);
                return null;
            }
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
            var nativeImage = GraphicsFactory.Handler
                .CreateImageHandlerFromSvg(s, width, height, color);
            var result = new Image(nativeImage);
            return result;
        }

        /// <summary>
        /// Creates an <see cref="Image"/> from a Base64-encoded string.
        /// </summary>
        /// <param name="base64str">The Base64-encoded string containing the image data.</param>
        /// <returns>An <see cref="Image"/> instance if the decoding is successful;
        /// otherwise, <c>null</c>.</returns>
        public static Image? FromBase64String(string base64str)
        {
            var stream = StreamUtils.ConvertBase64ToStream(base64str);
            try
            {
                Bitmap image = new(stream);
                return image;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class
        /// from the specified url which points to svg file or resource.
        /// </summary>
        /// <remarks>
        /// This is similar to <see cref="Image.FromSvgUrl"/> but uses
        /// <see cref="AbstractControl.GetDPI"/> and
        /// <see cref="ToolBarUtils.GetDefaultImageSize(Coord)"/>
        /// to get appropriate image size which is best suitable for toolbars.
        /// </remarks>
        /// <param name="url">The file or embedded resource url with Svg data used
        /// to load the image.</param>
        /// <param name="control">Control which <see cref="AbstractControl.GetDPI"/> method
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Image FromSkia(SKBitmap bitmap)
        {
            if (App.IsMaui)
            {
                var handler = new SkiaImageHandler(bitmap);
                var result = new Bitmap(handler);
                return result;
            }
            else
            {
                if (bitmap.Width == 0 || bitmap.Height == 0)
                    return new Bitmap();
                var genericImage = GenericImage.FromSkia(bitmap);
                return (Image)genericImage;
            }
        }

        /// <summary>
        /// Creates <see cref="SKBitmap"/> from <see cref="Image"/>.
        /// </summary>
        /// <param name="bitmap"><see cref="SKBitmap"/> with image data.</param>
        /// <param name="assignPixels">Whether to assign pixel data to the result image.
        /// Optional. Default is true</param>
        /// <returns></returns>
        public static SKBitmap ToSkia(Image bitmap, bool assignPixels = true)
        {
            if (bitmap.Handler is SkiaImageHandler skiaHandler)
                return skiaHandler.Bitmap;

            SKBitmap result;

            if (assignPixels)
            {
                var genericImage = (GenericImage)bitmap;
                result = GenericImage.ToSkia(genericImage, assignPixels);
            }
            else
            {
                result = GenericImage.CreateSkiaBitmapForImage(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HasAlpha);
            }

            if (bitmap.Immutable)
                result.SetImmutable();
            return result;
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
        /// <param name="url">Path to file or url with the image data.</param>
        /// <param name="type">One of the <see cref="BitmapType"/> values</param>
        /// <returns><c>true</c> if the operation succeeded, <c>false</c> otherwise.</returns>
        /// <remarks>
        /// Note: Not all values of <see cref="BitmapType"/> enumeration
        /// may be supported by the library and operating system for the load operation.
        /// </remarks>
        /// <remarks>
        /// You can specify <see cref="BitmapType.Any"/> to guess image type using file extension.
        /// </remarks>
        /// <remarks>Use <see cref="ExtensionsForLoad"/> to get supported formats
        /// for the load operation.</remarks>
        public virtual bool Load(string url, BitmapType type = BitmapType.Any)
        {
            if (Immutable)
                return false;

            return InsideTryCatch(() =>
            {
                using var stream = ResourceLoader.StreamFromUrl(url);
                if (stream is null)
                    return false;
                var result = InternalLoadFromStream(stream, type);
                if(result)
                    this.url = url;
                return result;
            });
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
        /// <remarks>Use <see cref="ExtensionsForSave"/> to get supported formats for
        /// the save operation.</remarks>
        /// <param name="quality">Image quality. Optional. If not specified,
        /// <see cref="DefaultSaveQuality"/> is used. Can be ignored on some platforms.</param>
        public virtual bool Save(string name, BitmapType type, int? quality = null)
        {
            quality ??= DefaultSaveQuality;

            return InsideTryCatch(() =>
            {
                using var stream = FileSystem.Default.Create(name);
                var result = Save(stream, type, quality.Value);
                if (result)
                    url = name;
                return result;
            });
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
        /// <remarks>Use <see cref="ExtensionsForSave"/> to get supported formats for
        /// the save operation.</remarks>
        /// <param name="quality">Image quality. Optional. If not specified,
        /// <see cref="DefaultSaveQuality"/> is used. Can be ignored on some platforms.</param>
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
        /// <remarks>Use <see cref="ExtensionsForLoad"/> to get supported formats
        /// for the load operation.</remarks>
        public virtual bool Load(Stream stream, BitmapType type)
        {
            if (Immutable)
                return false;

            return InternalLoadFromStream(stream, type);
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
            var result = new Image(converted);
            return result;
        }

        /// <summary>
        /// Sets <see cref="ScaleFactor"/> using DPI value.
        /// </summary>
        /// <param name="dpi"></param>
        public virtual void SetDPI(SizeD dpi)
        {
            if (Immutable)
                return;
            var factor = dpi.Width / 96;
            this.ScaleFactor = factor;
        }

        /// <summary>
        /// Creates new image using pixels of this image with changed lightness.
        /// </summary>
        /// <param name="alpha">Lightness (0..200).</param>
        /// <returns></returns>
        public virtual Image ChangeLightness(int alpha)
        {
            GenericImage image = (GenericImage)this;
            var converted = image.ConvertLightness(alpha);
            var result = (Image)converted;
            return result;
        }

        /// <summary>
        /// Returns disabled (dimmed) version of the image.
        /// </summary>
        /// <param name="brightness">Brightness. Default is 255.</param>
        /// <returns></returns>
        public virtual Image ConvertToDisabled(byte brightness = 255)
        {
            GenericImage image = (GenericImage)this;
            image.ChangeToDisabled(brightness);
            return (Image)image;
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
        public virtual bool Rescale(SizeI sizeNeeded)
        {
            if (Immutable)
                return false;
            return Handler.Rescale(sizeNeeded);
        }

        /// <summary>
        /// Resets alpha channel.
        /// </summary>
        public virtual bool ResetAlpha()
        {
            if (Immutable)
                return false;
            return Handler.ResetAlpha();
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
        /// Resets the cached grayscale version of the image by disposing of it.
        /// </summary>
        public virtual void ResetGrayScaleCache()
        {
            SafeDispose(ref grayScaleCache);
        }

        /// <summary>
        /// Returns a cached grayscale version of the image.
        /// If the cached version does not exist, it creates a new grayscale image and caches it.
        /// </summary>
        /// <returns>The cached grayscale version of the image.</returns>
        public virtual Image ToGrayScaleCached()
        {
            if (grayScaleCache is null)
            {
                grayScaleCache = ToGrayScale();
            }

            return grayScaleCache;
        }

        /// <summary>
        /// Converts the current image to a grayscale version.
        /// </summary>
        /// <remarks>
        /// If the <see cref="GrayScale"/> event is not null, it will be invoked to allow
        /// custom grayscale conversion logic. Otherwise, the default grayscale conversion
        /// logic will be applied using the <see cref="GenericImage.ChangeToGrayScale"/> method.
        /// </remarks>
        /// <returns>A new <see cref="Image"/> instance representing
        /// the grayscale version of the current image.</returns>
        public virtual Image ToGrayScale()
        {
            if(GrayScale is null)
            {
                return SkiaUtils.ConvertToGrayscale(this);
            }
            else
            {
                BaseEventArgs<Image> args = new(this);
                GrayScale(this, args);
                return args.Value;
            }
        }

        /// <summary>
        /// Creates an new image from this image with all pixels lighter
        /// (this method makes 2x lighter than <see cref="WithLightColors"/>).
        /// </summary>
        /// <returns></returns>
        public virtual Image WithLightLightColors()
        {
            return WithConvertedColors(ControlPaint.LightLight);
        }

        /// <summary>
        /// Creates an new image from this image with all pixels lighter
        /// </summary>
        /// <returns></returns>
        public virtual Image WithLightColors()
        {
            return WithConvertedColors(ControlPaint.Light);
        }

        /// <summary>
        /// Creates an new image from this image with all pixels converted using
        /// the specified function.
        /// </summary>
        /// <param name="func">Function used to convert color of the pixel.</param>
        /// <returns></returns>
        public virtual Image WithConvertedColors(Func<ColorStruct, ColorStruct> func)
        {
            var generic = (GenericImage)this;
            generic.ConvertColors(func);
            return (Image)generic;
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
        /// <param name="quality">Image quality. Optional. If not specified,
        /// <see cref="DefaultSaveQuality"/> is used. Can be ignored on some platforms.</param>
        public virtual bool Save(Stream stream, ImageFormat format, int? quality = null)
        {
            if (stream is null)
                return false;

            if (format is null)
                return false;

            quality ??= DefaultSaveQuality;

            return Handler.SaveToStream(stream, format.AsBitmapType(), quality.Value);
        }

        /// <summary>
        /// Saves this <see cref="Image"/> to the specified file.
        /// </summary>
        /// <param name="fileName">A string that contains the name of the file
        /// to which to save this <see cref="Image"/>.</param>
        /// <remarks>Use <see cref="ExtensionsForSave"/> to get supported formats
        /// for the save operation.</remarks>
        /// <param name="quality">Image quality. Optional. If not specified,
        /// <see cref="DefaultSaveQuality"/> is used. Can be ignored on some platforms.</param>
        public virtual bool Save(string fileName, int? quality = null)
        {
            if (fileName is null)
                return false;

            quality ??= DefaultSaveQuality;

            return InsideTryCatch(() =>
            {
                using var stream = FileSystem.Default.Create(fileName);
                var bitmapType = Image.GetBitmapTypeFromFileName(fileName);
                return Save(stream, bitmapType, quality.Value);
            });
        }

        /// <summary>
        /// Gets the size of the image in device-independent units using the specified scale factor.
        /// </summary>
        /// <param name="scaleFactor">Scale factor used for the conversion. Optional.
        /// If not specified, default value is used.</param>
        /// <returns></returns>
        public virtual SizeD SizeDip(Coord scaleFactor)
        {
            return GraphicsFactory.PixelToDip(PixelSize, scaleFactor);
        }

        /// <summary>
        /// Gets the size of the image in device-independent units.
        /// </summary>
        public virtual SizeD SizeDip(AbstractControl control)
            => control.PixelToDip(PixelSize);

        /// <summary>
        /// Saves the current image to the desktop with the specified file name.
        /// </summary>
        /// <param name="imageName">Optional. The name of the image file to save.
        /// If not specified, the default name "image.png" will be used.</param>
        /// <param name="uniqueName">
        /// A boolean value indicating whether to generate a unique file name if
        /// a file with the same name already exists. Optional. Default is <c>false</c>.
        /// </param>
        public virtual void SaveToDesktop(string? imageName = null, bool uniqueName = false)
        {
            var imagePath = PathUtils.GenFilePathOnDesktop(imageName ?? "image.png", uniqueName);
            FileUtils.DeleteIfExistsSafe(imagePath);
            Save(imagePath);
        }

        /// <summary>
        /// Gets image rect as (0, 0, SizeDip(control).Width, SizeDip(control).Height).
        /// </summary>
        public virtual RectD BoundsDip(AbstractControl control)
        {
            var size = SizeDip(control);
            return (0, 0, size.Width, size.Height);
        }

        /// <summary>
        /// Gets image rect as (0, 0, SizeDip(scaleFactor).Width, SizeDip(scaleFactor).Height).
        /// </summary>
        public virtual RectD BoundsDip(Coord scaleFactor)
        {
            var size = SizeDip(scaleFactor);
            return (0, 0, size.Width, size.Height);
        }

        /// <summary>
        /// Gets <see cref="ISkiaSurface"/> for this image.
        /// </summary>
        /// <param name="lockMode">Lock mode.</param>
        /// <returns></returns>
        public virtual ISkiaSurface LockSurface(ImageLockMode lockMode = ImageLockMode.ReadWrite)
        {
            if (Immutable && lockMode != ImageLockMode.ReadOnly)
                throw new Exception($"LockSurface({lockMode}) failed on the immutable image.");

            return GraphicsFactory.CreateSkiaSurface(this, lockMode);
        }

        /// <summary>
        /// Assigns <see cref="SKBitmap"/> to this image.
        /// </summary>
        /// <param name="bitmap">Bitmap to assign.</param>
        public virtual void Assign(SKBitmap bitmap)
        {
            if (Immutable)
                throw new Exception($"Assign(SKBitmap) is not possible on the immutable image.");
            Handler.Assign(bitmap);
        }

        /// <summary>
        /// Converts this image to a Base64-encoded string in the specified format.
        /// </summary>
        /// <param name="bitmapType">The format in which the image should be encoded.</param>
        /// <param name="quality">The quality of the image encoding. Optional.
        /// If not specified, <see cref="DefaultSaveQuality"/> is used.</param>
        /// <returns>A Base64-encoded string representation of the image.</returns>
        public virtual string ToBase64String(BitmapType bitmapType, int? quality = null)
        {
            var memStream = new MemoryStream();
            Save(memStream, bitmapType, quality);
            memStream.Seek(0, SeekOrigin.Begin);
            var result = StreamUtils.ConvertStreamToBase64(memStream);
            return result;
        }

        /// <summary>
        /// Assigns <see cref="GenericImage"/> to this image.
        /// </summary>
        /// <param name="genericImage">Image to assign.</param>
        public virtual void Assign(GenericImage genericImage)
        {
            if (Immutable)
                throw new Exception($"Assign(GenericImage) is not possible on the immutable image.");
            Handler.Assign(genericImage);
        }

        /// <summary>
        /// Gets <see cref="Graphics"/> for this image on which you can paint.
        /// </summary>
        /// <returns></returns>
        public virtual Graphics GetDrawingContext()
        {
            if (Immutable)
                throw new Exception($"GetDrawingContext() is not possible on the immutable image.");
            var dc = Graphics.FromImage(this);
            return dc;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string? ToString()
        {
            return url ?? base.ToString();
        }

        /// <inheritdoc/>
        protected override IImageHandler CreateHandler()
        {
            return GraphicsFactory.Handler.CreateImageHandler();
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            ResetGrayScaleCache();
            base.DisposeManaged();
        }

        /// <summary>
        /// Loads image data from the specified stream.
        /// </summary>
        /// <param name="stream">The data stream used to load the image.</param>
        /// <param name="type">One of the <see cref="BitmapType"/> values</param>
        protected virtual bool InternalLoadFromStream(Stream stream, BitmapType type = BitmapType.Any)
        {
            if (UseSkiaSharpForLoading)
            {
                try
                {
                    var bitmap = SKBitmap.Decode(stream);
                    Assign(bitmap);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return Handler.LoadFromStream(stream, type);
            }
        }
    }
}