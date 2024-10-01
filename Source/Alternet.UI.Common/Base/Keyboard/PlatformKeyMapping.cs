using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Implements platform key to/from <see cref="Key"/> mapping.
    /// </summary>
    /// <typeparam name="T">Type of the platform key enum.</typeparam>
    public class PlatformKeyMapping<T> : TwoWayEnumMapping<T, Key>
        where T : struct, Enum
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformKeyMapping{T}"/> class.
        /// </summary>
        /// <param name="maxPlatformKey">Max supported platform key value.</param>
        /// <param name="maxKey">Max supported key value.</param>
        public PlatformKeyMapping(T maxPlatformKey, Key maxKey)
            : base(maxPlatformKey, maxKey)
        {
        }
    }
}
