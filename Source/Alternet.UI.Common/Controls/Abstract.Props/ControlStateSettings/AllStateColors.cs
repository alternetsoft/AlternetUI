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
        public LightDarkColor? NormalForeColor;

        /// <summary>
        /// Gets or set background color for the normal state.
        /// </summary>
        public LightDarkColor? NormalBackColor;

        /// <summary>
        /// Gets or set foreground color for the hovered state.
        /// </summary>
        public LightDarkColor? HoveredForeColor;

        /// <summary>
        /// Gets or set background color for the hovered state.
        /// </summary>
        public LightDarkColor? HoveredBackColor;

        /// <summary>
        /// Gets or set foreground color for the pressed state.
        /// </summary>
        public LightDarkColor? PressedForeColor;

        /// <summary>
        /// Gets or set background color for the pressed state.
        /// </summary>
        public LightDarkColor? PressedBackColor;

        /// <summary>
        /// Gets or set foreground color for the disabled state.
        /// </summary>
        public LightDarkColor? DisabledForeColor;

        /// <summary>
        /// Gets or set background color for the disabled state.
        /// </summary>
        public LightDarkColor? DisabledBackColor;

        /// <summary>
        /// Gets or set foreground color for the focused state.
        /// </summary>
        public LightDarkColor? FocusedForeColor;

        /// <summary>
        /// Gets or set background color for the focused state.
        /// </summary>
        public LightDarkColor? FocusedBackColor;

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
