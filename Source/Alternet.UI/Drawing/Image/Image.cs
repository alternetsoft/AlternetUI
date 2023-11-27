using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Describes an image to be drawn on a <see cref="DrawingContext"/> or
    /// displayed in a UI control.
    /// </summary>
    [TypeConverter(typeof(ImageConverter))]
    public abstract class Image : IDisposable
    {
        private static ImageGrayScaleMethod defaultGrayScaleMethod = ImageGrayScaleMethod.SetColorRGB150;
        private static Brush? disabledBrush = null;
        private static Color disabledBrushColor = Color.FromArgb(171, 71, 71, 71);

        private bool isDisposed;
        private UI.Native.Image nativeImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class from
        /// the specified data stream.
        /// </summary>
        /// <param name="stream">The data stream used to load the image.</param>
        private protected Image(Stream stream)
        {
            nativeImage = new UI.Native.Image();
            using var inputStream = new UI.Native.InputStream(stream);
            NativeImage.LoadFromStream(inputStream);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class
        /// with the specified size.
        /// </summary>
        /// <param name="size">The size used to create the image.</param>
        private protected Image(Size size)
        {
            nativeImage = new UI.Native.Image();
            NativeImage.Initialize(size);
        }

        private protected Image(ImageSet imageSet, Int32Size size)
        {
            nativeImage = new UI.Native.Image();
            imageSet.NativeImageSet.InitImage(nativeImage, size.Width, size.Height);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        private protected Image()
            : this(new Size())
        {
        }

        private protected Image(UI.Native.Image nativeImage)
        {
            this.nativeImage = nativeImage;
        }

        /// <summary>
        /// Gets or sets default gray scale method used in <see cref="GrayScale"/>
        /// and other functions.
        /// </summary>
        public static ImageGrayScaleMethod DefaultGrayScaleMethod
        {
            get
            {
                return defaultGrayScaleMethod;
            }

            set
            {
                if (value == ImageGrayScaleMethod.Default)
                    return;
                defaultGrayScaleMethod = value;
            }
        }

        /// <summary>
        /// Get or set color used when gray scale method is
        /// <see cref="ImageGrayScaleMethod.FillWithDisabledBrush"/>.
        /// </summary>
        public static Color DisabledBrushColor
        {
            get
            {
                return disabledBrushColor;
            }

            set
            {
                if (disabledBrushColor == value)
                    return;
                disabledBrushColor = value;
                disabledBrush = null;
            }
        }

        /// <summary>
        /// Gets the size of the image in device-independent units (1/96th inch
        /// per unit).
        /// </summary>
        public Size Size => NativeImage.Size;

        /// <summary>
        /// Gets image rect as (0, 0, Size.Width, Size.Height).
        /// </summary>
        public Rect Rect
        {
            get
            {
                var size = Size;
                return (0, 0, size.Width, size.Height);
            }
        }

        /// <summary>
        /// Gets the size of the image in pixels.
        /// </summary>
        public Int32Size PixelSize => NativeImage.PixelSize;

        /// <summary>
        /// Gets whether image is ok (is not disposed and has non-zero width and height).
        /// </summary>
        public bool IsOk => !IsEmpty;

        /// <summary>
        /// Gets whether image is empty (is disposed or has an empty width or height).
        /// </summary>
        public bool IsEmpty => isDisposed || !NativeImage.IsOk || Size.AnyIsEmpty;

        // Color.FromArgb(171, 71, 71, 71)
        // Color.FromArgb(128, 0, 0, 0)
        internal static Brush DisabledBrush
        {
            get
            {
                if (disabledBrush == null)
                    disabledBrush = new SolidBrush(disabledBrushColor);
                return disabledBrush;
            }

            set
            {
                disabledBrush = value;
            }
        }

        internal UI.Native.Image NativeImage
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
        /// <see cref="Control.GetDPI"/> and <see cref="Toolbar.GetDefaultImageSize(double)"/>
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
            Size deviceDpi = control.GetDPI();
            var width = Toolbar.GetDefaultImageSize(deviceDpi.Width);
            var height = Toolbar.GetDefaultImageSize(deviceDpi.Height);
            var result = Image.FromSvgUrl(url, width, height, color);
            return result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class
        /// from the specified <see cref="Stream"/> which contains svg data.
        /// </summary>
        /// <param name="stream">Stream with Svg data.</param>
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
        /// Gets list of extensions (including ".") which can be used to filter out
        /// supported image formats when using
        /// <see cref="OpenFileDialog"/> and <see cref="SaveFileDialog"/>.
        /// </summary>
        public static IEnumerable<string> GetExtensionsForOpenSave()
        {
            return [
                ".bmp",
                ".png",
                ".jpeg",
                ".jpg",
                ".gif",
                ".pcx",
                ".pnm",
                ".tiff",
                ".tga",
                ".xpm",
                ".ico",
                ".cur"];
        }

        /// <summary>
        /// Gets list of extensions (including ".") which can be used to filter out
        /// supported image formats when using
        /// <see cref="OpenFileDialog"/>.
        /// </summary>
        public static IEnumerable<string> GetExtensionsForOpen()
        {
            var result = new List<string>();
            result.AddRange(GetExtensionsForOpenSave());
            result.Add("iff");
            result.Add("ani");
            return result;
        }

        /// <summary>
        /// Gets list of extensions (including ".") which can be used to filter out
        /// supported image formats when using
        /// <see cref="SaveFileDialog"/>.
        /// </summary>
        public static IEnumerable<string> GetExtensionsForSave() => GetExtensionsForOpenSave();

        /// <summary>
        /// Gets <see cref="DrawingContext"/> for this image on which you can paint.
        /// </summary>
        /// <returns></returns>
        public DrawingContext GetDrawingContext()
        {
            var dc = DrawingContext.FromImage(this);
            return dc;
        }

        /// <summary>
        /// Makes image grayscaled.
        /// </summary>
        /// <returns><c>true</c> if operation is successful. </returns>
        public bool GrayScale(ImageGrayScaleMethod method = ImageGrayScaleMethod.Default)
        {
            void GrayScaleWithBrush()
            {
                var size = Size;
                using var dc = GetDrawingContext();
                dc.FillRectangle(
                    DisabledBrush,
                    new(0, 0, size.Width, size.Height));
            }

            if(method == ImageGrayScaleMethod.Default)
            {
                method = DefaultGrayScaleMethod;
            }

            switch (method)
            {
                case ImageGrayScaleMethod.SetColorRGB150:
                default:
                    return NativeImage.GrayScale();
                case ImageGrayScaleMethod.FillWithDisabledBrush:
                    GrayScaleWithBrush();
                    return true;
            }
        }

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
        /// <remarks>
        /// This method uses <see cref="GrayScale"/> and if it is unsuccessfull,
        /// it fills the image with <see cref="DisabledBrushColor"/>.
        /// </remarks>
        public Image ToGrayScale(ImageGrayScaleMethod method = ImageGrayScaleMethod.Default)
        {
            Bitmap bitmap = new(this);

            if (bitmap.GrayScale(method))
                return bitmap;

            bitmap.GrayScale(ImageGrayScaleMethod.FillWithDisabledBrush);
            return bitmap;
        }

        /// <summary>
        /// Saves this image to the specified stream in the specified format.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> where the image will be
        /// saved.</param>
        /// <param name="format">An <see cref="ImageFormat"/> that specifies
        /// the format of the saved image.</param>
        public void Save(Stream stream, ImageFormat format)
        {
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (format is null)
                throw new ArgumentNullException(nameof(format));

            var outputStream = new UI.Native.OutputStream(stream);
            NativeImage.SaveToStream(outputStream, format.ToString());
        }

        /// <summary>
        /// Saves this <see cref="Image"/> to the specified file.
        /// </summary>
        /// <param name="fileName">A string that contains the name of the file
        /// to which to save this <see cref="Image"/>.</param>
        public void Save(string fileName)
        {
            if (fileName is null)
                throw new ArgumentNullException(nameof(fileName));

            NativeImage.SaveToFile(fileName);
        }

        /// <summary>
        /// Releases all resources used by the <see cref="Image"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="Image"/>
        /// and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    NativeImage.Dispose();
                    nativeImage = null!;
                }

                isDisposed = true;
            }
        }

        private void CheckDisposed()
        {
            if (isDisposed)
                throw new ObjectDisposedException(null);
        }
    }
}