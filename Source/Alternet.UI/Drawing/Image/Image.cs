using System;
using System.ComponentModel;
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
        /// Get or set color used in <see cref="ToGrayScale"/> method.
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
        /// Gets the size of the image in pixels.
        /// </summary>
        public Int32Size PixelSize => NativeImage.PixelSize;

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
        /// Initializes a new instance of the <see cref="Image"/> class
        /// from the specified url.
        /// </summary>
        /// <param name="url">The file or embedded resource url used
        /// to load the image.
        /// </param>
        /// <example>
        /// <code>
        /// var ImageSize = 16;
        /// var ResPrefix = $"embres:ControlsTest.resources.Png._{ImageSize}.";
        /// var url = $"{ResPrefix}arrow-left-{ImageSize}.png"
        /// button1.Image = Bitmap.FromUrl(url);
        /// </code>
        /// </example>
        public static Image FromUrl(string url)
        {
            using var stream = ResourceLoader.StreamFromUrl(url);
            return new Bitmap(stream);
        }

        /// <summary>
        /// Makes image grayscaled.
        /// </summary>
        /// <returns><c>true</c> if operation is successful. </returns>
        public bool GrayScale()
        {
            return NativeImage.GrayScale();
        }

        /// <summary>
        /// Creates grayscaled version of the image.
        /// </summary>
        /// <returns>Returns new grayscaled image from this image.</returns>
        /// <remarks>
        /// This method uses <see cref="GrayScale"/> and if it is unsuccessfull,
        /// it fills the image with <see cref="DisabledBrushColor"/>.
        /// </remarks>
        public Image ToGrayScale()
        {
            Bitmap bitmap = new(this);

            if (bitmap.GrayScale())
                return bitmap;

            var size = bitmap.Size;
            using var dc = DrawingContext.FromImage(bitmap);
            dc.FillRectangle(
                DisabledBrush,
                new(0, 0, size.Width, size.Height));
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