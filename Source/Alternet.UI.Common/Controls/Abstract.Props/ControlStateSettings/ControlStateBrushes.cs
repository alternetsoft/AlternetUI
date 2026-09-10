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
        public static readonly ControlStateBrushes Empty;

        static ControlStateBrushes()
        {
            Empty = new();
            Empty.SetImmutable();
        }

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

        /// <summary>
        /// Assigns values from <paramref name="source"/> to this object.
        /// </summary>
        /// <param name="source">The source <see cref="ControlStateColors"/> to assign values from.</param>
        /// <param name="isDark">A boolean value indicating whether to use the dark variant of the colors.</param>
        public virtual void Assign(ControlStateColors? source, bool isDark)
        {
            if (source is null)
            {
                Normal = default;
                Hovered = default;
                Pressed = default;
                Disabled = default;
                Focused = default;
                Selected = default;
            }
            else
            {
                Normal = source.Normal?.BackgroundColor?.LightOrDark(isDark).AsBrush;
                Hovered = source.Hovered?.BackgroundColor?.LightOrDark(isDark).AsBrush;
                Pressed = source.Pressed?.BackgroundColor?.LightOrDark(isDark).AsBrush;
                Disabled = source.Disabled?.BackgroundColor?.LightOrDark(isDark).AsBrush;
                Focused = source.Focused?.BackgroundColor?.LightOrDark(isDark).AsBrush;
                Selected = source.Selected?.BackgroundColor?.LightOrDark(isDark).AsBrush;
            }
        }
    }
}
