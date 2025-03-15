using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides default colors for the application.
    /// </summary>
    public static class DefaultColors
    {
        /// <summary>
        /// Gets the default border color for user controls when dark theme is used.
        /// </summary>
        public static readonly Color DefaultBorderColorDark = Color.FromRgb(123, 123, 123);

        /// <summary>
        /// Gets the default border color for user controls when light theme is used.
        /// </summary>
        // This is also used to get default disabled svg image color for light theme.
        // So please do not change this value unless you completely sure.
        public static readonly Color DefaultBorderColorLight = Color.FromRgb(204, 206, 219);

        private static LightDarkColor? borderColor;

        /// <summary>
        /// Gets or sets default border color of the user control.
        /// </summary>
        public static LightDarkColor BorderColor
        {
            get => borderColor ??= Color.LightDark(DefaultBorderColorLight, DefaultBorderColorDark);

            set
            {
                borderColor = value;
            }
        }

        /// <summary>
        /// Gets default border color of the user control.
        /// </summary>
        /// <returns></returns>
        public static Color GetBorderColor(bool isDark)
        {
            if (isDark)
            {
                return BorderColor.Dark;
            }
            else
            {
                return BorderColor.Light;
            }
        }

        /// <summary>
        /// Gets the color for the border of the specified user control.
        /// </summary>
        /// <param name="control">The user control for which to get the border color.</param>
        /// <returns>The color for the border of the specified user control.</returns>
        public static Color GetBorderColor(AbstractControl control)
        {
            var color = control.Borders?.GetObjectOrNull(VisualControlState.Normal)?.Color;
            color ??= ColorUtils.GetTabControlInteriorBorderColor(control.IsDarkBackground);
            return color;
        }

        /// <summary>
        /// Gets the brush for the border of the specified user control.
        /// </summary>
        /// <param name="control">The user control for which to get the border brush.</param>
        /// <returns>The brush for the border of the specified user control.</returns>
        public static Brush GetBorderBrush(AbstractControl control)
        {
            var brush = control.Background
                ?? control.BackgroundColor?.AsBrush
                ?? GetBorderColor(control).AsBrush;
            return brush;
        }
    }
}
