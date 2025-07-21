using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Defines color and style settings for dark and light theme.
    /// </summary>
    public class ControlColorAndStyle
    {
        private ControlStateSettings? dark;
        private ControlStateSettings? light;

        /// <summary>
        /// Gets color and style settings for the dark theme.
        /// </summary>
        public virtual ControlStateSettings Dark
        {
            get
            {
                return dark ??= new();
            }

            set
            {
                dark = value;
            }
        }

        /// <summary>
        /// Gets color and style settings for the light theme.
        /// </summary>
        public virtual ControlStateSettings Light
        {
            get
            {
                return light ??= new();
            }

            set
            {
                light = value;
            }
        }

        /// <summary>
        /// Clones this object.
        /// </summary>
        public virtual ControlColorAndStyle Clone()
        {
            ControlColorAndStyle result = new();
            result.dark = dark?.Clone();
            result.light = light?.Clone();
            return result;
        }

        /// <summary>
        /// Gets <see cref="Dark"/> if <paramref name="isDark"/> is <c>true</c>
        /// and dark theme settings were defined;
        /// <see cref="Light"/> otherwise.
        /// </summary>
        /// <param name="isDark">Whether to return <see cref="Dark"/> or
        /// <see cref="Light"/>.</param>
        /// <returns></returns>
        public virtual ControlStateSettings DarkOrLight(bool isDark)
        {
            if (isDark && dark is not null)
                return dark;
            else
                return Light;
        }

        /// <summary>
        /// Sets the corner radius for all initialized borders of the control.
        /// </summary>
        /// <remarks>This method updates the corner radius for multiple visual
        /// states of the control,
        /// including normal, hovered, pressed, disabled, focused, and selected states.
        /// If radius is null, no specific corner radius is applied.</remarks>
        /// <param name="radius">The corner radius to apply. Can be null to indicate
        /// no specific radius.</param>
        /// <param name="radiusIsPercent">A boolean value indicating whether the
        /// specified radius is a percentage of the control's size. <c>true</c> if the
        /// radius is a percentage; otherwise, <c>false</c>.</param>
        public virtual void SetCornerRadius(Coord? radius, bool radiusIsPercent)
        {
            Dark?.Borders?.SetCornerRadius(radius, radiusIsPercent);
            Light?.Borders?.SetCornerRadius(radius, radiusIsPercent);
        }

        /// <summary>
        /// Sets border color to all initialized borders.
        /// </summary>
        /// <param name="color">New color value</param>
        public virtual void SetBorderColor(Color? color)
        {
            Dark?.Borders?.SetColor(color);
            Light?.Borders?.SetColor(color);
        }

        /// <summary>
        /// Sets border width to all initialized borders.
        /// </summary>
        /// <param name="width">New border width.</param>
        public virtual void SetBorderWidth(Thickness width)
        {
            Dark?.Borders?.SetWidth(width);
            Light?.Borders?.SetWidth(width);
        }

        /// <summary>
        /// Sets border in the <paramref name="stateToChange"/> state to be equal
        /// to the border in the <paramref name="assignFromState"/> state.
        /// </summary>
        public virtual void SetBorderFromBorder(
            VisualControlState stateToChange,
            VisualControlState assignFromState)
        {
            Dark?.Borders?.SetStateFromState(stateToChange, assignFromState);
            Light?.Borders?.SetStateFromState(stateToChange, assignFromState);
        }

        /// <summary>
        /// Sets border in the disabled state equal to border in the hovered state.
        /// </summary>
        public virtual void DisabledBorderAsHovered()
        {
            Dark?.DisabledBorderAsHovered();
            Light?.DisabledBorderAsHovered();
        }

        /// <summary>
        /// Sets background paint actions to draw push button background.
        /// <see cref="AbstractControl.DrawDefaultBackground"/> uses
        /// this information when it paints background of the control.
        /// </summary>
        public virtual void SetAsPushButton()
        {
            Dark?.SetAsPushButton();
            Light?.SetAsPushButton();
        }

        /// <summary>
        /// Sets border in the normal state equal to border in the hovered state.
        /// </summary>
        public virtual void NormalBorderAsHovered()
        {
            Dark?.NormalBorderAsHovered();
            Light?.NormalBorderAsHovered();
        }
    }
}
