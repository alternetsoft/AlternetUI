using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies a set of <see cref="BorderSettings"/> for different control states.
    /// </summary>
    public class ControlStateBorders : ControlStateObjects<BorderSettings>
    {
        /// <summary>
        /// Gets <see cref="ControlStateBorders"/> with empty state images.
        /// </summary>
        public static readonly ControlStateBorders Empty = new()
        {
            Immutable = true,
        };
    }
}
