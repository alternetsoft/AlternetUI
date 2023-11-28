using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Enumerates levels of quality for different <see cref="GenericImage"/>
    /// resizing algorithms used by scaling and rescaling methods.
    /// </summary>
    public enum GenericImageResizeQuality
    {
        /// <summary>
        /// Simplest and fastest algorithm.
        /// </summary>
        Nearest = 0,

        /// <summary>
        /// Compromise between <see cref="Nearest"/> and <see cref="Bicubic"/>.
        /// </summary>
        Bilinear = 1,

        /// <summary>
        /// Highest quality but slowest execution time.
        /// </summary>
        Bicubic = 2,

        /// <summary>
        /// Use surrounding pixels to calculate an average that will be used for new pixels.
        /// This method is typically used when reducing the size of the image
        /// (meaning that both the new width and height will be smaller than the original size).
        /// </summary>
        BoxAverage = 3,

        /// <summary>
        /// Default image resizing algorithm. Default quality is low (but fast).
        /// Currently the same as <see cref="Nearest"/>.
        /// </summary>
        Normal = Nearest,

        /// <summary>
        /// Highest (but best) quality.
        /// </summary>
        High = 4,
    }
}