using System;
using Alternet.Base.Collections;
using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides methods to manage a collection of <see cref="Image"/> objects.
    /// </summary>
    /// <remarks>
    /// <see cref="ImageList"/> is used by some controls, such as the tree view and list view.
    /// You can add images to the <see cref="ImageList"/>, and the controls are
    /// able to use the images as they require.
    /// </remarks>
    public class ImageList : GraphicsObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageList"/> with default values.
        /// </summary>
        public ImageList()
        {
            Images.ItemInserted += Images_ItemInserted;
            Images.ItemRemoved += Images_ItemRemoved;
        }

        /// <summary>
        /// Gets the <see cref="Image"/> collection for this image list.
        /// </summary>
        /// <value>The collection of images.</value>
        public Collection<Image> Images { get; } = new();

        /// <summary>
        /// Gets or sets the size of the images in the image list, in pixels.
        /// </summary>
        /// <value>
        /// The <see cref="SizeD"/> that defines the height and width, in pixels, of the images
        /// in the list.
        /// The default size is 16 by 16 device-independent units (1/96th inch per unit).
        /// </value>
        /// <remarks>
        /// Setting the <see cref="PixelImageSize"/> to a different value than the actual size
        /// of the images in
        /// the <see cref="Images"/>collection causes the images to be resized to the size
        /// specified.
        /// </remarks>
        public SizeI PixelImageSize
        {
            get => NativeDrawing.Default.ImageListGetPixelImageSize(NativeObject);
            set => NativeDrawing.Default.ImageListSetPixelImageSize(NativeObject, value);
        }

        /// <summary>
        /// Gets or sets the size of the images in the image list, in device-independent units
        /// (1/96th inch per unit).
        /// </summary>
        /// <value>The <see cref="SizeD"/> that defines the height and width, in
        /// device-independent units (1/96th inch per unit),
        /// of the images in the list. The default size is 16 by 16.</value>
        /// <remarks>
        /// Setting the <see cref="ImageSize"/> to a different value than the actual size of
        /// the images in
        /// the <see cref="Images"/>collection causes the images to be resized to the size
        /// specified.
        /// </remarks>
        public SizeD ImageSize
        {
            get => NativeDrawing.Default.ImageListGetImageSize(NativeObject);
            set => NativeDrawing.Default.ImageListSetImageSize(NativeObject, value);
        }

        /// <inheritdoc/>
        protected override object CreateNativeObject()
        {
            return NativeDrawing.Default.CreateImageList();
        }

        private void Images_ItemInserted(object? sender, int index, Image item)
        {
            NativeDrawing.Default.ImageListAdd(NativeObject, index, item);
        }

        private void Images_ItemRemoved(object? sender, int index, Image item)
        {
            NativeDrawing.Default.ImageListRemove(NativeObject, index, item);
        }
    }
}