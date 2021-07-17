using System;
using System.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides methods to manage a collection of <see cref="Image"/> objects.
    /// </summary>
    /// <remarks>
    /// ImageList is typically used by other controls, such as the <see cref="ListView"/>.
    /// You can add images to the <see cref="ImageList"/>, and the other controls are
    /// able to use the images as they require.
    /// </remarks>
    public class ImageList : IDisposable
    {
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageList"/> with default values.
        /// </summary>
        public ImageList() // todo: lifetime
        {
            NativeImageList = new Native.ImageList();
        }

        /// <summary>
        /// Gets or sets the size of the images in the image list.
        /// </summary>
        /// <value>The <see cref="Size"/> that defines the height and width, in pixels, of the images in the list. The default size is 16 by 16.</value>
        public Size PixelImageSize { get => NativeImageList.PixelImageSize; set => NativeImageList.PixelImageSize = value; }

        internal Native.ImageList NativeImageList { get; private set; }

        /// <summary>
        /// Releases all resources used by the <see cref="ImageList"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="ImageList"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    NativeImageList.Dispose();
                    NativeImageList = null!;
                }

                isDisposed = true;
            }
        }
    }
}