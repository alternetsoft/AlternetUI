using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies a set of <see cref="IReadOnlyFontAndColor"/> for different control states.
    /// </summary>
    public class ControlStateColors : ControlStateObjects<IReadOnlyFontAndColor>
    {
        /// <summary>
        /// Gets <see cref="ControlStateColors"/> with empty state images.
        /// </summary>
        public static readonly ControlStateColors Empty;

        static ControlStateColors()
        {
            Empty = new();
            Empty.SetImmutable();
        }

        /// <summary>
        /// Creates clone of this object.
        /// </summary>
        /// <returns></returns>
        public virtual ControlStateColors Clone()
        {
            var result = new ControlStateColors();
            result.Normal = Normal;
            result.Hovered = Hovered;
            result.Pressed = Pressed;
            result.Disabled = Disabled;
            result.Focused = Focused;
            return result;
        }
    }
}
