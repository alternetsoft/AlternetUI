using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Enumerates known data parameter types for the constructor of svg image.
    /// </summary>
    public enum SvgImageDataKind
    {
        /// <summary>
        /// String is url.
        /// </summary>
        Url,

        /// <summary>
        /// String contains data.
        /// </summary>
        Data,
    }
}
