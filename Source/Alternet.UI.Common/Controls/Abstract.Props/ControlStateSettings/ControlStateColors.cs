﻿using System;
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
        /// Converts the specified <see cref='ControlStateColors'/>
        /// to a <see cref='ControlStateBrushes'/>.
        /// </summary>
        public static implicit operator ControlStateBrushes(ControlStateColors colors)
        {
            ControlStateBrushes brushes = new();
            brushes.Normal = colors?.Normal?.BackgroundColor?.AsBrush;
            brushes.Disabled = colors?.Disabled?.BackgroundColor?.AsBrush;
            brushes.Focused = colors?.Focused?.BackgroundColor?.AsBrush;
            brushes.Hovered = colors?.Hovered?.BackgroundColor?.AsBrush;
            brushes.Pressed = colors?.Pressed?.BackgroundColor?.AsBrush;
            return brushes;
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
