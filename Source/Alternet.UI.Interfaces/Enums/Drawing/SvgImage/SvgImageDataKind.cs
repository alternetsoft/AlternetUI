using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies the kind of SVG data contained in a string.
    /// Enumerates known data parameter types for the constructor of svg image.
    /// </summary>
    public enum SvgImageDataKind
    {
        /// <summary>
        /// The string represents a URL pointing to SVG data that does not require
        /// immediate loading in the constructor.
        /// The SVG will be loaded the first time it is used in the application.
        /// </summary>
        Url,

        /// <summary>
        /// The string contains SVG data in XML format.
        /// </summary>
        Data,

        /// <summary>
        /// The string represents a URL pointing to SVG data.
        /// The SVG requires immediate loading in the constructor.
        /// </summary>
        PreloadUrl,
    }
}
