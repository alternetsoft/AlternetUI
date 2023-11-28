using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Enumerates Tiff photometric values for use with <see cref="GenericImage"/>.
    /// </summary>
    public enum GenericImageTiffPhotometric
    {
        /// <summary>
        /// Photometric 'min value is white'.
        /// </summary>
        MINISWHITE = 0,

        /// <summary>
        /// Photometric 'min value is black'. 
        /// </summary>
        MINISBLACK = 1,

        /// <summary>
        /// Photometric 'RGB color model'. 
        /// </summary>
        RGB = 2,

        /// <summary>
        /// Photometric 'color map indexed'.
        /// </summary>
        PALETTE = 3,

        /// <summary>
        /// Photometric '$holdout mask'.
        /// </summary>
        MASK = 4,

        /// <summary>
        /// Photometric '!color separations'.
        /// </summary>
        SEPARATED = 5,

        /// <summary>
        /// Photometric '!CCIR 601'.
        /// </summary>
        YCBCR = 6,

        /// <summary>
        /// Photometric '!1976 CIE L*a*b*'.
        /// </summary>
        CIELAB = 8,

        /// <summary>
        /// Photometric 'ICC L*a*b* [Adobe TIFF Technote 4]'.
        /// </summary>
        ICCLAB = 9,

        /// <summary>
        /// Photometric 'ITU L*a*b*'.
        /// </summary>
        ITULAB = 10,	
    }
}