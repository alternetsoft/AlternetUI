using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies the kind of image source.
    /// </summary>
    public enum ImageSourceKind
    {
        /// <summary>
        /// No image.
        /// </summary>
        None,

        /// <summary>
        /// A single image.
        /// </summary>
        Image,

        /// <summary>
        /// A set of images.
        /// </summary>
        ImageSet,

        /// <summary>
        /// An image list.
        /// </summary>
        ImageList,

        /// <summary>
        /// An SVG image.
        /// </summary>
        SvgImage,

        /// <summary>
        /// Multiple image source.
        /// </summary>
        Multi,
    }
}
