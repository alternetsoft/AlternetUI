using System;
using System.ComponentModel;
using System.IO;

namespace Alternet.Drawing
{
    /// <summary>
    /// Describes an image to be drawn on a <see cref="DrawingContext"/> or displayed in a UI control.
    /// </summary>
    [TypeConverter(typeof(ImageConverter))]
    public abstract class Image : IDisposable
    {
        private bool isDisposed;
        private UI.Native.Image nativeImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class from the specified data stream.
        /// </summary>
        /// <param name="stream">The data stream used to load the image.</param>
        private protected Image(Stream stream)
        {
            nativeImage = new UI.Native.Image();
            using (var inputStream = new UI.Native.InputStream(stream))
                NativeImage.LoadFromStream(inputStream);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class with the specified size.
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

        public static Image FromUrl(string url)
        {
            var s = url;
            var uri = s.StartsWith("/")
                ? new Uri(s, UriKind.Relative)
                : new Uri(s, UriKind.RelativeOrAbsolute);

            if (uri.IsAbsoluteUri && uri.IsFile)
            {
                using var stream = File.OpenRead(uri.LocalPath);
                return new Bitmap(stream);
            }

            var assets = new Alternet.UI.ResourceLoader();
            return new Bitmap(assets.Open(uri));
        }

        /// <summary>
        /// Gets the size of the image in device-independent units (1/96th inch per unit).
        /// </summary>
        public Size Size => NativeImage.Size;

        /// <summary>
        /// Gets the size of the image in pixels.
        /// </summary>
        public Int32Size PixelSize => NativeImage.PixelSize;

        internal UI.Native.Image NativeImage
        {
            get
            {
                CheckDisposed();
                return nativeImage;
            }
        }

        private void CheckDisposed()
        {
            if (isDisposed)
                throw new ObjectDisposedException(null);
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
        /// Releases the unmanaged resources used by the <see cref="Image"/> and optionally releases the managed resources.
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

        /// <summary>
        /// Saves this image to the specified stream in the specified format.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> where the image will be saved.</param>
        /// <param name="format">An <see cref="ImageFormat"/> that specifies the format of the saved image.</param>
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
        /// <param name="fileName">A string that contains the name of the file to which to save this <see cref="Image"/>.</param>
        public void Save(string fileName)
        {
            if (fileName is null)
                throw new ArgumentNullException(nameof(fileName));

            NativeImage.SaveToFile(fileName);
        }
    }
}