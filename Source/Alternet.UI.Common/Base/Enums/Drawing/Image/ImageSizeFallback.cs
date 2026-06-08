using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Determines what happens when an image with the exact size is not found in the container.
    /// </summary>
    public enum ImageSizeFallback
    {
        /// <summary>
        /// No fallback. If image with is not found, <c>null</c> is returned.
        /// </summary>
        None = 0,

        /// <summary>
        /// If image is not found, image with the system icon size is returned.
        /// </summary>
        SystemIcon = 1,

        /// <summary>
        /// If image is not found, image with the small system icon size is returned.
        /// </summary>
        SmallSystemIcon = 2,

        /// <summary>
        /// If image is not found, the image with the closest larger size is returned.
        /// </summary>
        NearestLarger = 3,

        /// <summary>
        /// If image is not found, the image with the closest size is returned.
        /// </summary>
        Closest = 4,

        /// <summary>
        /// If image is not found, the first image is returned.
        /// </summary>
        First = 5,

        /// <summary>
        /// If image is not found, the smallest image is returned.
        /// </summary>
        Smallest = 6,
    }
}
