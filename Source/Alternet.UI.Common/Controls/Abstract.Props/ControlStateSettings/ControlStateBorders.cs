using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

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
            result.Normal = Normal?.Clone();
            result.Hovered = Hovered?.Clone();
            result.Pressed = Pressed?.Clone();
            result.Disabled = Disabled?.Clone();
            result.Focused = Focused?.Clone();
            result.Selected = Selected?.Clone();
            return result;
        }

        /// <summary>
        /// Sets color to all initialized borders.
        /// </summary>
        /// <param name="color">New color value.</param>
        public virtual void SetColor(Color? color)
        {
            Normal?.SetColor(color);
            Hovered?.SetColor(color);
            Pressed?.SetColor(color);
            Disabled?.SetColor(color);
            Focused?.SetColor(color);
            Selected?.SetColor(color);
        }

        /// <summary>
        /// Sets width to all initialized borders.
        /// </summary>
        /// <param name="width">New width value.</param>
        public virtual void SetWidth(Thickness width)
        {
            Normal?.SetWidth(width);
            Hovered?.SetWidth(width);
            Pressed?.SetWidth(width);
            Disabled?.SetWidth(width);
            Focused?.SetWidth(width);
            Selected?.SetWidth(width);
        }
    }
}
