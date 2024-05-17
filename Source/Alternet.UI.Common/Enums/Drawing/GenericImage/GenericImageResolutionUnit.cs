using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Enumerates possible values for the image resolution option in <see cref="GenericImage"/>.
    /// </summary>
    public enum GenericImageResolutionUnit
    {
        /// <summary>
        /// Resolution not specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// Resolution specified in inches.
        /// </summary>
        Inches = 1,

        /// <summary>
        /// Resolution specified in centimeters.
        /// </summary>
        Centimeters = 2,
    }
}
