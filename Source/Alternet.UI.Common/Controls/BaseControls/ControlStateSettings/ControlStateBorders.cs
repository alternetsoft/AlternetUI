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

        /// <summary>
        /// Creates clone of this object.
        /// </summary>
        /// <returns></returns>
        public virtual ControlStateBorders Clone()
        {
            var result = new ControlStateBorders();
            result.Normal = Normal;
            result.Hovered = Hovered;
            result.Pressed = Pressed;
            result.Disabled = Disabled;
            result.Focused = Focused;
            return result;
        }
    }
}
