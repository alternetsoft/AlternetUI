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
        /// Gets or sets svg color override for the single color svgs.
        /// </summary>
        public Color? SvgColor;

        /// <summary>
        /// Gets or sets svg width and height.
        /// </summary>
        public int SvgSize = 16;

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
        /// <param name="color">Svg color override for the single color svgs.</param>
        public SvgImageInfo(SvgImage image, Color? color = null)
        {
            SvgImage = image;
            SvgColor = color;
        }

        /// <summary>
        /// Gets this svg image as <see cref="Image"/>.
        /// </summary>
        public readonly Image? AsImage
        {
            get
            {
                return SvgImage?.ImageWithColor(SvgSize, SvgColor);
            }
        }
    }
}
