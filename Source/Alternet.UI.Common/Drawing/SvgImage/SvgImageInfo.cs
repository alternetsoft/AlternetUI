using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains properties and methods which allow to specify svg image information.
    /// </summary>
    public struct SvgImageInfo
    {
        /// <summary>
        /// Gets or sets svg image to draw.
        /// </summary>
        public SvgImage? SvgImage;

        /// <summary>
        /// Gets or sets svg color override for the single color svg images.
        /// </summary>
        public Color? SvgColor;

        private int? svgSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgImageInfo"/> struct.
        /// </summary>
        public SvgImageInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgImageInfo"/> struct.
        /// </summary>
        /// <param name="image">Svg image to draw.</param>
        /// <param name="color">Svg color override for the single color svg images.</param>
        public SvgImageInfo(SvgImage image, Color? color = null)
        {
            SvgImage = image;
            SvgColor = color;
        }

        /// <summary>
        /// Gets or sets svg width and height.
        /// </summary>
        public int? SvgSize
        {
            readonly get => svgSize;

            set
            {
                if (svgSize == value)
                    return;

                if(value == null)
                {
                    svgSize = null;
                    return;
                }

                if (value < 0)
                {
                    svgSize = 16;
                    return;
                }

                svgSize = value;
            }
        }

        /// <summary>
        /// Resets any cached images associated with the current instance.
        /// </summary>
        /// <remarks>This method clears the cached images to ensure that any
        /// subsequent operations use
        /// updated or refreshed image data. It is typically used when the
        /// underlying image source has changed and the
        /// cache needs to be invalidated.</remarks>
        public readonly void ResetCachedImages()
        {
            SvgImage?.ResetCachedImages();
        }
    }
}
