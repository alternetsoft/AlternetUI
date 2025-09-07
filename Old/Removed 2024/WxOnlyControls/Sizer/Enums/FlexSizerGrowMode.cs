using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines grow mode for the <see cref="IFlexGridSizer"/>.
    /// </summary>
    internal enum FlexSizerGrowMode
    {
        /// <summary>
        /// Sizer doesn't grow its elements at all in the non-flexible direction.
        /// </summary>
        None,

        /// <summary>
        /// Sizer honors growable columns/rows set with <see cref="IFlexGridSizer.AddGrowableCol"/>
        /// and <see cref="IFlexGridSizer.AddGrowableRow"/> in the non-flexible direction as well.
        /// In this case equal sizing applies to minimum sizes of columns or rows
        /// (this is the default value).
        /// </summary>
        Specified,

        /// <summary>
        /// Sizer equally stretches all columns or rows in the non-flexible direction,
        /// independently of the proportions applied in the flexible direction.
        /// </summary>
        All,
    }
}