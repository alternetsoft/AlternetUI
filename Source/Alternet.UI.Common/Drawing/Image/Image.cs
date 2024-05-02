using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Describes an image to be drawn on a <see cref="Graphics"/> or
    /// displayed in a UI control.
    /// </summary>
    [TypeConverter(typeof(ImageConverter))]
    public class Image : DisposableObject, IDisposable
    {
        /// <summary>
        /// Occurs when <see cref="ToGrayScale"/> is called. Used to override default
        /// grayscale method.
        /// </summary>
        public static EventHandler<BaseEventArgs<Image>>? GrayScale;

        private object nativeImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class from a stream.
        /// </summary>
        /// <param name="stream">Stream with bitmap.</param>
        /// <param name="bitmapType">Type of the bitmap.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Image(Stream stream, BitmapType bitmapType = BitmapType.Any)
        {
            nativeImage = NativeDrawing.Default.CreateImage();
            Load(stream, bitmapType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class with the image from
        /// <see cref="ImageSet"/>.
        /// </summary>
        /// <param name="imageSet">Source of the image.</param>
        /// <param name="size">Size of the image in device pixels.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Image(ImageSet imageSet, SizeI size)
        {
            nativeImage = NativeDrawing.Default.CreateImage(imageSet, size);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class with the image from
        /// <see cref="ImageSet"/>.
        /// </summary>
        /// <param name="imageSet">Source of the image.</param>
        /// <param name="control">Control used to get dpi.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Image(ImageSet imageSet, IControl control)
        {
            nativeImage = NativeDrawing.Default.CreateImage(imageSet, control);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class from the specified
        /// existing image.
        /// </summary>
        /// <param name="image">The <see cref="Image"/> from which to create the
        /// new <see cref="Bitmap"/>.</param>
        /// <remarks>
        /// Full image data is copied from the original image.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Image(Image original)
        {
            nativeImage = NativeDrawing.Default.CreateImageFromImage(original);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image" /> class from the specified
        /// existing image, scaled to the specified size.
        /// </summary>
        /// <param name="original">The <see cref="Image" /> from which to create the new image.</param>
        /// <param name="newSize">The <see cref="SizeI" /> structure that represent the
        /// size of the new image.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Image(Image original, SizeI newSize)
        {
            nativeImage = NativeDrawing.Default.CreateImageFromImage(original, newSize);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class from
        /// the specified <see cref="GenericImage"/>.
        /// </summary>
        /// <param name="genericImage">Generic image.</param>
        /// <param name="depth">Specifies the depth of the bitmap.
        /// Some platforms only support (1) for monochrome and (-1) for the current color setting.
        /// A depth of 32 including an alpha channel is supported under MSW, Mac and Linux.
        /// If this parameter is omitted
        /// (= -1), the display depth of the screen is used.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Image(GenericImage genericImage, int depth = -1)
        {
            nativeImage = NativeDrawing.Default.CreateImageFromGenericImage(genericImage, depth);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class from
        /// the specified data stream.
        /// </summary>
        /// <param name="stream">The data stream used to load the image.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Image(Stream? stream)
        {
            nativeImage = NativeDrawing.Default.CreateImage();
            if (stream is null)
                return;

            NativeDrawing.Default.ImageLoadFromStream(NativeObject, stream);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class
        /// with the specified size in device pixels.
        /// </summary>
        /// <param name="width">The width used to create the image</param>
        /// <param name="height">The height used to create the image</param>
        /// <param name="depth">Specifies the depth of the bitmap.
        /// Some platforms only support (1) for monochrome and (-1) for the current color setting.
        /// A depth of 32 including an alpha channel is supported under MSW, Mac and Linux.
        /// If this parameter is omitted
        /// (= -1), the display depth of the screen is used.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Image(int width, int height, int depth = 32)
            : this(new SizeI(width, height), depth)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class
        /// with the specified size in device pixels.
        /// </summary>
        /// <param name="width">The width used to create the image</param>
        /// <param name="height">The height used to create the image</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Image(double width, double height)
            : this((int)width, (int)height)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class
        /// with the specified size in device pixels.
        /// </summary>
        /// <param name="size">The size in device pixels used to create the image.</param>
        /// <param name="depth">Specifies the depth of the bitmap.
        /// Some platforms only support (1) for monochrome and (-1) for the current color setting.
        /// A depth of 32 including an alpha channel is supported under MSW, Mac and Linux.
        /// If this parameter is omitted
        /// (= -1), the display depth of the screen is used.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Image(SizeI size, int depth = 32)
        {
            nativeImage = NativeDrawing.Default.CreateImageWithSizeAndDepth(size, depth);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="url">Url to the image.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Image(string url)
        {
            nativeImage = NativeDrawing.Default.CreateImage();
            using var stream = ResourceLoader.StreamFromUrl(url);
            if (stream is null)
            {
                BaseApplication.LogError($"Image not loaded from: {url}");
                return;
            }

            var result = NativeDrawing.Default.ImageLoadFromStream(NativeObject, stream);

            if (!result)
            {
                BaseApplication.LogError($"Image not loaded from: {url}");
                return;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Image()
            : this(SizeI.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="nativeImage">Native image instance.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Image(object nativeImage)
        {
            this.nativeImage = nativeImage;
        }

        /// <summary>
        /// Gets default <see cref="BitmapType"/> value for the current operating system.
        /// </summary>
        /// <returns></returns>
        public static BitmapType DefaultBitmapType => NativeDrawing.Default.GetDefaultBitmapType();

        /// <summary>
        /// Converts this object to <see cref="GenericImage"/>.
        /// </summary>
        [Browsable(false)]
        public virtual GenericImage AsGeneric
        {
            get
            {
                var nativeGenericImage = NativeDrawing.Default.ImageConvertToGenericImage(this);
                return new GenericImage(nativeGenericImage);
            }
        }

        /// <summary>
        /// Gets the size of the image in pixels.
        /// </summary>
        public virtual SizeI PixelSize => NativeDrawing.Default.GetImagePixelSize(NativeObject);

        /// <summary>
        /// Gets whether image is ok (is not disposed and has non-zero width and height).
        /// </summary>
        public virtual bool IsOk => !IsDisposed && NativeDrawing.Default.GetImageIsOk(NativeObject);

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
                return NativeDrawing.Default.GetImageHasAlpha(NativeObject);
            }

            set
            {
                NativeDrawing.Default.SetImageHasAlpha(NativeObject, value);
            }
        }

        /// <summary>
        /// Gets image width in pixels.
        /// </summary>
        public virtual int Width => NativeDrawing.Default.GetImagePixelSize(NativeObject).Width;

        /// <summary>
        /// Gets image height in pixels.
        /// </summary>
        public virtual int Height => NativeDrawing.Default.GetImagePixelSize(NativeObject).Height;

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
        public virtual double ScaleFactor
        {
            get
            {
                return NativeDrawing.Default.GetImageScaleFactor(NativeObject);
            }

            set
            {
                NativeDrawing.Default.SetImageScaleFactor(NativeObject, value);
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
                return NativeDrawing.Default.GetImageDipSize(NativeObject);
            }
        }

        /// <summary>
        /// Gets the height of the bitmap in logical pixels.
        /// </summary>
        public virtual double ScaledHeight
        {
            get
            {
                return NativeDrawing.Default.GetImageScaledHeight(NativeObject);
            }
        }

        /// <summary>
        /// Gets the size of the bitmap in logical pixels.
        /// </summary>
        public virtual SizeI ScaledSize
        {
            get
            {
                return NativeDrawing.Default.GetImageScaledSize(NativeObject);
            }
        }

        /// <summary>
        /// Gets the width of the bitmap in logical pixels.
        /// </summary>
        public virtual double ScaledWidth
        {
            get
            {
                return NativeDrawing.Default.GetImageScaledWidth(NativeObject);
            }
        }

        /// <summary>
        /// Gets the color depth of the image. Returned value is 32, 24, or other.
        /// </summary>
        public virtual int Depth
        {
            get
            {
                return NativeDrawing.Default.GetImageDepth(NativeObject);
            }
        }

        /// <summary>
        /// Gets native image.
        /// </summary>
        public object NativeObject
        {
            get
            {
                CheckDisposed();
                return nativeImage;
            }

            protected set
            {
                nativeImage = value;
            }
        }

        /// <summary>
        /// Gets <see cref="Graphics"/> which allows to draw on the image.
        /// Same as <see cref="GetDrawingContext"/>.
        /// </summary>
        [Browsable(false)]
        public virtual Graphics Canvas => GetDrawingContext();

        /// <summary>
        /// Converts the specified <see cref='GenericImage'/> to a <see cref='Image'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Image(GenericImage image) => new(image);

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
            return new Image(stream);
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
            var nativeImage = NativeDrawing.Default.CreateImageFromScreen();
            return new Image(nativeImage);
        }

        /// <summary>
        /// Creates an <see cref="Image" /> from the specified data stream.
        /// </summary>
        /// <param name="stream">
        /// A <see cref="Stream" /> that contains the data for this <see cref="Image" />.</param>
        /// <returns>The <see cref="Image" /> this method creates.</returns>
        public static Image FromStream(Stream stream)
        {
            return new Image(stream);
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
            var nativeImage = NativeDrawing.Default.CreateImageFromSvgStream(
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
            var nativeImage = NativeDrawing.Default.CreateImageFromSvgString(s, width, height, color);
            var result = new Image(nativeImage);
            return result;
        }

        /// <summary>
        /// Gets list of extensions (including ".") which can be used to filter out
        /// supported image formats when using
        /// <see cref="OpenFileDialog"/> and <see cref="SaveFileDialog"/>.
        /// </summary>
        public static IEnumerable<string> GetExtensionsForLoadSave()
        {
            string[] ext =
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

            return ext;
        }

        /// <summary>
        /// Gets list of extensions (including ".") which can be used to filter out
        /// supported image formats when using
        /// <see cref="OpenFileDialog"/>.
        /// </summary>
        public static IEnumerable<string> GetExtensionsForLoad()
        {
            string[] ext =
            {
                ".gif",
                ".ico",
                ".cur",
                ".iff",
                ".ani",
            };

            var result = new List<string>();
            result.AddRange(GetExtensionsForLoadSave());
            result.AddRange(ext);
            return result;
        }

        /// <summary>
        /// Gets list of extensions (including ".") which can be used to filter out
        /// supported image formats when using
        /// <see cref="SaveFileDialog"/>.
        /// </summary>
        public static IEnumerable<string> GetExtensionsForSave() => GetExtensionsForLoadSave();

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class
        /// from the specified url which points to svg file or resource.
        /// </summary>
        /// <remarks>
        /// This is similar to <see cref="Image.FromSvgUrl"/> but uses
        /// <see cref="Control.GetDPI"/> and <see cref="ToolbarUtils.GetDefaultImageSize(double)"/>
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
            var width = ToolbarUtils.GetDefaultImageSize(deviceDpi.Width);
            var height = ToolbarUtils.GetDefaultImageSize(deviceDpi.Height);
            var result = Image.FromSvgUrl(url, width, height, color);
            return result;
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
            return NativeDrawing.Default.ImageLoad(NativeObject, name, type);
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
        public virtual bool Save(string name, BitmapType type)
        {
            return NativeDrawing.Default.ImageSaveToFile(NativeObject, name, type);
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
        public virtual bool Save(Stream stream, BitmapType type)
        {
            return NativeDrawing.Default.ImageSaveToStream(NativeObject, stream, type);
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
            return NativeDrawing.Default.ImageLoadFromStream(NativeObject, stream, type);
        }

        /// <summary>
        /// Returns a sub image of the current one as long as the <paramref name="rect"/> belongs
        /// entirely to the image.
        /// </summary>
        /// <param name="rect">Rectangle in this image.</param>
        /// <returns></returns>
        public virtual Image GetSubBitmap(RectI rect)
        {
            var converted = NativeDrawing.Default.ImageGetSubBitmap(NativeObject, rect);
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
            var converted = NativeDrawing.Default.ImageConvertToDisabled(NativeObject, brightness);
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
            NativeDrawing.Default.ImageRescale(NativeObject, sizeNeeded);
        }

        /// <summary>
        /// Resets alpha channel.
        /// </summary>
        public virtual void ResetAlpha()
        {
            NativeDrawing.Default.ImageResetAlpha(NativeObject);
        }

        /// <summary>
        /// Creates a clone of this image with fully copied image data.
        /// </summary>
        /// <returns></returns>
        public virtual Image Clone()
        {
            return new Image(this);
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
        public virtual bool Save(Stream stream, ImageFormat format)
        {
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (format is null)
                throw new ArgumentNullException(nameof(format));

            return NativeDrawing.Default.ImageSave(NativeObject, stream, format);
        }

        /// <summary>
        /// Saves this <see cref="Image"/> to the specified file.
        /// </summary>
        /// <param name="fileName">A string that contains the name of the file
        /// to which to save this <see cref="Image"/>.</param>
        /// <remarks>Use <see cref="GetExtensionsForSave"/> to get supported formats
        /// for the save operation.</remarks>
        public virtual bool Save(string fileName)
        {
            if (fileName is null)
                throw new ArgumentNullException(nameof(fileName));

            return NativeDrawing.Default.ImageSave(NativeObject, fileName);
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
        protected override void DisposeManagedResources()
        {
            if (nativeImage is not null)
            {
                ((IDisposable)nativeImage).Dispose();
                nativeImage = null!;
            }
        }
    }
}