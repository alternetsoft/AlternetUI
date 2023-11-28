using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Enumerates PNG image types for use with <see cref="GenericImage"/>.
    /// </summary>
    public enum GenericImagePngType
    {
        /// <summary>
        /// Color PNG image.
        /// </summary>
        Color = 0,

        /// <summary>
        /// Greyscale PNG image converted from RGB.
        /// </summary>
        Grey = 2,

        /// <summary>
        /// Greyscale PNG image using red color as grey color.
        /// </summary>
        GreyRed = 3,

        /// <summary>
        /// Palette encoding.
        /// </summary>
        Palette = 4,
    }
}
