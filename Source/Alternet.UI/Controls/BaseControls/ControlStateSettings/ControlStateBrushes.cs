using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies a set of <see cref="Brush"/> for different control states.
    /// </summary>
    public class ControlStateBrushes : ControlStateObjects<Brush>
    {
        /// <summary>
        /// Gets <see cref="ControlStateBrushes"/> with empty state images.
        /// </summary>
        public static readonly ControlStateBrushes Empty = new()
        {
            Immutable = true,
        };

        /// <summary>
        /// Creates clone of this object.
        /// </summary>
        /// <returns></returns>
        public virtual ControlStateBrushes Clone()
        {
            var result = new ControlStateBrushes();
            result.Normal = Normal;
            result.Hovered = Hovered;
            result.Pressed = Pressed;
            result.Disabled = Disabled;
            result.Focused = Focused;
            return result;
        }
    }
}
