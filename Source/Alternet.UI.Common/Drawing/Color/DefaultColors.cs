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
        private static LightDarkColor windowBackColor;
        private static LightDarkColor windowForeColor;
        private static LightDarkColor controlBackColor;
        private static LightDarkColor controlForeColor;

        static DefaultColors()
        {
            Color darkBackColor;
            Color darkForeColor;

            if (SystemColors.Control.IsDark())
            {
                darkBackColor = SystemColors.Control;
                darkForeColor = SystemColors.ControlText;
            }
            else
            {
                darkBackColor = (30, 30, 30);
                darkForeColor = (164, 164, 164);
            }

            windowBackColor = new(light: new(240, 240, 240), dark: darkBackColor);
            windowForeColor = new(light: Color.Black, dark: darkForeColor);

            if (SystemColors.Window.IsDark())
            {
                darkBackColor = SystemColors.Window;
                darkForeColor = SystemColors.WindowText;
            }
            else
            {
                darkBackColor = (30, 30, 30);
                darkForeColor = (164, 164, 164);
            }

            controlBackColor = new(light: Color.White, dark: darkBackColor);
            controlForeColor = new(light: Color.Black, dark: darkForeColor);
        }

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
        /// Gets or sets the default foreground color of the control.
        /// </summary>
        public static LightDarkColor ControlForeColor
        {
            get => controlForeColor;

            set
            {
                controlForeColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the default background color of the control.
        /// </summary>
        public static LightDarkColor ControlBackColor
        {
            get => controlBackColor;

            set
            {
                controlBackColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the default background color of the window.
        /// </summary>
        public static LightDarkColor WindowBackColor
        {
            get => windowBackColor;

            set
            {
                windowBackColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the default foreground color of the window.
        /// </summary>
        public static LightDarkColor WindowForeColor
        {
            get => windowForeColor;

            set
            {
                windowForeColor = value;
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
        /// Gets default fore color of the window.
        /// </summary>
        /// <returns></returns>
        public static Color GetWindowForeColor(bool isDark)
        {
            if (isDark)
            {
                return WindowForeColor.Dark;
            }
            else
            {
                return WindowForeColor.Light;
            }
        }

        /// <summary>
        /// Gets default back color of the window.
        /// </summary>
        /// <returns></returns>
        public static Color GetWindowBackColor(bool isDark)
        {
            if (isDark)
            {
                return WindowBackColor.Dark;
            }
            else
            {
                return WindowBackColor.Light;
            }
        }

        /// <summary>
        /// Gets the color for the border of the specified user control.
        /// </summary>
        /// <param name="control">The user control for which to get the border color.</param>
        /// <returns>The color for the border of the specified user control.</returns>
        public static Color GetControlBorderColor(AbstractControl control)
        {
            var color = control.Borders?.GetObjectOrNull(VisualControlState.Normal)?.Color;
            color ??= GetBorderColor(control.IsDarkBackground);
            return color;
        }

        /// <summary>
        /// Gets the brush for the border of the specified user control.
        /// </summary>
        /// <param name="control">The user control for which to get the border brush.</param>
        /// <returns>The brush for the border of the specified user control.</returns>
        public static Brush GetControlBorderBrush(AbstractControl control)
        {
            var brush = control.Background
                ?? control.BackgroundColor?.AsBrush
                ?? GetControlBorderColor(control).AsBrush;
            return brush;
        }
    }
}
