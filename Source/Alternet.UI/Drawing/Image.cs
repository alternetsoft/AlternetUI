using System;
using System.IO;

namespace Alternet.Drawing
{
    /// <summary>
    /// Describes an image to be drawn on a <see cref="DrawingContext"/> or displayed in a UI control.
    /// </summary>
    public class Image : IDisposable
    {
        private bool isDisposed;
        private UI.Native.Image nativeImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class from the specified data stream.
        /// </summary>
        /// <param name="stream">The data stream used to load the image.</param>
        public Image(Stream stream) : this()
        {
            using (var inputStream = new UI.Native.InputStream(stream))
                NativeImage.LoadFromStream(inputStream);
        }

        private protected Image()
        {
            nativeImage = new UI.Native.Image();
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
    }
}