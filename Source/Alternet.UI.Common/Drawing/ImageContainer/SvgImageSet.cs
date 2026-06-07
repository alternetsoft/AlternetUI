using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides a set of images in different resolutions based on the same svg image.
    /// </summary>
    public class SvgImageSet : ImageSet
    {
        private SvgImage svgImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgImageSet"/> class.
        /// </summary>
        /// <param name="svgImage">The SVG image to use for the image set.</param>
        public SvgImageSet(SvgImage svgImage)
        {
            this.svgImage = svgImage;
        }
    }
}
