using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains foreground and background colors for all the states.
    /// </summary>
    public class AllStateColors
    {
        /// <summary>
        /// Gets or set foreground color for the normal state.
        /// </summary>
        public Color? NormalForeColor;

        /// <summary>
        /// Gets or set background color for the normal state.
        /// </summary>
        public Color? NormalBackColor;

        /// <summary>
        /// Gets or set foreground color for the hovered state.
        /// </summary>
        public Color? HoveredForeColor;

        /// <summary>
        /// Gets or set background color for the hovered state.
        /// </summary>
        public Color? HoveredBackColor;

        /// <summary>
        /// Gets or set foreground color for the pressed state.
        /// </summary>
        public Color? PressedForeColor;

        /// <summary>
        /// Gets or set background color for the pressed state.
        /// </summary>
        public Color? PressedBackColor;

        /// <summary>
        /// Gets or set foreground color for the disabled state.
        /// </summary>
        public Color? DisabledForeColor;

        /// <summary>
        /// Gets or set background color for the disabled state.
        /// </summary>
        public Color? DisabledBackColor;

        /// <summary>
        /// Gets or set foreground color for the focused state.
        /// </summary>
        public Color? FocusedForeColor;

        /// <summary>
        /// Gets or set background color for the focused state.
        /// </summary>
        public Color? FocusedBackColor;

        /// <summary>
        /// Gets colors for the normal state.
        /// </summary>
        public virtual IReadOnlyFontAndColor Normal
            => new FontAndColor(NormalForeColor, NormalBackColor);

        /// <summary>
        /// Gets colors for the hovered state.
        /// </summary>
        public virtual IReadOnlyFontAndColor Hovered
            => new FontAndColor(HoveredForeColor, HoveredBackColor);

        /// <summary>
        /// Gets colors for the pressed state.
        /// </summary>
        public virtual IReadOnlyFontAndColor Pressed
            => new FontAndColor(PressedForeColor, PressedBackColor);

        /// <summary>
        /// Gets colors for the focused state.
        /// </summary>
        public virtual IReadOnlyFontAndColor Focused
        {
            get
            {
                var foreColor = FocusedForeColor ?? NormalForeColor;
                var backColor = FocusedBackColor ?? NormalBackColor;

                return new FontAndColor(foreColor, backColor);
            }
        }

        /// <summary>
        /// Gets colors for the disabled state.
        /// </summary>
        public virtual IReadOnlyFontAndColor Disabled =>
            new FontAndColor(DisabledForeColor, DisabledBackColor);

        /// <summary>
        /// Gets <see cref="ControlStateColors"/> filled with colors.
        /// </summary>
        public virtual ControlStateColors AllStates
        {
            get
            {
                ControlStateColors result = new();

                result.Hovered = Hovered;
                result.Pressed = Pressed;
                result.Normal = Normal;
                result.Focused = Focused;
                result.Disabled = Disabled;
                return result;
            }
        }
    }
}
