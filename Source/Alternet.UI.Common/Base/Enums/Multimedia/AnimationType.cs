using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines animation type.
    /// </summary>
    public enum AnimationType
    {
        /// <summary>
        /// Invalid animation.
        /// </summary>
        Invalid,

        /// <summary>
        /// Gif animation.
        /// </summary>
        Gif,

        /// <summary>
        /// Represents the WebP animation.
        /// </summary>
        Webp,

        /// <summary>
        /// Any animation type. This value can be used when the specific animation type is not known or when any supported animation type is acceptable.
        /// </summary>
        Any,
    }
}