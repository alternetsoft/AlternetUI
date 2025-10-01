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
        private static LightDarkColor? svgDisabledColor;
        private static LightDarkColor? svgNormalColor;

#pragma warning disable CS8618
        static DefaultColors()
        {
            Initialize();
        }
#pragma warning restore CS8618

        /// <summary>
        /// Gets or sets the default color of a checkbox in its normal state.
        /// </summary>
        public static LightDarkColor DefaultCheckBoxColor { get; set; }
            = new(light: (0, 103, 192), dark: new(76, 194, 255));

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
        /// Gets or sets the color used for SVG images in both light and dark themes.
        /// </summary>
        public static LightDarkColor SvgNormalColor
        {
            get => svgNormalColor ??= new LightDarkColor(
                light: Color.Black,
                dark: Color.White);
            set => svgNormalColor = value;
        }

        /// <summary>
        /// Gets or sets the color used for disabled SVG images in both light and dark themes.
        /// </summary>
        public static LightDarkColor SvgDisabledColor
        {
            get => svgDisabledColor ??= new LightDarkColor(
                light: SystemColors.GrayText,
                dark: DefaultColors.GetBorderColor(isDark: false));
            set => svgDisabledColor = value;
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

        /// <summary>
        /// Initializes the <see cref="DefaultColors"/> based on the current system theme.
        /// </summary>
        /// <remarks>This method determines appropriate light and dark color values for various UI
        /// elements (such as window and control backgrounds and foregrounds)
        /// by inspecting the system's current color
        /// scheme. It ensures that the application adapts to both light and dark themes
        /// for a consistent user
        /// experience.</remarks>
        public static void Initialize()
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
    }
}
