using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines properties for objects that can provide image sources in various forms,
    /// such as a single image, an image list, an image set, or an SVG image.
    /// </summary>
    public interface IImageSource
    {
        /// <summary>
        /// Gets the type of image source represented by this instance.
        /// </summary>
        ImageSourceKind Kind { get; }

        /// <summary>
        /// Gets a value indicating whether the image data is empty.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Gets the <see cref="Image"/> instance.
        /// </summary>
        Image? Image { get; }

        /// <summary>
        /// Gets the <see cref="ImageList"/> instance.
        /// </summary>
        ImageList? ImageList { get; }

        /// <summary>
        /// Gets the index of the image in the <see cref="ImageList"/>.
        /// </summary>
        int ImageIndex { get; }

        /// <summary>
        /// Gets the <see cref="ImageSet"/> instance.
        /// </summary>
        ImageSet? ImageSet { get; }

        /// <summary>
        /// Gets the <see cref="SvgImage"/> instance.
        /// </summary>
        SvgImage? SvgImage { get; }

        /// <summary>
        /// Gets the size of the SVG image, in pixels.
        /// </summary>
        int? SvgSize { get; }
    }
}
