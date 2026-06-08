using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies size fallback options for image retrieval.
    /// </summary>
    public class ImageSizeFallbackOptions : ImmutableObject
    {
        private ImageSizeFallback[]? sizeFallback;
        private bool downscaleFirst = true;
        private bool addScaled = true;
        private bool allowScaled;
        private SizeI? size;
        private ImageSizeKind sizeKind = ImageSizeKind.SmallIcon;

        /// <summary>
        /// Gets or sets the size kind for image retrieval.
        /// </summary>
        public virtual ImageSizeKind SizeKind
        {
            get => sizeKind;
            set
            {
                SetProperty(ref sizeKind, value);
            }
        }

        /// <summary>
        /// Gets or sets the specific size for image retrieval.
        /// </summary>
        public virtual SizeI? Size
        {
            get => size;
            set
            {
                SetProperty(ref size, value);
            }
        }

        /// <summary>
        /// Gets or sets an array of image size fallback strategies to apply when retrieving an image.
        /// </summary>
        public virtual ImageSizeFallback[]? SizeFallback
        {
            get => sizeFallback;
            set
            {
                this.SetProperty(ref sizeFallback, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to allow scale operations when retrieving an image with
        /// a size that does not exactly match any image in the container.
        /// </summary>
        public virtual bool AllowScaled
        {
            get => allowScaled;
            set
            {
                this.SetProperty(ref allowScaled, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to downscale or upscale the retrieved image
        /// to match the requested size if no exact match is found.
        /// This is used when <see cref="AllowScaled"/> is set to true and the container contains images of varying sizes.
        /// </summary>
        public virtual bool DownscaleFirst
        {
            get => downscaleFirst;
            set
            {
                this.SetProperty(ref downscaleFirst, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to add the scaled image to the container.
        /// </summary>
        public virtual bool AddScaled
        {
            get => addScaled;
            set
            {
                this.SetProperty(ref addScaled, value);
            }
        }
    }
}
