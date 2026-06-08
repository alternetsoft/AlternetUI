using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies size fallback options for image retrieval.
    /// </summary>
    public class ImageSizeFallbackOptions
    {
        /// <summary>
        /// Gets or sets an array of image size fallback strategies to apply when retrieving an image.
        /// </summary>
        public ImageSizeFallback[] SizeFallback = Array.Empty<ImageSizeFallback>();

        /// <summary>
        /// Gets or sets a value indicating whether to downscale or upscale the retrieved image
        /// to match the requested size if no exact match is found.
        /// This is used when <see cref="AllowScaled"/> is set to true and the container contains images of varying sizes.
        /// </summary>
        public bool DownscaleFirst = true;

        /// <summary>
        /// Gets or sets a value indicating whether to add the scaled image to the container.
        /// </summary>
        public bool AddScaled = true;

        /// <summary>
        /// Gets or sets a value indicating whether to allow scale operations when retrieving an image with
        /// a size that does not exactly match any image in the container.
        /// </summary>
        public bool AllowScaled;
    }
}
