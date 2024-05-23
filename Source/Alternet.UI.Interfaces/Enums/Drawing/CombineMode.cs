using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Drawing
{
    /// <summary>
    /// Specifies how different regions can be combined.
    /// </summary>
    public enum CombineMode
    {
        /// <summary>
        /// One region is replaced by another.
        /// </summary>
        Replace,

        /// <summary>
        /// Two regions are combined by taking their intersection.
        /// </summary>
        Intersect,

        /// <summary>
        /// Two regions are combined by taking the union of both.
        /// </summary>
        Union,

        /// <summary>
        /// Two regions are combined by taking only the areas enclosed by
        /// one or the other region, but not both.
        /// </summary>
        Xor,

        /// <summary>
        /// Existing region is replaced by the result of the new region
        /// being removed from the existing region. In other words, the new region is
        /// excluded from the existing region.
        /// </summary>
        Exclude,

        /// <summary>
        /// Existing region is replaced by the result of the existing
        /// region being removed from the new region. In other words, the existing
        /// region is excluded from the new region.
        /// </summary>
        Complement,
    }
}
