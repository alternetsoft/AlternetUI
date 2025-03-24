using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Enumerates known image pixel format.
    /// </summary>
    public enum ImageBitsFormatKind
    {
        /// <summary>
        /// Native image without alpha chanel.
        /// </summary>
        Native = 0,

        /// <summary>
        /// Native image with alpha chanel.
        /// </summary>
        Alpha = 1,

        /// <summary>
        /// Generic platform independent image.
        /// </summary>
        Generic = 2,

        /// <summary>
        /// Unknown image format.
        /// </summary>
        Unknown = -1,
    }
}
