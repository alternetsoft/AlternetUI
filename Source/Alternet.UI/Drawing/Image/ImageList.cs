using System;
using Alternet.Base.Collections;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides methods to manage a collection of <see cref="Image"/> objects.
    /// </summary>
    /// <remarks>
    /// ImageList is typically used by other controls, such as the <see cref="Alternet.UI.ListView"/>.
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
            NativeImageList = new UI.Native.ImageList();

            Images.ItemInserted += Images_ItemInserted;
            Images.ItemRemoved += Images_ItemRemoved;
        }

        private void Images_ItemInserted(object? sender, CollectionChangeEventArgs<Image> e)
        {
            NativeImageList.AddImage(e.Item.NativeImage);
        }

        private void Images_ItemRemoved(object? sender, CollectionChangeEventArgs<Image> e)
        {
            // todo
            throw new Exception();
        }

        /// <summary>
        /// Gets the <see cref="Image"/> collection for this image list.
        /// </summary>
        /// <value>The collection of images.</value>
        public Collection<Image> Images { get; } = new Collection<Image>();

        /// <summary>
        /// Gets or sets the size of the images in the image list, in pixels.
        /// </summary>
        /// <value>
        /// The <see cref="Size"/> that defines the height and width, in pixels, of the images in the list.
        /// The default size is 16 by 16 device-independent units (1/96th inch per unit).
        /// </value>
        /// <remarks>
        /// Setting the <see cref="PixelImageSize"/> to a different value than the actual size of the images in
        /// the <see cref="Images"/>collection causes the images to be resized to the size specified.
        /// </remarks>
        public Int32Size PixelImageSize { get => NativeImageList.PixelImageSize; set => NativeImageList.PixelImageSize = value; }

        /// <summary>
        /// Gets or sets the size of the images in the image list, in device-independent units (1/96th inch per unit).
        /// </summary>
        /// <value>The <see cref="Size"/> that defines the height and width, in device-independent units (1/96th inch per unit),
        /// of the images in the list. The default size is 16 by 16.</value>
        /// <remarks>
        /// Setting the <see cref="ImageSize"/> to a different value than the actual size of the images in
        /// the <see cref="Images"/>collection causes the images to be resized to the size specified.
        /// </remarks>
        public Size ImageSize { get => NativeImageList.ImageSize; set => NativeImageList.ImageSize = value; }

        internal UI.Native.ImageList NativeImageList { get; private set; }

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