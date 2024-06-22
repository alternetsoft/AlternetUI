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
    public class ImageList : ImageContainer<IImageListHandler>
    {
        /// <summary>
        /// Gets an empty <see cref="ImageSet"/>.
        /// </summary>
        public static readonly ImageList Empty = new(immutable: true);

        private SizeI size = 16;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageList"/> with default values.
        /// </summary>
        public ImageList(bool immutable)
            : base(immutable)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageList"/> with default values.
        /// </summary>
        public ImageList()
            : this(false)
        {
        }

        /// <summary>
        /// Gets or sets the size of the images in the image list, in pixels.
        /// </summary>
        /// <value>
        /// The <see cref="SizeI"/> that defines the height and width, in pixels, of the images
        /// in the list. The default size is 16 by 16.
        /// </value>
        /// <remarks>
        /// Setting the <see cref="ImageSize"/> to a different value than the actual size
        /// of the images in the image collection causes the images to be resized to the size
        /// specified.
        /// </remarks>
        public virtual SizeI ImageSize
        {
            get
            {
                return size;
            }

            set
            {
                if (size == value)
                    return;
                size = value;
                Handler.Size = value;
                RaiseChanged();
            }
        }

        /// <inheritdoc/>
        protected override IImageListHandler CreateHandler()
        {
            return GraphicsFactory.Handler.CreateImageListHandler() ?? DummyImageListHandler.Default;
        }
    }
}