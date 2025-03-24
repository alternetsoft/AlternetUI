using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Types of results returned from a call to 'Contains' methods of the region.
    /// </summary>
    public enum RegionContain
    {
        /// <summary>
        /// The specified value is not contained within this region.
        /// </summary>
        OutRegion = 0,

        /// <summary>
        /// The specified value is fully contained within this region.
        /// On Windows, this result will be returned even if only part of the
        /// specified value is contained in this region.
        /// </summary>
        InRegion = 2,
    }
}