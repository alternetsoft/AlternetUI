using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
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
        protected Image(Stream stream, BitmapType bitmapType = BitmapType.Any)
        {
            nativeImage = NativeDrawing.Default.CreateImage();
            Load(stream, bitmapType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class with the image from
        /// <see cref="ImageSet"/>.
        /// </summary>
        /// <param name="imageSet">Source of the image.</param>
        /// <param name="control">Control used to get dpi.</param>
        protected Image(ImageSet imageSet, Control control)
        {
            nativeImage = NativeDrawing.Default.CreateImage();
            imageSet.NativeImageSet.InitImageFor((UI.Native.Image)NativeObject, control.WxWidget);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image" /> class from the specified
        /// existing image, scaled to the specified size.
        /// </summary>
        /// <param name="original">The <see cref="Image" /> from which to create the
        /// new <see cref="Image" />.</param>
        /// <param name="newSize">The <see cref="SizeI" /> structure that represent the
        /// size of the new <see cref="Bitmap" />.</param>
        protected Image(Image original, SizeI newSize)
        {
            nativeImage = NativeDrawing.Default.CreateImage();
            ((UI.Native.Image)NativeObject).InitializeFromImage(
                (UI.Native.Image)original.NativeObject,
                newSize);
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
        protected Image(GenericImage genericImage, int depth = -1)
        {
            nativeImage = NativeDrawing.Default.CreateImage();
            ((UI.Native.Image)NativeObject).LoadFromGenericImage(genericImage.Handle, depth);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class with the specified size
        /// amd scaling factor from the <paramref name="control"/>.
        /// </summary>
        /// <param name="size">The size, in device pixels, of the new <see cref="Bitmap"/>.</param>
        /// <param name="control">The control from which pixel scaling factor is used.</param>
        protected Image(SizeI size, Control control)
            : this(size)
        {
            ScaleFactor = control.GetPixelScaleFactor();
        }

        /// <summary>
        /// Creates a bitmap compatible with the given <see cref="Graphics"/>, inheriting
        /// its magnification factor.
        /// </summary>
        /// <param name="width">The width of the bitmap in pixels, must be strictly positive.</param>
        /// <param name="height">The height of the bitmap in pixels, must be strictly positive.</param>
        /// <param name="dc"><see cref="Graphics"/> from which the scaling factor is inherited.</param>
        protected Image(int width, int height, Graphics dc)
        {
            nativeImage = NativeDrawing.Default.CreateImage();
            UI.Native.DrawingContext.ImageFromDrawingContext(
                (UI.Native.Image)NativeObject,
                width,
                height,
                (UI.Native.DrawingContext)dc.NativeObject);
        }

        /// <summary>
        /// Creates a bitmap compatible with the given <see cref="Graphics"/> from
        /// the given <see cref="GenericImage"/>.
        /// </summary>
        /// <param name="genericImage">Platform-independent image object.</param>
        /// <param name="dc"><see cref="Graphics"/> from which the scaling
        /// factor is inherited.</param>
        /// <remarks>
        /// This constructor initializes the bitmap with the data of the given image, which
        /// must be valid, but inherits the scaling factor from the given device context
        /// instead of simply using the default factor of 1.
        /// </remarks>
        protected Image(GenericImage genericImage, Graphics dc)
        {
            nativeImage = NativeDrawing.Default.CreateImage();
            UI.Native.DrawingContext.ImageFromGenericImageDC(
                (UI.Native.Image)NativeObject,
                genericImage.Handle,
                (UI.Native.DrawingContext)dc.NativeObject);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class from
        /// the specified data stream.
        /// </summary>
        /// <param name="stream">The data stream used to load the image.</param>
        protected Image(Stream? stream)
        {
            nativeImage = NativeDrawing.Default.CreateImage();
            if (stream is null)
                return;
            using var inputStream = new UI.Native.InputStream(stream);
            ((UI.Native.Image)NativeObject).LoadFromStream(inputStream);
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
        protected Image(int width, int height, int depth = 32)
        {
            nativeImage = NativeDrawing.Default.CreateImage();
            ((UI.Native.Image)NativeObject).Initialize((width, height), depth);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class
        /// with the specified size in device pixels.
        /// </summary>
        /// <param name="width">The width used to create the image</param>
        /// <param name="height">The height used to create the image</param>
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
        protected Image(SizeI size, int depth = 32)
        {
            nativeImage = NativeDrawing.Default.CreateImage();
            ((UI.Native.Image)NativeObject).Initialize(size, depth);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class with the image from
        /// <see cref="ImageSet"/>.
        /// </summary>
        /// <param name="imageSet">Source of the image.</param>
        /// <param name="size">Size of the image in device pixels.</param>
        protected Image(ImageSet imageSet, SizeI size)
        {
            nativeImage = NativeDrawing.Default.CreateImage();
            imageSet.NativeImageSet.InitImage((UI.Native.Image)NativeObject, size.Width, size.Height);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="url">Url to the image.</param>
        protected Image(string url)
        {
            nativeImage = NativeDrawing.Default.CreateImage();
            using var stream = ResourceLoader.StreamFromUrl(url);
            if (stream is null)
            {
                Application.LogError($"Image not loaded from: {url}");
                return;
            }

            using var inputStream = new UI.Native.InputStream(stream);
            if (inputStream is null)
            {
                Application.LogError($"Image not loaded from: {url}");
                return;
            }

            ((UI.Native.Image)NativeObject).LoadFromStream(inputStream);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        protected Image()
            : this(SizeI.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="nativeImage">Native image instance.</param>
        protected Image(object nativeImage)
        {
            this.nativeImage = nativeImage;
        }

        /// <summary>
        /// Gets default <see cref="BitmapType"/> value for the current operating system.
        /// </summary>
        /// <returns></returns>
        public static BitmapType DefaultBitmapType
            => (BitmapType)UI.Native.Image.GetDefaultBitmapType();

        /// <summary>
        /// Converts this object to <see cref="GenericImage"/>.
        /// </summary>
        [Browsable(false)]
        public GenericImage AsGeneric
        {
            get
            {
                return new GenericImage(((UI.Native.Image)NativeObject).ConvertToGenericImage());
            }
        }

        /// <summary>
        /// Gets the size of the image in pixels.
        /// </summary>
        public SizeI PixelSize => ((UI.Native.Image)NativeObject).PixelSize;

        /// <summary>
        /// Gets whether image is ok (is not disposed and has non-zero width and height).
        /// </summary>
        public bool IsOk => !IsEmpty;

        /// <summary>
        /// Creates texture brush with this image.
        /// </summary>
        [Browsable(false)]
        public TextureBrush AsBrush => new(this);

        /// <summary>
        /// Gets whether image is empty (is disposed or has an empty width or height).
        /// </summary>
        public bool IsEmpty => IsDisposed || !IsOk || Size.AnyIsEmpty;

        /// <summary>
        /// Gets or sets whether this image has an alpha channel.
        /// </summary>
        public bool HasAlpha
        {
            get
            {
                return ((UI.Native.Image)NativeObject).HasAlpha;
            }

            set
            {
                ((UI.Native.Image)NativeObject).HasAlpha = value;
            }
        }

        /// <summary>
        /// Gets <see cref="Graphics"/> which allows to draw on the image.
        /// Same as <see cref="GetDrawingContext"/>.
        /// </summary>
        [Browsable(false)]
        public Graphics Canvas => GetDrawingContext();

        /// <summary>
        /// Gets image width in pixels.
        /// </summary>
        public int Width => ((UI.Native.Image)NativeObject).PixelWidth;

        /// <summary>
        /// Gets image height in pixels.
        /// </summary>
        public int Height => ((UI.Native.Image)NativeObject).PixelHeight;

        /// <summary>
        /// Gets image bounds in pixels. This method returns (0, 0, Width, Height).
        /// </summary>
        public RectI Bounds
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
        public int PixelWidth => ((UI.Native.Image)NativeObject).PixelWidth;

        /// <summary>
        /// Gets image height in pixels.
        /// </summary>
        public int PixelHeight => ((UI.Native.Image)NativeObject).PixelHeight;

        /// <summary>
        /// Gets image size in pixels.
        /// </summary>
        public SizeI Size => (PixelWidth, PixelHeight);

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
        public double ScaleFactor
        {
            get
            {
                return ((UI.Native.Image)NativeObject).ScaleFactor;
            }

            set
            {
                ((UI.Native.Image)NativeObject).ScaleFactor = value;
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
        public SizeI DipSize
        {
            get
            {
                return ((UI.Native.Image)NativeObject).DipSize;
            }
        }

        /// <summary>
        /// Gets the height of the bitmap in logical pixels.
        /// </summary>
        public double ScaledHeight
        {
            get
            {
                return ((UI.Native.Image)NativeObject).ScaledHeight;
            }
        }

        /// <summary>
        /// Gets the size of the bitmap in logical pixels.
        /// </summary>
        public SizeI ScaledSize
        {
            get
            {
                return ((UI.Native.Image)NativeObject).ScaledSize;
            }
        }

        /// <summary>
        /// Gets the width of the bitmap in logical pixels.
        /// </summary>
        public double ScaledWidth
        {
            get
            {
                return ((UI.Native.Image)NativeObject).ScaledWidth;
            }
        }

        /// <summary>
        /// Gets the color depth of the image. Returned value is 32, 24, or other.
        /// </summary>
        public int Depth
        {
            get
            {
                return ((UI.Native.Image)NativeObject).Depth;
            }
        }

        internal object NativeObject
        {
            get
            {
                CheckDisposed();
                return nativeImage;
            }
        }

        /// <summary>
        /// Indicates whether the specified image is <c>null</c> or has an empty width (or height).
        /// </summary>
        /// <param name="image">The image to test.</param>
        /// <returns><c>true</c> if the <paramref name="image"/> parameter is <c>null</c> or
        /// has an empty width (or height); otherwise, <c>false</c>.</returns>
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
        /// Initializes a new instance of the <see cref="Image"/> class
        /// from the specified url which points to svg file or resource.
        /// </summary>
        /// <remarks>
        /// This is similar to <see cref="Image.FromSvgUrl"/> but uses
        /// <see cref="Control.GetDPI"/> and <see cref="ToolBar.GetDefaultImageSize(double)"/>
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
        public static Image FromSvgUrlForToolbar(string url, Control control, Color? color = null)
        {
            SizeD deviceDpi = control.GetDPI();
            var width = ToolBar.GetDefaultImageSize(deviceDpi.Width);
            var height = ToolBar.GetDefaultImageSize(deviceDpi.Height);
            var result = Image.FromSvgUrl(url, width, height, color);
            return result;
        }

        /// <summary>
        /// Creates images with screen pixels.
        /// </summary>
        /// <returns></returns>
        public static Image? FromScreen()
        {
            var nativeImage = new UI.Native.Image();
            if (nativeImage.InitializeFromScreen())
                return new Image(nativeImage);
            else
                return null;
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
            var nativeImage = new UI.Native.Image();
            using var inputStream = new UI.Native.InputStream(stream);
            nativeImage.LoadSvgFromStream(inputStream, width, height, color ?? Color.Black);
            var result = new Bitmap(nativeImage);
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
            var nativeImage = new UI.Native.Image();
            nativeImage.LoadSvgFromString(s, width, height, color ?? Color.Black);
            var result = new Bitmap(nativeImage);
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
        /// <remarks>Use <see cref="GetExtensionsForLoad"/> to get supported formats for the load operation.</remarks>
        public bool Load(string name, BitmapType type)
        {
            return ((UI.Native.Image)NativeObject).LoadFile(name, (int)type);
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
        /// <remarks>Use <see cref="GetExtensionsForSave"/> to get supported formats for the save operation.</remarks>
        public bool Save(string name, BitmapType type)
        {
            return ((UI.Native.Image)NativeObject).SaveFile(name, (int)type);
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
        /// <remarks>Use <see cref="GetExtensionsForSave"/> to get supported formats for the save operation.</remarks>
        public bool Save(Stream stream, BitmapType type)
        {
            var outputStream = new UI.Native.OutputStream(stream);
            return ((UI.Native.Image)NativeObject).SaveStream(outputStream, (int)type);
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
        /// <remarks>Use <see cref="GetExtensionsForLoad"/> to get supported formats for the load operation.</remarks>
        public bool Load(Stream stream, BitmapType type)
        {
            using var inputStream = new UI.Native.InputStream(stream);
            return ((UI.Native.Image)NativeObject).LoadStream(inputStream, (int)type);
        }

        /// <summary>
        /// Returns a sub image of the current one as long as the <paramref name="rect"/> belongs
        /// entirely to the image.
        /// </summary>
        /// <param name="rect">Rectangle in this image.</param>
        /// <returns></returns>
        public Image GetSubBitmap(RectI rect)
        {
            var converted = ((UI.Native.Image)NativeObject).GetSubBitmap(rect);
            return new Bitmap(converted);
        }

        /// <summary>
        /// Sets <see cref="ScaleFactor"/> using DPI value.
        /// </summary>
        /// <param name="dpi"></param>
        public void SetDPI(SizeD dpi)
        {
            var factor = dpi.Width / 96;
            this.ScaleFactor = factor;
        }

        /// <summary>
        /// Gets image rect as (0, 0, SizeDip().Width, SizeDip().Height).
        /// </summary>
        public RectD BoundsDip(Control control)
        {
            var size = SizeDip(control);
            return (0, 0, size.Width, size.Height);
        }

        /// <summary>
        /// Returns disabled (dimmed) version of the image.
        /// </summary>
        /// <param name="brightness">Brightness. Default is 255.</param>
        /// <returns></returns>
        public Image ConvertToDisabled(byte brightness = 255)
        {
            var converted = ((UI.Native.Image)NativeObject).ConvertToDisabled(brightness);
            return new Bitmap(converted);
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
        public void Rescale(SizeI sizeNeeded)
        {
            ((UI.Native.Image)NativeObject).Rescale(sizeNeeded);
        }

        /// <summary>
        /// Resets alpha channel.
        /// </summary>
        public void ResetAlpha()
        {
            ((UI.Native.Image)NativeObject).ResetAlpha();
        }

        /// <summary>
        /// Gets <see cref="Graphics"/> for this image on which you can paint.
        /// </summary>
        /// <returns></returns>
        public Graphics GetDrawingContext()
        {
            var dc = WxGraphics.FromImage(this);
            return dc;
        }

        /// <summary>
        /// Gets the size of the image in device-independent units (1/96th inch
        /// per unit).
        /// </summary>
        public SizeD SizeDip(Control control)
            => control.PixelToDip(NativeDrawing.Default.GetImagePixelSize(NativeObject));

        /// <summary>
        /// Creates a clone of this image with fully copied image data.
        /// </summary>
        /// <returns></returns>
        public Image Clone()
        {
            return new Bitmap(this);
        }

        /// <summary>
        /// Creates grayscaled version of the image.
        /// </summary>
        /// <returns>Returns new grayscaled image from this image.</returns>
        public Image ToGrayScale()
        {
            if(GrayScale is null)
            {
                var generic = (GenericImage)this;
                generic.ChangeToGrayScale();
                return (Bitmap)generic;
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
        public bool Save(Stream stream, ImageFormat format)
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
        public bool Save(string fileName)
        {
            if (fileName is null)
                throw new ArgumentNullException(nameof(fileName));

            return NativeDrawing.Default.ImageSave(NativeObject, fileName);
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