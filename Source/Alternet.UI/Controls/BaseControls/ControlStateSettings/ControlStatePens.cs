using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies a set of <see cref="Pen"/> for different control states.
    /// </summary>
    public class ControlStatePens : ControlStateObjects<Pen>
    {
        /// <summary>
        /// Gets <see cref="ControlStatePens"/> with empty state images.
        /// </summary>
        public static readonly ControlStatePens Empty = new()
        {
            Immutable = true,
        };

        /// <summary>
        /// Creates clone of this object.
        /// </summary>
        /// <returns></returns>
        public virtual ControlStatePens Clone()
        {
            var result = new ControlStatePens();
            result.Normal = Normal;
            result.Hovered = Hovered;
            result.Pressed = Pressed;
            result.Disabled = Disabled;
            result.Focused = Focused;
            return result;
        }
    }
}
