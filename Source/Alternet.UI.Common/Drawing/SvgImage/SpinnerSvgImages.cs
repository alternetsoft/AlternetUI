using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides a set of spinner images. These images can be rotated and used as different frames of the spinner gif animations.
    /// </summary>
    public static class SpinnerSvgImages
    {
        /// <summary>
        /// Gets the image of "arrows rotate".
        /// </summary>
        public static SvgImage ImgArrowsRotate => KnownSvgImages.ImgRetry;

        /// <summary>
        /// Gets the image of "rotate".
        /// </summary>
        public static SvgImage ImgRotate => KnownSvgImages.ImgRotate;
        
        /// <summary>
        /// Gets the image of "arrows spin".
        /// </summary>
        public static SvgImage ImgArrowsSpin => KnownSvgImages.ImgArrowsSpin;
    }
}