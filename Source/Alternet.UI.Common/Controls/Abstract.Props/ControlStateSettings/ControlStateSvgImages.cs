using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies a set of <see cref="SvgImage"/> for different control states.
    /// </summary>
    public class ControlStateSvgImages : ControlStateObjects<SvgImage>
    {
        /// <summary>
        /// Gets <see cref="ControlStateSvgImages"/> with empty state images.
        /// </summary>
        public static readonly ControlStateSvgImages Empty;

        static ControlStateSvgImages()
        {
            Empty = new();
            Empty.SetImmutable();
        }

        /// <summary>
        /// Creates clone of this object.
        /// </summary>
        /// <returns></returns>
        public virtual ControlStateSvgImages Clone()
        {
            var result = new ControlStateSvgImages();
            result.Normal = Normal;
            result.Hovered = Hovered;
            result.Pressed = Pressed;
            result.Disabled = Disabled;
            result.Focused = Focused;
            return result;
        }
    }
}