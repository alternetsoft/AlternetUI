using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies a set of <see cref="ImageSet"/> for different control states.
    /// </summary>
    public class ControlStateImageSets : ControlStateObjects<ImageSet>
    {
        /// <summary>
        /// Gets <see cref="ControlStateImageSets"/> with empty state images.
        /// </summary>
        public static readonly ControlStateImageSets Empty = new()
        {
            Immutable = true,
        };
    }
}
